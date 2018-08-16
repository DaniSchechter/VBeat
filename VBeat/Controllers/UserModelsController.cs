using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBeat.Models;
using Microsoft.AspNetCore.Http;
using VBeat.Models.Consts;

namespace VBeat.Controllers
{
    public class UserModelsController : Controller
    {
        private readonly VBeatDbContext _context;

        public UserModelsController(VBeatDbContext context)
        {
            _context = context;
        }

        // GET: UserModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: UserModels/Details/5
        public async Task<IActionResult> Details()
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;

            var userModel = await _context.Users
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: UserModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,FirstName,LastName,Email,Password")] UserModel userModel)
        {
            UserModel checkIfExists = _context.Users.Where(u => u.Username == userModel.Username || u.Email == userModel.Email).FirstOrDefault();
            if (checkIfExists!=null)
            {
                string error = "";
                if (checkIfExists.Username == userModel.Username)
                {
                    error = "Username is already taken.";
                }
                else if (checkIfExists.Email == userModel.Email)
                {
                    error = "Email is already taken.";
                }
                ViewData["Error"] = error;
                return View();
            }

            if (ModelState.IsValid)
            {
                userModel.TimeOfLastLogin = DateTime.UtcNow;
                userModel.DateOfRegistration = DateTime.UtcNow;
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("SignIn", "UserModels");
            }
            return View(userModel);
        }

        // GET: UserModels/Edit/5
        public async Task<IActionResult> Edit()
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;

            var userModel = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }
            return View(userModel);
        }

        // POST: UserModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId,Username,FirstName,LastName,Email,Password")] UserModel userModel)
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
            if (id != userModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserModelExists(userModel.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }

        // GET: UserModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userModel = await _context.Users
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: UserModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userModel = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost, ActionName("SignIn")]
        public async Task<IActionResult> SignInAction([Bind("Username,Password")] UserModel inputUser)
        {
            var userModel = await _context.Users.SingleOrDefaultAsync(u=>(u.Username==inputUser.Username && u.Password == inputUser.Password));
            if (userModel==null)
            {
                ViewData["Error"] = "username or password are incorrect";
                return View();
            }
            userModel.TimeOfLastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            HttpContext.Session.SetInt32(SessionConsts.UserId, userModel.UserId);
            return RedirectToAction("Display", "SongModels");// TODO check this
        }

        public IActionResult SignOutAction()
        {
            HttpContext.Session.Remove(SessionConsts.UserId);
            return RedirectToAction("HomePage", "Home");
        }
    }
}
