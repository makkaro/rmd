using System;
using System.Threading.Tasks;
using Remedium.Web.Data.Entities;

namespace Remedium.Web.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task<Boolean> SaveChangesAsync();
        Task<BlogPost[]> GetAllBlogPostsAsync();
    }
}