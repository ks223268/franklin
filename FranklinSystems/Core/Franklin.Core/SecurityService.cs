using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Core {
    public class SecurityService : ISecurityService {

        /// <summary>
        /// Validate and set authenticated token.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsValidLogin(string username, string password, out string token) {

            token = string.Empty;
            bool isValid = ((username.ToLower() == "sparrow") & (password.ToLower() == "bluesky"));
            if (isValid) token = Guid.NewGuid().ToString();

            return isValid;
        }

        /// <summary>
        /// Check the token for validity and expiration.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsValidToken(string token) {
            
            return true; // For now.
        }
    }
}
