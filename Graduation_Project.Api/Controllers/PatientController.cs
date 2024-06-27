using Domins.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interfaces;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class PatientController : ControllerBase
    {
        private readonly IBaseRepository<Patient> _baserepository;
        private readonly ApplicationDbcontext _dbcontext;


        public PatientController(IBaseRepository<Patient> baserepository, ApplicationDbcontext dbcontext)
        {
            _baserepository = baserepository;
            _dbcontext = dbcontext;
        }

        [HttpGet("GetAllPatient")]
        public async Task<ActionResult> GetAllpatient()
        {
            
            var data = await _dbcontext.Patients.Include(o => o.Doctor).ToListAsync();
            return Ok(data.Select(o => new {o.Id,o.FName,o.LName,o.UserName,o.DoctorId,o.UserId,o.Doctor?.Email }));
        }

    }
}
