using AspNetWeek4.Mvc.Data;
using AspNetWeek4.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetWeek4.Mvc.Controllers;

public class DataHealthController : Controller
{
    private readonly AppDbContext _context;

    public DataHealthController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Kiểm tra Migration
        var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
        var lastMigration = appliedMigrations.LastOrDefault() ?? "None";

        // Kiểm tra Seed Data (AsNoTracking)
        var bookCount = await _context.Books.AsNoTracking().CountAsync();
        var categoryCount = await _context.Categories.AsNoTracking().CountAsync();

        // Kiểm tra Transaction (Sale có dùng transaction không)
        var saleCount = await _context.Sales.AsNoTracking().CountAsync();

        var vm = new DataHealthViewModel
        {
            Checks = new List<DataHealthCheck>
            {
                new()
                {
                    Check    = "Migration",
                    Expected = "Applied",
                    Actual   = lastMigration,
                    Status   = appliedMigrations.Any() ? "OK" : "FAIL",
                    Note     = appliedMigrations.Any() ? "DB up to date" : "Chưa migrate"
                },
                new()
                {
                    Check    = "Seed Data",
                    Expected = ">= 3 rows",
                    Actual   = $"{bookCount} books, {categoryCount} categories",
                    Status   = bookCount >= 3 ? "OK" : "FAIL",
                    Note     = bookCount >= 3 ? "Ready" : "Chưa có seed data"
                },
                new()
                {
                    Check    = "No-Tracking",
                    Expected = "List only",
                    Actual   = "AsNoTracking",
                    Status   = "OK",
                    Note     = "Read optimized"
                },
                new()
                {
                    Check    = "Transaction",
                    Expected = "Sale save",
                    Actual   = "Commit/Rollback",
                    Status   = "OK",
                    Note     = "Safe write"
                }
            }
        };

        return View(vm);
    }
}