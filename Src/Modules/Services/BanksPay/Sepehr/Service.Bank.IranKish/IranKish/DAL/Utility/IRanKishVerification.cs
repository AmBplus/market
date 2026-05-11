using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IranKish.Interface;
using IranKish.Models;
using Newtonsoft.Json;

namespace IranKish.Utility
{


    public class IRanKishVerification : IIRanKishVerification
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://ikc.shaparak.ir/api/v3/confirmation/purchase";

        public IRanKishVerification(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            //_httpClient.BaseAddress = new Uri(ApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// ارسال درخواست تأیید تراکنش به سرویس ایران کیش
        /// </summary>
        /// <param name="requestVerify">اطلاعات تراکنش برای تأیید</param>
        /// <returns>نتیجه تأیید تراکنش</returns>
        public async Task<VerifyResult> Verify(RequestVerify requestVerify)
        {
            ArgumentNullException.ThrowIfNull(requestVerify);
            requestVerify.terminalId = "09223502";
            try
            {
                if (string.IsNullOrWhiteSpace(requestVerify.terminalId))
                    throw new ArgumentException("شناسه ترمینال الزامی است");

                if (string.IsNullOrWhiteSpace(requestVerify.tokenIdentity))
                    throw new ArgumentException("توکن الزامی است");

                //// سریالایز با تنظیمات UTF-8
                //string request = JsonConvert.SerializeObject(requestVerify, new JsonSerializerSettings
                //{
                //    //StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            
                //});

                //// ایجاد محتوای HTTP با انکودینگ UTF-8 و نوع محتوای application/json
                //var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                //var response = await _httpClient.PostAsync(ApiUrl, httpContent);

                //if (!response.IsSuccessStatusCode)
                //{
                //    var errorContent = await response.Content.ReadAsStringAsync();
                //    throw new HttpRequestException($"خطا در ارسال درخواست. وضعیت: {response.StatusCode}. محتوا: {errorContent}");
                //}

                //var content = await response.Content.ReadAsStringAsync();
                //var result = JsonConvert.DeserializeObject<VerifyResult>(content);

                //return result ?? throw new InvalidOperationException("پاسخ دریافتی از سرور نامعتبر است");


                string request = JsonConvert.SerializeObject(requestVerify);
                var newrequest = JsonConvert.DeserializeObject(request);
    
                var response = await _httpClient.PostAsJsonAsync(ApiUrl, requestVerify);
                var content = await response.Content.ReadAsStringAsync();

                VerifyResult result = JsonConvert.DeserializeObject<VerifyResult>(content);
                return result ?? throw new InvalidOperationException("پاسخ دریافتی از سرور نامعتبر است");

            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("خطا در پردازش پاسخ سرور", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("خطا در ارسال درخواست تأیید تراکنش", ex);
            }
        }
    }


}

//class WebHelper
//{
//    public string Post(Uri url, string value)
//    {
//        var request = HttpWebRequest.Create(url);
//        var byteData = Encoding.ASCII.GetBytes(value);
//        request.ContentType = "application/json";
//        request.Method = "POST";

//        try
//        {
//            using (var stream = request.GetRequestStream())
//            {
//                stream.Write(byteData, 0, byteData.Length);
//            }
//            var response = (HttpWebResponse)request.GetResponse();
//            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

//            return responseString;
//        }
//        catch (WebException e)
//        {
//            return null;
//        }
//    }
//}