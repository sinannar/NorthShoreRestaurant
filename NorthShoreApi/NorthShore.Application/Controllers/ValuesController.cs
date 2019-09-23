using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NorthShore.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetValues()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
