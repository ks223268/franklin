using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// Represents committed order transactions from the ledger.
    /// </summary>
    public class OrderTransactionModel {

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public int BuyOrderId { get; set; }
        public int SellOrderId { get; set; }
        public int QuantityFilled { get; set; }
        public decimal MatchedPrice { get; set; }
        /*
        public string SecurityCode { get; set; }
        public string Side { get; set; }
        public string OrderType { get; set; }
        */


    }
}
