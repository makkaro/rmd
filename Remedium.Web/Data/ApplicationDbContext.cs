using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Remedium.Web.Data.Entities;

namespace Remedium.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IConfiguration _config;


        public ApplicationDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        
        public DbSet<Article> Articles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Thread> Threads { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }
    }
}