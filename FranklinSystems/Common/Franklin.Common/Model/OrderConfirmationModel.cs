using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {

    /// <summary>
    /// Order details.
    /// </summary>
    public class OrderConfirmationModel {

        public int OrderId { get; set; }
        public string OrderGuid { get; set; }

    }
}
