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

namespace Remedium.Web.Controllers
{
    public sealed class ThreadController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        
        public ThreadController(IAuthorizationService authorizationService, ApplicationDbContext context, 
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _authorizationService = authorizationService;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(Int32 id)
        {
            var thread = _context.Threads
                .Include(x => x.Posts)
                .Single(x => x.Id == id);

            if (thread.Posts.Count <= 0)
            {
                _context.Remove(thread);

                if (await _context.SaveChangesAsync() <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return RedirectToAction(nameof(Index), "Forum");
            }

            ViewBag.Authorized = (await _authorizationService.AuthorizeAsync(User, "RequireModerator")).Succeeded;
            ViewBag.AnyoneSignedIn = _signInManager.IsSignedIn(User);
            if (ViewBag.AnyoneSignedIn)
            {
                ViewBag.User = await _userManager.FindByNameAsync(User.Identity!.Name);
            }

            return View
            (
                await _context.Posts
                    .Where(x => x.ThreadId == id)
                    .Include(x => x.Thread)
                    .Include(x => x.Author)
                    .OrderBy(x => x.Timestamp)
                    .ToArrayAsync()
            );
        }


        public async Task<IActionResult> Update(Int32 threadId, Int32? id) => await _context.Posts.FindAsync(id) switch
        {
            var post when post is not null => View(post),
            _ when id is null => View(new Post {ThreadId = threadId}),
            _ => NotFound()
        };

        [HttpPost]
        public async Task<IActionResult> Update(Post formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var timestamp = DateTime.Now;
            
            if (formData.Id == default)
            {
                _context.Add(formData with
                {
                    Timestamp = timestamp, 
                    Author = await _userManager.FindByNameAsync(User.Identity!.Name)
                });
            }
            else
            {
                _context.Update(formData);
            }

            var thread = await _context.Threads.FindAsync(formData.ThreadId);
            thread.LastUpdateTimestamp = timestamp;
            _context.Update(thread);

            if (await _context.SaveChangesAsync() <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            return RedirectToAction(nameof(Index), new {id = thread.Id});
        }

        public async Task<IActionResult> Delete(Int32 id) => await _context.Posts.FindAsync(id) switch
        {
            var post when post is not null => View(post),
            _ => NotFound()
        };

        [HttpPost]
        public async Task<IActionResult> Delete(Post formData)
        {
            _context.Remove(formData);
            
            if (await _context.SaveChangesAsync() <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var thread = _context.Threads
                .Include(x => x.Posts)
                .Single(x => x.Id == formData.ThreadId);

            if (thread.Posts.Count > 0)
            {
                return RedirectToAction(nameof(Index), new {id = formData.ThreadId});
            }
            
            _context.Remove(thread);

            if (await _context.SaveChangesAsync() <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            return RedirectToAction(nameof(Index), "Forum");
        }
    }
}