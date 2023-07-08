namespace Entities.Dtos;

public record BookDtoForInsertion : BookDtoForManipulation
{
    public BookDtoForInsertion(string title, decimal price) : base (title, price)
    {
    }
    
}
