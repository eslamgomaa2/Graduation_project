using Domins.Dtos.Dto;
using Domins.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OA.Domain.Auth;
using OA.Service.Implementation;
using Repository;
using Repository.Interfaces;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class PatientController : ControllerBase
    {
        
        private readonly ApplicationDbcontext _dbcontext;
        

        public PatientController(ApplicationDbcontext dbcontext)
        {

            _dbcontext = dbcontext;
            
        }

        [HttpGet("GetAllPatient")]
        public async Task<ActionResult> GetAllpatient()
        {
            
            var data = await _dbcontext.Patients.Include(o => o.Doctor).ToListAsync();
            return Ok(data.Select(o => new {o.Id,o.FName,o.LName, o.UserName,o.patientEmail ,o.DoctorId,o.UserId,o.Doctor?.Email }));
        }
        

       

    }
}
