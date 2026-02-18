using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models;

public class Book
{
    public int BookId { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Display(Name = "Author")]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    [Display(Name = "Genre")]
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}