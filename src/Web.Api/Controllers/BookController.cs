using Application.Books;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Route("api/book")]
public class BookController : ControllerBase, IBookAppService
{
    private readonly IBookAppService _bookAppService;

    public BookController(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }

    [Route("{id}")]
    [HttpGet()]
    public async Task<BookDto> GetAsync(Guid id)
    {
        return await _bookAppService.GetAsync(id);
    }

    [HttpPost]
    public async Task<BookDto> CreateAsync([FromBody] BookDto book)
    {
        return await _bookAppService.CreateAsync(book);
    }

    [Route("{id}")]
    [HttpPut()]
    public async Task<BookDto> UpdateAsync([FromRoute] Guid id, [FromBody] CreateUpdateBookDto book)
    {
        return await _bookAppService.UpdateAsync(id, book);
    }

    [Route("{id}")]
    [HttpDelete()]
    public async Task DeleteAsync([FromRoute] Guid id)
    {
        await _bookAppService.DeleteAsync(id);
    }

    [HttpGet]
    public async Task<List<BookDto>> GetAllAsync()
    {
        return await _bookAppService.GetAllAsync();
    }
}
