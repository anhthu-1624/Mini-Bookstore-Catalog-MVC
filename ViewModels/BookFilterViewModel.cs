namespace AspNetWeek4.Mvc.ViewModels;

public class BookFilterViewModel
{
    // Input
    public int?     CategoryId { get; set; }
    public decimal? MinPrice   { get; set; }
    public decimal? MaxPrice   { get; set; }

    // Output
    public List<BookListItemViewModel> Results     { get; set; } = new();
    public List<string>                Genres      { get; set; } = new();
    public List<CategoryViewModel>     Categories  { get; set; } = new();
}

public class CategoryViewModel
{
    public int    Id   { get; set; }
    public string Name { get; set; } = "";
}