using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// Order submitted by the user.
    /// </summary>
    public class OrderModel {

        public DateTime SubmittedOn { get; set; }
        public DateTime ExecutedOn { get; set; }

        public string SecurityCode { get; set; }
        public string Side { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
