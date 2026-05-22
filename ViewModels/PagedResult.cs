using AspNetWeek2.Mvc.Models;
public class PagedResult<T>
    {
        public List<T> Items      { get; set; } = new();
        public int     Page       { get; set; }
        public int     PageSize   { get; set; }
        public int     TotalItems { get; set; }
        public int     TotalPages { get; set; }

        public bool HasPrevPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
