using Domins.Dtos.Dto;
using Domins.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Patient")]
    public class AlarmController : ControllerBase
    {
        private readonly IAlarmServices _alarmsService;
        
        public AlarmController(IAlarmServices alarmsService)
        {
            _alarmsService = alarmsService;
            
          
        }
        
        [HttpGet("GetAlarmsfor_patient")]
        public async Task<IActionResult> GetPatintAlarms()
        {
            return Ok(await _alarmsService.GetPtientAlarms());

        }

        
        [HttpPost("AddAlarm")]
        public async Task<IActionResult> AddAlarm([FromBody] Alarm  model)
        {
 
            return Ok( await _alarmsService.Create(model));
        }
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Update([FromBody] Alarmdto model, int id)
        {
           
            return Ok(await _alarmsService.Update(model,id));
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           
            return Ok(await _alarmsService.Delete(id));
        }
    }
}
