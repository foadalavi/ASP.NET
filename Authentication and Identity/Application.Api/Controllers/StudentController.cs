using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "CaimBasedPolicy")]
    public class StudentController : ControllerBase
    {
        [HttpGet("GetStudent")]
        //[Authorize(Policy = TS.Policies.ReadPolicy)]
        public string Get()
        {
            return "Get a Student";
        }

        [HttpPost("AddStudent")]
        //[Authorize(Policy = TS.Policies.ReadAndWritePolicy)]
        public string Add()
        {
            return "Add a Student";
        }

        [HttpPut("UpdateStudent")]
        //[Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Update()
        {
            return "Update a Student";
        }

        [HttpDelete("DeleteStudent")]
        //[Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Delete()
        {
            return "Delete a Student";
        }
    }
}
