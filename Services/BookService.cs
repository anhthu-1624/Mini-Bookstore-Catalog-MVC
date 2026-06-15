using AspNetWeek4.Mvc.Repositories;
using AspNetWeek4.Mvc.ViewModels;
using AspNetWeek4.Mvc.Models;
using AspNetWeek4.Mvc.Options;

namespace AspNetWeek4.Mvc.Services;
using Microsoft.Extensions.Options;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly AppSettings _appSettings;

    public BookService(IBookRepository bookRepository, IOptions<AppSettings> appSettings)
    {
        _bookRepository = bookRepository;
        _appSettings = appSettings.Value;
    }

    public async Task<PagedResult<BookListItemViewModel>> GetBookListAsync(
        string keyword, string genre, string stock, string sortBy, int page, int pageSize)
    {
        var books = await _bookRepository.GetAllReadOnlyAsync();

        // Filter
        if (!string.IsNullOrWhiteSpace(keyword))
            books = books.Where(b =>
                b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Isbn.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(genre))
            books = books.Where(b => b.Genre == genre).ToList();

        if (stock == "ok")  books = books.Where(b => b.Stock > b.MinStock).ToList();
        if (stock == "low") books = books.Where(b => b.Stock > 0 && b.Stock <= b.MinStock).ToList();
        if (stock == "out") books = books.Where(b => b.Stock == 0).ToList();

        // Sort
        books = sortBy switch
        {
            "author"     => books.OrderBy(b => b.Author).ToList(),
            "price"      => books.OrderBy(b => b.Price).ToList(),
            "price_desc" => books.OrderByDescending(b => b.Price).ToList(),
            "stock"      => books.OrderBy(b => b.Stock).ToList(),
            "newest"     => books.OrderByDescending(b => b.PublishedDate).ToList(),
            _            => books.OrderBy(b => b.Title).ToList()
        };

        var totalItems = books.Count;
        var items = books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookListItemViewModel
            {
                Id        = b.Id,
                BookCode = b.BookCode,
                Isbn      = b.Isbn,
                Title     = b.Title,
                Author    = b.Author,
                Genre     = b.Genre,
                Publisher = b.Publisher,
                Price     = b.Price,
                Stock     = b.Stock,
                MinStock  = b.MinStock
            }).ToList();

        return new PagedResult<BookListItemViewModel>
        {
            Items      = items,
            Page       = page,
            PageSize   = pageSize,
            TotalItems = totalItems
        };
    }

    public async Task<List<string>> GetGenresAsync()
    {
        var books = await _bookRepository.GetAllReadOnlyAsync();
        return books
            .Where(b => !string.IsNullOrWhiteSpace(b.Genre))
            .Select(b => b.Genre!)
            .Distinct()
            .OrderBy(g => g)
            .ToList();
    }

    public async Task<BookStatsViewModel> GetBookStatsAsync()
    {
        var books = await _bookRepository.GetAllReadOnlyAsync();
        var items = books.Select(b => new BookListItemViewModel
        {
            Id        = b.Id,
            BookCode = b.BookCode,
            Isbn      = b.Isbn,
            Title     = b.Title,
            Author    = b.Author,
            Genre     = b.Genre,
            Publisher = b.Publisher,
            Price     = b.Price,
            Stock     = b.Stock,
            MinStock  = b.MinStock
        }).ToList();

        return new BookStatsViewModel
        {
            TotalBooks          = items.Count,
            TotalStock          = items.Sum(b => b.Stock),
            TotalInventoryValue = books.Sum(b => b.Price * b.Stock),
            OutOfStockCount     = items.Count(b => b.Stock == 0),
            NeedReorderCount    = items.Count(b => b.Stock > 0 && b.Stock <= b.MinStock),
            UrgentBooks         = items.Where(b => b.Stock <= b.MinStock).ToList(),
            TopExpensive        = items.OrderByDescending(b => b.Price).Take(3).ToList()
        };
    }
    public async Task<BookDetailViewModel?> GetBookByIdAsync(int id)
{
    var book = await _bookRepository.GetByIdAsync(id);
    if (book == null) return null;

    return new BookDetailViewModel
    {
        Id            = book.Id,
        BookCode      = book.BookCode,
        Isbn          = book.Isbn,
        Title         = book.Title,
        Author        = book.Author,
        Genre         = book.Genre,
        Publisher     = book.Publisher,
        Price         = book.Price,
        Stock         = book.Stock,
        MinStock      = book.MinStock,
        PublishedDate = book.PublishedDate,
        LastUpdatedAt = book.LastUpdatedAt,
        CategoryName  = book.Category?.Name
    };
}
public async Task<List<BookListItemViewModel>> GetRelatedBooksAsync(int excludeId, string? genre)
{
    var books = await _bookRepository.GetAllReadOnlyAsync();
    return books
        .Where(b => b.Id != excludeId && b.Genre == genre)
        .Take(3)
        .Select(b => new BookListItemViewModel
        {
            Id       = b.Id,
            BookCode = b.BookCode,
            Isbn     = b.Isbn,
            Title    = b.Title,
            Author   = b.Author,
            Price    = b.Price,
            Stock    = b.Stock,
            MinStock = b.MinStock
        }).ToList();
}
public async Task<List<Category>> GetCategoriesAsync()
{
    var books = await _bookRepository.GetAllReadOnlyAsync();
    // Lấy categories từ books vì không có CategoryRepository
    return books
        .Where(b => b.Category != null)
        .Select(b => b.Category!)
        .DistinctBy(c => c.Id)
        .OrderBy(c => c.Name)
        .ToList();
}

