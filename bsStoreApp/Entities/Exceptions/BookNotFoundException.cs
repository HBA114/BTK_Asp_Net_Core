namespace Entities.Exceptions;

public sealed class BookNotFoundException : NotFoundException     // sealed classes can not inheritable
{
    public BookNotFoundException(int id) : base($"The book with id {id} not found!")
    {
    }
}
