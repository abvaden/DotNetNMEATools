using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace NMEA_Tools.GPS_DataLogger
{
    public class Logger : IObserver<DataSource>
    {

        #region Public Properties
        public bool IsLogging {
            get
            {
                return _IsLogging;
            }
        }
        public TimeSpan LoggingRate
        {
            get
            {
                return _LoggingRate;
            }
        }
        #endregion

        #region Private Fields
        private ILogTarget _TargetLog;
        private DataSource _DataSource;
        private bool _IsLogging;
        private TimeSpan _LoggingRate;

        private Thread _LoggingThread;
        #endregion

        public Logger(DataSource dataSource, ILogTarget targetLog)
        {
            _DataSource = dataSource;
            _TargetLog = targetLog;

            if (_DataSource.IsPushBased)
            {
                _DataSource.Subscribe(this);
            }

            _LoggingThread = new Thread(new ThreadStart(_LoggingThreadTarget));
        }

        #region Public Methods
        public void BeginLogging()
        {
            try
            {
                _TargetLog.OpenLog();
                if(!_DataSource.IsPushBased)
                {
                    // Check to make sure the logger has a rate to run at
                    if(_LoggingRate == null)
                    {
                        throw new LoggerException("The logger can not start logging the data type if the logging rate is not provided", null);
                    }

                    // Start the logging thread if the thread is unstarted
                    if(_LoggingThread.ThreadState == System.Threading.ThreadState.Unstarted)
                    {
                        _LoggingThread.Start();
                    }
                    else
                    {
                        throw new LoggerException("The logger is not in a state where logging can be started", null);
                    }
                }
            }
            catch (Exception excpt)
            {
                throw new LoggerException("The logger encountered an exception while trying to open the log", excpt);
            }
        }

        public void EndLogging()
        {
            try
            {
                _IsLogging = false;
                if(_LoggingThread.IsAlive)
                {
                    _LoggingThread.Abort();
                }
                _TargetLog.CloseLog();
            }
            catch (Exception excpt)
            {
                throw new LoggerException("The logger threw an exception while closing the log", excpt);
            }
        }

        public void SetLoggingRate(TimeSpan rate)
        {
            if(rate != null && rate.Milliseconds > 1)
            {
                _LoggingRate = rate;
            }
        }

        #region IObservable implementation
        public void OnNext(DataSource value)
        {
            try
            {
                _TargetLog.Log();
            }
            catch (Exception excpt)
            {
                throw new LoggerException("Logger failed while logging a data point", excpt);
            }
        }

        public void OnError(Exception error)
        {
            throw new LoggerException("The logger encountered an error with it's data source", error);
        }

        public void OnCompleted()
        {
            EndLogging();
        }
        #endregion
        #endregion

        #region Private Methods
        private void _LoggingThreadTarget()
        {
            Stopwatch stpWatch = new Stopwatch();
            while(_IsLogging)
            {
                Task logTask = new Task(new Action(_TargetLog.Log));
                logTask.Start();
                Thread.Sleep(_LoggingRate);
            }
        }
        #endregion
    }
}
