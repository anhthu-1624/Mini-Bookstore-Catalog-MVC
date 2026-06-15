using AspNetWeek4.Mvc.Models;
using AspNetWeek4.Mvc.Services;
using AspNetWeek4.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetWeek4.Mvc.Controllers;

public class SalesController : Controller
{
    private readonly ISaleService  _saleService;
    private readonly IBookService  _bookService;

    public SalesController(ISaleService saleService, IBookService bookService)
    {
        _saleService = saleService;
        _bookService = bookService;
    }

    // GET /Sales
    public async Task<IActionResult> Index()
    {
        var sales = await _saleService.GetOrdersAsync();
        return View(sales);
    }

    // GET /Sales/Detail/5
    public async Task<IActionResult> Detail(int id)
    {
        var sale = await _saleService.GetOrderByIdAsync(id);
        if (sale == null) return NotFound();
        return View(sale);
    }

    // GET /Sales/Create
    public async Task<IActionResult> Create()
    {
        var books = await _bookService.GetBookListAsync("", "", "ok", "title", 1, 100);
        ViewBag.Books = new SelectList(books.Items, "Id", "Title");
        return View(new SaleCreateViewModel());
    }

    // POST /Sales/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaleCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var books = await _bookService.GetBookListAsync("", "", "ok", "title", 1, 100);
            ViewBag.Books = new SelectList(books.Items, "Id", "Title");
            return View(model);
        }

        try
        {
            await _saleService.CreateOrderAsync(model);
            TempData["Success"] = "Bán hàng thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var books = await _bookService.GetBookListAsync("", "", "ok", "title", 1, 100);
            ViewBag.Books = new SelectList(books.Items, "Id", "Title");
            return View(model);
        }
    }
    public async Task<IActionResult> History()
{
    var vm = await _saleService.GetSaleHistoryAsync();
    return View(vm);
}
}