using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {

    public interface IOrderManagementService {

        public OrderResponseModel ValidateOrderRequest(OrderRequestModel orderRequest);
        public OrderResponseModel SubmitOrder(OrderRequestModel orderRequest);
       
    }
}
