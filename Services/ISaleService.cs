using AspNetWeek4.Mvc.Models;
using AspNetWeek4.Mvc.ViewModels;

namespace AspNetWeek4.Mvc.Services;

public interface ISaleService
{
    Task<List<Sale>> GetOrdersAsync();

    Task<Sale?> GetOrderByIdAsync(int id);

    Task CreateOrderAsync(SaleCreateViewModel model);
    Task<List<SaleHistoryViewModel>> GetSaleHistoryAsync();
}