public async Task CreateBookAsync(BookCreateViewModel model)
{
    using var transaction = await _bookRepository.BeginTransactionAsync();
    try
    {
        var book = new Book
        {
            Isbn          = model.Isbn,
            BookCode      = model.BookCode,
            Title         = model.Title,
            Author        = model.Author,
            Genre         = model.Genre,
            Publisher     = model.Publisher,
            Price         = model.Price,
            Stock         = model.Stock,
            MinStock      = model.MinStock,
            PublishedDate = model.PublishedDate,
            LastUpdatedAt = DateTime.Now,
            CategoryId    = model.CategoryId
        };

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
        await transaction.CommitAsync();  
    }
    catch
    {
        await transaction.RollbackAsync();  
        throw;
    }
}
public async Task<List<BookListItemViewModel>> GetLowStockBooksAsync()
    {
        var books = await _bookRepository.GetAllReadOnlyAsync();
        var threshold = _appSettings.LowStockThreshold;

        return books
            .Where(b => b.Stock <= threshold)
            .OrderBy(b => b.Stock)
            .Select(b => new BookListItemViewModel
            {
                Id       = b.Id,
                BookCode = b.BookCode,
                Isbn     = b.Isbn,
                Title    = b.Title,
                Author   = b.Author,
                Genre    = b.Genre,
                Publisher = b.Publisher,
                Price    = b.Price,
                Stock    = b.Stock,
                MinStock = b.MinStock
            }).ToList();
    }
    public async Task<BookFilterViewModel> GetFilteredBooksAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
{
    var books = await _bookRepository.GetFilteredReadOnlyAsync(categoryId, minPrice, maxPrice);
    var all   = await _bookRepository.GetAllReadOnlyAsync();

    return new BookFilterViewModel
    {
        CategoryId = categoryId,
        MinPrice   = minPrice,
        MaxPrice   = maxPrice,
        Results = books.Select(b => new BookListItemViewModel
        {
            Id        = b.Id,
            BookCode = b.BookCode,
            Isbn      = b.Isbn,
            Title     = b.Title,
            Author    = b.Author,
            Genre     = b.Genre,
            Publisher = b.Publisher,
            Price     = b.Price,
            Stock     = b.Stock,
            MinStock  = b.MinStock
        }).ToList(),
        Categories = all
            .Where(b => b.Category != null)
            .Select(b => b.Category!)
            .DistinctBy(c => c.Id)
            .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name })
            .OrderBy(c => c.Name)
            .ToList()
    };
}
public async Task<BookSearchViewModel> SearchBooksAsync(
    string keyword, string genre, string publisher,
    decimal? minPrice, decimal? maxPrice)
{
    var genres = await GetGenresAsync();

    var vm = new BookSearchViewModel
    {
        Keyword    = keyword,
        Genre      = genre,
        Publisher  = publisher,
        MinPrice   = minPrice,
        MaxPrice   = maxPrice,
        AllGenres  = genres,
        HasSearched = true
    };

    var books = await _bookRepository.GetAllReadOnlyAsync();

    if (!string.IsNullOrWhiteSpace(keyword))
        books = books.Where(b =>
            b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();

    if (!string.IsNullOrWhiteSpace(genre))
        books = books.Where(b => b.Genre == genre).ToList();

    if (!string.IsNullOrWhiteSpace(publisher))
        books = books.Where(b =>
            b.Publisher != null &&
            b.Publisher.Contains(publisher, StringComparison.OrdinalIgnoreCase))
            .ToList();

    if (minPrice.HasValue)
        books = books.Where(b => b.Price >= minPrice.Value).ToList();

    if (maxPrice.HasValue)
        books = books.Where(b => b.Price <= maxPrice.Value).ToList();

    vm.Results = books.Select(b => new BookListItemViewModel
    {
        Id        = b.Id,
        BookCode  = b.BookCode,
        Isbn      = b.Isbn,
        Title     = b.Title,
        Author    = b.Author,
        Genre     = b.Genre,
        Publisher = b.Publisher,
        Price     = b.Price,
        Stock     = b.Stock,
        MinStock  = b.MinStock
    }).ToList();

    return vm;
}
}