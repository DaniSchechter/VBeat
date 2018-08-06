using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace VBeat.Models
{
    public class ShowModelsController : Controller
    {
        private readonly VBeatDbContext _context;

        public ShowModelsController(VBeatDbContext context)
        {
            _context = context;
        }

        // GET: ShowModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shows.ToListAsync());
        }

        // GET: ShowModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showModel = await _context.Shows
                .SingleOrDefaultAsync(m => m.ShowId == id);
            if (showModel == null)
            {
                return NotFound();
            }

            return View(showModel);
        }

        // GET: ShowModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShowModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowId,ShowName,Country,City,StreetName,HouseNumber,ShowTime")] ShowModel showModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(showModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(showModel);
        }

        // GET: ShowModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showModel = await _context.Shows.SingleOrDefaultAsync(m => m.ShowId == id);
            if (showModel == null)
            {
                return NotFound();
            }
            return View(showModel);
        }

        // POST: ShowModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowId,ShowName,Country,City,StreetName,HouseNumber,ShowTime")] ShowModel showModel)
        {
            if (id != showModel.ShowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowModelExists(showModel.ShowId))
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
            return View(showModel);
        }

        // GET: ShowModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showModel = await _context.Shows
                .SingleOrDefaultAsync(m => m.ShowId == id);
            if (showModel == null)
            {
                return NotFound();
            }

            return View(showModel);
        }

        // POST: ShowModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showModel = await _context.Shows.SingleOrDefaultAsync(m => m.ShowId == id);
            _context.Shows.Remove(showModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowModelExists(int id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}
