namespace Library.MLService.ML;

public class BookPriceData
{
    public string Title { get; set; } = "";
    public string BookCategory { get; set; } = "";
    public float StarRating { get; set; }
    public float Quantity { get; set; }

    public float Price { get; set; }
}

public class BookPricePrediction
{
    public float Score { get; set; }
}