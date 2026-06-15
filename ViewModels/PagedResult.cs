namespace AspNetWeek4.Mvc.ViewModels;

public class PagedResult<T>
{
    public List<T> Items      { get; set; } = new();
    public int     Page       { get; set; }
    public int     PageSize   { get; set; }
    public int     TotalItems { get; set; }

    // Computed tự động — không cần set tay
    public int  TotalPages  => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasPrevPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}