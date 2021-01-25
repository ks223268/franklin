using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {
    public class OrderManagementService : IOrderManagementService {

        // 
        string[] _securities = new string[] { "AA01", "AA02", "AA03", "AA04", "AA05", "AA06", "AA07", "AA08", "AA09", "AA10" };

        /// <summary>
        /// Validate the order request and provide messages.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Returns validation response for user.</returns>
        public OrderResponseModel ValidateOrderRequest(OrderRequestModel order) {

            var response = new OrderResponseModel() {
                Alerts = new List<string>()
            };

            bool isValid = true;

            if (!_securities.Contains(order.SecurityCode, StringComparer.OrdinalIgnoreCase)) {
                response.Alerts.Add("Security code is not valid.");
                isValid = false;
            }

            if ((Util.IsEmpty(order.OrderType))
                               || ((order.OrderType.ToUpper() != OrderTypeCode.IOC.ToString().ToUpper())
                               || (order.OrderType.ToUpper() != OrderTypeCode.GTC.ToString().ToUpper()))) {
                response.Alerts.Add("Order type should be IOC or GTC");
                isValid = false;
            }

            if ((Util.IsEmpty(order.Side))
                               || ((order.Side.ToUpper() != OrderSideCode.BUY.ToString().ToUpper())
                               || (order.Side.ToUpper() != OrderSideCode.SELL.ToString().ToUpper()))) {
                response.Alerts.Add("Order side should be BUY or SELL");
                isValid = false;
            }

            if (order.Quantity <= 0) {
                isValid = false;
                response.Alerts.Add("Quantity should be greater than 0");                
            }

            if (order.Price <= 0) {
                isValid = false;
                response.Alerts.Add("Price should be greater than 0");

            }

            response.IsValid = isValid;

            return response;

        }


        /// <summary>
        /// Submit the order according to the rules specified.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public OrderResponseModel SubmitOrder(OrderRequestModel orderRequest) {
           
            OrderResponseModel response = ValidateOrderRequest(orderRequest);
            if (response.IsValid) {
                // Mock - submit the order and generate a dummy GUID.
                response.OrderConfirmation = new OrderConfirmationModel() {                    
                    OrderGuid = Guid.NewGuid()
                };                
            }

            return response;

        }

        public IList<OrderTransactionModel> GetOrderTransactions(DateTime fromDateTime, DateTime toDateTime) {
            
            IList<OrderTransactionModel> ordersFound = new List<OrderTransactionModel>();

            return ordersFound;
        }


        /// <summary>
        /// Gets all the orders submitted by the trader using the specified token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IList<OrderModel> GetOrdersPerTrader(string token) {
            
            IList<OrderModel> order = new List<OrderModel>();


            //Assumption is that there will only be on trader id fo rnow.

            return order;
        }

        /// <summary>
        /// Cancel the order.
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        public bool CancelOrder(string orderGuid) {

            Guid validGuid;
            if (!Guid.TryParse(orderGuid, out validGuid))
                return false;

            // Cancel the order - Mock that the order is cancelled.
            return true;
        }

    }
}
