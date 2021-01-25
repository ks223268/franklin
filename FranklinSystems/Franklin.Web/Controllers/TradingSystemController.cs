using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Franklin.Core;
using Franklin.Common;
using Franklin.Common.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Franklin.Web.Controllers {

    [Route("[controller]")]
    [ApiController]
    public class TradingSystemController : ControllerBase {

        ISecurityService _securitySvc;
        IOrderManagementService _orderSvc;

        public TradingSystemController(ISecurityService diSecuritySvc, IOrderManagementService diOms) {

            _securitySvc = diSecuritySvc;
            _orderSvc = diOms;
        }

        /// <summary>
        /// Validate and return a token Guid.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LOGIN")]
        public IActionResult Login(string username, string password) {

            LoginResultModel result = new LoginResultModel();

            if ((string.IsNullOrEmpty(username)) || (string.IsNullOrEmpty(password))) {
                Response.StatusCode = (int) System.Net.HttpStatusCode.BadRequest;
                result.IsValid = false;
            }else {
                result = _securitySvc.ValidateLogin(username, password);
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                
            }
            
            return new JsonResult(result);
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
        public IActionResult SubmitOrder(OrderRequestModel newOrder) {

            var response = _orderSvc.SubmitOrder(newOrder);
            if (!response.IsValid)
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            return new JsonResult(response);

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
