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
    public class TransactionController : ControllerBase
    {
        TransactionRepository repo;

        public TransactionController()
        {
            this.repo = new TransactionRepository();
        }

        [HttpGet("get")]
        public ActionResult Get()
        {
            return Ok(this.repo.Get());
        }

        [HttpGet("getwithentities")]
        public ActionResult GetWithEntities()
        {
            return Ok(this.repo.GetWithEntities());
        }
    }
}
