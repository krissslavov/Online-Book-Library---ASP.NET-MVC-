namespace OBL.Data.Context
{
    using OBL.Data.Entities;
    using System.Data.Entity;

    public class BookCatalogDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
