using Microsoft.AspNetCore.Mvc;

namespace Remedium.Web.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index() => View();
    }
}