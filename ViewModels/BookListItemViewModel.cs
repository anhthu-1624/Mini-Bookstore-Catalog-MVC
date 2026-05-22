namespace AspNetWeek2.Mvc.ViewModels;

public class BookListItemViewModel
{
    public int Id { get; set; }

    public string Isbn { get; set; } = "";

    public string Title { get; set; } = "";

    public string Author { get; set; } = "";

    public string Genre { get; set; } = "";

    public string Publisher { get; set; } = "";

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int MinStock { get; set; }

    public DateTime PublishedDate { get; set; }

    // ── Computed properties ──────────────────────────────────────

    public string PriceText => $"{Price:N0} VND";

    public decimal InventoryValue => Price * Stock;

    public string InventoryValueText => $"{InventoryValue:N0} VND";

    public string StockStatus
    {
        get
        {
            if (Stock <= 0)
                return "Hết sách";

            if (Stock <= MinStock)
                return "Sắp hết";
            if (Stock >=20)
                return "Tồn kho cao";
            return "Còn sách";
            
        }
    }

    public string StockStatusClass
    {
        get
        {
            if (Stock <= 0)
                return "badge badge-danger";

            if (Stock <= MinStock)
                return "badge badge-warning";

            return "badge badge-success";
        }
    }
}