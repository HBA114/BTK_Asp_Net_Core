using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public abstract record BookDtoForManipulation
{
    [Required(ErrorMessage = "Title is a required field!")]
    [MinLength(2, ErrorMessage = "Title must consist of at least 2 character!")]
    [MaxLength(50, ErrorMessage = "Title must consist maximum 50 character!")]
    public string Title { get; init; }

    [Required(ErrorMessage = "Price is a required field!")]
    [Range(10, 1000)]
    public decimal Price { get; init; }
}
