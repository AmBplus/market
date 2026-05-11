using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IranKish.Interface;
using IranKish.Models;
using System.Threading.Tasks;

namespace IranKish.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class VerifypageController : Controller
    {
        private readonly IIRanKishVerification _verification;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VerifypageController(IIRanKishVerification verification, IHttpContextAccessor httpContextAccessor)
        {
            _verification = verification;
            _httpContextAccessor = httpContextAccessor;
        }
        public class RequestVerify2
        {
            public string terminalId { get; set; }
            public string token { get; set; }
            public string amount { get; set; }
            public string retrievalReferenceNumber { get; set; }
            public string systemTraceAuditNumber { get; set; }
            public string responseCode { get; set; }
            public string paymentId { get; set; }
            public string requestId { get; set; }
        }
        [HttpPost()]
        public async Task<IActionResult> Verify([FromForm] RequestVerify request)
        {
            try
            {
                // دریافت داده‌ها از فرم
                //var form = _httpContextAccessor.HttpContext.Request.Form;

                //var requestVerify = new RequestVerify2
                //{
                //    tokenIdentity = form["token"],
                //    responseCode = form["responseCode"],
                //    paymentId = form["paymentId"],
                //    requestId = form["requestId"],
                //    retrievalReferenceNumber = form["retrievalReferenceNumber"],
                //    systemTraceAuditNumber = form["systemTraceAuditNumber"],
                //    amount = form["amount"],
                //};

                // بررسی معتبر بودن داده‌ها
                if (string.IsNullOrEmpty(request.tokenIdentity) )
                {
                    return Ok("داده ها نا معتبر است"); // فرضاً یک View برای نمایش نتیجه
                }

                // بررسی responseCode
            
                if (request.responseCode == "00")
                {
                 var   response = await _verification.Verify(request);
                }
                else
                {
                   var response = await _verification.Verify(request);
                }

                //ViewBag.response = response;
                return Ok("Result"); // فرضاً یک View برای نمایش نتیجه
            }
            catch (System.Exception ex)
            {
                var error  = $"خطا: {ex.Message}";
                return Ok(error); // فرضاً یک View برای خطا
            }
        }

        // متد Verify2 احتمالاً لازم نیست، مگر اینکه استفاده خاصی داشته باشد
        [HttpPost()]
        public async Task<IActionResult> Verify2()
        {
            // اگر این متد لازم نیست، می‌توانید آن را حذف کنید یا منطق مشابه Verify را پیاده‌سازی کنید
            return RedirectToAction("Verify");
        }
    }
}