using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace CQRS.Database.Controllers
{
    [ApiController, Route("v1/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
            => await Task.Run(() => Ok("pong"));

        [HttpGet, Route("gc")]
        public IActionResult GCCollector()
        {
            GC.Collect();
            return Ok();
        }
    }
}
