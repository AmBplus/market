using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace IranKish.Models
{
    /// <summary>
    /// مدل درخواست تأیید تراکنش
    /// </summary>
    public class RequestVerify
    {
        [DataMember]
        //شماره ترمینال
        public string terminalId { get; set; }

        [DataMember]
        //شماره ارجاع
        public string retrievalReferenceNumber { get; set; }

        [DataMember]
        //شماره سند/ پیگیری
        public string systemTraceAuditNumber { get; set; }

        [DataMember]
        //توکن
        public string tokenIdentity { get; set; }


        //[DataMember]
        ////شماره پذیرنده
        //public string acceptorId { get; set; }



        [DataMember]
        //مقدار
        public long Amount { get; set; }



        [DataMember]

        ////ریسپانس کد
        public string responseCode { get; set; }
    }

    /// <summary>
    /// مدل نتیجه تأیید تراکنش
    /// </summary>
    public class VerifyResult
    {
        public string responseCode { get; set; }
        public string description { get; set; }
        public bool status { get; set; }
        public VerifySubResult result { get; set; }

    }

    /// <summary>
    /// مدل جزئیات نتیجه تراکنش
    /// </summary>
    public class VerifySubResult
    {
        public string responseCode { get; set; }
        public string systemTraceAuditNumber { get; set; }
        public string retrievalReferenceNumber { get; set; }
        public DateTime transactionDateTime { get; set; }
        public int transactionDate { get; set; }
        public int transactionTime { get; set; }
        public string processCode { get; set; }
        public object requestId { get; set; }
        public object additional { get; set; }
        public object billType { get; set; }
        public object billId { get; set; }
        public string paymentId { get; set; }
        public string amount { get; set; }
        public object revertUri { get; set; }
        public object acceptorId { get; set; }
        public object terminalId { get; set; }
        public object tokenIdentity { get; set; }

    }
}
