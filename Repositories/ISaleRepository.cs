using AspNetWeek4.Mvc.Models;

namespace AspNetWeek4.Mvc.Repositories;

public interface ISaleRepository
{
    Task<List<Sale>> GetAllAsync();
    Task<Sale?> GetByIdAsync(int id);
    Task AddAsync(Sale sale);
    Task SaveChangesAsync();
    Task<List<Sale>> GetAllReadOnlyAsync();
}