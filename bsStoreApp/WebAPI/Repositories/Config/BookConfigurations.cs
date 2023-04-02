using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Models;

namespace WebAPI.Repositories.Config;

public class BookConfigurations : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book { Id = 1, Title = "1984", Price = 75 },
            new Book { Id = 2, Title = "Sherlock Holmes", Price = 80 },
            new Book { Id = 3, Title = "Harry Potter", Price = 90 }
        );
    }
}
