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

        public static bool IsBuySide(string side) {
            return side.ToUpper() == OrderSideCode.Buy.ToUpper();
        }

        public static bool IsSellSide(string side) {
            return side.ToUpper() == OrderSideCode.Sell.ToUpper();
        }

        public static bool IsOrderGtc(string orderType) {
            return orderType.ToUpper() == OrderTypeCode.Gtc.ToUpper();
        }

        public static bool IsOrderIoc(string orderType) {
            return orderType.ToUpper() == OrderTypeCode.Ioc.ToUpper();
        }

    }
}
