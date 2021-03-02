using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remedium.Web.Data;

namespace Remedium.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository _repository;

        
        public BlogController(IRepository repository)
        {
            _repository = repository;
        }


        public async Task<IActionResult> Index() => View(await _repository.GetAllBlogPostsAsync());
    }
}