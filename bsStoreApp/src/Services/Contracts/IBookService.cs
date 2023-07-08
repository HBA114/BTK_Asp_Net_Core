using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts;

public interface IBookService
{
    IEnumerable<BookDto> GetAllBooks(bool trackChanges);
    BookDto GetOneBookById(int id, bool trackChanges);
    BookDto CreateOneBook(BookDtoForInsertion bookDtoForInsertion);
    void UpdateOneBook(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
}
