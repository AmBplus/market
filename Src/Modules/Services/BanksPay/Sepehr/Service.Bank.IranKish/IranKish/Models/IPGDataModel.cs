using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IranKish.Models
{

    [XmlRoot("DataXmlFile")]
    public class IPGDataModel
    {

        // کد پایانه 
        //[XmlElement("TerminalId")]
        public string? TerminalId { get; set; }
       // کد پذیرنده 
        public string? AcceptorId { get; set; }
        // عبارت عبور
        public string? PassPhrase { get; set; }
         // مقدار-ریال
        public int Amount { get; set; }
   
        public string? RevertURL { get; set; }
        public string? PaymentId { get; set; }
        public string? RequestId { get; set; }
        public string? CmsPreservationId { get; set; } = "";
        public string? TransactionType { get; set; }

        public BillInfo? BillInfo { get; set; }

        public string? RsaPublicKey { get; set; }
        public List<MultiplexParameter>? MultiplexParameters { get; set; }
    }
}
