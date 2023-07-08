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
        var book = GetBookWithIdOrThrowException(id, trackChanges);

        _manager.Book.DeleteOneBook(book);
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
        var book = GetBookWithIdOrThrowException(id, trackChanges);

        return _mapper.Map<BookDto>(book);
    }

    public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
    {
        var book = GetBookWithIdOrThrowException(id, trackChanges);

        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

        return (bookDtoForUpdate, book);
    }

    public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        _manager.Save();
    }

    public void UpdateOneBook(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges)
    {
        var book = GetBookWithIdOrThrowException(id, trackChanges);

        // entity = _mapper.Map<Book>(bookDtoForUpdate);    //! using this line causes:
        //! 500
        //! Error Message:  The instance of entity type 'Book' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.

        book.Title = bookDtoForUpdate.Title;
        book.Price = bookDtoForUpdate.Price;

        _manager.Book.UpdateOneBook(book); // if entity tracking is true, save will update without needing this line
        _manager.Save();
    }

    private Book GetBookWithIdOrThrowException(int id, bool trackChanges)
    {
        var book = _manager.Book.GetBookById(id, trackChanges);

        if (book is null)
            throw new BookNotFoundException(id);
        
        return book;
    }
}
