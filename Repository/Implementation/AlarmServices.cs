using AutoMapper;
using Domins.Dtos.Dto;
using Domins.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Net.Http;
using System.Security.Claims;

namespace Repository.Implementation
{
    public class AlarmServices : BaseRepositry<Alarm>, IAlarmServices
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly IHttpContextAccessor _httpcontext;
        private readonly IMapper _mapper;
        public AlarmServices(ApplicationDbcontext dbcontext, IHttpContextAccessor httpcontext, IMapper mapper) : base(dbcontext)
        {
            _dbcontext = dbcontext;
            _httpcontext = httpcontext;
            _mapper = mapper;
        }

        public async Task<Alarm> Create(Alarm model)
        {
            var userid = _httpcontext.HttpContext.User.Claims.First(o => o.Type == ClaimTypes.NameIdentifier).Value;
           
            var patient = await _dbcontext.Patients.FirstOrDefaultAsync(c => c.UserId == userid);
            if (patient == null)
            {
               throw new Exception("there is no patinet") ;
            }

            var alarm = new Alarm
            {
                AlarmMessage = model.AlarmMessage,
                TimeStamp = DateTime.Now,
                PatientId= patient.Id,
                
            };
            await _dbcontext.Alarms.AddAsync(alarm);
            await _dbcontext.SaveChangesAsync();
            //var map = _mapper.Map<Alarmdto>(alarm);
            return alarm;
           
        }

        public async Task<bool> Delete(int id)
        {
            var userId = _httpcontext.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            
            var patient = await _dbcontext.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
            {
                return false; 
            }

            
            var alarm = await _dbcontext.Alarms.FirstOrDefaultAsync(a => a.id == id && a.PatientId == patient.Id);
            if (alarm == null)
            {
                return false; 
            }

            
            _dbcontext.Alarms.Remove(alarm);
            await _dbcontext.SaveChangesAsync();

            return true; 
        }


        public async Task<IEnumerable<Alarmdto>> GetPtientAlarms()
        {
            var userId = _httpcontext.HttpContext?.User.Claims.First(o => o.Type == ClaimTypes.NameIdentifier).Value;
            var patient = await _dbcontext.Patients.SingleOrDefaultAsync(c => c.UserId == userId);

            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            var alarms = await _dbcontext.Alarms
                .Where(c => c.PatientId == patient.Id)
                .ToListAsync();

            // Map Alarm entities to Alarmdto using AutoMapper
            var alarmDtos = _mapper.Map<IEnumerable<Alarmdto>>(alarms);

            return alarmDtos;
        }

      

        public async Task<Alarmdto> Update(Alarmdto model, int id)
        {
            var userId = _httpcontext.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

           
            var patient = await _dbcontext.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
            if (patient == null)
            {
                throw new Exception("Patient not found for the logged-in user.");
            }

            
            var alarm = await _dbcontext.Alarms.FirstOrDefaultAsync(a => a.id == id && a.PatientId == patient.Id);
            if (alarm == null)
            {
                throw new Exception("Alarm not found for the specified ID and patient.");
            }

            alarm.AlarmMessage = model.AlarmMessage;
            alarm.TimeStamp = model.TimeStamp;

            
            _dbcontext.Alarms.Update(alarm);
            await _dbcontext.SaveChangesAsync();

            return model;
        }



    }
}
