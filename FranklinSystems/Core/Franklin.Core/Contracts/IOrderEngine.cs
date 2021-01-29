using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Franklin.Common;
using Franklin.Common.Model;
using Franklin.Data;
using Franklin.Data.Entities;

namespace Franklin.Core {

    public interface IOrderEngine {

        IRepository Repository { get; set; }

        int CreateClientOrder(ClientOrder clientOrder);
        
        Task<Guid> ExecuteGtcOrderAsync(ClientOrder newGtcClientOrder);

        Task<int> ExecuteIocOrderAsync(ClientOrder newIocClientOrder);

        bool DeleteOrder(Guid orderGuid);

    }
}
