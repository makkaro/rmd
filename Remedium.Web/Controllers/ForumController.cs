using Microsoft.AspNetCore.Mvc;

namespace Remedium.Web.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Index() => View();
    }
}