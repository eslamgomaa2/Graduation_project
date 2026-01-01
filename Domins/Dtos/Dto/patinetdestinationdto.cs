using System.ComponentModel.DataAnnotations;
namespace Domins.Dtos.Dto
{
    public class PatinetDestinationDto
    {

       
        public int Id { get; set; }

        
        public string? FName { get; set; }
       
        public string? LName { get; set; }
        
        public string? UserName { get; set; }

        
        public string? patientEmail { get; set; }

    }
}
