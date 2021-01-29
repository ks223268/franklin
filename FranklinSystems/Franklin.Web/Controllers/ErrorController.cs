using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Franklin.Web.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase {

        [HttpGet]
       [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
