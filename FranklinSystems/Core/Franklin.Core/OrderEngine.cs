using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Franklin.Common;
using Franklin.Common.Model;
using Franklin.Data;
using Franklin.Data.Entities;

//[assembly: InternalsVisibleTo("Franklin.Tests")]
namespace Franklin.Core {

    /// <summary>
    /// This should serve as a single(ton) instance for all modifications related to Orders.
    /// Ensure only one instance of this class exists.
    /// </summary>
    public class OrderEngine : IOrderEngine {

        static object _orderLock = new object();
        IRepository _repo;

        /// <summary>
        /// Cannot inject Repository instance because it is of transient scope, while OrderEngine is singleton.
        /// </summary>
        public IRepository Repository { get { return _repo; } set { _repo = value; } }

        /// <summary>
        /// Create a client order to save the original request.  
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        public int CreateClientOrder(ClientOrder newOrder) {
            
            if (newOrder == null)
                return 0;

            _repo.Create<ClientOrder>(newOrder);
            _repo.Save();

            return newOrder.OrderId;

        }

        /// <summary>
        /// Delete the order from the Book.
        /// </summary>
        /// <param name="orderGuid"></param>
        /// <returns></returns>
        public bool DeleteOrder(Guid orderGuid) {

            // In case orders are being entered, updated.
            lock (_orderLock) {

                using (var transaction = _repo.DbContext.Database.BeginTransaction()) {

                    try {
                        var found = _repo.GetFirst<OrderBookEntry>(obe => obe.OrderGuid == orderGuid);
                        if (found == null)
                            return false;

                        _repo.Delete<OrderBookEntry>(found.EntryId);
                        _repo.Save();
                        transaction.Commit();

                    } catch (Exception exp) {
                        new OrderException("Error while deleting order. Guid: " + orderGuid, exp);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Add GTC order to Book and execute.
        /// </summary>
        /// <param name="newGtcClientOrder"></param>
        /// <returns></returns>
        public Guid ExecuteGtcOrder(ClientOrder newGtcClientOrder) {

            // Create a new book entry based from the client order.
            DateTime now = Util.GetCurrentDateTime();
            Guid newOrderGuid = Guid.NewGuid();

            OrderBookEntry newBookEntry = new OrderBookEntry() {
                OrderId = newGtcClientOrder.OrderId,
                TraderId = newGtcClientOrder.TraderId,
                OrderGuid = newOrderGuid,
                Price = newGtcClientOrder.Price,
                Quantity = newGtcClientOrder.Quantity,
                SecurityId = newGtcClientOrder.SecurityId,
                SideCode = newGtcClientOrder.SideCode,
                TypeCode = newGtcClientOrder.TypeCode,
                StatusCode = OrderStatusCode.New,
                CreatedOn = now,
                ModifiedOn = now
            };

            _repo.Create<OrderBookEntry>(newBookEntry);
            _repo.Save(); // This makes it available to other orders coming in.

            // Lock for request here so it obtains the list of matching orders, which would not be modified by another request.
            lock (_orderLock) {

                // Check for this newly added order book entry again in case, a previous request that just exited the section was able to fill and delete it
                if (_repo.GetFirst<OrderBookEntry>(o => o.EntryId == newBookEntry.EntryId) == null) {
                    return newBookEntry.OrderGuid;
                }

                /*
                 * For price-crossing match, the new order will be matched based on price and the side.
                 * 
                 * So, if I'm buying, match sellers selling at my price or below it .i.e. my buying price >= their sell price
                 * And if I'm selling, match buyers that buy at my price or higher i.e. buyer price >= my selling price
                 * 
                 * In both cases, keep the right-left order i.e. new order price >= matched order price
                */

                string otherSideCode = Util.IsBuySide(newBookEntry.SideCode) == true ? OrderSideCode.Sell : OrderSideCode.Buy;

                var matchedOrders = _repo.GetAll<OrderBookEntry>().Where(oth => (oth.SecurityId == newBookEntry.SecurityId)
                                        & (oth.Quantity > 0) & (newBookEntry.Price >= oth.Price)
                                        & (oth.SideCode == otherSideCode) & (oth.TraderId != newBookEntry.TraderId)) // Don't want to trade against oneself.
                                    .OrderByDescending(o => o.CreatedOn); // FIFO


                bool orderFilled = false;
                int totalFilled = 0;

                // Determine qty available on each order, fill, update and create transaction.
                foreach (OrderBookEntry matchedOrder in matchedOrders) {

                    int quantityFilled;

                    // Buy or Sell - Reduce the quantity as its greater than the new order being filled.
                    if (matchedOrder.Quantity >= newBookEntry.Quantity) {

                        quantityFilled = newBookEntry.Quantity;
                        matchedOrder.Quantity = matchedOrder.Quantity - newBookEntry.Quantity;
                        newBookEntry.Quantity = 0;

                    } else {

                        quantityFilled = matchedOrder.Quantity;
                        //matchedOrder.Quantity = 0;
                        newBookEntry.Quantity = newBookEntry.Quantity - quantityFilled;
                    }

                    // Determine the buy and sell orders to record with the transaction.
                    int buyOrderId = 0;
                    int sellOrderId = 0;

                    if (Util.IsBuySide(newBookEntry.SideCode)) {
                        buyOrderId = newBookEntry.OrderId;
                        sellOrderId = matchedOrder.OrderId;
                    } else {
                        sellOrderId = newBookEntry.OrderId;
                        buyOrderId = matchedOrder.OrderId;
                    }

                    // Ensure that the OrderBook and OrderTransaction updates are one transaction.
                    using (var gtcTransaction = _repo.DbContext.Database.BeginTransaction()) {

                        try {
                            now = Util.GetCurrentDateTime();
                            OrderTransaction orderTransaction = new OrderTransaction() {
                                BuyOrderId = buyOrderId,
                                SellOrderId = sellOrderId,
                                MatchedPrice = matchedOrder.Price,
                                QuantityFilled = quantityFilled,
                                CreatedOn = now,
                                ModifiedOn = now
                            };
                            _repo.Create<OrderTransaction>(orderTransaction);

                        } catch (Exception exp) {
                            string msg = string.Format("Error while creating a transaction for Buy Order #{0} and Sell Order #{1}", buyOrderId, sellOrderId);
                            throw new OrderException(msg, exp);
                        }


                        // Update buy/sell order including quantity and remove order if qty is 0.
                        try {
                            if (matchedOrder.Quantity == 0) {
                                _repo.Delete<OrderBookEntry>(matchedOrder);
                            } else {
                                matchedOrder.ModifiedOn = now;
                                _repo.Update<OrderBookEntry>(matchedOrder);
                            }

                            if (newBookEntry.Quantity == 0) {
                                orderFilled = true;
                                _repo.Delete<OrderBookEntry>(newBookEntry);
                            } else {
                                newBookEntry.ModifiedOn = now;
                                _repo.Update<OrderBookEntry>(newBookEntry);
                            }
                        } catch (Exception exp) {
                            string msg = string.Format("Error while updating the Order Book entries for Buy Order #{0} and Sell Order #{1}", buyOrderId, sellOrderId);
                            throw new OrderException(msg, exp);
                        }

                        //
                        totalFilled += quantityFilled; // Track the total securities filled.

                        _repo.Save();
                        gtcTransaction.Commit();
                    }

                    // If new order filled, no need to go through the others.
                    if (orderFilled)
                        break;

                }

                // After going through all the matched orders, update the GTC Client Order only if it was filled total or partially.
                if (totalFilled > 0) {
                    newGtcClientOrder.FilledQuantity = totalFilled; // This could be reconciled with the transactions.
                    newGtcClientOrder.ModifiedOn = Util.GetCurrentDateTime();
                    _repo.Update<ClientOrder>(newGtcClientOrder);
                    _repo.Save();
                }

                return newOrderGuid;
            }

        }

        /// <summary>
        /// Execute IOC order against book.
        /// </summary>
        /// <param name="newIocClientOrder"></param>
        public void ExecuteIocOrder(ClientOrder newIocClientOrder) {

            // Get lock for request so it obtains the list of matching orders, which would not be modified by another request.
            lock (_orderLock) {

                string otherSideCode = Util.IsBuySide(newIocClientOrder.SideCode) == true ? OrderSideCode.Sell : OrderSideCode.Buy;

                /*
                 * For price-crossing match, the new order will be matched based on price and the side.
                 * 
                 * So, if I'm buying, match sellers selling at my price or below it .i.e. my buying price >= their sell price
                 * And if I'm selling, match buyers that buy at my price or higher i.e. buyer price >= my selling price
                 * 
                 * In both cases, keep the right-left order i.e. new order price >= matched order price
                */
                var matchedOrders = _repo.GetAll<OrderBookEntry>().Where(oth => (oth.SecurityId == newIocClientOrder.SecurityId) 
                                    & (oth.Quantity > 0) & (newIocClientOrder.Price >= oth.Price)
                                    & (oth.SideCode == otherSideCode) & (oth.TraderId != newIocClientOrder.TraderId)) // Don't want to trade against oneself.
                                .OrderByDescending(o => o.CreatedOn); // FIFO

                bool orderFilled = false;
                int totalFilled = 0;
                int totalQty = newIocClientOrder.Quantity;

                // Determine qty available on each order, fill, update and create transaction.
                foreach (OrderBookEntry matchedOrder in matchedOrders) {

                    int quantityFilled;

                    // Buy or Sell - Reduce the quantity as its greater than the new order being filled.
                    if (matchedOrder.Quantity >= newIocClientOrder.Quantity) {

                        quantityFilled = newIocClientOrder.Quantity;
                        matchedOrder.Quantity = matchedOrder.Quantity - newIocClientOrder.Quantity;
                        // Don't update qty for the IOC order to preserve the original quantity as part of client order.
                    } else {

                        quantityFilled = matchedOrder.Quantity; // Note the quantity before it is updated to reflect the sale.
                        matchedOrder.Quantity = 0;
                        totalQty = totalQty - quantityFilled;
                    }

                    // Determine the buy and sell orders to record with the transaction.
                    int buyOrderId = 0;
                    int sellOrderId = 0;

                    if (Util.IsBuySide(newIocClientOrder.SideCode)) {
                        buyOrderId = newIocClientOrder.OrderId;
                        sellOrderId = matchedOrder.OrderId;
                    } else {
                        sellOrderId = newIocClientOrder.OrderId;
                        buyOrderId = matchedOrder.OrderId;
                    }

                    // Ensure that the OrderBook entry for the one side and OrderTransaction updates are one transaction.
                    using (var iocTransaction = _repo.DbContext.Database.BeginTransaction()) {

                        DateTime now = Util.GetCurrentDateTime();
                        try {

                            OrderTransaction orderTransaction = new OrderTransaction() {
                                BuyOrderId = buyOrderId,
                                SellOrderId = sellOrderId,
                                MatchedPrice = matchedOrder.Price,
                                QuantityFilled = quantityFilled,
                                CreatedOn = now,
                                ModifiedOn = now
                            };
                            _repo.Create<OrderTransaction>(orderTransaction);

                        } catch (Exception exp) {
                            string msg = string.Format("Error while creating a transaction for Buy Order #{0} and Sell Order #{1}", buyOrderId, sellOrderId);
                            throw new OrderException(msg, exp);
                        }

                        try {
                            // Update Order Book entry or remove based on quantity being 0.
                            if (matchedOrder.Quantity == 0) {
                                _repo.Delete<OrderBookEntry>(matchedOrder);
                            } else {
                                matchedOrder.ModifiedOn = now;
                                _repo.Update<OrderBookEntry>(matchedOrder);
                            }
                        } catch (Exception exp) {
                            string msg = string.Format("Error updating the Order Book entry for Order #{1}", matchedOrder.OrderId);
                            throw new OrderException(msg, exp);
                        }

                        // Since this is an IOC, there is not book entry to update or delete. If filled, exit.
                        orderFilled = (newIocClientOrder.Quantity == quantityFilled);
                        totalFilled += quantityFilled; // Track the total securities filled.

                        _repo.Save();
                        iocTransaction.Commit();

                    }

                    // If new order filled, no need to go through the others.
                    if (orderFilled)
                        break;
                }

                // After going through all the matched orders, update the IOC Client Order only if it was filled total or partially.
                if (totalFilled > 0) {                    
                    newIocClientOrder.FilledQuantity = totalFilled; // This could be reconciled with the transactions.
                    newIocClientOrder.ModifiedOn = Util.GetCurrentDateTime();
                    _repo.Update<ClientOrder>(newIocClientOrder);
                    _repo.Save();
                }

            }

        }

    }
}
