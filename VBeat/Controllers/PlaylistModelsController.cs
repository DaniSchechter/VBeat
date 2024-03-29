﻿using System;
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
    public class PlaylistModelsController : Controller
    {
        private const int PAGE_SIZE = 10;
        private readonly VBeatDbContext _context;

        public PlaylistModelsController(VBeatDbContext context)
        {
            _context = context;
        }

        // GET: PlaylistModels
        public async Task<IActionResult> Index()
        {

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

            return View(userModel.SavedPlaylists);
        }

        // GET: PlaylistModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistModel = await _context.Playlists
                .SingleOrDefaultAsync(m => m.PlaylistId == id);
            if (playlistModel == null)
            {
                return NotFound();
            }
            return View(playlistModel);
        }

        // GET: PlaylistModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlaylistModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,Public,PlaylistName")] PlaylistModel playlistModel, IFormFile playlistImage)
        {
            playlistModel.PlaylistImage = FileHelper.SaveFile(playlistImage, "images", playlistImage.FileName);
            if (ModelState.IsValid)
            {
                _context.Add(playlistModel);
                //adds the crated playlist too the current user lit of playlists
                int id = HttpContext.Session.GetInt32(SessionConsts.UserId).Value;
                UserModel user = await _context.Users.SingleOrDefaultAsync(m => m.UserId == id);
                user.SavedPlaylists.Add(playlistModel);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playlistModel);
        }

        // GET: PlaylistModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistModel = await _context.Playlists.SingleOrDefaultAsync(m => m.PlaylistId == id);
            if (playlistModel == null)
            {
                return NotFound();
            }
            return View(playlistModel);
        }

        // POST: PlaylistModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaylistId,Public,PlaylistImage,PlaylistName")] PlaylistModel playlistModel)
        {
            if (id != playlistModel.PlaylistId)
            {
                return NotFound();
            }

            PlaylistModel orig = _context.Playlists.AsNoTracking().Where(p => p.PlaylistId == id).FirstOrDefault();
            playlistModel.PlaylistImage = orig.PlaylistImage;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlistModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistModelExists(playlistModel.PlaylistId))
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
            return View(playlistModel);
        }

        // GET: PlaylistModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlistModel = await _context.Playlists
                .SingleOrDefaultAsync(m => m.PlaylistId == id);
            if (playlistModel == null)
            {
                return NotFound();
            }

            return View(playlistModel);
        }

        // POST: PlaylistModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlistModel = await _context.Playlists.SingleOrDefaultAsync(m => m.PlaylistId == id);
            _context.Playlists.Remove(playlistModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistModelExists(int id)
        {
            return _context.Playlists.Any(e => e.PlaylistId == id);
        }

        public async Task<IActionResult> addSongToPlaylist(int playlistId, int songId)
        {
            var playListModel = await _context.Playlists.SingleOrDefaultAsync(p => p.PlaylistId == playlistId);
            if (playListModel == null)
            {
                return NotFound();
            }
            var songModel = await _context.Songs.SingleOrDefaultAsync(s => s.SongId == songId);
            if (songModel == null)
            {
                return NotFound();
            }

            //check if the song is alreay in this playlist
            var song = playListModel.Songs.SingleOrDefault(s => s.SongId == songId);      
            if(song==null)
            {
                Models.BridgeModel.PlaylistSongModel playlistSongModel = new Models.BridgeModel.PlaylistSongModel();
                playlistSongModel.Playlist = playListModel;
                playlistSongModel.PlaylistId = playlistId;
                playlistSongModel.Song = songModel;
                playlistSongModel.SongId = songId;
                _context.Add(playlistSongModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Display","SongModels");
        }

        [HttpGet]
        public IActionResult Search(string playListName, int? offset)
        {

            IQueryable<PlaylistModel> playLists = from p in _context.Playlists select p;

            if (!string.IsNullOrWhiteSpace(playListName))
            {
                playLists = playLists.Where(s => s.PlaylistName.ToLower().Contains(playListName.ToLower()));
            }
            int realOffset = !offset.HasValue ? 0 : offset.Value;

            playLists = playLists
                .Skip(realOffset * PAGE_SIZE)
                .Take(PAGE_SIZE);

            ViewData["PlayListName"] = playListName;
            if (offset.HasValue)
            {
                ViewData["Offset"] = offset.Value;
            }
            else
            {
                ViewData["Offset"] = 0;
            }
            ViewData["PlayLists"] = playLists;
            return View("~/Views/PlayListModels/Search.cshtml");
        }
    }
}
