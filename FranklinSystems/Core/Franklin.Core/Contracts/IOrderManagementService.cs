using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;

namespace Franklin.Core {

    public interface IOrderManagementService {

        OrderResponseModel ValidateOrderRequest(OrderRequestModel orderRequest);
        OrderResponseModel SubmitOrder(OrderRequestModel orderRequest);
        IList<OrderTransactionModel> GetOrderTransactions(DateTime fromDateTime, DateTime toDateTime);
        IList<OrderModel> GetOrdersPerTrader(string token);
        bool CancelOrder(string orderGuid);

    }
}
