using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    public class LoginResultModel : BaseModel{

        public string Token { get; set; }
        public string Role { get; set; }
        
    }
}
