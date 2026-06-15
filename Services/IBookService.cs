using AspNetWeek4.Mvc.Models;
using AspNetWeek4.Mvc.ViewModels;

namespace AspNetWeek4.Mvc.Services;

public interface IBookService
{
    Task<PagedResult<BookListItemViewModel>> GetBookListAsync(
        string keyword, string genre, string stock, string sortBy, int page, int pageSize);
    Task<List<string>> GetGenresAsync();
    Task<BookStatsViewModel> GetBookStatsAsync();
    Task<BookDetailViewModel?> GetBookByIdAsync(int id);
    Task<List<BookListItemViewModel>> GetRelatedBooksAsync(int excludeId, string? genre);
    Task<List<Category>> GetCategoriesAsync();      
    Task CreateBookAsync(BookCreateViewModel model); 
    Task<List<BookListItemViewModel>> GetLowStockBooksAsync();
    Task<BookFilterViewModel> GetFilteredBooksAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
    Task<BookSearchViewModel> SearchBooksAsync(string keyword, string genre, string publisher, decimal? minPrice, decimal? maxPrice);
}