using Entities.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Repositories.Contracts;
using Repositories.EFCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IRepositoryManager _manager;

    public BooksController(IRepositoryManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        return Ok(_manager.Book.GetAllBooks(false));
    }

    [HttpGet("{id}")]
    public IActionResult GetOneBook([FromRoute] int id)
    {
        var book = _manager.Book.GetBookById(id, false);
        _manager.Save();

        if (book is null) return NotFound();

        return Ok(book);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOneBook([FromRoute] int id, [FromBody] Book book)
    {
        var entity = _manager.Book.GetBookById(id, true);

        if (entity is null) return NotFound();

        if (id != book.Id) return BadRequest();

        entity.Title = book.Title;
        entity.Price = book.Price;

        // _manager.Book.UpdateOneBook(entity); // not used in course ??
        _manager.Save();

        return Ok(book);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOneBook([FromRoute] int id)
    {
        var entity = _manager.Book.GetBookById(id, false);

        if (entity is null) return NotFound();

        _manager.Book.DeleteOneBook(entity);
        _manager.Save();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute] int id,
        [FromBody] JsonPatchDocument<Book> bookPatch)
    {
        var entity = _manager.Book.GetBookById(id, true);

        if (entity is null) return NotFound();

        bookPatch.ApplyTo(entity);
        _manager.Book.Update(entity);

        return Ok();
    }
}
