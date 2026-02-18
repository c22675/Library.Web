namespace Library.MLService.ML;

public class BookPriceData
{
    public string Title { get; set; } = "";
    public float AuthorId { get; set; }
    public float GenreId { get; set; }
    public float Price { get; set; }
}

public class BookPricePrediction
{
    public float Score { get; set; }
}