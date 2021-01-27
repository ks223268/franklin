using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Franklin.Data.Entities {

    /*
     * Note to self:
     * For some reason, if class is not annotated with the table name, DbContext tries to look for the DbSet name in the database.
     * 
     */ 

    [Table("OrderTransaction")]
    public class OrderTransaction {
                
        [Key]        
        public int Id { get; set; }
      
        public int BuyOrderId { get; set; }
        public int SellOrderId { get; set; }

        public decimal MatchedPrice { get; set; }

        public DateTime  CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
