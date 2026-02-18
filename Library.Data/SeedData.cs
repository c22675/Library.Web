using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        context.Database.Migrate();

        if (context.Authors.Any() || context.Genres.Any() || context.Books.Any())
            return;

        var authors = new[]
        {
            new Author { FullName = "Mihai Eminescu" },
            new Author { FullName = "Ion Creanga" },
            new Author { FullName = "Marin Preda" },
            new Author { FullName = "George Orwell" },
        };

        var genres = new[]
        {
            new Genre { Name = "Clasic" },
            new Genre { Name = "Fictiune" },
            new Genre { Name = "Dystopia" },
            new Genre { Name = "Children" },
        };

        context.Authors.AddRange(authors);
        context.Genres.AddRange(genres);
        context.SaveChanges();

        var books = new[]
        {
            new Book { Title = "Morometii", Price = 45, AuthorId = authors[2].AuthorId, GenreId = genres[1].GenreId },
            new Book { Title = "1984", Price = 35, AuthorId = authors[3].AuthorId, GenreId = genres[2].GenreId },
            new Book { Title = "Amintiri din copilarie", Price = 25, AuthorId = authors[1].AuthorId, GenreId = genres[3].GenreId },
            new Book { Title = "Poezii", Price = 30, AuthorId = authors[0].AuthorId, GenreId = genres[0].GenreId },
        };
        context.Books.AddRange(books);

        var customers = new[]
        {
            new Customer { Name = "Ana Popescu", Address = "Bucuresti", BirthDate = new DateTime(2000, 5, 10) },
            new Customer { Name = "Radu Ionescu", Address = "Cluj", BirthDate = new DateTime(1998, 2, 3) },
        };
        context.Customers.AddRange(customers);

        context.SaveChanges();

        var loans = new[]
        {
            new Loan { BookId = books[0].BookId, CustomerId = customers[0].CustomerId, LoanDate = DateTime.Today.AddDays(-10), ReturnDate = null },
            new Loan { BookId = books[1].BookId, CustomerId = customers[1].CustomerId, LoanDate = DateTime.Today.AddDays(-30), ReturnDate = DateTime.Today.AddDays(-5) },
        };
        context.Loans.AddRange(loans);

        context.SaveChanges();
    }
}