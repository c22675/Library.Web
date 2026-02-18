using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models;

public class BookPricePredictVm
{
    [Required]
    public string Title { get; set; } = "";

    [Display(Name = "Author")]
    public int AuthorId { get; set; }

    [Display(Name = "Genre")]
    public int GenreId { get; set; }

    public SelectList? Authors { get; set; }
    public SelectList? Genres { get; set; }

    public float? PredictedPrice { get; set; }
    public string? Error { get; set; }
}