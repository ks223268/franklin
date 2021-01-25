using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// Order response that may have validation messages or confirmation Guid.
    /// </summary>
    public class ResponseModel : BaseModel {

        /// <summary>
        /// Generic approach to pass any object that is expected to be serialized to Json.
        /// </summary>
        public object Data { get; set; }
        

    }
}
