using Newtonsoft.Json;
using IranKish.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace IranKish.Utility
{
    public class WebHelper
    {
        public WebHelper(HttpClient httpClient)
        {
            client = httpClient;
        }

        public HttpClient client { get; }

        public async Task<string> Post(string url,string value)
        {

                client.BaseAddress = new Uri("http://localhost:55587/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var newValue = JsonConvert.DeserializeObject(value);


                try
                {


                     var response = await client.PostAsJsonAsync(url, newValue);
                      var content = await response.Content.ReadAsStringAsync();


                    return content;

                        

                }
                catch (Exception ex)
                {

                    throw ex;
                }



        
        }
                   


    }
}
