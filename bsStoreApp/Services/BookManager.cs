using Entities.Models;

using Repositories.Contracts;

using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;
    private readonly ILoggerService _loggerService;

    public BookManager(IRepositoryManager manager, ILoggerService loggerService)
    {
        _manager = manager;
        _loggerService = loggerService;
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

    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        return _manager.Book.GetAllBooks(trackChanges);
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);
        if (entity is null)
        {
            _loggerService.LogWarning($"Book with id : {id} not found!");
            throw new Exception($"Book with id : {id} not found!");
        }

        return entity;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = GetOneBookById(id, trackChanges);

        entity.Title = book.Title;
        entity.Price = book.Price;

        _manager.Book.UpdateOneBook(entity); // if entity tracking is true, save will update without needing this line
        _manager.Save();
    }
}
