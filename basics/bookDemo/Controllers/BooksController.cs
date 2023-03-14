using System.Linq;
using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{id}")]
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

    [HttpPut("{id}")]
    public IActionResult UpdateBook([FromRoute] int id, [FromBody] Book bookModel)
    {
        Book? book = ApplicationContext.Books.Find(b => b.Id == id);
        if (book is null) return NotFound();

        book.Title = bookModel.Title;
        book.Price = bookModel.Price;

        return Ok(book);
    }

    [HttpDelete]
    public IActionResult DeleteAllBooks()
    {
        ApplicationContext.Books.Clear();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook([FromRoute] int id)
    {
        Book? book = ApplicationContext.Books.Find(x => x.Id == id);

        if (book is null)
        {
            return NotFound(new
            {
                statusCode = 404,
                message = $"Book with id:{id} not found!!"
            });
        }

        ApplicationContext.Books.Remove(book);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PartiallyUpdateBook([FromRoute] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
    {
        Book? book = ApplicationContext.Books.Find(x => x.Id == id);

        if (book is null) return NotFound();

        bookPatch.ApplyTo(book);

        return NoContent();
    }
}
