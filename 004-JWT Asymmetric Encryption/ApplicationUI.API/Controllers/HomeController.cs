using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok("Hi there!");
        }

        [HttpGet("Profile")]
        [Authorize]
        public IActionResult UserProfile()
        {
            return Ok("You are Authorized");
        }
    }
}
