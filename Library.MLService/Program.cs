using Library.Data;
using Microsoft.EntityFrameworkCore;
using Library.MLService.ML;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var modelPath = Path.Combine(builder.Environment.ContentRootPath, "MLModels", "BookPriceModel.zip");
var csvPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "books_scraped.csv");

BookPriceTrainer.EnsureModelTrained(csvPath, modelPath);

builder.Services.AddPredictionEnginePool<BookPriceData, BookPricePrediction>()
    .FromFile(modelName: "BookPriceModel", filePath: modelPath, watchForChanges: true);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
