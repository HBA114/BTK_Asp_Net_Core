using Entities.Models;

using Repositories.Contracts;

using Services.Contracts;

namespace Services;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _manager;

    public BookManager(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public Book CreateOneBook(Book book)
    {
        _manager.Book.CreateOneBook(book);
        _manager.Save();
        return book;
    }

    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);
        if (entity is null) throw new Exception($"Book with id : {id} not found!");
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
        if (entity is null) throw new Exception($"Book with id : {id} not found!");
        return entity;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _manager.Book.GetBookById(id, trackChanges);
        if (entity is null) throw new Exception($"Book with id : {id} not found!");

        // check params
        if (book is null) throw new ArgumentNullException(nameof(book));

        entity.Title = book.Title;
        entity.Price = book.Price;

        _manager.Book.UpdateOneBook(entity);    // if entity tracking is true, save will update without needing this line
        _manager.Save();
    }
}
