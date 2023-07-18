using Application.Services.Model.TypeSafe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        [HttpGet("GetModule")]
        [Authorize(Policy = TS.Policies.ReadPolicy)]
        public string Get()
        {
            return "Get a Module";
        }

        [HttpPost("AddModule")]
        [Authorize(Policy = TS.Policies.ReadAndWritePolicy)]
        public string Add()
        {
            return "Add a Module";
        }

        [HttpPut("UpdateModule")]
        [Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Update()
        {
            return "Update a Module";
        }

        [HttpDelete("DeleteModule")]
        [Authorize(Policy = TS.Policies.FullControlPolicy)]
        public string Delete()
        {
            return "Delete a Module";
        }
    }
}
