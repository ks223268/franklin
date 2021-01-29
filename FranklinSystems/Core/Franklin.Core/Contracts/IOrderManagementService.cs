using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Franklin.Common;
using Franklin.Common.Model;
using Franklin.Data.Entities;

namespace Franklin.Core {

    public interface IOrderManagementService {

        IDictionary<string, string> Info { get; }

        OrderResponseModel ValidateOrderRequest(OrderRequestModel orderRequest);
        Task<OrderResponseModel> SubmitOrder(string token, OrderRequestModel orderRequest);
        IEnumerable<OrderTransactionModel> GetOrderTransactions(DateTime fromDateTime, DateTime toDateTime);
        IEnumerable<OrderModel> GetOrdersPerTrader(string token);
        
        bool CancelOrder(string orderGuid);

        //Guid ExecuteOrder(ClientOrder newOrder);

        }
    }
