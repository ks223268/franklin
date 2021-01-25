using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Core {

    public interface ISecurityService {

        bool IsValidLogin(string username, string password, out string token);

        bool IsValidToken(string token);
    }
}
