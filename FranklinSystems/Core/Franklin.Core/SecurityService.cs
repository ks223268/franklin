using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {
    public class SecurityService : ISecurityService {

        // Mock the token values for now.
        const string _traderToken = "11370426-5bfe-4cfb-b5c3-10c2a1238eae";
        const string _auditorToken = "1c71270f-59f0-43d5-814d-4891955fc071";


        /// <summary>
        /// Validate login and determine role, token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginResultModel ValidateLogin(string username, string password) {

            LoginResultModel result = new LoginResultModel();            
            bool isValid = false;

            if (((username.ToLower() == "sparrow") & (password.ToLower() == "blue$ky123"))) {
                result.Role = FranklinSystemRole.Trader.ToString("g");
                result.Token = _traderToken;
                isValid = true;

            } else if ((username.ToLower() == "jaguar") & (password.ToLower() == "forest#789")) {
                result.Role = FranklinSystemRole.Auditor.ToString("g");
                result.Token = _auditorToken;
                isValid = true;

            }
            result.IsValid = isValid;

            return result;
        }

        /// <summary>
        /// Validate token expected from a trader role.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsValidTraderToken(string token) {

            return _traderToken.Equals(token, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Validate token expected from an auditor role.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsValidAuditorToken(string token) {

            return _auditorToken.Equals(token, StringComparison.OrdinalIgnoreCase);
        }

    }
}
