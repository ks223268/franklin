using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {
    public class SecurityService : ISecurityService {

        // Mock the token values for now.
        const string _traderToken = "11370426-5bfe-4cfb-b5c3-10c2a1238eae";
        const int _traderUserId = 7;
        const string _auditorToken = "1c71270f-59f0-43d5-814d-4891955fc071";
        const int _auditorUserId = 22;


        /// <summary>
        /// Validate login and determine role, token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public LoginResultModel ValidateLogin(string username, string password) {

            LoginResultModel result = new LoginResultModel();            
            
            if (((username.ToLower() == "sparrow") & (password == "blue$ky123"))) {
                result.Role = FranklinSystemRole.Trader;
                result.Token = _traderToken;
                result.IsValid = true;

            } else if ((username.ToLower() == "jaguar") & (password == "forest#789")) {
                result.Role = FranklinSystemRole.Auditor;
                result.Token = _auditorToken;
                result.IsValid = true;
            }
            
            return result;
        }


        /// <summary>
        /// Determine role from token assignment to user.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool HasTraderRole(string token) {

            return _traderToken.Equals(token, StringComparison.OrdinalIgnoreCase);
                    
        }

        public int GetUserId(string token) {

            int userId = 0;

            if (_auditorToken.Equals(token, StringComparison.OrdinalIgnoreCase))
                userId = _auditorUserId;
            else if (_traderToken.Equals(token, StringComparison.OrdinalIgnoreCase))
                userId = _traderUserId;

            return userId;
        }

        /// <summary>
        /// Determine role from token assignment to user.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool HasAuditorRole(string token) {

            return _auditorToken.Equals(token, StringComparison.OrdinalIgnoreCase);
        }


        public bool IsValidToken(string token) {

            return ((Util.IsEmpty(token)) || (_auditorToken.Equals(token, StringComparison.OrdinalIgnoreCase))
                        || (_traderToken.Equals(token, StringComparison.OrdinalIgnoreCase)));
        }

    }
}
