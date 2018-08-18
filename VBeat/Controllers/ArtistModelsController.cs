using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBeat.Models;
using VBeat.Models.Consts;

namespace VBeat.Controllers
{
    public class ArtistModelsController : Controller
    {
        private static int PAGE_SIZE = 10;
        private readonly VBeatDbContext _context;

        public ArtistModelsController(VBeatDbContext context)
        {
            _context = context;
        }

        // GET: ArtistModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artists.ToListAsync());
        }

        // GET: ArtistModels/Details/5
        public async Task<IActionResult> Details()
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;

            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (artistModel == null)
            {
                return NotFound();
            }

            return View(artistModel);
        }

        // GET: ArtistModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArtistModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistName,FirstName,LastName,UserId,Username,Email,Password")] ArtistModel artistModel, IFormFile artistImageFile)
        {
            var checkIfExistsUserName = await _context.Users.SingleOrDefaultAsync(u => u.Username == artistModel.Username);
            var checkIfExistsEmail = await _context.Users.SingleOrDefaultAsync(u => u.Email == artistModel.Email);
            string error = "";
            if (checkIfExistsUserName != null)
            {
                error = "Username is already taken.";
            }

            else if (checkIfExistsEmail != null)
            {
                error = "Email is already taken.";
            }
            ViewData["Error"] = error;
            if (error != "") return View();


            if (ModelState.IsValid)
            {
                artistModel.TimeOfLastLogin = DateTime.UtcNow;
                artistModel.DateOfRegistration = DateTime.UtcNow;
                artistModel.ArtistImage = FileHelper.SaveFile(artistImageFile, "images", artistImageFile.FileName);
                _context.Add(artistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "SongModels");
            }
            return View(artistModel);
        }

        // GET: ArtistModels/Edit/5
        public async Task<IActionResult> Edit()
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;

            var artistModel = await _context.Artists.SingleOrDefaultAsync(m => m.UserId == id);
            if (artistModel == null)
            {
                return NotFound();
            }
            return View(artistModel);
        }

        // POST: ArtistModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ArtistName,FirstName,LastName,UserId,Username,Email,Password")] ArtistModel artistModel, IFormFile artistImage)
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
            if (id != artistModel.UserId)
            {
                return NotFound();
            }

            // keeping original image if artist doesn't change it
            ArtistModel originalArtist = _context.Artists.Where(a => a.UserId == id).FirstOrDefault();
            if(artistImage == null)
            {
                artistModel.ArtistImage = originalArtist.ArtistImage;
            }


            if (ModelState.IsValid)
            {
                if (artistImage != null)
                    artistModel.ArtistImage = FileHelper.SaveFile(artistImage, "images", artistImage.FileName);

                try
                {
                    _context.Update(artistModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistModelExists(artistModel.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "ArtistModels");
            }
            return View(artistModel);
        }

        // GET: ArtistModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.UserId == id);
            if (artistModel == null)
            {
                return NotFound();
            }

            return View(artistModel);
        }

        // POST: ArtistModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artistModel = await _context.Artists.SingleOrDefaultAsync(m => m.UserId == id);
            _context.Artists.Remove(artistModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Search(string artistName, int? offset)
        {
            int realOffset = offset.HasValue ? offset.Value : 0;
            IQueryable<ArtistModel> artistModels = _context.Artists;

            if (!string.IsNullOrEmpty(artistName))
            {
                artistModels = artistModels.Where(a => a.ArtistName.ToLower().Contains(artistName));
            }

            artistModels = artistModels
                .Skip(realOffset * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewData["ArtistList"] = artistModels;
            ViewData["ArtistName"] = artistName;
            ViewData["Offset"] = realOffset;
            return View("~/Views/ArtistModels/Search.cshtml");
        }

        private bool ArtistModelExists(int id)
        {
            return _context.Artists.Any(e => e.UserId == id);
        }
    }
}
