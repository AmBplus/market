namespace IranKish.Models
{
    public class TokenResult
    {

        TokenResult()
        {
            result = new Result();
        }

        public string responseCode { get; set; }
        public object description { get; set; }
        public bool status { get; set; }
        public Result result { get; set; }
    }
}
