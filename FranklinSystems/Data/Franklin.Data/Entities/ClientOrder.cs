using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Franklin.Common;

namespace Franklin.Data.Entities {

    /// <summary>
    /// Represents the orignal order placed.
    /// </summary>
    [Table("ClientOrder")]
    public class ClientOrder {
                
        [Key]        
        public int OrderId { get; set; }        
        public int TraderId { get; set; }

        public int SecurityId { get; set; }
        public string SideCode { get; set; }
        public string TypeCode { get; set; }
        public int Quantity { get; set; }
        public int FilledQuantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        public DateTime  CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
