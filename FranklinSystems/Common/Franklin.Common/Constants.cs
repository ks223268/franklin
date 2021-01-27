using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common {


    public static class Constants {
        public const int TraderUserId = 1;
        public const int AuditorUserId = 2;
    }

    public struct FranklinSystemRole {
        public const string Trader = "Trader";
        public const string Auditor = "Auditor";
    }

    // Note: The member values end up in the db.
    public struct OrderTypeCode {
        public const string Ioc = "IOC";
        public const string Gtc = "GTC";
    }

    public struct OrderSideCode {
        public const string Buy = "BUY";
        public const string Sell = "SELL";
    }

    public struct OrderStatusCode {
        public const string New = "N";
        public const string Filled = "F";
        public const string PartialFilled = "P";
        public const string Cancelled = "C";
    }
}
