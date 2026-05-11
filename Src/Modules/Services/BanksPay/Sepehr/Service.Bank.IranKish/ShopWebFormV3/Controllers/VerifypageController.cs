using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopWebFormV3.DAL.Interface;
using ShopWebFormV3.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopWebFormV3.Controllers
{
    public class VerifypageController : Controller
    {


        private readonly IVerification _verification;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public VerifypageController(IVerification _verification, IHttpContextAccessor httpContextAccessor)
        {

          
             this._verification = _verification;
            _httpContextAccessor = httpContextAccessor;

        }





        public async Task<IActionResult> Verify()
         {

            try
            {


                RequestVerify requestVerify = new RequestVerify();


                //شماره ترمینال 
                requestVerify.terminalId= _httpContextAccessor.HttpContext.Response.HttpContext.Request.Form["acceptorId"].ToString().Substring(7, 8);


                //مقدار
                requestVerify.Amount = _httpContextAccessor.HttpContext.Response.HttpContext.Request.Form["Amount"];

                //شماره ارجاع 
                requestVerify.retrievalReferenceNumber = _httpContextAccessor.HttpContext.Response.HttpContext.Request.Form["retrievalReferenceNumber"];

                //توکن
                requestVerify.tokenIdentity = _httpContextAccessor.HttpContext.Response.HttpContext.Request.Form["token"];

                // شماره سند/ پیگیری
                requestVerify.systemTraceAuditNumber = _httpContextAccessor.HttpContext.Response.HttpContext.Request.Form["systemTraceAuditNumber"];
                

                //ریسپانس کد
                requestVerify.responseCode = _httpContextAccessor.HttpContext.Response.HttpContext.Request.Form["responseCode"];

                return View(requestVerify);


            }
            catch (System.Exception ex)
            {

                throw ex;
            }

   
              
        }



        public async Task<IActionResult> btnVerification_Click(RequestVerify requestVerify)
        {


            string response = "";


            try
            {


                if (!string.IsNullOrEmpty(requestVerify.responseCode) && requestVerify.responseCode == "00")
                {



                        response =  await _verification.btnVerification_Click(requestVerify);

                       ViewBag.response = response;


                }

                else
                {


                    response = await _verification.btnVerification_Click(requestVerify);
                    ViewBag.response = response;

                }


            }
            catch (System.Exception ex)
            {



               throw ex;



            }


            

            return View();
        
        
        
        }



    }
}
