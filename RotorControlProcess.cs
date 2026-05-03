using System;
using System.Globalization;
using System.IO.Ports;
using System.Timers;

namespace HamSatTune
{
    public class RotorControlProcess : IDisposable
    {
        private readonly SerialPort serialPort = new SerialPort();
        private readonly Timer pollTimer = new Timer();
        private readonly object serialLock = new object();
        private readonly double tolerance;

        public event Action RotorUpdated;

        public RotorControlProcess(string portName, int baudRate, double tolerance)
        {
            this.tolerance = tolerance;

            serialPort.PortName = portName;
            serialPort.BaudRate = baudRate;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.NewLine = "\n";
            serialPort.DataReceived += SerialPort_DataReceived;

            pollTimer.Interval = 1000;
            pollTimer.Elapsed += PollTimer_Elapsed;
        }

        public double Az { get; private set; }
        public double El { get; private set; }
        public bool IsConnected { get { return serialPort.IsOpen; } }

        public string Connect()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                pollTimer.Start();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Disconnect()
        {
            Stop();
            pollTimer.Stop();

            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
            catch
            {
            }
        }

        public void SetPosition(double az, double el)
        {
            az = NormalizeAzimuth(az);
            el = Math.Max(0, Math.Min(180, el));

            if(el <= 0)
            {
                return;
            }

            if (Math.Abs(az - Az) <= tolerance && Math.Abs(el - El) <= tolerance)
            {
                return;
            }

            WriteCommand("X1");
            WriteCommand(string.Format(CultureInfo.InvariantCulture, "W{0:000} {1:000}", az, el));
            WriteCommand("X4");
        }

        public void Up()
        {
            WriteCommand("U");
        }

        public void Down()
        {
            WriteCommand("D");
        }

        public void Left()
        {
            WriteCommand("L");
        }

        public void Right()
        {
            WriteCommand("R");
        }

        public void Stop()
        {
            WriteCommand("S");
        }

        public void RequestPosition()
        {
            WriteCommand("C2");
        }

        public void Dispose()
        {
            Disconnect();
            pollTimer.Dispose();
            serialPort.Dispose();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort.ReadLine().Replace("\r", "").Trim();
                if (data.Length >= 10)
                {
                    double az;
                    double el;
                    if (double.TryParse(data.Substring(1, 4), NumberStyles.Float, CultureInfo.InvariantCulture, out az) &&
                        double.TryParse(data.Substring(6, 4), NumberStyles.Float, CultureInfo.InvariantCulture, out el))
                    {
                        Az = az;
                        El = el;
                        RotorUpdated?.Invoke();
                    }
                }
            }
            catch
            {
            }
        }

        private void PollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RequestPosition();
        }

        private void WriteCommand(string command)
        {
            try
            {
                lock (serialLock)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Write(command + "\r\n");
                    }
                }
            }
            catch
            {
            }
        }

        private double NormalizeAzimuth(double az)
        {
            while (az < 0)
            {
                az += 360;
            }

            while (az >= 360)
            {
                az -= 360;
            }

            return az;
        }
    }
}
