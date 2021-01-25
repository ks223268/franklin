using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    public class BaseModel {

        public bool IsValid { get; set; }

        public string Status { get; set; }

        public IList<string> Alerts { get; set; }
    }
}
