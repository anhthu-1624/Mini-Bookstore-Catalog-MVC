using AspNetWeek4.Mvc.Data;
using AspNetWeek4.Mvc.Models;
using AspNetWeek4.Mvc.ViewModels;
using Microsoft.EntityFrameworkCore;
using AspNetWeek4.Mvc.Repositories;

namespace AspNetWeek4.Mvc.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly AppDbContext _context;

    public SaleService(ISaleRepository saleRepository, AppDbContext context)
    {
        _saleRepository = saleRepository;
        _context = context;
    }

    public async Task<List<Sale>> GetOrdersAsync()
    {
        return await _saleRepository.GetAllAsync();
    }

    public async Task<Sale?> GetOrderByIdAsync(int id)
    {
        return await _saleRepository.GetByIdAsync(id);
    }

    public async Task CreateOrderAsync(SaleCreateViewModel model)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == model.BookId);

            if (book == null)
                throw new Exception("Book not found");

            if (book.Stock < model.Quantity)
                throw new Exception("Not enough stock");

            var sale = new Sale
            {
                CreatedAt = DateTime.Now,
                TotalAmount = book.Price * model.Quantity
            };

            await _saleRepository.AddAsync(sale);      
            await _saleRepository.SaveChangesAsync();

            var item = new SaleItem
            {
                SaleId = sale.Id,
                BookId = book.Id,
                Quantity = model.Quantity,
                UnitPrice = book.Price
            };

            _context.SaleItems.Add(item);

            book.Stock -= model.Quantity;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<List<SaleHistoryViewModel>> GetSaleHistoryAsync()
{
    var sales = await _saleRepository.GetAllReadOnlyAsync();

        return sales
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SaleHistoryViewModel
            {
                Id          = s.Id,
                CreatedAt   = s.CreatedAt,
                TotalAmount = s.TotalAmount,
                TotalItems  = s.SaleItems.Sum(si => si.Quantity),
                Items = s.SaleItems.Select(si => new SaleItemRowViewModel
                {
                    BookTitle = si.Book?.Title ?? "",
                    BookCode  = si.Book?.BookCode ?? "",
                    Quantity  = si.Quantity,
                    UnitPrice = si.UnitPrice
                }).ToList()
            }).ToList();
}
}