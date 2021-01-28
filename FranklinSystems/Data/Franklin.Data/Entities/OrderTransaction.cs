using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Franklin.Data.Entities {

   
    /// <summary>
    /// Transactions made as per trade.
    /// </summary>
    [Table("OrderTransaction")]
    public class OrderTransaction {
                
        [Key]        
        public int Id { get; set; }
      
        public int BuyOrderId { get; set; }
        public int SellOrderId { get; set; }
        public int QuantityFilled { get; set; }

        public decimal MatchedPrice { get; set; }

        public DateTime  CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
