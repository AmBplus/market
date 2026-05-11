namespace Services.Bank.Sepehr.Models;

public class SepehrTokenRequest
{
    public long Amount { get; set; }
    public string CallbackURL { get; set; }
    public string InvoiceID { get; set; }
    public long TerminalID { get; set; }
    public string Payload { get; set; }
    public string? Email { get; set; }
    public int GetMethod { get; set; } = 1; // Optional, 0 for POST, 1 for GET
}

