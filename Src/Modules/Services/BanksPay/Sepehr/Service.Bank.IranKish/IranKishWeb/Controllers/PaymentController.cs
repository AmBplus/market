using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IranKish;
using IranKish.Utility;
using IranKish.Models;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IranKishWeb.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;

        private readonly IIranKishIPGServices _ipgRepository;




        public PaymentController(ILogger<PaymentController> logger, IIranKishIPGServices _ipgRepository)
        {
            this._ipgRepository = _ipgRepository;
            _logger = logger;


        }




        //public IActionResult Index()
        //{
        //    var result = GetData();
        //    var doc = new System.Xml.XmlDocument();
        //    doc.Load("DataXmlFile.xml");
        //    string url = doc.SelectNodes("DocumentElement/BaseAddress/token")[0]?.InnerXml?.ToString();
        //    System.Xml.XmlNodeList xnList = doc.SelectNodes("DocumentElement/BaseAddress/payment");
        //    var urlpayment = xnList[0]?.InnerXml?.Trim();
        //    ViewBag.urlpayment = urlpayment;
        //    return View(result);
        //}


        //private IPGDataModel GetData()
        //{
        //    AssignXml obj = new AssignXml();
        //    var data = obj.GetLoad();
        //    return data;
        //}

        //[HttpGet]
        //public async Task<IActionResult> getToken_onclick1()
        //{
        //    return Ok("success");
        //}


        [HttpPost]
        public async Task<IActionResult> getToken_onclick([FromBody] IPGDataModel IPGData)
        {
            if (IPGData == null || IPGData.TerminalId == null)
                return BadRequest();

            string result;

            try
            {
                string requestId = new Random().Next(1000).ToString();
                var iban = "IR700180000000177016115765";
                var request = new IranKishGetPayTokenRequest() 
                {
                    Amount = IPGData.Amount,
                    IBan = iban,
                    PaymentId = requestId,
                    RequestId = requestId,
                    VerifyUrl = "https://localhost:7242/Verifypage/Verify",
                };
                //result = await _ipgRepository.GetPayToken(IPGData, HttpContext);
                result = await _ipgRepository.GetPayToken(request);
                if (!ModelState.IsValid)
                {

                    return BadRequest("Enter required fields");
                }

                if (result != null)
                {
                    return Ok(new { token = result });
                }
                else
                    return Ok(new { token = result });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

    }
}
