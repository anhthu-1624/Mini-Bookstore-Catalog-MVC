namespace AspNetWeek2.Mvc.ViewModels;

public class BookSearchViewModel
{
    public string Keyword { get; set; } = "";
    public string Genre { get; set; } = "";
    public string Publisher { get; set; } = "";  
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public List<BookListItemViewModel> Results { get; set; } = new();
    public List<string> AllGenres { get; set; } = new();
    public bool HasSearched { get; set; } = false;
}