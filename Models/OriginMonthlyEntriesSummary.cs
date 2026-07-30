namespace MyWallet.Models;

public class OriginMonthlyEntriesSummary
{
    public int Year { get; set; }
    public int Month { get; set; }
    public OriginType Origin { get; set; }
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
}
