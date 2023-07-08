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

    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDtoForInsertion)
    {
        Book book = _mapper.Map<Book>(bookDtoForInsertion);
        _manager.Book.CreateOneBook(book);
        await _manager.SaveAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var book = await GetBookWithIdOrThrowException(id, trackChanges);

        _manager.Book.DeleteOneBook(book);
        await _manager.SaveAsync();
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
    {
        var books = await _manager.Book.GetAllBooksAsync(trackChanges);
        var mappedBooks = _mapper.Map<IEnumerable<BookDto>>(books);
        return mappedBooks;
    }

    public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
    {
        var book = await GetBookWithIdOrThrowException(id, trackChanges);

        return _mapper.Map<BookDto>(book);
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
    {
        var book = await GetBookWithIdOrThrowException(id, trackChanges);

        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        await _manager.SaveAsync();
    }

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDtoForUpdate, bool trackChanges)
    {
        var book = await GetBookWithIdOrThrowException(id, trackChanges);

        // entity = _mapper.Map<Book>(bookDtoForUpdate);    //! using this line causes:
        //! 500
        //! Error Message:  The instance of entity type 'Book' cannot be tracked because another instance with the same key value for {'Id'} is already being tracked. When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.

        book.Title = bookDtoForUpdate.Title;
        book.Price = bookDtoForUpdate.Price;

        _manager.Book.UpdateOneBook(book); // if entity tracking is true, save will update without needing this line
        await _manager.SaveAsync();
    }

    private async Task<Book> GetBookWithIdOrThrowException(int id, bool trackChanges)
    {
        var book = await _manager.Book.GetBookByIdAsync(id, trackChanges);

        if (book is null)
            throw new BookNotFoundException(id);
        
        return book;
    }
}
