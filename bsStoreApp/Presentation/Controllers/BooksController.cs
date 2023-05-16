using Entities.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Services.Contracts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _manager;

    public BooksController(IServiceManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        return Ok(_manager.BookService.GetAllBooks(false));
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        return StatusCode(201, _manager.BookService.CreateOneBook(book));
    }

    [HttpGet("{id}")]
    public IActionResult GetOneBook([FromRoute] int id)
    {
        var book = _manager.BookService.GetOneBookById(id, false);

        return Ok(book);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOneBook([FromRoute] int id, [FromBody] Book book)
    {
        _manager.BookService.UpdateOneBook(id, book, true);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOneBook([FromRoute] int id)
    {
        _manager.BookService.DeleteOneBook(id, false);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute] int id,
        [FromBody] JsonPatchDocument<Book> bookPatch)
    {
        var entity = _manager.BookService.GetOneBookById(id, true);

        bookPatch.ApplyTo(entity);
        _manager.BookService.UpdateOneBook(id, entity, true);

        return NoContent();
    }
}
