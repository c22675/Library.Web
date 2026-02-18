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

    private async Task LoadDropdowns(BookPricePredictVm vm)
    {
        var authors = await _db.Authors.OrderBy(a => a.FullName).ToListAsync();
        var genres = await _db.Genres.OrderBy(g => g.Name).ToListAsync();
        vm.Authors = new SelectList(authors, "AuthorId", "FullName", vm.AuthorId);
        vm.Genres = new SelectList(genres, "GenreId", "Name", vm.GenreId);
    }

    [HttpGet]
    public async Task<IActionResult> PredictBookPrice()
    {
        var vm = new BookPricePredictVm();
        await LoadDropdowns(vm);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> PredictBookPrice(BookPricePredictVm vm)
    {
        await LoadDropdowns(vm);

        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            var result = await _ml.PredictBookPriceAsync(
                new MlServiceClient.BookPriceRequest(vm.Title, vm.AuthorId, vm.GenreId));

            vm.PredictedPrice = result?.PredictedPrice;
        }
        catch (Exception ex)
        {
            vm.Error = ex.Message;
        }

        return View(vm);
    }
}