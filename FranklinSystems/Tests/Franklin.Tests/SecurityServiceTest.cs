using System;
using Xunit;
using Franklin.Core;

namespace Franklin.Tests {

    public class SecurityServiceTest {

        [Fact]
        public void TestIsValidLogin_Pass() {

            SecurityService svc = new SecurityService();
            string token;
            
            svc.IsValidLogin("sparrow", "bluesky", out token);
            
            Assert.True(token.Length > 0);

        }
    }
}
