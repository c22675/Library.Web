using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models;

public class Customer
{
    public int CustomerId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}