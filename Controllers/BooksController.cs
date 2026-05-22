using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.Services;
using AspNetWeek2.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek2.Mvc.Controllers;

public class BooksController : Controller
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    // ════════════════════════════════════════════════════════════
    // INDEX — danh sách, tìm kiếm, lọc, sắp xếp, phân trang
    // ════════════════════════════════════════════════════════════

    public IActionResult Index(
        string? keyword    = null,
        string? genre      = null,
        string? stock      = null,
        string  sortBy     = "title",
        int     page       = 1,
        int     pageSize   = 5)
    {
        // 1. Lấy toàn bộ sách
        var books = _bookService.GetAll();

        // 2. Tìm kiếm
        if (!string.IsNullOrWhiteSpace(keyword))
            books = _bookService.Search(keyword);

        // 3. Lọc thể loại
        if (!string.IsNullOrWhiteSpace(genre))
            books = books.Where(b =>
                b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

        // 4. Lọc tồn kho
        if (!string.IsNullOrWhiteSpace(stock))
            books = stock switch
            {
                "out" => books.Where(b => b.Stock <= 0).ToList(),
                "low" => books.Where(b => b.Stock > 0 && b.Stock <= b.MinStock).ToList(),
                "ok"  => books.Where(b => b.Stock > b.MinStock).ToList(),
                _     => books
            };

        // 5. Sắp xếp
        books = _bookService.Sort(books, sortBy);

        // 6. Phân trang
        var paged = _bookService.GetPaged(books, page, pageSize);

        // 7. Truyền filter hiện tại sang View (giữ lại trên form)
        ViewBag.Keyword  = keyword;
        ViewBag.Genre    = genre;
        ViewBag.Stock    = stock;
        ViewBag.SortBy   = sortBy;
        ViewBag.Genres   = _bookService.GetAllGenres();

        // 8. Map sang ViewModel
        var result = new PagedResult<BookListItemViewModel>
        {
            Items      = paged.Items.Select(ToListItemViewModel).ToList(),
            Page       = paged.Page,
            PageSize   = paged.PageSize,
            TotalItems = paged.TotalItems,
            TotalPages = paged.TotalPages
        };

        return View(result);
    }

    // DETAIL — chi tiết + gợi ý sách liên quan


    public IActionResult Detail(int id)
    {
        var book = _bookService.GetById(id);

        if (book is null)
            return NotFound($"Không tìm thấy sách có id = {id}");

        // Gợi ý sách cùng thể loại
        ViewBag.RelatedBooks = _bookService
            .GetRelated(id, take: 3)
            .Select(ToListItemViewModel)
            .ToList();

        return View(ToDetailViewModel(book));
    }

    // STATS — thống kê + cần nhập hàng gấp


    public IActionResult Stats()
    {
        ViewBag.UrgentBooks  = _bookService.GetUrgentReorder()
            .Select(ToListItemViewModel).ToList();

        ViewBag.TopExpensive = _bookService.GetTopExpensive(3)
            .Select(ToListItemViewModel).ToList();

        return View(_bookService.GetStats());
    }

    // CRUD
    [HttpGet]
    public IActionResult Create() => View(new Book());

    [HttpPost]
    public IActionResult Create(Book book)
    {
        _bookService.Add(book);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = _bookService.GetById(id);
        if (book is null) return NotFound();
        return View(book);
    }

    [HttpPost]
    public IActionResult Edit(Book book)
    {
        _bookService.Update(book);
        return RedirectToAction(nameof(Detail), new { id = book.Id });
    }

    public IActionResult Delete(int id)
    {
        _bookService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    // UTILITY ACTIONS


    public IActionResult Welcome() =>
        Content("Welcome to Mini Bookstore Catalog MVC");

    public IActionResult BookJson()
    {
        var books = _bookService.GetAll().Select(b => new
        {
            b.Id, b.Isbn, b.Title, b.Author,
            b.Genre, b.Price, b.Stock
        });
        return Json(books);
    }

    public IActionResult GoToList() =>
        RedirectToAction(nameof(Index));

    public IActionResult Force404() =>
        NotFound("Đây là response 404 demo từ action Force404.");
    public IActionResult GenreInfo()
    {
        return Content("Thể loại hiện có: Kỹ năng sống, Tiểu thuyết, Lịch sử, Tâm lý học, Văn học Việt Nam");
    }
    // PRIVATE MAPPERS


    private static BookListItemViewModel ToListItemViewModel(Book book) => new()
    {
        Id            = book.Id,
        Isbn          = book.Isbn,
        Title         = book.Title,
        Author        = book.Author,
        Genre         = book.Genre,
        Publisher     = book.Publisher,
        Price         = book.Price,
        Stock         = book.Stock,
        MinStock      = book.MinStock,
        PublishedDate = book.PublishedDate
    };

    private static BookDetailViewModel ToDetailViewModel(Book book) => new()
    {
        Id            = book.Id,
        Isbn          = book.Isbn,
        Title         = book.Title,
        Author        = book.Author,
        Genre         = book.Genre,
        Publisher     = book.Publisher,
        Price         = book.Price,
        Stock         = book.Stock,
        MinStock      = book.MinStock,
        PublishedDate = book.PublishedDate,
        LastUpdatedAt = book.LastUpdatedAt
    };
}