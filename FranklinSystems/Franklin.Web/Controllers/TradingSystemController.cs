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

        [HttpGet]
        [Route("Info")]
        public IActionResult Info() {
            
            return new JsonResult(_orderSvc.Info);
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
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                result.IsValid = false;
            } else {
                result = _securitySvc.ValidateLogin(username, password);
                if (!result.IsValid)
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
        public IActionResult GetAllOrders(string token) {

            ResponseModel response = new ResponseModel();

            if ((!_securitySvc.IsValidToken(token)) || (!_securitySvc.HasTraderRole(token))) {
                response.Alerts.Add("Invalid token or role.");                
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return new JsonResult(response);

            }

            var orders = _orderSvc.GetOrdersPerTrader(token);
            return new JsonResult(orders);
        }

        // POST api/<TradingSystemController>
        [HttpPost]
        [Route("POST")]
        public IActionResult SubmitOrder(string token, OrderRequestModel newOrder) {

            ResponseModel response = new ResponseModel();

            if ((!_securitySvc.IsValidToken(token)) || (!_securitySvc.HasTraderRole(token))) {
                response.Alerts.Add("Invalid token or role.");
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return new JsonResult(response);

            }
            
            var orderResult = _orderSvc.SubmitOrder(token, newOrder);
            if (!orderResult.IsValid)
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            response.Data = orderResult;

            return new JsonResult(response);

        }


        /// <summary>
        /// This action applies to both roles.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fromDateTime"></param>
        /// <param name="toDateTime"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SELECT")]
        public IActionResult GetTransactions(string token, string fromDateTime, string toDateTime) {

            ResponseModel response = new ResponseModel();

            // This action applies to all roles so just check for the token.
            if ((!_securitySvc.IsValidToken(token)) || (!_securitySvc.HasAuditorRole(token))) {
                response.Alerts.Add("Invalid token....");
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return new JsonResult(response);
                
            }

            //
            DateTime searchFrom, searchTo;                       

            if ((!DateTime.TryParse(fromDateTime, out searchFrom)) || (!DateTime.TryParse(toDateTime, out searchTo))) {
                response.Alerts.Add("One or both the dates are invalid.");
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            } else if (searchTo < searchFrom) {
                response.Alerts.Add("Please specify the date-time range in order.");
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

            } else {
                response.Data = _orderSvc.GetOrderTransactions(searchFrom, searchTo);
            }

            return new JsonResult(response);
        }

        
        // DELETE api/<TradingSystemController>/5
        [HttpDelete]
        [Route("DELETE")]
        public IActionResult CancelOrder(string token, string orderGuid) {

            ResponseModel response = new ResponseModel();

            if ((!_securitySvc.IsValidToken(token)) || (!_securitySvc.HasTraderRole(token))) {
                response.Alerts.Add("Invalid token or role.");
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                return new JsonResult(response);

            }

            if (_orderSvc.CancelOrder(orderGuid)) {
                response.Alerts.Add("Order has been cancelled. Order Guid: " + orderGuid);
            }else {
                response.Alerts.Add("Invalid or non-existant order Guid: " + orderGuid);
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            }

            return new JsonResult(response);

        }
    }
}
