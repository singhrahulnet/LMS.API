using LMS.Model;
using Microsoft.EntityFrameworkCore;

namespace LMS.Persistence
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int Save();
    }
    public class DataContext : DbContext, IDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<IssuedBook> IssuedBooks { get; set; }

        public int Save()
        {
            return base.SaveChanges();
        }
    }
}
