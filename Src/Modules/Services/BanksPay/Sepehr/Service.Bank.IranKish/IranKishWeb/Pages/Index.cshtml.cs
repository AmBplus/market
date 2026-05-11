using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IranKish.Utility;
using IranKish.Models;

namespace IranKishWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public IPGDataModel IPGDataModel { get; set; }
        public void OnGet()
        {
            var result = GetData();
            var doc = new System.Xml.XmlDocument();
            doc.Load("DataXmlFile.xml");
            string url = doc.SelectNodes("DocumentElement/BaseAddress/token")[0]?.InnerXml?.ToString();
            System.Xml.XmlNodeList xnList = doc.SelectNodes("DocumentElement/BaseAddress/payment");
            var urlpayment = xnList[0]?.InnerXml?.Trim();
        
            IPGDataModel = result;
        }


        public IPGDataModel GetData()
        {
            AssignXml obj = new AssignXml();
            var data = obj.GetLoad();
            return (data);
        }

    }
}
