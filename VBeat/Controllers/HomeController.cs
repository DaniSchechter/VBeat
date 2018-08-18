using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VBeat.Models;
using Microsoft.AspNetCore.Http;
using VBeat.Models.Consts;

namespace VBeat.Controllers
{
    public class HomeController : Controller
    {
        private readonly VBeatDbContext dbContext;
        public HomeController()
        {
            dbContext = new VBeatDbContext();
        }
        public IActionResult Index()
        {

            UserModel userModel = dbContext.Users.SingleOrDefault(u => u.UserId == HttpContext.Session.GetInt32(SessionConsts.UserId));
            if (userModel == null)
            {
                return RedirectToAction("SignIn", "UserModels");
            }
            ViewData["DisplayName"] = userModel.Username;
            ViewData["DisplayId"] = userModel.UserId;
            ViewData["PlaylistCollection"] = userModel.SavedPlaylists;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound()
        {
            return View("~/Views/Home/NotFound.cshtml");
        }

        public IActionResult HomePage()
        {
            return View();
        }
    }
}
