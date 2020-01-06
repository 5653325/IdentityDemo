using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreIdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [Authorize]
        public IActionResult Secert()
        {
            return View("Secert");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            if (user != null)
            {
                var signResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signResult.Succeeded)
                {
                    return View("Secert");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser
            {
                UserName = username,
                Email = "lihua@qq.com",
            };
            var createResult = await _userManager.CreateAsync(user, password);
            if (createResult.Succeeded)
            {
                var signResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signResult.Succeeded)
                {
                    return View("Index");
                }
                else
                {
                    return View("Index");
                }
            }
            else
                return View("Register");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Curs = user;
            return View("Update");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(string username, string email)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                user.Email = email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Update");
                else
                    return Ok("失败");
            }
            else
                return Ok("user is not existed");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.Curs = user;
            return View("Remove");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    return Ok("失败");
            }
            return Ok("user is not existed");
        }
    }
}