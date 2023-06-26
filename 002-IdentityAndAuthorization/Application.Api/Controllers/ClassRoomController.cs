using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassRoomController : ControllerBase
    {
        [HttpGet("GetClassRoom")]
        [Authorize(Policy =TS.Policies.ReadPolicy)]
        public string Get()
        {
            return "Get a Class Room";
        }

        [HttpPost("AddClassRoom")]
        [Authorize(Policy = TS.Policies.ReadAndWritePolicy)]
        public string Add()
        {
            return "Add a Class Room";
        }

        [HttpPut("UpdateClassRoom")]
        [Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Update()
        {
            return "Update a Class Room";
        }

        [HttpDelete("DeleteClassRoom")]
        [Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Delete()
        {
            return "Delete a Class Room";
        }
    }
}

