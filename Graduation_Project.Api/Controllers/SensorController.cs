using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Graduation_Project.Api.Controllers
{
    public class SensorController : Controller
    {
        [HttpGet("Sensor_Reading")]
        public async Task<IActionResult> GetSensorreading()
        {
            return Ok();
        }
    }
}
