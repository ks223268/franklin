using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Franklin.Common;

namespace Franklin.Data.Entities {

    /// <summary>
    /// Order book has duplicate columns compared to the [ClientOrder] for lesser joins.
    /// And the records get deleted once order is filled completely.
    /// </summary>
    [Table("OrderBook")]
    public class OrderBookEntry {
                
        [Key]        
        public int EntryId { get; set; }        
        public int OrderId { get; set; }        
        public Guid OrderGuid { get; set; }
        public int TraderId { get; set; }

        public int SecurityId { get; set; }
        public string SideCode { get; set; }
        public string TypeCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string StatusCode { get; set; }
        public DateTime  CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public OrderBookEntry() {
            this.StatusCode = OrderStatusCode.New;
        }

    }
}
