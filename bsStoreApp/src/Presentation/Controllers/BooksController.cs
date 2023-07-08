using Entities.Dtos;

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
    public async Task<IActionResult> GetAllBooks()
    {
        return Ok(await _manager.BookService.GetAllBooksAsync(false));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion bookDtoForInsertion)
    {
        if (bookDtoForInsertion is null) 
            return BadRequest();    // 400

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState); // 422

        return StatusCode(201, await _manager.BookService.CreateOneBookAsync(bookDtoForInsertion));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneBook([FromRoute] int id)
    {
        var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

        return Ok(book);    // 200
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOneBook([FromRoute] int id, [FromBody] BookDtoForUpdate bookDtoForUpdate)
    {
        if (bookDtoForUpdate is null)
            return BadRequest();    // 400

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState); // 422

        await _manager.BookService.UpdateOneBookAsync(id, bookDtoForUpdate, true);

        return NoContent(); // 204
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOneBook([FromRoute] int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, false);

        return NoContent(); // 204
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute] int id,
        [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
    {
        if (bookPatch is null) 
            return BadRequest();    // 400
        
        var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

        bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

        TryValidateModel(result.bookDtoForUpdate);

        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState); // 422

        await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

        return NoContent(); // 204
    }
}
