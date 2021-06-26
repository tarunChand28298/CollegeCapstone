using ServerApplication.HardwareInterface;
using ServerApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ServerApplication.Storage
{
    public class StorageUnit : IStorageUnit
    {
        private IHardwareInterface arduino;

        public StorageUnit(IHardwareInterface hwIface)
        {
            arduino = hwIface;

            arduino.DataRecieved += OnDataRecieved;
        }

        private void OnDataRecieved(string data)
        {
            
            using(StreamWriter streamWriter = new StreamWriter("data.csv", append: true))
            {
                if(data.StartsWith('>'))
                {
                    data = data.Remove(0, 1);
                    streamWriter.Write(DateTime.Now.ToString() + ',');

                }
                streamWriter.Write(data);
            }
            
        }

        public Task<IEnumerable<SensorDataRecord>> RetrieveSensorDataRecords(int nRecords)
        {
            var result = new List<SensorDataRecord>();

            var lines = File.ReadAllLines("data.csv");
            foreach (var line in lines)
            {
                try
                {
                    var data = line.Split(',');
                    SensorDataRecord retrievedRec = new SensorDataRecord()
                    {
                        Timestamp = DateTime.Parse(data[0]),
                        Temperature = float.Parse(data[1]),
                        Humidity = float.Parse(data[2]),
                        HeartRate = float.Parse(data[3]),
                        Oxygen = float.Parse(data[4])
                    };
                    result.Add(retrievedRec);
                }

                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write(e.Message);
                }
            }

            return Task.FromResult(result.AsEnumerable());
        }
    }
}
