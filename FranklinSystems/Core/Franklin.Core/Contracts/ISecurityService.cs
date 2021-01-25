using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {

    public interface ISecurityService {

        public LoginResultModel ValidateLogin(string username, string password);

        bool IsValidTraderToken(string token);
        bool IsValidAuditorToken(string token);
    }
}
