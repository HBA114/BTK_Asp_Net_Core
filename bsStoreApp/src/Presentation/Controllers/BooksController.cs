using Entities.Dtos;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Presentation.ActionFilters;

using Services.Contracts;

namespace Presentation.Controllers;

[ServiceFilter(typeof(LogFilterAttribute))]
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
    public async Task<IActionResult> GetAllBooksAsync()
    {
        return Ok(await _manager.BookService.GetAllBooksAsync(false));
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPost]
    public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDtoForInsertion)
    {
        var book = await _manager.BookService.CreateOneBookAsync(bookDtoForInsertion);

        return StatusCode(201, book);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneBookAsync([FromRoute] int id)
    {
        var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

        return Ok(book);    // 200
    }

    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOneBookAsync([FromRoute] int id, [FromBody] BookDtoForUpdate bookDtoForUpdate)
    {
       await _manager.BookService.UpdateOneBookAsync(id, bookDtoForUpdate, true);

        return NoContent(); // 204
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOneBookAsync([FromRoute] int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, false);

        return NoContent(); // 204
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute] int id,
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
