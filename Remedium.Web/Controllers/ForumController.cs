using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remedium.Web.Data;
using Remedium.Web.Data.Entities;

namespace Remedium.Web.Controllers
{
    public sealed class ForumController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public ForumController(IAuthorizationService authorizationService, ApplicationDbContext context, 
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _authorizationService = authorizationService;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        
        public async Task<IActionResult> Index()
        {
            ViewBag.Authorized = (await _authorizationService.AuthorizeAsync(User, "RequireModerator")).Succeeded;
            ViewBag.AnyoneSignedIn = _signInManager.IsSignedIn(User);
            if (ViewBag.AnyoneSignedIn)
            {
                ViewBag.User = await _userManager.FindByNameAsync(User.Identity!.Name);
            }

            return View
            (
                await _context.Threads
                    .Include(x => x.Author)
                    .OrderByDescending(x => x.LastUpdateTimestamp)
                    .ToArrayAsync()
            );
        }

        public async Task<IActionResult> Update(Int32? id) => await _context.Threads.FindAsync(id) switch
        {
            var thread when thread is not null => View(thread),
            _ when id is null => View(new Thread()),
            _ => NotFound()
        };

        [HttpPost]
        public async Task<IActionResult> Update(Thread formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var timestamp = DateTime.Now;
            
            if (formData.Id == default)
            {
                formData.LastUpdateTimestamp = DateTime.Now;
                formData.Author = await _userManager.FindByNameAsync(User.Identity!.Name);
                _context.Add(formData);
                
                if (await _context.SaveChangesAsync() <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                
                return RedirectToAction(nameof(Update), "Thread", new {threadId = formData.Id});
                
            }

            _context.Update(formData with {LastUpdateTimestamp = timestamp});
            
            if (await _context.SaveChangesAsync() <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Int32 id) => await _context.Threads.FindAsync(id) switch
        {
            var thread when thread is not null => View(thread),
            _ => NotFound()
        };

        [HttpPost]
        public async Task<IActionResult> Delete(Thread formData)
        {
            _context.Remove(formData);
            
            if (await _context.SaveChangesAsync() <= 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}