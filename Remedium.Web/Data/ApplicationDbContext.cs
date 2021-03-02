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


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientQuantity> IngredientQuantities { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }
    }
}