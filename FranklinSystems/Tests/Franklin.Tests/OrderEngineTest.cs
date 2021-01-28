using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Franklin.Core;
using Franklin.Data;
using Franklin.Common;
using Franklin.Data.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using Moq;

namespace Franklin.Tests {
    
    /// <summary>
    /// Test against a database as it involves transactions.
    /// Keep Price odd and quantity even for easy spotting.
    /// </summary>

    public class OrderEngineTest {

        IRepository _repo;
        IOrderEngine _engine;
        int _traderA = 7;
        int _traderB = 8;
        int _traderC = 10;

        int _tg1_securityId = 3; // Test group 1

        public OrderEngineTest() {

            _repo = GetRepository();
            _engine = new OrderEngine() {
                Repository = _repo
            };
        }

        /// <summary>
        /// Test creation of client order and use for other tests below.        
        /// </summary>
        //[Fact(Skip = "Tested as part of another method.")]
        [Fact]
        public void Test_CreateClientOrder_GTC_Pass() {

            var gtcOrder = new ClientOrder() {
                TraderId = _traderA,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = 2,
                Price = 55,
                Quantity = 24,
                SideCode = OrderSideCode.Sell,
                TypeCode = OrderTypeCode.Gtc,
            };       

            // Save and execute
            _engine.CreateClientOrder(gtcOrder);
                        
            Assert.True(gtcOrder.OrderId > 0);
        }

        /// <summary>
        /// Execute GTC orders for the same trader.        
        /// </summary>
        //[Fact(Skip = "Tested as part of another method.")]
        [Fact]
        public void Test_ExecuteGtcOrder_GTC_Pass() {

            var gtcOrder1 = new ClientOrder() {
                TraderId = _traderA,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = 2,
                Price = 55,
                Quantity = 24,
                SideCode = OrderSideCode.Sell,
                TypeCode = OrderTypeCode.Gtc,
            };

            var gtcOrder2 = new ClientOrder() {
                TraderId = _traderA,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = 2,
                Price = 77.50m,
                Quantity = 100,
                SideCode = OrderSideCode.Buy,
                TypeCode = OrderTypeCode.Gtc,
            };

            // Save and execute
            _engine.CreateClientOrder(gtcOrder1);
            _engine.CreateClientOrder(gtcOrder2);

            Guid bookEntry1 = _engine.ExecuteGtcOrder(gtcOrder1);
            Guid bookEntry2 = _engine.ExecuteGtcOrder(gtcOrder2);

            Assert.True(bookEntry1 != Guid.Empty);
            Assert.True(bookEntry2 != Guid.Empty);
        }

        [Fact(Skip = "Tested as part of another method.")]
        public void Test_CreateClientOrder_IOC_Pass() {

            var iocOrder = new ClientOrder() {
                TraderId = _traderA,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = 2,
                Price = 27.80m,
                Quantity = 10,
                SideCode = OrderSideCode.Sell,
                TypeCode = OrderTypeCode.Ioc,
            };

            // Save and execute
            _engine.CreateClientOrder(iocOrder);

            Assert.True(iocOrder.OrderId > 0);
        }

        //[Fact]
        //public void Test_PriceMatch_Buy() {

        //    int securityId = 3;
        //    decimal buyPrice = 15;
        //    string side = OrderSideCode.Sell;


        //    var matchedOrders = _repo.GetAll<OrderBookEntry>().Where(oth => (oth.SecurityId == securityId)
        //                & (oth.Quantity > 0) & (buyPrice >= oth.Price)
        //                & (oth.SideCode == side) & (oth.TraderId != _traderA)) // Don't want to trade against oneself.
        //            .ToList();

        //    Assert.True(matchedOrders.Count() > 0);
        //}

        /// <summary>
        /// Execut A GTC sell matched with IOC buy.
        /// </summary>
        [Fact]
        public void Test_1_ExecuteIoc_GTC() {
            
            var gtcOrder = new ClientOrder() {
                TraderId = _traderA,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = _tg1_securityId,
                Price = 13,
                Quantity = 50,
                SideCode = OrderSideCode.Sell,
                TypeCode = OrderTypeCode.Gtc,
            };

            var iocOrder = new ClientOrder() {
                TraderId = _traderB,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = _tg1_securityId,
                Price = 15,
                Quantity = 10,
                SideCode = OrderSideCode.Buy,
                TypeCode = OrderTypeCode.Ioc,
            };

            // Save and execute
            _engine.CreateClientOrder(gtcOrder);
            _engine.CreateClientOrder(iocOrder);
            _engine.ExecuteGtcOrder(gtcOrder); // Execute so it ends up in the book

            _engine.ExecuteIocOrder(iocOrder);

            // Verify transaction
            var iocTran = _repo.GetFirst<OrderTransaction>(ot => ot.BuyOrderId == iocOrder.OrderId);
            Assert.True(iocTran != null);
            Assert.True(iocTran.MatchedPrice == gtcOrder.Price);
            Assert.True(iocTran.QuantityFilled == iocOrder.Quantity);

            // Verify GTC book entry
            var gtcBook = _repo.GetFirst<OrderBookEntry>(ob => ob.OrderId == gtcOrder.OrderId);            
            Assert.True(gtcBook.Quantity == 40);
        }

        /// <summary>
        /// Continuation of Test_ExecuteIoc_GTC()
        /// </summary>
        [Fact]
        public void Test_1_ExecuteGtcOrder_Pass() {

            var gtcOrder1 = new ClientOrder() {
                TraderId = _traderC,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = _tg1_securityId,
                Price = 9,
                Quantity = 60,
                SideCode = OrderSideCode.Buy,
                TypeCode = OrderTypeCode.Gtc,
            };
            var gtcOrder2 = new ClientOrder() {
                TraderId = _traderC,
                CreatedOn = Util.GetCurrentDateTime(),
                ModifiedOn = Util.GetCurrentDateTime(),
                SecurityId = _tg1_securityId,
                Price = 21,
                Quantity = 60,
                SideCode = OrderSideCode.Buy,
                TypeCode = OrderTypeCode.Gtc,
            };
            // Save and execute
            _engine.CreateClientOrder(gtcOrder1);
            _engine.CreateClientOrder(gtcOrder2);
            _engine.ExecuteGtcOrder(gtcOrder1);
            Guid guid2 = _engine.ExecuteGtcOrder(gtcOrder2);

            var tran = _repo.GetFirst<OrderBookEntry>(ob => ob.OrderGuid == guid2);

            Assert.True(tran.Quantity == 20);
        }


        [Fact]
        public void Test_DeleteOrder_Fail() {

            bool retVal = _engine.DeleteOrder(Guid.NewGuid());

            Assert.False(retVal);
        }

        private IRepository GetRepository() {

            string connectionString = TestHelper.GetIConfigurationRoot(AppContext.BaseDirectory)
                                                .GetConnectionString("FranklinDbContext");
            
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);            
            FranklinDbContext franklinDbContext = new FranklinDbContext(builder.Options);
            return new Repository(franklinDbContext);

            //Todo: Attempt to mock the db context and specially transaction.
            //
        }


    }

}
