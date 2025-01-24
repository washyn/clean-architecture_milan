using Application.Abstractions.Data;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace DbMigrator;

public class BookDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ILogger<BookDataSeedContributor> _logger;
    private readonly IApplicationDbContext _context;

    public BookDataSeedContributor(
        IApplicationDbContext context,
        ILogger<BookDataSeedContributor> logger
        )
    {
        _logger = logger;
        _context = context;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding books...");
        _context.Books.Add(new Domain.Books.Book
        {
            Title = "Clean Architecture",
            Author = "Robert C. Martin",
            Description = "Clean Architecture: A Craftsman's Guide to Software Structure and Design"
        });
        _context.Books.Add(new Domain.Books.Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            Description = "Clean Code: A Handbook of Agile Software Craftsmanship"
        });
        _context.Books.Add(new Domain.Books.Book
        {
            Title = "Clean Coder",
            Author = "Robert C. Martin",
            Description = "Clean Coder: A Handbook of Software Craftsmanship"
        });
        _context.Books.Add(new Domain.Books.Book
        {
            Title = "Clean Architecture",
            Author = "Robert C. Martin",
            Description = "Clean Architecture: A Craftsman's Guide to Software Structure and Design"
        });
        await _context.SaveChangesAsync();
    }
}
