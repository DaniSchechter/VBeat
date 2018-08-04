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
    public class ArtistModelsController : Controller
    {
        private readonly ArtistDbContext _context;

        public ArtistModelsController(ArtistDbContext context)
        {
            _context = context;
        }

        // GET: ArtistModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artists.ToListAsync());
        }

        // GET: ArtistModels/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> Create([Bind("ArtistName,ArtistImage,UserId,Username,Email,Password,DateOfRegistration,TimeOfLastLogin")] ArtistModel artistModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artistModel);
        }

        // GET: ArtistModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(int id, [Bind("ArtistName,ArtistImage,UserId,Username,Email,Password,DateOfRegistration,TimeOfLastLogin")] ArtistModel artistModel)
        {
            if (id != artistModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
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

        private bool ArtistModelExists(int id)
        {
            return _context.Artists.Any(e => e.UserId == id);
        }
    }
}
