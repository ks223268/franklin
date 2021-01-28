using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {

    /// <summary>
    /// Confirmation of an or
    /// </summary>
    public class OrderConfirmationModel : BaseModel {

        public int OrderId { get; set; }
        public string OrderGuid { get; set; }

        public OrderConfirmationModel() {                        
            this.Alerts = new List<string>();
        }
    }
}
