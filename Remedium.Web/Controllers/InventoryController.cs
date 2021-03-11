using Microsoft.AspNetCore.Mvc;

namespace Remedium.Web.Controllers
{
    public sealed class InventoryController : ControllerBase
    {
        public IActionResult Index() => RedirectToAction("Index", "NotImplemented");
    }
}