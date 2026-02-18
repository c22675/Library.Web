using Library.Data;
using Library.Web.Models;
using Library.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Controllers;

public class MlController : Controller
{
    private readonly AppDbContext _db;
    private readonly MlServiceClient _ml;

    public MlController(AppDbContext db, MlServiceClient ml)
    {
        _db = db;
        _ml = ml;
    }

    private void LoadCategories(BookPricePredictVm vm)
    {
        var categories = new[]
        {
        "Travel", "Mystery", "Historical Fiction", "Classics", "Science Fiction",
        "Fantasy", "Romance", "Nonfiction", "Thriller", "Young Adult"
    };

        vm.Categories = new SelectList(categories, vm.BookCategory);
    }

    [HttpGet]
    public IActionResult PredictBookPrice()
    {
        var vm = new BookPricePredictVm();
        LoadCategories(vm);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> PredictBookPrice(BookPricePredictVm vm)
    {
        LoadCategories(vm);

        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            var result = await _ml.PredictBookPriceAsync(
                new Library.Web.Services.MlServiceClient.BookPriceRequest(
                    vm.Title, vm.BookCategory, vm.StarRating, vm.Quantity));

            vm.PredictedPrice = result?.PredictedPrice;
        }
        catch (Exception ex)
        {
            vm.Error = ex.Message;
        }

        return View(vm);
    }
}