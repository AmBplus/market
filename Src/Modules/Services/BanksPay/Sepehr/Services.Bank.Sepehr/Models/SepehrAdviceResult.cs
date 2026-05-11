namespace Services.Bank.Sepehr.Models;

public class SepehrAdviceResult
{
    public int Status { get; set; }
    public string Message { get; set; }
    public string ReturnId { get; set; }
    public bool IsSuccessStatusCode { get; set; }
}
