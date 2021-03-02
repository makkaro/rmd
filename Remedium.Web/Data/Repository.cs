using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Remedium.Web.Data.Entities;

namespace Remedium.Web.Data
{
    public sealed class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;


        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add<T>(T entity) where T : class => _context.Add(entity);

        public void Remove<T>(T entity) where T : class => _context.Remove(entity);

        public async Task<Boolean> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public async Task<BlogPost[]> GetAllBlogPostsAsync()
        {
            return await _context.BlogPosts.OrderByDescending(x => x.Timestamp).ToArrayAsync();
        }
    }
}