using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Context;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User objUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, objUser.Email),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return View("CreateUser");
        }

        [HttpGet]
        public IActionResult Index()
        {
            _context.User.ToList();
            return View(_context.User.ToList());
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (ModelState.IsValid)
            {
                User u = new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                };
                _context.Add(u);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            if(id == null) {
                return null;
            }
            User user = _context.User.Find(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {

                User u = new User()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    
                };
                _context.Update(u);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            if (id == null)
            {
                return null;
            }
            var user = await _context.User.FindAsync(id);
            _context.Remove(user);
            await _context.SaveChangesAsync();

            return View("Index");
        }

    }
}
