using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBeat.Models;

namespace VBeat.Controllers
{
    public class SongModelsController : Controller
    {
        private const int PAGE_SIZE = 10;

        public readonly string NEW_RELEASES_LIST_KEY = "NEW_RELEASES";

        private readonly int NUM_NEW_RELEASES = 10;

        private readonly VBeatDbContext _context;

        public SongModelsController(VBeatDbContext context)
        {
            _context = context;
        }

        // GET: SongModels
        public async Task<IActionResult> Index()
        {
            ViewData[NEW_RELEASES_LIST_KEY] = await _context.Songs.OrderByDescending(t => t.AddedDate).Take(NUM_NEW_RELEASES).ToListAsync();
            return View(await _context.Songs.ToListAsync());
        }

        // GET: SongModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs
                .SingleOrDefaultAsync(m => m.SongId == id);
            if (songModel == null)
            {
                return NotFound();
            }

            return View(songModel);
        }

        // GET: SongModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SongModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,SongName,Genre,SongPath,SongImagePath,ReleaseDate")] SongModel songModel)
        {
            if (ModelState.IsValid)
            {
                songModel.AddedDate = DateTime.UtcNow;
                songModel.SongImagePath = "/images/" + songModel.SongImagePath;
                _context.Add(songModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(songModel);
        }

        // GET: SongModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.SongId == id);
            if (songModel == null)
            {
                return NotFound();
            }
            return View(songModel);
        }

        // POST: SongModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SongId,SongName,Genre,SongPath,SongImagePath,ReleaseDate")] SongModel songModel)
        {
            if (id != songModel.SongId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongModelExists(songModel.SongId))
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
            return View(songModel);
        }

        // GET: SongModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs
                .SingleOrDefaultAsync(m => m.SongId == id);
            if (songModel == null)
            {
                return NotFound();
            }

            return View(songModel);
        }

        // POST: SongModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.SongId == id);
            _context.Songs.Remove(songModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Statistics()
        {
            return null;
        }

        public IActionResult Search()
        {
            return View("~/Views/SongModels/SearchView.cshtml");
        }


        [HttpGet]
        public IActionResult Search(string artistName, string songName, string genre, int? offset)
        {
            IQueryable<SongModel> songs = from s in _context.Songs select s;

            if (!string.IsNullOrWhiteSpace(songName))
            {
                songs = songs.Where(s => s.SongName.ToLower().Contains(songName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(artistName))
            {
                songs = songs.Where(s =>
                s.ArtistList.Where(
                    a => a.Artist.ArtistName.ToLower().Contains(artistName.ToLower())).Count() > 0
                );
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                songs = songs.Where(s => s.Genre.ToLower().Contains(genre.ToLower()));
            }

            int realOffset = !offset.HasValue ? 0 : offset.Value;

            songs = songs
                .Skip(realOffset * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewData["Genre"] = genre;
            ViewData["ArtistName"] = artistName;
            ViewData["SongName"] = songName;
            if (offset.HasValue)
            {
                ViewData["Offset"] = offset.Value;
            }
            else
            {
                ViewData["Offset"] = 0;
            }
            ViewData["Songs"] = songs;
            return View("~/Views/SongModels/SearchView.cshtml");
        }

        private bool SongModelExists(int id)
        {
            return _context.Songs.Any(e => e.SongId == id);
        }
    }
}
