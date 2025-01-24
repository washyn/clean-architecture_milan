using System.ComponentModel.DataAnnotations;
using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Application.Books;

public interface IBookAppService : ITransientDependency
{
    Task<BookDto> GetAsync(Guid id);
    Task CreateAsync(CreateUpdateBookDto book);
    Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto book);
    Task DeleteAsync(Guid id);
    Task<List<BookDto>> GetAllAsync();
}

public class BookAppService : IBookAppService
{
    private readonly IApplicationDbContext _context;

    public BookAppService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<BookDto> GetAsync(Guid id)
    {
        var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == id) ?? throw new ApplicationException("Book not found");
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description
        };
    }
    
    public async Task CreateAsync(CreateUpdateBookDto book)
    {
        _context.Books.Add(new Domain.Books.Book
        {
            Title = book.Title,
            Author = book.Author,
            Description = book.Description
        });
        await _context.SaveChangesAsync();
    }
    
    public async Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto book)
    {
        var bookToUpdate = await _context.Books.SingleOrDefaultAsync(b => b.Id == id) ?? throw new ApplicationException("Book not found");
        bookToUpdate.Title = book.Title;
        bookToUpdate.Author = book.Author;
        bookToUpdate.Description = book.Description;
        await _context.SaveChangesAsync();
        return new BookDto
        {
            Id = bookToUpdate.Id,
            Title = bookToUpdate.Title,
            Author = bookToUpdate.Author,
            Description = bookToUpdate.Description
        };
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var bookToDelete = await _context.Books.SingleOrDefaultAsync(b => b.Id == id) ?? throw new ApplicationException("Book not found");
        _context.Books.Remove(bookToDelete);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<BookDto>> GetAllAsync()
    {
        return await _context.Books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Description = b.Description
        }).ToListAsync();
    }
}


public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
}

public class CreateUpdateBookDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public string Description { get; set; }
}
