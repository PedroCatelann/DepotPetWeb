using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApplication1.Context;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;

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
        public ActionResult Login(User objUser)
        {
            
                
            var obj = _context.User.Where(a => a.Email.Equals(objUser.Email) && a.Password.Equals(objUser.Password)).FirstOrDefault();
            if (obj != null)
            {
                
            }
            return View(objUser);
        }

        [HttpGet]
        public IActionResult Index()
        {
            _context.User.ToList();
            return View(_context.User.ToList());
        }

        [HttpGet]
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
