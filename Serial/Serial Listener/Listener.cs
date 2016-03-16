using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace NMEA_Tools.Serial
{
    /// TODO: Improve the thread safety of this class and develop test to ensure that it is thread safe.
    // of special note is the LastString Property, there could be issues regarding the thread safety of that 
    /// <summary>
    /// The listener class will listen to serial port traffic for lines of data and will push notifications to subscrubers
    /// when the line end characters are met
    /// </summary>
    public class Listener
    {
        #region Public Properties
        public string PortName
        {
            get
            {
                if (_IsInitalizied)
                {
                    return _Port.PortName;
                }
                else
                {
                    return _PortName;
                }
            }
            set
            {
                if (_IsInitalizied)
                {
                    throw new ListenerException(Listener._ProtInializedExceptionMessage, null);
                }

                _PortName = value;
            }
        }

        public int BaudRate
        {
            get
            {
                if(_IsInitalizied)
                {
                    return _Port.BaudRate;
                }
                else
                {
                    return _BaudRate;
                }
            }
            set
            {
                if (_IsInitalizied)
                {
                    throw new ListenerException(Listener._ProtInializedExceptionMessage, null);
                }

                _BaudRate = value;
            }
        }
        public Parity Parity
        {
            get
            {
                if(_IsInitalizied)
                {
                    return _Port.Parity;
                }
                else
                {
                    return _Parity;
                }
            }
            set
            {
                if (_IsInitalizied)
                {
                    throw new ListenerException(Listener._ProtInializedExceptionMessage, null);
                }
                _Parity = value;
            }
        }
        public int DataBits
        {
            get
            {
                if(_IsInitalizied)
                {
                    return _Port.DataBits;
                }
                else
                {
                    return _DataBits;
                }
            }
            set
            {
                if (_IsInitalizied)
                {
                    throw new ListenerException(Listener._ProtInializedExceptionMessage, null);
                }

                _DataBits = value;
            }
        }
        public StopBits StopBits
        {
            get
            {
                if (_IsInitalizied)
                {
                    return _Port.StopBits;
                }
                else
                {
                    return _StopBits;
                }
            }
            set
            {
                if (_IsInitalizied)
                {
                    throw new ListenerException(Listener._ProtInializedExceptionMessage, null);
                }

                _StopBits = value;
            }
        }
        public string LastString
        {
            get
            {
                return _LastSentence;
            }
        }
        public string LineEndChars
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach(byte value in _SentenceEndChars)
                {
                    builder.Append(Convert.ToChar(value));
                }
                return builder.ToString();
            }
            set
            {
                List<byte> bytesValue = new List<byte>();
                for(int i = 0; i < value.Length; i++)
                {
                    bytesValue.Add(Convert.ToByte(value[i]));
                }
                _SentenceEndChars = bytesValue.ToArray();
            }
        }
        #endregion

        #region Private Fields
        private bool _IsInitalizied;

        private string _PortName;
        private int _BaudRate;
        private Parity _Parity;
        private int _DataBits;
        private StopBits _StopBits;

        private SerialPort _Port;

        private ICollection<IListenerSubscriber> _Subscribers;
        private string _LastSentence;
        private Task _NotifySubscribersTask;
        private object _NotifyingSubscriberLock = new object();

        private byte[] _SentenceEndChars;
        private bool _LastByteWasEnding;
        private int _SentenceEndCharLast;
        private object _AddingLock = new object();

        private ConcurrentQueue<byte> _ReceivedBytes;
        private bool _ReceiveLoop;
        #endregion

        #region Private Static Fields
        private static string _ProtInializedExceptionMessage= "Can not change base port properties once the listening port Has been Initalized";
        #endregion

        public Listener()
        {
            _IsInitalizied = false;
            _Port = new SerialPort();
            _Subscribers = new List<IListenerSubscriber>();

            _SentenceEndChars = new byte[] { 0x0D, 0x0A };
            _SentenceEndCharLast = -1;

            _LastSentence = String.Empty;
            _ReceivedBytes = new ConcurrentQueue<byte>();

            _NotifySubscribersTask = new Task(new Action(_StringReceived));
        }

        #region Public Methods
        public string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public void SetupPort()
        {
            if(_Port.IsOpen)
            {
                _Port.Close();
            }
            _SetupPort();
        }

        public void Open()
        {
            if(!_IsInitalizied)
            {
                throw new ListenerException("Port must be initalizied before opening", null);
            }

            _Port.Open();
            _ReceiveLoop = true;
            System.Threading.Thread serialThread = new System.Threading.Thread(new System.Threading.ThreadStart(_PortReadThread));
            serialThread.Start();
        }

        public IDisposable Subscribe(IListenerSubscriber observer)
        {
            if (!_Subscribers.Contains(observer))
            {
                _Subscribers.Add(observer);
            }

            return null;
        }

        public void Unsubscribe(IListenerSubscriber listener)
        {
            if(_Subscribers.Contains(listener))
            {
                _Subscribers.Remove(listener);
            }
        }
        #endregion

        #region Private Methods
        private void _SetupPort()
        {
            try
            {
                if (_PortName == null) 
                {
                    throw new ListenerException("Port name not initalized port can not be setup",null);
                }

                if((_BaudRate != default(int)) && (_DataBits == default(int)))
                {
                    _Port = new SerialPort(_PortName,_BaudRate);
                }
                else
                {
                    _Port = new SerialPort(_PortName, _BaudRate, _Parity, _DataBits, _StopBits);
                }
                
                
            }
            catch (Exception excpt)
            {
                ListenerException exception = new ListenerException("Error while initalizing the com port",excpt);
                throw exception;
            }

            _IsInitalizied = true;
            return;
        }

        private void _NotifySubscribers()
        {
            foreach(IListenerSubscriber subscriber in _Subscribers)
            {
                subscriber.SentenceReceived(this._LastSentence);
            }
        }

        private void _StringReceived()
        {
            StringBuilder sentenceChars = new StringBuilder();
            lock (this._NotifyingSubscriberLock)
            {
                int processingSentenceEndCharsPosition = -1;
                bool processingLastByteWasEnding = false;
                byte lastByte;
                do
                {
                    #region Get the next byte in the queue
                    while(true)
                    {
                        try
                        {
                            if(_ReceivedBytes.TryDequeue(out lastByte))
                            {
                                break;
                            }
                            throw new Exception("Could not dequeue item");
                        }
                        catch (Exception excpt)
                        {
                            System.Diagnostics.Debug.WriteLine("Error while processing received bytes queue : {0}",excpt.InnerException);
                        }
                    }
                    #endregion

                    #region Check if the byte was part of the the line ending chars if not then reset and start looking for the ending chars again
                    if (processingLastByteWasEnding || (lastByte == _SentenceEndChars[0]))
                    {
                        processingLastByteWasEnding = true;
                        if (lastByte == _SentenceEndChars[processingSentenceEndCharsPosition + 1])
                        {
                            processingSentenceEndCharsPosition++;
                        }
                        else
                        {
                            processingLastByteWasEnding = false;
                            _SentenceEndCharLast = -1;
                        }
                    }
                    #endregion

                    sentenceChars.Append(Convert.ToChar(lastByte));

                } while ((processingSentenceEndCharsPosition + 1) < _SentenceEndChars.Length);
            }

            sentenceChars.Remove(sentenceChars.Length - 1 - _SentenceEndChars.Length, _SentenceEndChars.Length);

            _LastSentence = sentenceChars.ToString();
            _NotifySubscribers();

            _NotifySubscribersTask = new Task(new Action(_StringReceived));
        }

        private void _PortReadThread()
        {
            while (_ReceiveLoop)
            {
                try
                {
                    if (_Port.BytesToRead != 0)
                    {
                        byte[] buffer = new byte[_Port.BytesToRead];
                        Task<int> readTask = _Port.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                        while (true)
                        {
                            System.Threading.Thread.Sleep(1);
                            if (readTask.IsCompleted)
                            {
                                break;
                            }
                        }
                        _SerialDataEvent(buffer);
                    }
                    System.Threading.Thread.Sleep(1);
                }
                catch(Exception excpt)
                {
                    if(_Port.IsOpen)
                    {
                        _Port.Close();
                        _ReceiveLoop = false;
                    }
                    System.Diagnostics.Debug.WriteLine(excpt.Message);
                }
            }   
        }

        private void _SerialDataEvent(byte[] bytes)
        {
            
            lock (_AddingLock)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    // Get the byte that was just received
                    byte receivedByte = bytes[i];

                    // Check if the byte was part of the the line ending chars if not then reset and start looking for the ending chars again
                    if (_LastByteWasEnding || (receivedByte == _SentenceEndChars[0]))
                    {
                        _LastByteWasEnding = true;
                        if (receivedByte == _SentenceEndChars[_SentenceEndCharLast + 1])
                        {
                            _SentenceEndCharLast++;
                        }
                        else
                        {
                            _LastByteWasEnding = false;
                            _SentenceEndCharLast = -1;
                        }
                    }

                    // Add the received bytes to the queue
                    _ReceivedBytes.Enqueue(receivedByte);

                    // If we have have received the entire line ending sequence then start notifying subscribers and return so that the 
                    // current method does not block execution
                    if ((_SentenceEndCharLast + 1) == _SentenceEndChars.Length)
                    {
                        _NotifySubscribersTask.Start();
                        _SentenceEndCharLast = -1;
                    }
                }
            }
        }
        #endregion
    }
}
