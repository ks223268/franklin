using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Franklin.Data.Entities {

    /*
     * Note to self:
     * For some reason, if class is not annotated with the table name, DbContext tries to look for the DbSet name in the database.
     * 
     */ 

    [Table("MarketSecurity")]
    public class MarketSecurity {
        
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
