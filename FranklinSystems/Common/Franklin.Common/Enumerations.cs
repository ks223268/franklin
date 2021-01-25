using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common {
    
    public enum FranklinSystemRole {
        Trader,
        Auditor
    }

    public enum OrderTypeCode { 
        IOC,
        GTC 
    }

    public enum OrderSideCode {
        BUY,
        SELL
    }

}
