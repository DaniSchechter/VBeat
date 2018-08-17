using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBeat.Models.Consts;
using VBeat.Models.BridgeModel;

namespace VBeat.Models
{
    public class ShowModelsController : Controller
    {
        private const int PAGE_SIZE = 10;
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
        public async Task<IActionResult> Create([Bind("ShowId,ShowName,Country,City,StreetName,HouseNumber,ShowTime")] ShowModel showModel, IFormFile showImagePath)
        {

            showModel.ShowImagePath = FileHelper.SaveFile(showImagePath, "images", showImagePath.FileName);
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
            //list of artists
            IQueryable<ArtistModel> artists = from a in _context.Artists select a;
            if (id == null)
            {
                return NotFound();
            }

            var showModel = await _context.Shows.SingleOrDefaultAsync(s => s.ShowId == id);
            if (showModel == null)
            {
                return NotFound();
            }
            //all the artists in this show
            var allArtistsInThishow = artists.Where(a => a.Shows.Where(s => s.ShowId.Equals(id)).Count() > 0);
            //get all the artists not in this show
            LinkedList<ArtistModel> allArtistsNotInThisShow = new LinkedList<ArtistModel>();
            foreach (var art in artists)
            {
                if (!(allArtistsInThishow.Contains(art)))
                {
                    allArtistsNotInThisShow.AddLast(art);
                }
            }
            ViewData["Artists"] = allArtistsNotInThisShow;
            return View(showModel);
        }

        // POST: ShowModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowId,ShowName,Country,City,StreetName,HouseNumber,ShowTime,ShowImagePath")] ShowModel showModel)
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

        [HttpGet]
        public IActionResult Search(string showtName, string country, DateTime show_Time, int? offset)
        {

            IQueryable<ShowModel> shows = from s in _context.Shows select s;

            if (!string.IsNullOrWhiteSpace(country))
            {
                shows = shows.Where(s => s.Country.ToLower().Contains(country.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(showtName))
            {
                shows = shows.Where(s => s.ShowName.ToLower().Contains(showtName.ToLower()));
            }
            DateTime d = DateTime.Parse("01/01/0001 00:00:00");
            if (!d.Equals(show_Time))
            {
                shows = shows.Where(s => s.ShowTime.Equals(show_Time));
            }

            int realOffset = !offset.HasValue ? 0 : offset.Value;

            shows = shows
                .Skip(realOffset * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewData["Show_Time"] = show_Time;
            ViewData["ShowtName"] = showtName;
            ViewData["Country"] = country;
            if (offset.HasValue)
            {
                ViewData["Offset"] = offset.Value;
            }
            else
            {
                ViewData["Offset"] = 0;
            }
            ViewData["Shows"] = shows;
            return View("~/Views/ShowModels/Search.cshtml");
        }
        public async Task<IActionResult> AllShows()
        {
            return View(_context.Shows.ToList());
        }
        public async Task<IActionResult> addArtistToShow(int showId, int artistId)
        {
            var showModel = await _context.Shows.SingleOrDefaultAsync(s => s.ShowId == showId);
            if (showModel == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists.SingleOrDefaultAsync(a => a.UserId == artistId);
            if (artistModel == null)
            {
                return NotFound();
            }

            ArtistShowModel artistShowModel = new ArtistShowModel();
            artistShowModel.Show = showModel;
            artistShowModel.ShowId = showId;
            artistShowModel.UserId = artistId;
            artistShowModel.Artist = artistModel;
            _context.Add(artistShowModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Display", "SongModels");
        }
        public async Task<IActionResult> removeArtistFromShow(int showId, int artistId)
        {
            var showModel = await _context.Shows.SingleOrDefaultAsync(s => s.ShowId == showId);
            if (showModel == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists.SingleOrDefaultAsync(a => a.UserId == artistId);
            if (artistModel == null)
            {
                return NotFound();
            }

            ArtistShowModel artistShowModel = new ArtistShowModel();
            artistShowModel.Show = showModel;
            artistShowModel.ShowId = showId;
            artistShowModel.UserId = artistId;
            artistShowModel.Artist = artistModel;
            _context.Remove(artistShowModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Display", "SongModels");
        }
    }
}
