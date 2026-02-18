using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models;

public class Genre
{
    public int GenreId { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}