using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBeat.Models;
using Microsoft.AspNetCore.Http;
using VBeat.Models.BridgeModel;
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
            ViewData["ArtistsList"] = await _context.Artists.ToListAsync();
            return View(await _context.Users.ToListAsync());
        }


        // GET: UserModels/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if(id != null)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int Id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;


            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.UserId == Id);
            if (artistModel != null)
            {
                return RedirectToAction("Details", "ArtistModels");
            }

            var userModel = await _context.Users
                .SingleOrDefaultAsync(m => m.UserId == Id);
            if (userModel == null)
            {
                return NotFound();
            }
            ViewData["UserName"] = userModel.Username;
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
            string error = CheckIfExists(userModel.Username, userModel.Email);
            if (error != "")
            {
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            if (id != null)
            {
                var user = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            

            int Id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.UserId == Id);
            if (artistModel != null)
            {
                return RedirectToAction("Edit", "ArtistModels");
            }

            var userModel = await _context.Users.SingleOrDefaultAsync(m => m.UserId == Id);
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
        public async Task<IActionResult> Edit(int id,[Bind("UserId,Username,FirstName,LastName,Email,Password")] UserModel userModel)
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            if (id != userModel.UserId)
            {
                return NotFound();
            }
            UserModel user = _context.Users.SingleOrDefault(u => u.UserId == id);
            if (userModel.Username != user.Username)
            {
                string error = CheckIfUserNameExists(userModel.Username);
                if ( error != "")
                {
                    ViewData["Error"] = error;
                    return View();
                }
            }
            if (userModel.Email != user.Email)
            {
                string error = CheckIfEmailExists(userModel.Email);
                if (error != "")
                {
                    ViewData["Error"] = error;
                    return View();
                }
            }
            if (ModelState.IsValid)
            {
                // To prevent two instances with the same id tracked
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Username = userModel.Username;
                user.Email = userModel.Email;
                try
                {
                    _context.Update(user);
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

                var userId = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
                var user1 = await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
                if (user1.Username=="admin")
                    return RedirectToAction("Index", "UserModels");
                else
                    return RedirectToAction("Details", "UserModels");
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
            //take all the playlists of the deleted user
            var playlistsOfThisUser = await _context.Playlists.Where(p => p.UserModel.UserId == id).ToListAsync();
            foreach (var playList in playlistsOfThisUser)
            {
                _context.Playlists.Remove(playList);
            }
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
            var userModel = await _context.Users.SingleOrDefaultAsync(u => (u.Username == inputUser.Username && u.Password == inputUser.Password));
            if (userModel == null)
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

        public string CheckIfExists (string userName, string email)
        {
            UserModel checkIfExists = _context.Users.Where(u => u.Username == userName || u.Email == email).FirstOrDefault();
            string error = "";
            if (checkIfExists != null)
            {
                if (checkIfExists.Username == userName)
                {
                    error = "Username is already taken.";
                }
                else if (checkIfExists.Email == email)
                {
                    error = "Email is already taken.";
                }
            }
            return error;
        }
        public string CheckIfUserNameExists(string userName)
        {
            UserModel checkIfExists = _context.Users.Where(u => u.Username == userName).FirstOrDefault();
            string error = "";
            if (checkIfExists != null)
            {
                if (checkIfExists.Username == userName)
                {
                    error = "Username is already taken.";
                }
            }
            return error;
        }
        public string CheckIfEmailExists(string email)
        {
            UserModel checkIfExists = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            string error = "";
            if (checkIfExists != null)
            {
                if (checkIfExists.Email == email)
                {
                    error = "Email is already taken.";
                }
            }
            return error;
        }
    }
}
