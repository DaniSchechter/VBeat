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
        public readonly string NEW_RELEASES_LIST_KEY = "NEW_RELEASES";

        private readonly int NUM_NEW_RELEASES = 7;

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

        private bool SongModelExists(int id)
        {
            return _context.Songs.Any(e => e.SongId == id);
        }
    }
}
