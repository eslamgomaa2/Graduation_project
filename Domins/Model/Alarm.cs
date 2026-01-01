using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace Domins.Model
{
    public class Alarm
    {
        [Key]
        [JsonIgnore]
        public int id { get; set; }
        public string? AlarmMessage { get; set; }
        public DateTime TimeStamp { get; set; }
        [JsonIgnore]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        
       
    }
}
