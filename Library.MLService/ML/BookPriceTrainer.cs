using Library.MLService.ML;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace Library.MLService.ML;

public static class BookPriceTrainer
{
    private class CsvRow
    {
        public string Title { get; set; } = "";

        [Name("Book_category")]
        public string BookCategory { get; set; } = "";

        [Name("Star_rating")]
        public string StarRating { get; set; } = "";

        public float Price { get; set; }
        public int Quantity { get; set; }
    }

    private static float RatingToNumber(string s) => s.Trim() switch
    {
        "One" => 1f,
        "Two" => 2f,
        "Three" => 3f,
        "Four" => 4f,
        "Five" => 5f,
        _ => 0f
    };

    public static void EnsureModelTrained(string csvPath, string modelPath)
    {
        if (File.Exists(modelPath))
            return;

        if (!File.Exists(csvPath))
            throw new FileNotFoundException($"CSV not found at: {csvPath}");

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var rows = csv.GetRecords<CsvRow>().ToList();

        var data = rows.Select(r => new BookPriceData
        {
            Title = r.Title,
            BookCategory = r.BookCategory,
            StarRating = RatingToNumber(r.StarRating),
            Quantity = r.Quantity,
            Price = r.Price
        }).ToList();

        var ml = new MLContext(seed: 1);
        var dv = ml.Data.LoadFromEnumerable(data);
        var split = ml.Data.TrainTestSplit(dv, testFraction: 0.2);

        var pipeline =
            ml.Transforms.CopyColumns("Label", nameof(BookPriceData.Price))
            .Append(ml.Transforms.Text.FeaturizeText("TitleFeats", nameof(BookPriceData.Title)))
            .Append(ml.Transforms.Categorical.OneHotEncoding("CatFeats", nameof(BookPriceData.BookCategory)))
            .Append(ml.Transforms.Concatenate("Features",
                "TitleFeats", "CatFeats", nameof(BookPriceData.StarRating), nameof(BookPriceData.Quantity)))
            .Append(ml.Regression.Trainers.FastTree());

        var model = pipeline.Fit(split.TrainSet);

        Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);
        ml.Model.Save(model, split.TrainSet.Schema, modelPath);
    }
}