using AutoMapper;

using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;

using Repositories.Contracts;

using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;
    private readonly IMapper _mapper;

    public BookManager(IRepositoryManager manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    public Book CreateOneBook(Book book)
    {
        _manager.Book.CreateOneBook(book);
        _manager.Save();
        return book;
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = GetOneBookById(id, trackChanges);

        _manager.Book.DeleteOneBook(entity);
        _manager.Save();
    }

    public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
    {
        var books = _manager.Book.GetAllBooks(trackChanges);
        var mappedBooks = _mapper.Map<IEnumerable<BookDto>>(books);
        return mappedBooks;
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);
        if (entity is null)
            throw new BookNotFoundException(id);

        return entity;
    }

    public void UpdateOneBook(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges)
    {
        var entity = GetOneBookById(id, trackChanges);

        entity = _mapper.Map<Book>(bookDtoForUpdate);

        _manager.Book.UpdateOneBook(entity); // if entity tracking is true, save will update without needing this line
        _manager.Save();
    }
}
