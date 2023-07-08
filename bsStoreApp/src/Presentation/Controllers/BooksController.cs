using Entities.Dtos;
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
    public IActionResult CreateOneBook([FromBody] BookDtoForInsertion bookDtoForInsertion)
    {
        if (bookDtoForInsertion is null) 
            return BadRequest();    // 400

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState); // 422

        return StatusCode(201, _manager.BookService.CreateOneBook(bookDtoForInsertion));
    }

    [HttpGet("{id}")]
    public IActionResult GetOneBook([FromRoute] int id)
    {
        var book = _manager.BookService.GetOneBookById(id, false);

        return Ok(book);    // 200
    }

    [HttpPut("{id}")]
    public IActionResult UpdateOneBook([FromRoute] int id, [FromBody] BookDtoForUpdate bookDtoForUpdate)
    {
        if (bookDtoForUpdate is null)
            return BadRequest();    // 400

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState); // 422

        _manager.BookService.UpdateOneBook(id, bookDtoForUpdate, true);

        return NoContent(); // 204
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOneBook([FromRoute] int id)
    {
        _manager.BookService.DeleteOneBook(id, false);

        return NoContent(); // 204
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute] int id,
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
    {
        if (bookPatch is null) 
            return BadRequest();    // 400
        
        var result = _manager.BookService.GetOneBookForPatch(id, false);

        bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

        TryValidateModel(result.bookDtoForUpdate);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState); // 422

        _manager.BookService.SaveChangesForPatch(result.bookDtoForUpdate, result.book);

        return NoContent(); // 204
    }
}
