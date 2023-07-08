using Entities.Models;

namespace Repositories.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
    Task<Book?> GetBookByIdAsync(int id, bool trackChanges);
    void CreateOneBook(Book book);
    void UpdateOneBook(Book book);
    void DeleteOneBook(Book book);
}
