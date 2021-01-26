using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Franklin.Data.Entities {

    /*
     * Note to self:
     * For some reason, if class is not annotated with the table name, DbContext tries to look for the DbSet name in the database.
     * 
     */ 

    [Table("ClientOrder")]
    public class ClientOrder {
                
        [Key]
        public int OrderId { get; set; }
        public Guid OrderGuid { get; set; }
        public int SecurityId { get; set; }
        public string SideCode { get; set; }
        public string TypeCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime  CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }


    }
}
