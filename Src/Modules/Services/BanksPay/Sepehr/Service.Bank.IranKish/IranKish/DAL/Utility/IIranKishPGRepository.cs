using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using IranKish.Utility;
using IranKish.Models;
using System;
using System.Collections.Generic;
using System.Xml;
using CoreApiResponse;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace IranKish
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    public class IranKishGetPayTokenRequest()
    {
        public required int Amount { get;set; }
        public required string IBan { get;set; }
        public required string VerifyUrl { get;set; }
        public required string RequestId { get;set; }
        public required string PaymentId { get;set; }
        public string? MobileReserve { get; set; } = null;
        public Dictionary<string, string>? AdditionalKeys { get; set; } = null;
        
    }
    public class IranKishSettings
    {
        public required string TerminalId { get; set; } = "09223502";
        public required string AcceptorId { get; set; } = "992200009223502";
        public required string PassPhrase { get; set; } = "1A7D1750779CE5D3";
        public required string RsaPublicKey { get; set; } = "\r\n\t\t\t-----BEGIN PUBLIC KEY-----\r\n\t\t\tMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCFsq4eCoAfaDjTrdGmZyPcDzaW\r\n\t\t\twEg9PoksJJ9QbZffXMvPEWp29R0rlWzMK/s4GeBnHXPyrCnUOrvU/fp+F2hoO03U\r\n\t\t\tZ/h3f9vwpxib8MdUNIhebQkOdZkEPrt4XsI+Wh98Uc+LXA6rXHgz7zPhp+LvTPWA\r\n\t\t\tlWTYysXtS1Kup56xzQIDAQAB\r\n\t\t\t-----END PUBLIC KEY-----\r\n\t\t";

        public required string GetTokenUrl = "https://ikc.shaparak.ir/api/v3/tokenization/make";
    }
    public class IranKishIPGRepository :  IIranKishIPGServices
    {
        public IranKishSettings IranKishSettings { get; set; }
        public WebHelper webHelper { get; }

        public IranKishIPGRepository(IranKishSettings iranKishSettings, WebHelper _webHelper)
        {
            this.IranKishSettings = iranKishSettings;
            webHelper = _webHelper;
        }

        // خرید عادی
        public async Task<string> GetPayToken(IranKishGetPayTokenRequest request)
        {

            string Text = string.Empty;
            try
            {
                var requestResult = string.Empty;
                //string paymentId = new Random().Next().ToString();
                //string paymentId = null;
                IPGDataModel _iPGData = new IPGDataModel();
                _iPGData.TerminalId = IranKishSettings.TerminalId;
                _iPGData.AcceptorId = IranKishSettings.AcceptorId;
                //آدرس برگشت اینجا ست می شود
                _iPGData.RevertURL = request.VerifyUrl;
                _iPGData.Amount = request.Amount;
                _iPGData.PaymentId = request.PaymentId;
                _iPGData.RequestId = request.RequestId;
                //the url that must be hard coded
                _iPGData.CmsPreservationId = request.MobileReserve;
                _iPGData.TransactionType = TransactionType.Purchase;
                _iPGData.BillInfo = null;
                _iPGData.PassPhrase = IranKishSettings.PassPhrase;
                // کلید عمومی 
                _iPGData.RsaPublicKey = IranKishSettings.RsaPublicKey ;

                _iPGData.MultiplexParameters = new List<MultiplexParameter>()
                {
                    new MultiplexParameter(){Amount = (int)request.Amount,Iban = request.IBan}
                };
                /// call json request 
                requestResult = CreateJsonRequest.CreateJasonRequest(_iPGData);
                string url = IranKishSettings.GetTokenUrl;
                // پاس دادن اطلاعات ورودی به api
                string jresponse = await webHelper.Post(url, requestResult);
                TokenResult jResult = JsonConvert.DeserializeObject<TokenResult>(jresponse);
                string Token = jResult.result?.token;

                if (jresponse != null)
                {
                    Text = string.Format("result:{0} desc:{1}", jResult.responseCode, jResult.description);

                    switch (jResult.responseCode)
                    {
                        case "901":


                            Text = String.Format(" درخواست نا معتبر است  {0}", jResult.responseCode);



                            break;

                        case "902":



                            Text = String.Format(" پارامترهای اضافی درخواست نامعتبر می باشد  {0}", jResult.responseCode);

                            break;

                        case "903":



                            Text = String.Format(" شناسه پرداخت نامعتبر می باشد {0}", jResult.responseCode);

                            break;
                        case "904":



                            Text = String.Format(" اطلاعات مرتبط با قبض نا معتبر می باشد {0}", jResult.responseCode);

                            break;

                        case "905":




                            Text = String.Format(" شناسه درخواست نامعتبر می باشد {0}", jResult.responseCode);

                            break;
                        case "906":



                            Text = String.Format(" درخواست تاریخ گذشته است {0}", jResult.responseCode);

                            break;
                        case "907":


                            Text = String.Format(" آدرس بازگشت نتیجه پرداخت نامعتبر می باشد {0}", jResult.responseCode);

                            break;

                        case "909":


                            Text = String.Format(" پذیرنده نامعتبر می باشد {0}", jResult.responseCode);

                            break;


                        case "910":


                            Text = String.Format(" پارامترهای مورد انتشار پرداخت تسهیمی تامین نگردیده است {0}", jResult.responseCode);

                            break;


                        case "911":


                            Text = String.Format(" پارامترهای مورد انتشار پرداخت تسهیمی نا معتبر یا دارای اشکال می باشد {0}", jResult.responseCode);

                            break;


                        case "912":


                            Text = String.Format(" تراکنش درخواستی برای پذیرنده فعال نیست {0}", jResult.responseCode);

                            break;


                        case "913":


                            Text = String.Format(" تراکنش تسهیم برای پذیرنده  فعال نیست {0}", jResult.responseCode);

                            break;



                        case "914":


                            Text = String.Format("  آدرس آی پی دریافتی درخواست نا معتبر می باشد {0}", jResult.responseCode);

                            break;


                        case "915":


                            Text = String.Format("   شماره پایانه نامعتبر می باشد {0}", jResult.responseCode);

                            break;



                        case "916":


                            Text = String.Format("   شماره پذیرنده نا معتبر می باشد {0}", jResult.responseCode);

                            break;



                        case "917":


                            Text = String.Format("   نوع تراکنش اعلام شده در خواست نا معتبر می باشد {0}", jResult.responseCode);

                            break;




                        case "918":


                            Text = String.Format("   پذیرنده فعال نیست {0}", jResult.responseCode);

                            break;



                        case "919":


                            Text = String.Format("   مبالغ تسهیمی ارائه شده با توجه به قوانین حاکم بر وضعیت تسهیم پذیرنده ، نا معتبر است {0}", jResult.responseCode);

                            break;


                        case "920":


                            Text = String.Format("   شناسه نشانه نامعتبر می باشد {0}", jResult.responseCode);

                            break;

                        case "921":


                            Text = String.Format("   شناسه نشانه نامعتبر و یا منقضی شده است {0}", jResult.responseCode);

                            break;

                        case "922":


                            Text = String.Format(" نقض امنیت درخواست  {0}", jResult.responseCode);

                            break;



                        case "923":


                            Text = String.Format(" ارسال شناسه پرداخت در تراکنش قبض مجاز نیست  {0}", jResult.responseCode);


                            break;



                        case "928":


                            Text = String.Format(" مبلغ مبادله شده نا معتبر می باشد  {0}", jResult.responseCode);

                            break;



                        case "929":


                            Text = String.Format(" شناسه پرداخت ارائه شده با توجه به الگوریتم متناظر نا معتبر می باشد  {0}", jResult.responseCode);

                            break;



                        case "930":


                            Text = String.Format(" کد ملی ارائه شده نا معتبر می باشد  {0}", jResult.responseCode);

                            break;

                        case "96":


                            Text = String.Format(" General failure  {0}", jResult.responseCode);


                            break;
                    }




                }
                return (Token != null) ? Token : Text;


            }
            catch (Exception ex)
            {

                throw ex;
            }



        }


        // خرید عادی
        public async Task<string> GetPayToken(IPGDataModel iPGData, HttpContext httpContext)
        {

            XmlDocument doc = new XmlDocument();
            
            string Text = string.Empty;


            try
            {


                doc.Load("DataXmlFile.xml");

                //string paymentId = new Random().Next().ToString();
                //string paymentId = null;
                string requestId = new Random().Next(1000).ToString();
                string request = string.Empty;
                IPGDataModel _iPGData = new IPGDataModel();
                _iPGData.TerminalId = iPGData.TerminalId;
                _iPGData.AcceptorId = iPGData.AcceptorId;

                //آدرس برگشت اینجا ست می شود

                _iPGData.RevertURL = doc.SelectNodes("DocumentElement/IPGData")[0].SelectNodes("URL")[0].InnerText;

                _iPGData.Amount = iPGData.Amount;
                _iPGData.PaymentId = requestId;
                _iPGData.RequestId = requestId;

                //the url that must be hard coded
                _iPGData.CmsPreservationId = doc.SelectNodes("DocumentElement/IPGData")[0].SelectNodes("CmsPreservationId")[0].InnerText;
                _iPGData.TransactionType = TransactionType.Purchase;
                _iPGData.BillInfo = null;
                _iPGData.PassPhrase = iPGData.PassPhrase;


                // کلید عمومی 
                _iPGData.RsaPublicKey = doc.SelectNodes("DocumentElement/IPGData/RSAPublicKey")[0].InnerXml.ToString(); ;

                _iPGData.MultiplexParameters = new List<MultiplexParameter>()
                {
                    new MultiplexParameter(){Amount = (int)iPGData.Amount,Iban = "IR700180000000177016115765"}
                };
                /// call json request 
                request = CreateJsonRequest.CreateJasonRequest(_iPGData);



                string url = doc.SelectNodes("DocumentElement/BaseAddress/token")[0].InnerXml.ToString();

                // پاس دادن اطلاعات ورودی به api
                string jresponse = await webHelper.Post(url, request);


                TokenResult jResult = JsonConvert.DeserializeObject<TokenResult>(jresponse);




                string Token = jResult.result?.token;



                if (jresponse != null)
                {
                    Text = string.Format("result:{0} desc:{1}", jResult.responseCode, jResult.description);

                    switch (jResult.responseCode)
                    {
                        case "901":


                            Text = String.Format(" درخواست نا معتبر است  {0}", jResult.responseCode);



                            break;

                        case "902":



                            Text = String.Format(" پارامترهای اضافی درخواست نامعتبر می باشد  {0}", jResult.responseCode);

                            break;

                        case "903":



                            Text = String.Format(" شناسه پرداخت نامعتبر می باشد {0}", jResult.responseCode);

                            break;
                        case "904":



                            Text = String.Format(" اطلاعات مرتبط با قبض نا معتبر می باشد {0}", jResult.responseCode);

                            break;

                        case "905":




                            Text = String.Format(" شناسه درخواست نامعتبر می باشد {0}", jResult.responseCode);

                            break;
                        case "906":



                            Text = String.Format(" درخواست تاریخ گذشته است {0}", jResult.responseCode);

                            break;
                        case "907":


                            Text = String.Format(" آدرس بازگشت نتیجه پرداخت نامعتبر می باشد {0}", jResult.responseCode);

                            break;

                        case "909":


                            Text = String.Format(" پذیرنده نامعتبر می باشد {0}", jResult.responseCode);

                            break;


                        case "910":


                            Text = String.Format(" پارامترهای مورد انتشار پرداخت تسهیمی تامین نگردیده است {0}", jResult.responseCode);

                            break;


                        case "911":


                            Text = String.Format(" پارامترهای مورد انتشار پرداخت تسهیمی نا معتبر یا دارای اشکال می باشد {0}", jResult.responseCode);

                            break;


                        case "912":


                            Text = String.Format(" تراکنش درخواستی برای پذیرنده فعال نیست {0}", jResult.responseCode);

                            break;


                        case "913":


                            Text = String.Format(" تراکنش تسهیم برای پذیرنده  فعال نیست {0}", jResult.responseCode);

                            break;



                        case "914":


                            Text = String.Format("  آدرس آی پی دریافتی درخواست نا معتبر می باشد {0}", jResult.responseCode);

                            break;


                        case "915":


                            Text = String.Format("   شماره پایانه نامعتبر می باشد {0}", jResult.responseCode);

                            break;



                        case "916":


                            Text = String.Format("   شماره پذیرنده نا معتبر می باشد {0}", jResult.responseCode);

                            break;



                        case "917":


                            Text = String.Format("   نوع تراکنش اعلام شده در خواست نا معتبر می باشد {0}", jResult.responseCode);

                            break;




                        case "918":


                            Text = String.Format("   پذیرنده فعال نیست {0}", jResult.responseCode);

                            break;



                        case "919":


                            Text = String.Format("   مبالغ تسهیمی ارائه شده با توجه به قوانین حاکم بر وضعیت تسهیم پذیرنده ، نا معتبر است {0}", jResult.responseCode);

                            break;


                        case "920":


                            Text = String.Format("   شناسه نشانه نامعتبر می باشد {0}", jResult.responseCode);

                            break;

                        case "921":


                            Text = String.Format("   شناسه نشانه نامعتبر و یا منقضی شده است {0}", jResult.responseCode);

                            break;

                        case "922":


                            Text = String.Format(" نقض امنیت درخواست  {0}", jResult.responseCode);

                            break;



                        case "923":


                            Text = String.Format(" ارسال شناسه پرداخت در تراکنش قبض مجاز نیست  {0}", jResult.responseCode);


                            break;



                        case "928":


                            Text = String.Format(" مبلغ مبادله شده نا معتبر می باشد  {0}", jResult.responseCode);

                            break;



                        case "929":


                            Text = String.Format(" شناسه پرداخت ارائه شده با توجه به الگوریتم متناظر نا معتبر می باشد  {0}", jResult.responseCode);

                            break;



                        case "930":


                            Text = String.Format(" کد ملی ارائه شده نا معتبر می باشد  {0}", jResult.responseCode);

                            break;

                        case "96":


                            Text = String.Format(" General failure  {0}", jResult.responseCode);


                            break;
                    }




                }


                return (Token != null) ? Token : Text;






            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        
    }
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.