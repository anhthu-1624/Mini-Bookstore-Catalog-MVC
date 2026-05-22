namespace AspNetWeek2.Mvc.ViewModels;

public class BookStatsViewModel
{
    public int TotalBooks { get; set; }

    public int TotalStock { get; set; }

    public decimal TotalInventoryValue { get; set; }

    public int OutOfStockCount { get; set; }

    public int NeedReorderCount { get; set; }

    public string TotalInventoryValueText => $"{TotalInventoryValue:N0} VND";
}