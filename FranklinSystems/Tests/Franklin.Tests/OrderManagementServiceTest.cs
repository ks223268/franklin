using System;
using Xunit;
using Franklin.Core;
using Franklin.Common.Model;
using Franklin.Data;
using Franklin.Data.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Franklin.Tests {

    public class OrderManagementServiceTest {

        IRepository _repo;

        public OrderManagementServiceTest() {

            // Setup similar to the Startup configuration.
            string connectionString = TestHelper.GetIConfigurationRoot(AppContext.BaseDirectory)
                                                .GetConnectionString("FranklinDbContext");

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(connectionString);            
            FranklinDbContext franklinDbContext = new FranklinDbContext(builder.Options);
            _repo = new Repository(franklinDbContext);
        }

        [Fact]
        public void TestValidateOrderRequest_Security_Fail() {

            IOrderManagementService svc = GetService();

            OrderRequestModel invalidOrder = new OrderRequestModel() {
               SecurityCode = "AA05x",
               OrderType = "IOC",
               Side = "Buy",
               Price = 23.11m,
               Quantity = 100,              
            };

            var result = svc.ValidateOrderRequest(invalidOrder);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void TestValidateOrderRequest_Security_Pass() {

            IOrderManagementService svc = GetService();

            OrderRequestModel invalidOrder = new OrderRequestModel() {
                SecurityCode = "AA10",
                OrderType = "GTc",
                Side = "BUy",
                Price = 4.11m,
                Quantity = 200,
            };

            var result = svc.ValidateOrderRequest(invalidOrder);
            Assert.False(result.IsValid);
        }

        private IOrderManagementService GetService() {            
            return new OrderManagementService(_repo);
        }
    }
}
