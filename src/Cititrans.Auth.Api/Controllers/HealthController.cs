using Microsoft.AspNetCore.Mvc;

namespace Cititrans.Auth.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class HealthController : ControllerBase
    {
        [HttpGet("health-check")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public IActionResult CheckHealth() => Ok("ready");
    }
}