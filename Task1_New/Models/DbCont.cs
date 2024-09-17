using Microsoft.EntityFrameworkCore;
using Task1_New.Models;

namespace Task1_New
{
    public class DbCont : DbContext
    {
        public DbCont(DbContextOptions<DbCont> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<ParentInfo> Parents { get; set; }
    }
}