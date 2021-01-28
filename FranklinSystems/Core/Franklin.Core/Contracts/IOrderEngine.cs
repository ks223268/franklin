using System;
using System.Collections.Generic;
using System.Text;

using Franklin.Common;
using Franklin.Common.Model;
using Franklin.Data;
using Franklin.Data.Entities;

namespace Franklin.Core {

    public interface IOrderEngine {

        IRepository Repository { get; set; }

        int CreateClientOrder(ClientOrder clientOrder);

        Guid ExecuteGtcOrder(ClientOrder newGtcClientOrder);

        void ExecuteIocOrder(ClientOrder newIocClientOrder);

        bool DeleteOrder(Guid orderGuid);

    }
}
