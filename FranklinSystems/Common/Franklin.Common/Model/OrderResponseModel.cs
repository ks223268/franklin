using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// Order response that may have validation messages or confirmation Guid.
    /// </summary>
    public class OrderResponseModel : BaseModel {

        public OrderConfirmationModel OrderConfirmation { get; set; }

    }
}
