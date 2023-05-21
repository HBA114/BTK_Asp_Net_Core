namespace Entities.Exceptions;

public sealed class BookNotFound : NotFoundException     // sealed classes can not inheritable
{
    public BookNotFound(int id) : base($"The book with id {id} not found!")
    {
    }
}
