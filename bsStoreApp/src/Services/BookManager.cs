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

    public BookDto CreateOneBook(BookDtoForInsertion bookDtoForInsertion)
    {
        Book book = _mapper.Map<Book>(bookDtoForInsertion);
        _manager.Book.CreateOneBook(book);
        _manager.Save();
        return _mapper.Map<BookDto>(book);
    }

    public async void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);

        _manager.Book.DeleteOneBook(entity);
        _manager.Save();
    }

    public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
    {
        var books = _manager.Book.GetAllBooks(trackChanges);
        var mappedBooks = _mapper.Map<IEnumerable<BookDto>>(books);
        return mappedBooks;
    }

    public BookDto GetOneBookById(int id, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);
        if (entity is null)
            throw new BookNotFoundException(id);

        return _mapper.Map<BookDto>(entity);
    }

    public void UpdateOneBook(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);

        // entity = _mapper.Map<Book>(bookDtoForUpdate);    //! using this line causes:
        //! 500
        //! Error Message:  The instance of entity type 'Book' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.

        entity.Title = bookDtoForUpdate.Title;
        entity.Price = bookDtoForUpdate.Price;

        _manager.Book.UpdateOneBook(entity); // if entity tracking is true, save will update without needing this line
        _manager.Save();
    }
}
