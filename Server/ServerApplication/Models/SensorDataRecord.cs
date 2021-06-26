using System;

namespace ServerApplication.Models
{
    public class SensorDataRecord
    {
        public DateTime Timestamp { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float HeartRate { get; set; }
        public float Oxygen { get; set; }
    }
}
