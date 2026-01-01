using Domins.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OA.Domain.Auth;
using OA.Domain.Enum;
using Org.BouncyCastle.Bcpg;
using Repository;
using Repository.Interfaces;
using System.Security.Claims;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class DoctorController : ControllerBase
    {


        
        private readonly IBaseRepository<Doctor> _baserepository;
        private readonly IDoctorService _doctorservice;
        private readonly IHttpContextAccessor _httpcontext;
        

        public DoctorController(IDoctorService doctorservice,  IBaseRepository<Doctor> baserepository, IHttpContextAccessor httpcontext)
        {
            _doctorservice = doctorservice;
            
            _baserepository = baserepository;
            _httpcontext = httpcontext;
           
        }
        
        
        [HttpGet("GetDoctorPatinets")]
        public async Task<IActionResult> GetDoctorPatinets()
        {
            var userid =_httpcontext.HttpContext.User.Claims.First(o=>o.Type==ClaimTypes.NameIdentifier).Value;
           

            return Ok( await _doctorservice.GetDoctorPatient(userid));
        } 

        [HttpGet("GetAllDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var res = await _baserepository.GetAllAsync();
            return Ok(res.Select(o=>new { o.Id,o.FName,o.LName,o.UserName,o.Email, o.PhoneNumber,o.Password,o.UserId }));
        }
        
    }
}
