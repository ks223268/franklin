using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Franklin.Data;
using Franklin.Common;
using Franklin.Data.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace Franklin.Tests {

    public class RepositoryTest {

        IRepository _repo;

        public RepositoryTest() {
            string connectionString = TestHelper.GetIConfigurationRoot(AppContext.BaseDirectory)
                                                .GetConnectionString("FranklinDbContext");

            //_repo = new Repository(connectionString);
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);
            //DbContextOptions opt = builder.Options;
            FranklinDbContext franklinDbContext = new FranklinDbContext(builder.Options);
            _repo = new Repository(franklinDbContext);
        }

        [Fact]
        public void Test_GetSecurities_Pass() {

            var securities = _repo.GetAll<MarketSecurity>();                                   
            Assert.True(securities.Count() == 10);
        }

        [Fact]
        public void Test_CreateClientOrder_Pass() {

            var security = _repo.GetFirst<MarketSecurity>(s => s.Code.ToUpper() == "AA04");

            DateTime now = Util.GetCurrentDateTime();
            ClientOrder order = new ClientOrder() {                
                TraderId = Constants.TraderUserId,
                CreatedOn = now,
                ModifiedOn = now,
                SecurityId = security.Id,                
                Price = 22.11m,
                Quantity = 21,
                SideCode = "SELL",
                TypeCode = "GTC",
            };

            _repo.Create<ClientOrder>(order);
            _repo.Save();

            Assert.True(order.OrderId > 0);
        }


        [Fact]
        public void Test_CreateTransaction_Pass() {

            DateTime now = Util.GetCurrentDateTime();
            OrderTransaction order = new OrderTransaction() {
                BuyOrderId = 1,
                SellOrderId = 2,
                QuantityFilled = 11,
                MatchedPrice = 12.60m,
                CreatedOn = now,
                ModifiedOn = now,
            };

            _repo.Create<OrderTransaction>(order);
            _repo.Save();

            Assert.True(order.Id > 0);
        }

        [Fact]
        public void Test_UpdateClientOrder_Pass() {

            var order = _repo.GetFirst<ClientOrder>(o => o.OrderId == 1);

            DateTime now = DateTime.Now;            
            order.ModifiedOn = DateTime.Now;
            order.Quantity = 10;
            
            _repo.Update<ClientOrder>(order);
            _repo.Save();
        }

        [Fact]
        public void Test_CreateOrderBookEntry_Pass() {

            DateTime now = Util.GetCurrentDateTime();
            OrderBookEntry bookEntry = new OrderBookEntry() {
                OrderGuid = Guid.NewGuid(),
                TraderId = Constants.TraderUserId,
                CreatedOn = now,
                ModifiedOn = now,
                SecurityId = 1,
                Price = 22.11m,
                Quantity = 21,
                SideCode = "SELL",
                TypeCode = "GTC",
                StatusCode = OrderStatusCode.New,               
            };

            _repo.Create<OrderBookEntry>(bookEntry);
            _repo.Save();

            Assert.True(bookEntry.EntryId > 0);
        }

       
    }
}
