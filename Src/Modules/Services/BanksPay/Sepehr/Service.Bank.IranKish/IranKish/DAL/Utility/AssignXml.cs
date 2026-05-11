using IranKish.Models;
using System.Xml;

namespace IranKish.Utility
{
    public class AssignXml
    {




        public IPGDataModel GetLoad()

        {

            IPGDataModel ad = new IPGDataModel();
            XmlDocument doc = new XmlDocument();

            doc.Load("DataXmlFile.xml");
        

            ad.TerminalId= doc.SelectNodes("DocumentElement/IPGData")[0].SelectNodes("TerminalId")[0].InnerText;
            ad.AcceptorId= doc.SelectNodes("DocumentElement/IPGData")[0].SelectNodes("AcceptorId")[0].InnerText;
            ad.PassPhrase= doc.SelectNodes("DocumentElement/IPGData")[0].SelectNodes("PassPhrase")[0].InnerText;
            ad.Amount = 2000;


            return ad;



        }




    }
}
