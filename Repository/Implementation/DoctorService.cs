using AutoMapper;
using Domins.Dtos.Dto;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OA.Service.Implementation;
using Repository.Interfaces;

namespace Repository.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly IMapper _mapper;
        




        public DoctorService(ApplicationDbcontext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
           
        }

        public async Task<IEnumerable<PatinetDestinationDto>> GetDoctorPatient(string userid)
        {
            var doctor = await _dbcontext.Doctors.SingleOrDefaultAsync(c => c.UserId == userid);
            if (doctor == null)
            {
                return Enumerable.Empty<PatinetDestinationDto>();
            }

            var patients = await _dbcontext.Patients.Where(c => c.DoctorId == doctor.Id).ToListAsync();
            var destinationDtos = _mapper.Map<IEnumerable<PatinetDestinationDto>>(patients);

            return destinationDtos;
        }


      
    }
}
