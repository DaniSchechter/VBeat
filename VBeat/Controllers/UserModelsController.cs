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
        public async Task<IActionResult> Details(int? id)
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
            var checkIfExists = await _context.Users.SingleOrDefaultAsync(u => u.Username == userModel.Username);
            if (checkIfExists != null)
            {
                ViewData["Error"] = "UserName already exists, please try again";
                return View();
            }

            if (ModelState.IsValid)
            {
                userModel.TimeOfLastLogin = DateTime.UtcNow;
                userModel.DateOfRegistration = DateTime.UtcNow;
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "SongModels");
            }
            return View(userModel);
        }

        // GET: UserModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,FirstName,LastName,Email,Password")] UserModel userModel)
        {
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


        public async Task<IActionResult> SignIn()
        {
            return View();
        }


        [HttpPost, ActionName("SignIn")]
        public async Task<IActionResult> SignInAction([Bind("Username,Password")] UserModel inputUser)
        {
            var userModel = await _context.Users.SingleOrDefaultAsync(u => (u.Username == inputUser.Username && u.Password == inputUser.Password));
            if (userModel == null)
            {
                ViewData["Error"] = "username or password are incorrect";
                return View();
            }
            userModel.TimeOfLastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32(SessionConsts.UserId, userModel.UserId);
            return RedirectToAction("Index", "SongModels");// TODO check this
        }

    }
}
