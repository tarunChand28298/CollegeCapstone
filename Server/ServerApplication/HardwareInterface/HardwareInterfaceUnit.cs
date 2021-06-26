using ServerApplication.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ServerApplication.HardwareInterface
{
    public class HardwareInterfaceUnit : IHardwareInterface
    {
        public event Action<string> DataRecieved;

        private SerialPort arduino;

        public HardwareInterfaceUnit()
        {
            try
            {
                arduino = new SerialPort("/dev/ttyACM0", 9600);
                arduino.Open();

                arduino.DataReceived += OnDataRecieved;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private void OnDataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            DataRecieved?.Invoke(arduino.ReadLine());
        }

        public void SetSetpoint(float point)
        {
            arduino.Write(point.ToString());
        }
    }
}
