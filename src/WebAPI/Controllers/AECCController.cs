using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AECCWebAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class AECCController : Controller
    {
        private IHostingEnvironment _env;
        public AECCController(IHostingEnvironment env)
        {
            _env = env;
        }
     
        [HttpGet]
        public IActionResult Get()
        {
            return View("Index");
          //  return "Service is up and running";
        }

  

    }
}
