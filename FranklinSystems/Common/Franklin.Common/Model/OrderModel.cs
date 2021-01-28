using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// Order book entry.
    /// </summary>
    public class OrderModel {

        public string OrderGuid { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public string SecurityCode { get; set; }
        public string Side { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
