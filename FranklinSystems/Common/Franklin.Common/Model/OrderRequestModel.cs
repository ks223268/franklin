using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// This is the order submitted by the trader.
    /// </summary>
    public class OrderRequestModel  {

        public string SecurityCode { get; set; }
        public string Side { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
