using ServerApplication.Models;
using System;
using System.Collections.Generic;
using ServerML.Model;
using System.IO;
using ServerApplication.HardwareInterface;

namespace ServerApplication.PredictionUnit
{
    public class PredictionUnit : IPredictionUnit
    {
        public string CurrentPrediction { get; set; }
        private IHardwareInterface arduino;
        public PredictionUnit(IHardwareInterface hwIface)
        {
            arduino = hwIface;
            arduino.DataRecieved += Arduino_DataRecieved;
        }

        private void Arduino_DataRecieved(string obj)
        {
            var accumList = new List<SensorDataRecord>();

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
                    accumList.Add(retrievedRec);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write(e.Message);
                }
            }
            if (accumList.Count > 0)
            {
                var lastValid = accumList[accumList.Count - 1];

                var input = new ModelInput();
                input.Temperature = lastValid.Temperature;
                input.Humidity = lastValid.Humidity;
                input.Pulse = lastValid.HeartRate;
                input.Sp02 = lastValid.Oxygen;

                ModelOutput result = ConsumeModel.Predict(input);

                CurrentPrediction = result.Prediction;
            }
        }
    }
}
