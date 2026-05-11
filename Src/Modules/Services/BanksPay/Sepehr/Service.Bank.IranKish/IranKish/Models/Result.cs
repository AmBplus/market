
namespace IranKish.Models
{
    public class Result
    {
        public Result()
        {
            billInfo = new BillInfo();
        }
        public string token { get; set; }
        public int initiateTimeStamp { get; set; }
        public int expiryTimeStamp { get; set; }
        public string transactionType { get; set; }
        public BillInfo billInfo { get; set; }


    }
}
