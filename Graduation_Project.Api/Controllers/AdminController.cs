using Domins.Dtos.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OA.Domain.Auth;
using Repository;
using Repository.Interfaces;

namespace Graduation_Project.Api.Controllers
{

    [Route("Api/[Controller]")]
    [ApiController]

    public class AdminController : Controller
    {



        private readonly IAccountServices _accountServices;


        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IAdminService _adminservice;

        public AdminController(IAccountServices accountServices, UserManager<ApplicationUser> usermanger, IAdminService adminservice, UserManager<ApplicationUser> usermanager)
        {
            _accountServices = accountServices;



            _adminservice = adminservice;
            _usermanager = usermanager;
        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromBody] RegisterAsDoctorRequest model)
        {
            if (!ModelState.IsValid) BadRequest(ModelState);
            var res = await _accountServices.AddDoctor(model);
            if (!res.IsAuthenticated) {
                return BadRequest(res.Message );
            }

            return Ok(res);
        }
        [HttpPut("EditDoctor/{id}")]
        public async Task<IActionResult> UpdateDoctor([FromBody] RegisterAsDoctorRequest model, string id)
        {
            if (!ModelState.IsValid) BadRequest(ModelState);
            

            return Ok(await _adminservice.UpdateDoctor(model, id));
        }
        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor( string id)
        {
            if (!ModelState.IsValid) BadRequest(ModelState);

           var res= await _adminservice.Deletedoctor(id);
            return Ok(res);
        }




    } 
}
