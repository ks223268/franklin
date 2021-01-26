using System;

namespace Franklin.Common {
    public class Util {

        public static bool IsEmpty(string val) {

            return (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val));
        }

        /// <summary>
        /// Wrapper to flip this to UTC if needed.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentDateTime() {

            return DateTime.Now;
        }
    }
}
