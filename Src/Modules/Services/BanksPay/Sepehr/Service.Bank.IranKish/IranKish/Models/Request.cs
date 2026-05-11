using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranKish.Models
{
    public class Request
    {

        public string AcceptorId { get; set; }
        public long amount { get; set; }
        public BillInfo BillInfo { get; set; }

        public string CmsPreservationId { get; set; }
        public List<MultiplexParameter> multiplexParameters { get; set; }
        public string PaymentId { get; set; }
        public string RequestId { get; set; }
        public long RequestTimestamp { get; set; }
        public string RevertUri { get; set; }
        public string terminalId { get; set; }
        public string transactionType { get; set; }
        public List<KeyValuePair<string, string>> additionalParameters { get; set; }

    }
}
