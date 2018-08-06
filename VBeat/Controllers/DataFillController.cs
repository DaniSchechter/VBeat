using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VBeat.Models;

namespace VBeat.Controllers
{
    public class DataFillController : Controller
    {
        private VBeatDbContext dbContext;

        public DataFillController()
        {
            dbContext = new VBeatDbContext();
        }

        public bool Index()
        {
            ArtistModel artistModel = new ArtistModel() { ArtistImage = "https://i.imgur.com/GkyKh.jpg", ArtistName = "Fake Artist #1", Email = "fakeartist@fake.com", DateOfRegistration = DateTime.UtcNow, FirstName = "Idodo", LastName = "Aloni", Password = "12312fdasd", TimeOfLastLogin = DateTime.Now, Username = "deadmau5" };
            ArtistModel artistModel2 = new ArtistModel() { ArtistImage = "https://i.imgur.com/GkyKh.jpg", ArtistName = "Fake Artist #2", Email = "fakeartist2@fake.com", DateOfRegistration = DateTime.UtcNow, FirstName = "Idodo2", LastName = "Aloni", Password = "12312fdasd", TimeOfLastLogin = DateTime.Now, Username = "deadmau53" };
            ArtistModel artistModel3 = new ArtistModel() { ArtistImage = "https://i.imgur.com/GkyKh.jpg", ArtistName = "Fake Artist #3", Email = "fakeartis3t@fake.com", DateOfRegistration = DateTime.UtcNow, FirstName = "Idodo3", LastName = "Aloni", Password = "12312fdasd", TimeOfLastLogin = DateTime.Now, Username = "deadmau52" };
            SongModel songModel = new SongModel()
            {
                AddedDate = DateTime.Now,
                Genre = "EDM",
                ReleaseDate = DateTime.Now,
                SongImagePath = "https://i.imgur.com/rbXZcVH.jpg",
                SongName = "Blah Blah Blah",
                SongPath = "ftp://cdn.musiccdn.com/blablabla.mp3",
            };

            SongModel songModel2 = new SongModel()
            {
                AddedDate = DateTime.Now,
                Genre = "Country Music",
                ReleaseDate = DateTime.Now,
                SongImagePath = "https://imgix.ranker.com/user_node_img/50065/1001280631/original/dr-no-chill-photo-u1?w=650&q=50&fm=jpg&fit=crop&crop=faces",
                SongName = "HaTikva",
                SongPath = "ftp://cdn.musiccdn.com/HaTikva.mp3",
            };

            SongModel songModel3 = new SongModel()
            {
                AddedDate = DateTime.Now,
                Genre = "Pop",
                ReleaseDate = DateTime.Now,
                SongImagePath = "https://static.highsnobiety.com/wp-content/uploads/2018/05/07132600/this-is-america-what-you-missed-01-480x320.jpg",
                SongName = "This Is America",
                SongPath = "ftp://cdn.musiccdn.com/this_is_america.mp3",
            };

            dbContext.Add(songModel);
            dbContext.Add(songModel2);
            dbContext.Add(songModel3);
            dbContext.Add(artistModel);
            dbContext.Add(artistModel2);
            dbContext.Add(artistModel3);
            dbContext.SaveChanges();
            return true;
        }
    }
}
