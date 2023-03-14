using hello_empty_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace hello_empty_web_api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMessage()
    {
        var result = new ResponseModel()
        {
            HttpStatus = 200,
            Message = "Asp.Net Core Home Controller"
        };

        return Ok(result); // BadRequest(); NotFound(); Created();
    }
}
