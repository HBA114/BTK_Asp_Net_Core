using Microsoft.AspNetCore.Mvc;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly RepositoryContext _context;

    public BooksController(RepositoryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        return Ok(_context.Books.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetOneBook([FromRoute] int id)
    {
        var book = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

        if (book is null) return NotFound();
        
        return Ok(book);
    }
}
