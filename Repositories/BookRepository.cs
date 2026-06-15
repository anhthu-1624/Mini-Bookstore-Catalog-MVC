using AspNetWeek4.Mvc.Data;
using AspNetWeek4.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspNetWeek4.Mvc.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Category)
            .ToListAsync();
    }

    public async Task<List<Book>> GetAllReadOnlyAsync()
    {
        return await _context.Books
            .Include(b => b.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    // ── THÊM MỚI ──
    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<Book>> GetFilteredReadOnlyAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
{
    var query = _context.Books
        .Include(b => b.Category)
        .AsNoTracking()
        .AsQueryable();

    if (categoryId.HasValue)
        query = query.Where(b => b.CategoryId == categoryId.Value);

    if (minPrice.HasValue)
        query = query.Where(b => b.Price >= minPrice.Value);

    if (maxPrice.HasValue)
        query = query.Where(b => b.Price <= maxPrice.Value);

    return await query.OrderBy(b => b.Title).ToListAsync();

}
    public async Task<IDbContextTransaction> BeginTransactionAsync()
{
    return await _context.Database.BeginTransactionAsync();
}
}