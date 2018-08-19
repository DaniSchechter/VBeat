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
using VBeat.Models.Facebook;
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
            ViewData["NUM_NEW_RELEASES"] = Math.Min(_context.Songs.Count(), NUM_NEW_RELEASES);
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
            ViewData["UserName"] = userModel.Username;

            var playlists =  _context.Playlists.Where(p => p.UserModel.UserId == id).Take(5).ToList();
            ViewData["PlayLists"] = playlists;
            return View(await _context.Songs.ToListAsync());
        }




        private bool IsSongInList(List<Models.BridgeModel.ArtistSongModel> songList, int songId)
        {
            var foundSong = songList.SingleOrDefault(s => s.SongId == songId);
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

            var songList = artistModel.SongList.ToList();

            return View(_context.Songs.Where(s => IsSongInList(songList, s.SongId)).ToList());

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
            int userId = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
            ViewData["USER_PLAYLISTS"] = await _context.Playlists.Where(p => p.UserModel.UserId == userId).ToListAsync();
            return View(songModel);
        }

        // GET: SongModels/Create
        public IActionResult Create()
        {
            if (!HttpContext.Session.GetInt32(SessionConsts.UserId).HasValue)
            {
                return Unauthorized();
            }

            int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;

            return View();
        }

        // POST: SongModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,SongName,Genre,ReleaseDate")] SongModel songModel, IFormFile songImagePath, IFormFile songPath)
        {
            if (ModelState.IsValid)
            {
                songModel.SongPath = FileHelper.SaveFile(songPath, "songs", songPath.FileName);
                songModel.AddedDate = DateTime.UtcNow;
                songModel.SongImagePath = FileHelper.SaveFile(songImagePath, "images", songImagePath.FileName);
                _context.Add(songModel);
                //adds the created song to the current artist according to the session
                int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
                Models.BridgeModel.ArtistSongModel temp = new Models.BridgeModel.ArtistSongModel();
                temp.SongId = songModel.SongId;
                temp.UserId = id;
                _context.Add(temp);

                await _context.SaveChangesAsync();
                FacebookModel facebookModel = new FacebookModel();
                await facebookModel.Post(string.Format("A new song has been added to the site -> {0}", songModel.SongName));
                return RedirectToAction(nameof(Display));
            }
            return View(songModel);
        }

        // GET: SongModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //list of artists
            IQueryable<ArtistModel> artists = from a in _context.Artists select a;
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.SongId == id);
            if (songModel == null)
            {
                return NotFound();
            }
            //all the artists in this song
            var allArtistsInThisSong = artists.Where(a => a.SongList.Where(s => s.SongId.Equals(id)).Count() > 0);
            //get all the artists not in this song
            LinkedList<ArtistModel> allArtistsNotInThisSong = new LinkedList<ArtistModel>();
            foreach (var art in artists)
            {
                if (!(allArtistsInThisSong.Contains(art)))
                {
                    allArtistsNotInThisSong.AddLast(art);
                }
            }
            ViewData["Artists"] = allArtistsNotInThisSong;
            return View(songModel);
        }

        // POST: SongModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SongId,SongName,SongImagePath,SongPath,Genre,ReleaseDate")] SongModel songModel)
        {
            if (id != songModel.SongId)
            {
                return NotFound();
            }

            SongModel origSongModel = _context.Songs.AsNoTracking().Where(s => songModel.SongId == id).FirstOrDefault();

            songModel.SongImagePath = origSongModel.SongImagePath;
            songModel.SongPath = origSongModel.SongPath;

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
                // implicit Join
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
                dataLabelModel.Add(new DataLabelModel() { Value = numSongs, Label = months[i] + " (" + numSongs.ToString() + ")" });
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

        public async Task<IActionResult> AllSongs()
        {
            return View(_context.Songs.ToList());
        }

        public async Task<IActionResult> addArtistToSong(int songId, int artistId) 
        {
            var songModel = await _context.Songs.SingleOrDefaultAsync(s => s.SongId == songId);
            if (songModel == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists.SingleOrDefaultAsync(a => a.UserId == artistId);
            if (artistModel == null)
            {
                return NotFound();
            }

            Models.BridgeModel.ArtistSongModel artistSongModel = new ArtistSongModel();
            artistSongModel.Song = songModel;
            artistSongModel.SongId = songId;
            artistSongModel.UserId = artistId;
            artistSongModel.Artist = artistModel;
            _context.Add(artistSongModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Display", "SongModels");
        }
        public async Task<IActionResult> removeArtistFromSong(int songId, int artistId)
        {
            var songModel = await _context.Songs.SingleOrDefaultAsync(s => s.SongId == songId);
            if (songModel == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists.SingleOrDefaultAsync(a => a.UserId == artistId);
            if (artistModel == null)
            {
                return NotFound();
            }

            Models.BridgeModel.ArtistSongModel artistSongModel = new ArtistSongModel();
            artistSongModel.Song = songModel;
            artistSongModel.SongId = songId;
            artistSongModel.UserId = artistId;
            artistSongModel.Artist = artistModel;
            _context.Remove(artistSongModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Display", "SongModels");
        }

    }
}
