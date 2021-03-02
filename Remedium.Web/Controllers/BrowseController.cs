using Microsoft.AspNetCore.Mvc;

namespace Remedium.Web.Controllers
{
    public class BrowseController : Controller
    {
        public IActionResult Index() => View();
    }
}