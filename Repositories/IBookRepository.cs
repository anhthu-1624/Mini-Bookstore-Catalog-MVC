using AspNetWeek4.Mvc.Models;

namespace AspNetWeek4.Mvc.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();
    Task<List<Book>> GetAllReadOnlyAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task SaveChangesAsync();
    Task<List<Book>> GetFilteredReadOnlyAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
    Task<IDbContextTransaction> BeginTransactionAsync();
}
