using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.ViewModels;

namespace AspNetWeek2.Mvc.Services;

public class BookService
{
    private readonly List<Book> _books = new()
    {
        new Book
        {
            Id = 1,
            Isbn = "978-604-2-30701-4",
            Title = "Đắc Nhân Tâm",
            Author = "Dale Carnegie",
            Genre = "Kỹ năng sống",
            Publisher = "NXB Tổng hợp TP.HCM",
            Price = 88000,
            Stock = 42,
            MinStock = 10,
            PublishedDate = new DateTime(2023, 1, 1),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Book
        {
            Id = 2,
            Isbn = "978-604-77-6309-3",
            Title = "Nhà Giả Kim",
            Author = "Paulo Coelho",
            Genre = "Tiểu thuyết",
            Publisher = "NXB Hội Nhà Văn",
            Price = 79000,
            Stock = 5,
            MinStock = 10,
            PublishedDate = new DateTime(2022, 6, 15),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Book
        {
            Id = 3,
            Isbn = "978-604-0-06006-5",
            Title = "Sapiens: Lược Sử Loài Người",
            Author = "Yuval Noah Harari",
            Genre = "Lịch sử",
            Publisher = "NXB Tri Thức",
            Price = 189000,
            Stock = 0,
            MinStock = 5,
            PublishedDate = new DateTime(2021, 3, 20),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Book
        {
            Id = 4,
            Isbn = "978-604-1-20456-7",
            Title = "Tuổi Trẻ Đáng Giá Bao Nhiêu",
            Author = "Rosie Nguyễn",
            Genre = "Kỹ năng sống",
            Publisher = "NXB Hội Nhà Văn",
            Price = 69000,
            Stock = 3,
            MinStock = 8,
            PublishedDate = new DateTime(2020, 9, 10),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Book
        {
            Id = 5,
            Isbn = "978-604-2-19874-1",
            Title = "Atomic Habits",
            Author = "James Clear",
            Genre = "Tâm lý học",
            Publisher = "NXB Lao Động",
            Price = 149000,
            Stock = 2,
            MinStock = 10,
            PublishedDate = new DateTime(2023, 4, 5),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
        new Book
        {
            Id = 6,
            Isbn = "978-604-9-88532-0",
            Title = "Người Tử Tế",
            Author = "Nguyễn Nhật Ánh",
            Genre = "Văn học Việt Nam",
            Publisher = "NXB Trẻ",
            Price = 95000,
            Stock = 28,
            MinStock = 8,
            PublishedDate = new DateTime(2024, 2, 14),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
            new Book
        {
            Id = 7,
            Isbn = "978-604-9-88532-1",
            Title = "Dế Mèn Phiêu Lưu Ký",
            Author = "Tô Hoài",
            Genre = "Văn học Việt Nam",
            Publisher = "NXB Trẻ",
            Price = 85000,
            Stock = 15,
            MinStock = 5,
            PublishedDate = new DateTime(2023, 8, 20),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
            new Book
        {
            Id = 8,
            Isbn = "978-604-9-88532-2",
            Title = "Đất Rừng Phương Nam",
            Author = "Đoàn Giỏi",
            Genre = "Văn học Việt Nam",
            Publisher = "NXB Trẻ",
            Price = 90000,
            Stock = 0,
            MinStock = 5,
            PublishedDate = new DateTime(2022, 11, 5),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
            new Book
        {
            Id = 9,
            Isbn = "978-604-9-88532-3",
            Title = "Tuổi Thơ Dữ Dội",
            Author = "Phùng Quán",
            Genre = "Văn học Việt Nam",
            Publisher = "NXB Trẻ",
            Price = 92000,
            Stock = 4,
            MinStock = 5,
            PublishedDate = new DateTime(2021, 5, 30),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        },
            new Book
        {
            Id = 10,
            Isbn = "978-604-9-88532-4",
            Title = "Kiêu hãnh và định kiến",
            Author = "Jane Austen",
            Genre = "Văn học nước ngoài",
            Publisher = "NXB Trẻ",
            Price = 88000,
            Stock = 7,
            MinStock = 5,
            PublishedDate = new DateTime(2020, 1, 15),
            LastUpdatedAt = new DateTime(2025, 5, 9, 9, 12, 0)
        }
    };
    
    private int _nextId => _books.Max(b => b.Id) + 1;

    // 1. GET ALL / GET BY ID

    public List<Book> GetAll() => _books;

    public Book? GetById(int id) =>
        _books.FirstOrDefault(b => b.Id == id);

    // 2. TÌM KIẾM / LỌC


    /// <summary>Tìm theo tên, tác giả, hoặc thể loại (không phân biệt hoa thường).</summary>
    public List<Book> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return _books;

        var kw = keyword.Trim().ToLower();

        return _books.Where(b =>
            b.Title.ToLower().Contains(kw) ||
            b.Author.ToLower().Contains(kw) ||
            b.Genre.ToLower().Contains(kw) ||
            b.Isbn.Contains(kw) ||
            b.Publisher.ToLower().Contains(kw)
        ).ToList();
    }

    /// <summary>Lọc theo thể loại chính xác.</summary>
    public List<Book> FilterByGenre(string genre) =>
        _books.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
              .ToList();

    /// <summary>Lọc theo khoảng giá.</summary>
    public List<Book> FilterByPrice(decimal minPrice, decimal maxPrice) =>
        _books.Where(b => b.Price >= minPrice && b.Price <= maxPrice).ToList();

    /// <summary>Lọc theo trạng thái tồn kho.</summary>
    public List<Book> FilterByStockStatus(string status) => status switch
    {
        "out"    => _books.Where(b => b.Stock <= 0).ToList(),
        "low"    => _books.Where(b => b.Stock > 0 && b.Stock <= b.MinStock).ToList(),
        "ok"     => _books.Where(b => b.Stock > b.MinStock).ToList(),
        _        => _books
    };

    /// <summary>Lấy danh sách tất cả thể loại (dùng cho dropdown filter).</summary>
    public List<string> GetAllGenres() =>
        _books.Select(b => b.Genre).Distinct().OrderBy(g => g).ToList();

    // 3. SẮP XẾP

    /// <summary>
    /// Sắp xếp danh sách sách.
    /// sortBy: "title" | "author" | "price" | "price_desc" | "stock" | "stock_desc" | "newest"

    public List<Book> Sort(List<Book> books, string sortBy) => sortBy switch
    {
        "title"      => books.OrderBy(b => b.Title).ToList(),
        "author"     => books.OrderBy(b => b.Author).ToList(),
        "price"      => books.OrderBy(b => b.Price).ToList(),
        "price_desc" => books.OrderByDescending(b => b.Price).ToList(),
        "stock"      => books.OrderBy(b => b.Stock).ToList(),
        "stock_desc" => books.OrderByDescending(b => b.Stock).ToList(),
        "newest"     => books.OrderByDescending(b => b.PublishedDate).ToList(),
        _            => books
    };

    // 4. PHÂN TRANG


    /// <summary>Trả về trang cụ thể từ danh sách đã lọc/sắp xếp.</summary>
    public PagedResult<Book> GetPaged(List<Book> books, int page, int pageSize = 5)
    {
        page = Math.Max(1, page);
        var totalItems = books.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<Book>
        {
            Items      = items,
            Page       = page,
            PageSize   = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    // ════════════════════════════════════════════════════════════
    // 5. CRUD
    // ════════════════════════════════════════════════════════════

    public Book Add(Book book)
    {
        book.Id          = _nextId;
        book.LastUpdatedAt = DateTime.Now;
        _books.Add(book);
        return book;
    }

    public bool Update(Book updated)
    {
        var existing = GetById(updated.Id);
        if (existing is null) return false;

        existing.Isbn          = updated.Isbn;
        existing.Title         = updated.Title;
        existing.Author        = updated.Author;
        existing.Genre         = updated.Genre;
        existing.Publisher     = updated.Publisher;
        existing.Price         = updated.Price;
        existing.Stock         = updated.Stock;
        existing.MinStock      = updated.MinStock;
        existing.PublishedDate = updated.PublishedDate;
        existing.LastUpdatedAt = DateTime.Now;

        return true;
    }

    public bool Delete(int id)
    {
        var book = GetById(id);
        if (book is null) return false;

        _books.Remove(book);
        return true;
    }

    // ════════════════════════════════════════════════════════════
    // 6. TÍNH NĂNG MỞ RỘNG ĐẶC BIỆT
    // ════════════════════════════════════════════════════════════

    /// <summary>Top N sách đắt nhất.</summary>
    public List<Book> GetTopExpensive(int top = 3) =>
        _books.OrderByDescending(b => b.Price).Take(top).ToList();

    /// <summary>Top N sách tồn kho nhiều nhất (bestseller potential).</summary>
    public List<Book> GetTopInStock(int top = 3) =>
        _books.OrderByDescending(b => b.Stock).Take(top).ToList();

    /// <summary>Danh sách sách cần nhập hàng gấp (hết hoặc sắp hết).</summary>
    public List<Book> GetUrgentReorder() =>
        _books.Where(b => b.Stock <= b.MinStock)
              .OrderBy(b => b.Stock)
              .ToList();

    /// <summary>Gợi ý sách cùng thể loại (dùng cho trang Detail).</summary>
    public List<Book> GetRelated(int bookId, int take = 3)
    {
        var current = GetById(bookId);
        if (current is null) return new();

        return _books
            .Where(b => b.Id != bookId && b.Genre == current.Genre)
            .Take(take)
            .ToList();
    }

    /// <summary>Thống kê tổng hợp.</summary>
    public BookStatsViewModel GetStats() => new()
    {
        TotalBooks         = _books.Count,
        TotalStock         = _books.Sum(b => b.Stock),
        TotalInventoryValue = _books.Sum(b => b.Price * b.Stock),
        OutOfStockCount    = _books.Count(b => b.Stock <= 0),
        NeedReorderCount   = _books.Count(b => b.Stock > 0 && b.Stock <= b.MinStock)
    };
}