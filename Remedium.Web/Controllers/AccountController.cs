using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Remedium.Web.Data.Entities;
using Remedium.Web.Models;

namespace Remedium.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _autoMapper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(ApplicationUserViewModel user)
        {
            if (!ModelState.IsValid) return View(user);
            try
            {
                if (await _userManager.FindByEmailAsync(user.Email) is not null)
                {
                    return BadRequest("Account with this email address already exists.");
                }
                if (await _userManager.FindByNameAsync(user.UserName) is not null)
                {
                    return BadRequest("Account with this login already exists.");
                }
                if ((await _userManager
                    .CreateAsync(_autoMapper.Map<ApplicationUser>(user), user.Password)).Succeeded)
                {
                    return StatusCode(StatusCodes.Status201Created, "Account created successfully.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal database error.");
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(ApplicationUserViewModel user)
        {
            try
            {
                var applicationUser = await _userManager.FindByEmailAsync(user.Email);
                if (applicationUser is null ||
                    !(await _signInManager
                        .PasswordSignInAsync(applicationUser, user.Password, false, false)).Succeeded)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Invalid user credentials");
                }
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal database error");
            }
        }

        [HttpDelete]
        public new async Task<IActionResult> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal database error");
            }
        }
    }
}