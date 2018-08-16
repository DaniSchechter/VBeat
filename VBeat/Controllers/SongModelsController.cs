using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VBeat.Models;
using VBeat.Models.BridgeModel;
using VBeat.Models.Consts;
using VBeat.Models.Json;

namespace VBeat.Controllers
{
    public class SongModelsController : Controller
    {
        private const int PAGE_SIZE = 10;

        public readonly string NEW_RELEASES_LIST_KEY = "NEW_RELEASES";

        private readonly int NUM_NEW_RELEASES = 8;

        private readonly VBeatDbContext _context;

        public SongModelsController(VBeatDbContext context)
        {
            _context = context;
        }

        // GET: SongModels
        public async Task<IActionResult> Display()
        {
            ViewData[NEW_RELEASES_LIST_KEY] = await _context.Songs.OrderByDescending(t => t.AddedDate).Take(NUM_NEW_RELEASES).ToListAsync();
            ViewData["NUM_NEW_RELEASES"] = Math.Min(_context.Songs.Count(),NUM_NEW_RELEASES);
            return View(await _context.Songs.ToListAsync());
        }




        private bool IsSongInList(List<Models.BridgeModel.ArtistSongModel> songList,int songId)
        {
            var foundSong = songList.SingleOrDefault(s=>s.SongId==songId);
            if (foundSong == null) return false;
            return true;
        }




        // GET: SongModels
        public async Task<IActionResult> Index()
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
            var artist = await _context.Artists.SingleOrDefaultAsync(u => u.UserId == artistModel.UserId);
            var songList = artist.SongList.ToList();
            return View(_context.Songs.Where( s =>IsSongInList(songList,s.SongId)).ToList());
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
            ViewData["USER_PLAYLISTS"]= await _context.Playlists.ToListAsync();
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
                //adds the created song to the current artist according to the session
                int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
                Models.BridgeModel.ArtistSongModel temp = new Models.BridgeModel.ArtistSongModel();
                temp.SongId = songModel.SongId;
                temp.UserId = id;
                _context.Add(temp);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Display));
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
                return RedirectToAction(nameof(Display));
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
            return RedirectToAction(nameof(Display));
        }

        public IActionResult Statistics()
        {
            return View();
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
                songs = songs.Where(s => s.Genre.ToString().ToLower().Contains(genre.ToLower()));
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
            return View("~/Views/SongModels/Search.cshtml");
        }

        private bool SongModelExists(int id)
        {
            return _context.Songs.Any(e => e.SongId == id);
        }

        public JsonResult GetSongsByMonth()
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            List<DataLabelModel> dataLabelModel = new List<DataLabelModel>();

            for (int i = 0; i < months.Length; i++)
            {
                int numSongs = _context.Songs.Where(s => s.ReleaseDate.Month.Equals(i + 1)).Count();
                dataLabelModel.Add(new DataLabelModel() { Value = numSongs, Label = months[i]  + " (" + numSongs.ToString()  + ")"});
            }

            return Json(dataLabelModel);
        }

        public JsonResult GetSongsByGenreCount()
        {
            List<DataLabelModel> dataLabelMdoel = new List<DataLabelModel>();
            IQueryable<DataLabelModel> genreResult = from s in _context.Songs
                              group s by s.Genre into sGenre
                              let genreCount = sGenre.Count()
                              select new DataLabelModel() { Label = sGenre.Key.ToString(), Value = genreCount };

            return Json(genreResult.ToList());
        }

        public void AddSongToPlayList(int playlistId,int songId)
        {
            
        }


        public async Task<IActionResult> AllSongs()
        {
            return View(_context.Songs.ToList());
        }

    }
}
