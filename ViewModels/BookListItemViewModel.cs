namespace AspNetWeek4.Mvc.ViewModels;

public class BookListItemViewModel
{
    public int Id { get; set; }
    public string BookCode { get; set; } = "";

    public string Isbn { get; set; } = "";

    public string Title { get; set; } = "";

    public string Author { get; set; } = "";

    public string Genre { get; set; } = "";

    public string Publisher { get; set; } = "";

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int MinStock { get; set; }

    public string CategoryName { get; set; } = "";

    // Computed properties

    public string PriceText => $"{Price:N0} VNĐ";

    public string StockStatus =>
        Stock <= 0 ? "Out of Stock" :
        Stock <= MinStock ? "Low Stock" :
        "In Stock";

    public string StockStatusClass =>
        Stock <= 0 ? "danger" :
        Stock <= MinStock ? "warning" :
        "success";
}