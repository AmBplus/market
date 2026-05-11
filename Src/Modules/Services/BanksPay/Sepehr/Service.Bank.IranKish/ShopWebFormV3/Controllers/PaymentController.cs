using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopWebFormV3.DAL;
using ShopWebFormV3.DAL.Utility;
using ShopWebFormV3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebFormV3.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;

        private readonly IIPGRepository _ipgRepository;




        public PaymentController(ILogger<PaymentController> logger, IIPGRepository _ipgRepository)
        {
            this._ipgRepository = _ipgRepository;
            _logger = logger;


        }




        public IActionResult Index()
        {
            var result = GetData();
            var doc = new System.Xml.XmlDocument();
            doc.Load("DataXmlFile.xml");
            string url = doc.SelectNodes("DocumentElement/BaseAddress/token")[0]?.InnerXml?.ToString();
            System.Xml.XmlNodeList xnList = doc.SelectNodes("DocumentElement/BaseAddress/payment");
            var urlpayment = xnList[0]?.InnerXml?.Trim();
            ViewBag.urlpayment = urlpayment;
            return View(result);
        }


        public IPGDataModel GetData()
        {
            AssignXml obj = new AssignXml();
            var data = obj.GetLoad();
            return (data);
        }



        [HttpPost]
        public async Task<IActionResult> getToken_onclick([FromBody]IPGDataModel IPGData)
        {

           

            if(IPGData==null || IPGData.TerminalId==null)

                return BadRequest();

            string result;

            try
            {

                result = await _ipgRepository.getToken_onclick(IPGData, HttpContext);



                if (!ModelState.IsValid)
                {

                    return BadRequest("Enter required fields");
                }



                if (result != null)
                {
                    return this.Ok(new { token = result });
                }

                else

                    return this.Ok(new { token = result });
               

            



            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
