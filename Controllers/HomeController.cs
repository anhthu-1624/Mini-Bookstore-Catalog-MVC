using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetWeek4.Mvc.Models;
using AspNetWeek4.Mvc.Services;
using AspNetWeek4.Mvc.ViewModels;

namespace AspNetWeek4.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IBookService _bookService;

    public HomeController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _bookService.GetBookListAsync(
            keyword: "",
            genre: "",
            stock: "",
            sortBy: "newest",
            page: 1,
            pageSize: 3
        );

        var vm = new HomeIndexViewModel
        {
            FeaturedBooks = result.Items
        };

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}