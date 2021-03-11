using Microsoft.AspNetCore.Mvc;

namespace Remedium.Web.Controllers
{
    public sealed class NotImplementedController : Controller
    {
        public IActionResult Index() => View();
    }
}