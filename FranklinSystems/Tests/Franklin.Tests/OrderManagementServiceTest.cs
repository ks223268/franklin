using System;
using System.Collections.Generic;

using Xunit;
using Franklin.Core;
using Franklin.Common.Model;

namespace Franklin.Tests {

    public class OrderManagementServiceTest {


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
            return new OrderManagementService();
        }
    }
}
