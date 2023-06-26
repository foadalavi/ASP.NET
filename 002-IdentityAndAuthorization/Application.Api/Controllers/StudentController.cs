using Application.Services.Infrastructure.Authorizaion;
using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = TS.Policies.GenericPolicy)]
    //[CustomAuthorize]
    public class StudentController : ControllerBase
    {
        [HttpGet("GetStudent")]
        public string Get()
        {
            return "Get a Student";
        }

        [HttpPost("AddStudent")]
        public string Add()
        {
            return "Add a Student";
        }

        [HttpPut("UpdateStudent")]
        public string Update()
        {
            return "Update a Student";
        }

        [HttpDelete("DeleteStudent")]
        public string Delete()
        {
            return "Delete a Student";
        }
    }
}
