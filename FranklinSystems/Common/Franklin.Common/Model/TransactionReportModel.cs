using System;
using System.Collections.Generic;
using System.Text;

namespace Franklin.Common.Model {
    
    /// <summary>
    /// Report of the transactions.
    /// </summary>
    public class TransactionReportModel {

        public DateTime ReporDateFrom { get; set; }
        public DateTime ReportDateTo { get; set; }

        public List<OrderTransactionModel> Transactions { get; set; }
    }
}
