using Library.MLService.ML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;

namespace Library.MLService.Controllers;

[ApiController]
[Route("api/bookprice")]
public class BookPriceController : ControllerBase
{
    private readonly PredictionEnginePool<BookPriceData, BookPricePrediction> _pool;

    public BookPriceController(PredictionEnginePool<BookPriceData, BookPricePrediction> pool)
    {
        _pool = pool;
    }

    public record PredictRequest(string Title, string BookCategory, int StarRating, int Quantity);
    public record PredictResponse(float PredictedPrice);

    [HttpPost("predict")]
    public ActionResult<PredictResponse> Predict([FromBody] PredictRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
            return BadRequest("Title is required.");
        if (string.IsNullOrWhiteSpace(req.BookCategory))
            return BadRequest("BookCategory is required.");

        var input = new BookPriceData
        {
            Title = req.Title,
            BookCategory = req.BookCategory,
            StarRating = req.StarRating,
            Quantity = req.Quantity,
            Price = 0
        };

        var pred = _pool.Predict("BookPriceModel", input);
        return Ok(new PredictResponse(pred.Score));
    }
}