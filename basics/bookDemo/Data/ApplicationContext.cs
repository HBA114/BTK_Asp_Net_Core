using bookDemo.Models;

namespace bookDemo.Data;

public static class ApplicationContext
{
    public static List<Book> Books { get; set; }
    static ApplicationContext()
    {
        Books = new List<Book>()
        {
            new() {Id=1, Title="Dede Korkut", Price=45},
            new() {Id=2, Title="Mesnevi", Price=42},
            new() {Id=3, Title="1984", Price=38},
        };
    }
}
