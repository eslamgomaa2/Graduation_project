using System.ComponentModel.DataAnnotations.Schema;

namespace Domins.Model
{
    public class SensorData
    {
        public int  id { get; set; }
        public int HeartRate { get; set; }
        public int OxygenRate { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
