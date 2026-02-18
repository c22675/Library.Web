using Library.Data;
using Library.MLService.ML;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace Library.MLService.ML;

public static class BookPriceTrainer
{
    public static void EnsureModelTrained(IServiceProvider services, string modelPath)
    {
        if (File.Exists(modelPath))
            return;

        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // luăm cărțile existente (training set)
        var books = db.Books.AsNoTracking().ToList();

        // dacă sunt prea puține, generăm câteva exemple sintetice (pentru ca ML să aibă cu ce lucra)
        var data = books.Select(b => new BookPriceData
        {
            Title = b.Title,
            AuthorId = b.AuthorId,
            GenreId = b.GenreId,
            Price = (float)b.Price
        }).ToList();

        if (data.Count < 20)
        {
            var rnd = new Random(1);
            for (int i = data.Count; i < 30; i++)
            {
                var baseItem = data[rnd.Next(data.Count)];
                data.Add(new BookPriceData
                {
                    Title = baseItem.Title + " " + (char)('A' + rnd.Next(0, 26)),
                    AuthorId = baseItem.AuthorId,
                    GenreId = baseItem.GenreId,
                    Price = Math.Max(10, baseItem.Price + rnd.Next(-8, 15))
                });
            }
        }

        var ml = new MLContext(seed: 1);
        var dv = ml.Data.LoadFromEnumerable(data);

        var split = ml.Data.TrainTestSplit(dv, testFraction: 0.2);

        var pipeline =
            ml.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(BookPriceData.Price))
            .Append(ml.Transforms.Text.FeaturizeText(outputColumnName: "TitleFeats", inputColumnName: nameof(BookPriceData.Title)))
            .Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "AuthorFeats", inputColumnName: nameof(BookPriceData.AuthorId)))
            .Append(ml.Transforms.Categorical.OneHotEncoding(outputColumnName: "GenreFeats", inputColumnName: nameof(BookPriceData.GenreId)))
            .Append(ml.Transforms.Concatenate("Features", "TitleFeats", "AuthorFeats", "GenreFeats"))
            .Append(ml.Regression.Trainers.FastTree());

        var model = pipeline.Fit(split.TrainSet);

        Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);
        ml.Model.Save(model, split.TrainSet.Schema, modelPath);
    }
}