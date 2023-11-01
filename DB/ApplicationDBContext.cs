using Microsoft.EntityFrameworkCore;
using myBlog.Models;
namespace myBlog.DB
{

    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() { }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DUYNK;Initial Catalog=OJTBlog;Integrated Security=True");
        }

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     builder.Entity<OrderDetail>().HasKey(table => new
        //     {
        //         table.OrderId,
        //         table.ProductId
        //     });
        // }


        public DbSet<Account> Accounts { get; set; }

        public DbSet<Blog> Blogs { get; set; }


    }

}