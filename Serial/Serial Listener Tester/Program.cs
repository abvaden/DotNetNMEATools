using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Serial_Listener_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            NMEA_Tools.Serial.Listener listener = new NMEA_Tools.Serial.Listener();

            string COMPort = "COM5";
            int baudrate = 4800;


            Console.WriteLine("Available ports");
            foreach (string portName in listener.GetAvailablePorts())
            {
                Console.WriteLine(portName);
            }
            Console.WriteLine();

            Console.WriteLine("Starting listener on {0}", COMPort);
            Console.WriteLine("\t{0}", baudrate);
            listener.PortName = COMPort;
            listener.BaudRate = baudrate;
            listener.SetupPort();
            listener.Open();

            System.Threading.Thread.Sleep(1000);
            while (!Console.KeyAvailable)
            {
                Console.CursorTop = 10;
                Console.CursorLeft = 0;
                if(listener.LastString.Contains("GPGLL"))
                {
                    NMEA_Tools.Decoder.Sentences.GPGLL gpgll = null;
                    try
                    {
                        gpgll = new NMEA_Tools.Decoder.Sentences.GPGLL(listener.LastString);
                        Console.WriteLine(gpgll.Latitude.Value);
                        Console.WriteLine(gpgll.Longitude.Value);
                    }
                    catch
                    {
                        Console.WriteLine("Error improper NMEA string");
                    }
                    
                }
                System.Threading.Thread.Sleep(100);
            }
        }
    }


}
