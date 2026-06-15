using AspNetWeek4.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using AspNetWeek4.Mvc.ViewModels;
using AspNetWeek4.Mvc.Models;

namespace AspNetWeek4.Mvc.Controllers;


public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Index(
        string keyword = "", string genre = "",
        string stock = "", string sortBy = "title", int page = 1)
    {
        var result = await _bookService.GetBookListAsync(keyword, genre, stock, sortBy, page, pageSize: 9);
        var genres = await _bookService.GetGenresAsync();

        ViewBag.Keyword = keyword;
        ViewBag.Genre   = genre;
        ViewBag.Stock   = stock;
        ViewBag.SortBy  = sortBy;
        ViewBag.Genres  = genres;

        return View(result);
    }

    public async Task<IActionResult> Stats()
    {
        var stats = await _bookService.GetBookStatsAsync();

        ViewBag.UrgentBooks  = stats.UrgentBooks;
        ViewBag.TopExpensive = stats.TopExpensive;

        return View(stats);
    }
    public async Task<IActionResult> Detail(int id)
{
    var book = await _bookService.GetBookByIdAsync(id);
    if (book == null) return NotFound();

    var related = await _bookService.GetRelatedBooksAsync(id, book.Genre);
    ViewBag.RelatedBooks = related;

    return View(book);
}
public async Task<IActionResult> Search(
    string keyword = "", string genre = "",
    string publisher = "", decimal? minPrice = null,
    decimal? maxPrice = null)
{
    // Nếu chưa search thì trả về form rỗng
    if (Request.Query.Count == 0)
    {
        var genres = await _bookService.GetGenresAsync();
        return View(new BookSearchViewModel
        {
            AllGenres   = genres,
            HasSearched = false
        });
    }

    var vm = await _bookService.SearchBooksAsync(
        keyword, genre, publisher, minPrice, maxPrice);

    return View(vm);
}

public async Task<IActionResult> Create()
{
    var categories = await _bookService.GetCategoriesAsync();
    var model = new AspNetWeek4.Mvc.ViewModels.BookCreateViewModel
    {
        BookCode = $"BOOK{DateTime.Now:yyyyMMddHHmmss}"
    };
    ViewBag.Categories = categories;
    return View(model);
}

[HttpPost]
[ValidateAntiForgeryToken]

public async Task<IActionResult> Create(AspNetWeek4.Mvc.ViewModels.BookCreateViewModel model)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");
        
        TempData["ErrorMessage"] = "Validation lỗi: " + string.Join(" | ", errors);
        ViewBag.Categories = await _bookService.GetCategoriesAsync();
        return View(model);
    }

    try
    {
        await _bookService.CreateBookAsync(model);
        TempData["SuccessMessage"] = "✅ Thêm sách thành công.";
        return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = $"❌ Rollback. Lỗi: {ex.Message}";
        ViewBag.Categories = await _bookService.GetCategoriesAsync();
        return View(model);
    }
}
public async Task<IActionResult> Filter(int? categoryId, decimal? minPrice, decimal? maxPrice)
{
    var vm = await _bookService.GetFilteredBooksAsync(categoryId, minPrice, maxPrice);
    return View(vm);
}
public async Task<IActionResult> LowStock()
{
    var vm = await _bookService.GetLowStockBooksAsync();
    return View(vm);
}
}