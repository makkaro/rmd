using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remedium.Web.Data;
using Remedium.Web.Data.Entities;
using Remedium.Web.Utilities;

namespace Remedium.Web.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index() => View
        (
            await _context.Articles
                .Include(x => x.Author)
                .Include(x => x.LastUpdateAuthor)
                .OrderByDescending(x => x.Timestamp)
                .ToArrayAsync()
        );
        
        [Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> Update(Int32? id) => await _context.Articles.FindAsync(id) switch
        {
            var article when article is not null => View(article),
            _ when id is null => View(new Article()),
            _ => NotFound()
        };
        
        [HttpPost, Authorize(Policy = "RequireAdministrator")]
        public async Task<IActionResult> Update(Article formData)
        {
            if (!ModelState.IsValid) return Json(new
            {
                valid = false,
                viewResult = await this.RenderViewAsync(nameof(Update), formData)
            });

            if (formData.Id == default) _context.Add(formData with
            {
                Author = await _userManager.FindByNameAsync(User.Identity!.Name),
                Timestamp = DateTime.Now
            });
            else _context.Update(formData with
            {
                LastUpdateAuthor = await _userManager.FindByNameAsync(User.Identity!.Name),
                LastUpdateTimestamp = DateTime.Now
            });
            
            if (await _context.SaveChangesAsync() > 0)
            {
                return Json(new
                {
                    valid = true,
                    viewResult = await this.RenderViewAsync
                    (
                        model: await _context.Articles
                            .Include(x => x.Author)
                            .Include(x => x.LastUpdateAuthor)
                            .OrderByDescending(x => x.Timestamp)
                            .ToArrayAsync(),
                        view: nameof(_ViewAll),
                        partialView: true
                    )
                });
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public async Task<IActionResult> Delete(Int32 id) => await _context.Articles.FindAsync(id) switch
        {
            var article when article is not null => View(article),
            _ => NotFound()
        };

        [HttpPost]
        public async Task<IActionResult> Delete(Article formData)
        {
            _context.Remove(await _context.Articles.FindAsync(formData.Id));
            if (await _context.SaveChangesAsync() > 0)
            {
                return Json(new
                {
                    valid = true,
                    viewResult = await this.RenderViewAsync
                    (
                        model: await _context.Articles
                            .Include(x => x.Author)
                            .Include(x => x.LastUpdateAuthor)
                            .OrderByDescending(x => x.Timestamp)
                            .ToArrayAsync(),
                        view: nameof(_ViewAll),
                        partialView: true
                    )
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        
        [NonAction] private static void _ViewAll() => throw new NotImplementedException();
    }
}