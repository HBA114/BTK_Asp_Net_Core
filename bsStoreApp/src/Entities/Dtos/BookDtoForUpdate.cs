using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public record BookDtoForUpdate : BookDtoForManipulation
{
    [Required]
    public int Id { get; init; }

    public BookDtoForUpdate(int id, string title, decimal price) : base (title,price)
    {
        Id = id;
    }
    
}
