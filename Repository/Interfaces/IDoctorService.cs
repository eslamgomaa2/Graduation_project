using Domins.Dtos.Dto;
using Domins.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IDoctorService
    {
        public Task<IEnumerable<PatinetDestinationDto>> GetDoctorPatient(string userid);

    }
}
