using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Franklin.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Franklin.Web.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class TradingSystemController : ControllerBase {

        ISecurityService _securitySvc;

        public TradingSystemController(ISecurityService diSecuritySvc) {

            _securitySvc = diSecuritySvc;
        }

        /// <summary>
        /// Validate and return a token Guid.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LOGIN")]
        public string Login(string username, string password) {

            string token = string.Empty;

            if ((string.IsNullOrEmpty(username)) || (string.IsNullOrEmpty(password))) {
                Response.StatusCode = (int) System.Net.HttpStatusCode.BadRequest;

            }else if (!_securitySvc.IsValidLogin(username, password, out token)) {
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            }

            return token;
        }

        /// <summary>
        /// Return all orders with a quantity > 0
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GET")]
        public IEnumerable<string> GetAllOrders() {

            return new string[] { "value1", "value2" };
        }

        // POST api/<TradingSystemController>
        [HttpPost]
        [Route("POST")]
        public void SubmitOrder([FromBody] string value) {
        }


        [HttpGet]
        [Route("SELECT")]
        public IActionResult GetTransactions() {

            return new JsonResult("List of transactions");
        }


        // DELETE api/<TradingSystemController>/5
        [HttpDelete]
        [Route("DELETE")]
        public void DeleteOrder(string guid) {

        }
    }
}
