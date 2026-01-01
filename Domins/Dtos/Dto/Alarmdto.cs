using System.Text.Json.Serialization;

namespace Domins.Dtos.Dto
{
    public class Alarmdto
    {
        [JsonIgnore]
        public  int Id { get; set; }
        public string? AlarmMessage { get; set; }
        public DateTime TimeStamp { get; set; }
        
    }
}
