using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        //role based 
        [HttpGet("GetTeacher")]
        [Authorize(Roles = $"{TS.Roles.Admin},{TS.Roles.Contributor},{TS.Roles.User}")]
        public string Get()
        {
            return "Get a Teacher";
        }

        [HttpPost("AddTeacher")]
        [Authorize(Roles = $"{TS.Roles.Admin},{TS.Roles.Contributor}")]
        public string Add()
        {
            return "Add a Teacher";
        }

        [HttpPut("UpdateTeacher")]
        [Authorize(Roles = TS.Roles.Admin)]
        public string Update()
        {
            return "Update a Teacher";
        }

        [HttpDelete("DeleteTeacher")]
        [Authorize(Roles = TS.Roles.Admin)]
        public string Delete()
        {
            return "Delete a Teacher";
        }
    }
}
