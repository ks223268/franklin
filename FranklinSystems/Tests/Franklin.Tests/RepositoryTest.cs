using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Franklin.Data;
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

            DateTime now = DateTime.Now;
            ClientOrder order = new ClientOrder() {
                OrderGuid = Guid.NewGuid(),
                CreatedOn = now,
                ModifiedOn = now,
                SecurityId = security.Id,
                Price = 23.45m,
                Quantity = 25,
                SideCode = "BUY",
                TypeCode = "IOC",
            };

            _repo.Create<ClientOrder>(order);
            _repo.Save();

            Assert.True(order.OrderGuid != null);
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

    }
}
