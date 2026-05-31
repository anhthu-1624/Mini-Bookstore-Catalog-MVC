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

    // INDEX
    public IActionResult Index(
        string? keyword = null,
        string? genre = null,
        string? stock = null,
        string sortBy = "title",
        int page = 1,
        int pageSize = 5)
    {
        var books = _bookService.GetAll();

        if (!string.IsNullOrWhiteSpace(keyword))
            books = _bookService.Search(keyword);

        if (!string.IsNullOrWhiteSpace(genre))
            books = books.Where(b =>
                b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrWhiteSpace(stock))
            books = stock switch
            {
                "out" => books.Where(b => b.Stock <= 0).ToList(),
                "low" => books.Where(b => b.Stock > 0 && b.Stock <= b.MinStock).ToList(),
                "ok" => books.Where(b => b.Stock > b.MinStock).ToList(),
                _ => books
            };

        books = _bookService.Sort(books, sortBy);

        var paged = _bookService.GetPaged(books, page, pageSize);

        ViewBag.Keyword = keyword;
        ViewBag.Genre = genre;
        ViewBag.Stock = stock;
        ViewBag.SortBy = sortBy;
        ViewBag.Genres = _bookService.GetAllGenres();

        var result = new PagedResult<BookListItemViewModel>
        {
            Items = paged.Items.Select(ToListItemViewModel).ToList(),
            Page = paged.Page,
            PageSize = paged.PageSize,
            TotalItems = paged.TotalItems,
            TotalPages = paged.TotalPages
        };

        return View(result);
    }

    // DETAIL
    public IActionResult Detail(int id)
    {
        var book = _bookService.GetById(id);
        if (book is null)
            return NotFound($"Không tìm thấy sách có id = {id}");

        ViewBag.RelatedBooks = _bookService
            .GetRelated(id, take: 3)
            .Select(ToListItemViewModel)
            .ToList();

        return View(ToDetailViewModel(book));
    }

    // STATS
    public IActionResult Stats()
    {
        ViewBag.UrgentBooks = _bookService.GetUrgentReorder()
            .Select(ToListItemViewModel).ToList();
        ViewBag.TopExpensive = _bookService.GetTopExpensive(3)
            .Select(ToListItemViewModel).ToList();

        return View(_bookService.GetStats());
    }

    // SEARCH — BÀI TẬP 1: thêm lọc theo Publisher
    [HttpGet]
    public IActionResult Search(string? keyword, string? publisher, string? genre,
                                decimal? minPrice, decimal? maxPrice)
    {
        var viewModel = new BookSearchViewModel
        {
            Keyword = keyword ?? "",
            Publisher = publisher ?? "",
            Genre = genre ?? "",
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            AllGenres = _bookService.GetAllGenres(),
            HasSearched = Request.Query.Count > 0
        };

        if (viewModel.HasSearched)
        {
            var results = _bookService.Search(keyword ?? "");

            // Lọc theo NXB (BÀI TẬP 1)
            if (!string.IsNullOrWhiteSpace(publisher))
                results = results.Where(b =>
                    b.Publisher.Contains(publisher, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(genre))
                results = results.Where(b =>
                    b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

            if (minPrice.HasValue)
                results = results.Where(b => b.Price >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                results = results.Where(b => b.Price <= maxPrice.Value).ToList();

            viewModel.Results = results.Select(ToListItemViewModel).ToList();
        }

        return View(viewModel);
    }

    // CREATE GET
    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new BookCreateViewModel
        {
            Stock = 1,
            MinStock = 5,
            PublishedDate = DateTime.Today
        };
        return View(viewModel);
    }

    // CREATE POST — BÀI TẬP 2: dùng Isbn từ user nhập (đã có sẵn trong ViewModel)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var book = new Book
        {
            Isbn = model.Isbn,       // BÀI TẬP 2: dùng Isbn user nhập
            Title = model.Title,
            Author = model.Author,
            Genre = model.Genre,
            Publisher = model.Publisher,
            Price = model.Price,
            Stock = model.Stock,
            MinStock = model.MinStock,
            PublishedDate = model.PublishedDate
        };

        _bookService.Add(book);

        TempData["SuccessMessage"] = $"Đã thêm sách \"{model.Title}\" thành công!";
        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = _bookService.GetById(id);
        if (book is null) return NotFound();

        var viewModel = new BookCreateViewModel
        {
            Isbn = book.Isbn,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            Publisher = book.Publisher,
            Price = book.Price,
            Stock = book.Stock,
            MinStock = book.MinStock,
            PublishedDate = book.PublishedDate
        };
        ViewBag.EditId = id;
        return View(viewModel);
    }

    // EDIT POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.EditId = id;
            return View(model);
        }

        var book = new Book
        {
            Id = id,
            Isbn = model.Isbn,
            Title = model.Title,
            Author = model.Author,
            Genre = model.Genre,
            Publisher = model.Publisher,
            Price = model.Price,
            Stock = model.Stock,
            MinStock = model.MinStock,
            PublishedDate = model.PublishedDate
        };

        _bookService.Update(book);
        TempData["SuccessMessage"] = $"Đã cập nhật sách \"{model.Title}\" thành công!";
        return RedirectToAction(nameof(Detail), new { id });
    }

    // DELETE
    public IActionResult Delete(int id)
    {
        _bookService.Delete(id);
        TempData["SuccessMessage"] = "Đã xoá sách thành công!";
        return RedirectToAction(nameof(Index));
    }

    // UTILITY
    public IActionResult Welcome() => Content("Welcome to Mini Bookstore Catalog MVC");
    public IActionResult BookJson()
    {
        var books = _bookService.GetAll().Select(b => new
        {
            b.Id,
            b.Isbn,
            b.Title,
            b.Author,
            b.Genre,
            b.Price,
            b.Stock
        });
        return Json(books);
    }
    public IActionResult GoToList() => RedirectToAction(nameof(Index));
    public IActionResult Force404() => NotFound("Đây là response 404 demo từ action Force404.");
    public IActionResult GenreInfo() =>
        Content("Thể loại hiện có: Kỹ năng sống, Tiểu thuyết, Lịch sử, Tâm lý học, Văn học Việt Nam");

    // MAPPERS
    private static BookListItemViewModel ToListItemViewModel(Book book) => new()
    {
        Id = book.Id,
        Isbn = book.Isbn,
        Title = book.Title,
        Author = book.Author,
        Genre = book.Genre,
        Publisher = book.Publisher,
        Price = book.Price,
        Stock = book.Stock,
        MinStock = book.MinStock,
        PublishedDate = book.PublishedDate
    };

    private static BookDetailViewModel ToDetailViewModel(Book book) => new()
    {
        Id = book.Id,
        Isbn = book.Isbn,
        Title = book.Title,
        Author = book.Author,
        Genre = book.Genre,
        Publisher = book.Publisher,
        Price = book.Price,
        Stock = book.Stock,
        MinStock = book.MinStock,
        PublishedDate = book.PublishedDate,
        LastUpdatedAt = book.LastUpdatedAt
    };
}