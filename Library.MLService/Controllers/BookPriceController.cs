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

    public record BookPriceRequest(string Title, int AuthorId, int GenreId);
    public record BookPriceResponse(float PredictedPrice);

    [HttpPost("predict")]
    public ActionResult<BookPriceResponse> Predict([FromBody] BookPriceRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
            return BadRequest("Title is required.");

        var input = new BookPriceData
        {
            Title = req.Title,
            AuthorId = req.AuthorId,
            GenreId = req.GenreId,
            Price = 0
        };

        var pred = _pool.Predict(modelName: "BookPriceModel", example: input);
        return Ok(new BookPriceResponse(PredictedPrice: pred.Score));
    }
}