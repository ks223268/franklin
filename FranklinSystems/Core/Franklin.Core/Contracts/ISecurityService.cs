using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {

    public interface ISecurityService {

        public LoginResultModel ValidateLogin(string username, string password);

        bool IsValidToken(string token);

        bool HasTraderRole(string token);

        bool HasAuditorRole(string token);

        int GetUserId(string token);

    }
}
