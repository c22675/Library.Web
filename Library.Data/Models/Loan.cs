using System.ComponentModel.DataAnnotations;

namespace Library.Data.Models;

public class Loan
{
    public int LoanId { get; set; }

    [Display(Name = "Book")]
    public int BookId { get; set; }
    public Book? Book { get; set; }

    [Display(Name = "Customer")]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [DataType(DataType.Date)]
    public DateTime LoanDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ReturnDate { get; set; }
}