using Domins.Dtos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace Graduation_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarningController : ControllerBase
    {
        private readonly IWarningService _warningService;

        public WarningController(IWarningService warningService)
        {
            _warningService = warningService;
        }

        [HttpPost]
        public async Task<IActionResult> SendWarning(Warningdto dto)
        {
            await _warningService.SendWarning(dto);

            return(Ok());
        }

    }
}
