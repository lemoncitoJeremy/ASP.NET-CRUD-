using Microsoft.EntityFrameworkCore;
using myWebApp.Models.Domain;

namespace myWebApp.Data
{
    public class webAppDbContext : DbContext
    {
        public webAppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
