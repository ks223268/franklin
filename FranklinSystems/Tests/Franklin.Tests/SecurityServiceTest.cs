using System;
using Xunit;
using Franklin.Core;

namespace Franklin.Tests {

    public class SecurityServiceTest {

        
        [Fact]
        public void TestValidateLogin_Trader_Pass() {

            ISecurityService svc = GetService();            
            var result = svc.ValidateLogin("sparrow", "blue$ky123");            
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TestValidateLogin_Auditor_Pass() {

            ISecurityService svc = GetService();
            var result = svc.ValidateLogin("jaguar", "forest#789");
            Assert.True(result.IsValid);
        }

        [Fact]
        public void TestValidateLogin_Auditor_Fail() {

            ISecurityService svc = GetService();
            var result = svc.ValidateLogin("jaguar", "xyz#789");
            Assert.True(!result.IsValid);
        }

        private ISecurityService GetService() {
            return new SecurityService();
        }
    }
}
