using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

using Franklin.Common;
using Franklin.Common.Model;
using Franklin.Data;
using Franklin.Data.Entities;

namespace Franklin.Core {

 
    public class OrderManagementService : IOrderManagementService {

        IRepository _repo;
        IOrderEngine _orderEngine;
        ISecurityService _securitySvc;

        //string[] _securities = new string[] { "AA01", "AA02", "AA03", "AA04", "AA05", "AA06", "AA07", "AA08", "AA09", "AA10" };

        List<MarketSecurity> _securities;
        IDictionary<string, string> _info;

        public OrderManagementService(IRepository diRepo, IOrderEngine diOrderEngine,
                                        ISecurityService diSecuritySvc) {

            _securitySvc = diSecuritySvc;
            _repo = diRepo;
            _orderEngine = diOrderEngine;
            _orderEngine.Repository = diRepo;
            
            _securities = _repo.GetAll<MarketSecurity>().ToList();  

            //
            _info = new Dictionary<string, string>();
            _info.Add("OrderMgmt", this.GetHashCode().ToString());
            _info.Add("Engine", _orderEngine.GetHashCode().ToString());
        }

        public IDictionary<string, string> Info { get { return _info; } }

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

            if (_securities.FirstOrDefault(s => order.SecurityCode.Equals(s.Code, StringComparison.OrdinalIgnoreCase)) == null) {
                response.Alerts.Add("Security code is not valid.");
                isValid = false;
            }

            if ((Util.IsEmpty(order.OrderType)) & (!Util.IsOrderIoc(order.OrderType))
                                                & (!Util.IsOrderGtc(order.OrderType))) {
                response.Alerts.Add("Order type should be IOC or GTC");
                isValid = false;
            }

            if ((Util.IsEmpty(order.Side)) & (!Util.IsBuySide(order.Side))
                                            & (!Util.IsSellSide(order.Side))) {
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

        public IEnumerable<OrderTransactionModel> GetOrderTransactions(DateTime fromDateTime, DateTime toDateTime) {

            var ordersFound = _repo.GetAll<OrderTransaction>()
                .Where(ob => ob.ModifiedOn >= fromDateTime & ob.ModifiedOn <= toDateTime)
            
                .Select(ob => new OrderTransactionModel() {
                    Id = ob.Id,
                    BuyOrderId = ob.BuyOrderId,
                    SellOrderId = ob.SellOrderId,                    
                    MatchedPrice = ob.MatchedPrice,
                    QuantityFilled= ob.QuantityFilled,
                    CreatedOn = ob.CreatedOn,
                    ModifiedOn = ob.ModifiedOn
                });                                   

            return ordersFound;
        }


        /// <summary>
        /// Gets all the orders submitted by the trader using the specified token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IEnumerable<OrderModel> GetOrdersPerTrader(string token) {

            int traderId = _securitySvc.GetUserId(token);
            if (traderId <= 0)
                throw new OrderException("User not found.");
            
            var bookOrders = _repo.GetAll<OrderBookEntry>().Where(ob => (ob.TraderId == traderId) && (ob.Quantity > 0))
                .Select(ob => new OrderModel() {
                    OrderGuid = ob.OrderGuid.ToString(),
                    SecurityCode = _securities.Find(s => s.Id == ob.SecurityId).Code,
                    Price = ob.Price,
                    Quantity = ob.Quantity,
                    OrderType = ob.TypeCode,
                    Side = ob.SideCode,
                    CreatedOn = ob.CreatedOn,
                    ModifiedOn = ob.ModifiedOn
                });
                                
            return bookOrders;
        }

        /// <summary>
        /// Validate, create client order and order book entries as per rules.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public OrderResponseModel SubmitOrder(string token, OrderRequestModel orderRequest) {

            OrderResponseModel response = ValidateOrderRequest(orderRequest);
            if (!response.IsValid)
                return response;

            int traderId = _securitySvc.GetUserId(token);
            if (traderId <= 0)
                throw new OrderException("User not found.");

            var security = _repo.GetFirst<MarketSecurity>(s => s.Code == orderRequest.SecurityCode);
            DateTime now = Util.GetCurrentDateTime();
            var newOrder = new ClientOrder() {
                TraderId = traderId,
                CreatedOn = now,
                ModifiedOn = now,
                SecurityId = security.Id,
                Price = orderRequest.Price,
                Quantity = orderRequest.Quantity,
                SideCode = orderRequest.Side,
                TypeCode = orderRequest.OrderType,
            };

            // Create a client order to save the original request.  

            try {
                _orderEngine.CreateClientOrder(newOrder);                
            } catch (Exception exp) {
                // Log exp and throw with some order details, time etc.
                throw new OrderException("Error while creating client order for Trader Id # " + traderId, exp);

            }

            // Place order
            string newOrderGuid = string.Empty;
            if (Util.IsOrderGtc(orderRequest.OrderType)) {

                newOrderGuid = _orderEngine.ExecuteGtcOrder(newOrder).ToString();

            } else if (Util.IsOrderIoc(orderRequest.OrderType)) {

                _orderEngine.ExecuteIocOrder(newOrder);
            }

            response.OrderConfirmation = new OrderConfirmationModel() {
                IsValid = true,
                OrderId = newOrder.OrderId,
                OrderGuid = newOrderGuid
            };

            return response;
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

            return _orderEngine.DeleteOrder(validGuid);

        }

    }
}
