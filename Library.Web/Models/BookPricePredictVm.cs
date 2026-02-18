using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models;

public class BookPricePredictVm
{
    [Required]
    public string Title { get; set; } = "";

    [Required]
    [Display(Name = "Category")]
    public string BookCategory { get; set; } = "";

    [Range(1, 5)]
    [Display(Name = "Star rating (1-5)")]
    public int StarRating { get; set; } = 3;

    [Range(0, 100000)]
    public int Quantity { get; set; } = 1;

    public SelectList? Categories { get; set; }

    public float? PredictedPrice { get; set; }
    public string? Error { get; set; }
}