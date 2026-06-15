namespace AspNetWeek4.Mvc.ViewModels;

public class SaleHistoryViewModel
{
    public int      Id          { get; set; }
    public DateTime CreatedAt   { get; set; }
    public decimal  TotalAmount { get; set; }
    public int      TotalItems  { get; set; }
    public List<SaleItemRowViewModel> Items { get; set; } = new();
}

public class SaleItemRowViewModel
{
    public string  BookTitle { get; set; } = "";
    public string  BookCode  { get; set; } = "";
    public int     Quantity  { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal  => Quantity * UnitPrice;
}