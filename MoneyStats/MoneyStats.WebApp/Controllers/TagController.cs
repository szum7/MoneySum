using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        [HttpGet("testcall")]
        public ActionResult TestCall()
        {
            return Ok(123);
        }

        [HttpGet("gettags")]
        public ActionResult GetTags()
        {
            var repo = new TagRepository();
            return Ok(repo.Get());
        }
    }
}
