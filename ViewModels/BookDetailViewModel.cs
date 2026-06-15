namespace AspNetWeek4.Mvc.ViewModels;

public class BookDetailViewModel
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

    public DateTime PublishedDate { get; set; }

    public DateTime LastUpdatedAt { get; set; }
    public string?  CategoryName  { get; set; }


    // ── Computed properties ──────────────────────────────────────

    public string PriceText => $"{Price:N0} VND";

    public decimal InventoryValue => Price * Stock;

    public string InventoryValueText => $"{InventoryValue:N0} VND";

    public string PublishedDateText => PublishedDate.ToString("dd/MM/yyyy");

    public string LastUpdatedText => LastUpdatedAt.ToString("dd/MM/yyyy HH:mm");

    public string StockStatus
    {
        get
        {
            if (Stock <= 0)
                return "Hết sách";

            if (Stock <= MinStock)
                return "Sắp hết";

            return "Còn sách";
        }
    }

    public string ReorderSuggestion
    {
        get
        {
            if (Stock <= 0)
                return "Cần nhập sách ngay vì kho đã hết.";

            if (Stock <= MinStock)
                return $"Nên nhập thêm. Tồn kho hiện tại chỉ còn {Stock} cuốn, mức tối thiểu là {MinStock}.";

            return "Tồn kho đang ổn định, chưa cần nhập thêm.";
        }
    }
    
}