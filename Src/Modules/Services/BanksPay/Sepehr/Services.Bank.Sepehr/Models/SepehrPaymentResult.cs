namespace Services.Bank.Sepehr.Models;


public class SepehrPaymentResult
{

    public string? TerminalId { get; set; }
    public string? InvoiceId { get; set; }
    public long? Amount { get; set; }
    public string? CardNumber { get; set; }
    public string? Payload { get; set; }
    public string? Hash { get; set; }
    public string? Rrn { get; set; }
    public string? TraceNumber { get; set; }
    public string? DigitalReceipt { get; set; }
    public string? DatePaid { get; set; }
    public int? RespCode { get; set; } = -1;
    public string? RespMsg { get; set; }
    public string? IssuerBank { get; set; }
    public bool IsSuccess { get => RespCode == 0; } 
}

