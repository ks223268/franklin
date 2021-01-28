using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Core {

    public class OrderException : Exception {
        public OrderException() {
        }

        public OrderException(string message)
            : base(message) {
        }

        public OrderException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}