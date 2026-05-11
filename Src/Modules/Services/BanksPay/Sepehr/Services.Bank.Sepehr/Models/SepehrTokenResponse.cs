namespace Services.Bank.Sepehr.Models;

public class SepehrTokenResponse
{
    public bool IsSuccessStatusCode { get; set; } = false;
    public int Status { get; set; } = 0;
    public string? Accesstoken { get; set; }
    public string? ErrorMessage { get; set; } // Added for potential error message
}

