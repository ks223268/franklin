using System;

namespace Franklin.Common {
    public class Util {

        public bool IsEmpty(string val) {

            return (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val));
        }
    }
}
