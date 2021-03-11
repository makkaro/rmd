using Microsoft.AspNetCore.Mvc;

namespace Remedium.Web.Controllers
{
    public sealed class BrowseController : ControllerBase
    {
        public IActionResult Index() => RedirectToAction("Index", "NotImplemented");
    }
}