using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VBeat.Models;
using VBeat.Models.Algorithm.Learning;

namespace VBeat.Controllers.Test
{
    public class AprioriController : Controller
    {
        AprioriSuggestionAlgorithm suggestionAlgorithm;
        VBeatDbContext _context;

        public AprioriController()
        {
            _context = new VBeatDbContext();
            suggestionAlgorithm = AprioriSuggestionAlgorithm.GetInstance();
        }


        public List<int> Test(int playlistId)
        {

            suggestionAlgorithm.Train(_context.Playlists
                .Where(p => p.PlaylistId != playlistId).ToList());
            PlaylistModel targetPlaylist = _context.Playlists.Where(p => p.PlaylistId == playlistId).FirstOrDefault();
            List<int> intList = suggestionAlgorithm.Suggset(targetPlaylist);
            return intList;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}