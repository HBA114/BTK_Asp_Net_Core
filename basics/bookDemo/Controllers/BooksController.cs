using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = ApplicationContext.Books;
        return Ok(books);
    }

    [HttpGet("/{id}")]
    public IActionResult GetBookById([FromRoute] int id)
    {
        var book = ApplicationContext.Books.Where(x => x.Id == id).FirstOrDefault();

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    public IActionResult CreateBook([FromBody] Book book)
    {
        if (book is null) return BadRequest();

        ApplicationContext.Books.Add(book);
        
        return StatusCode(201);
    }
}