using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WebAPI.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
}
