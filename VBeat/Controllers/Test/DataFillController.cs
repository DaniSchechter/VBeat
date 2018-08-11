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

        public void ClearDataBase()
        {

        }


        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        public ICollection<SongModel> randomSongs(int num)
        {
            ICollection<SongModel> ret = new List<SongModel>();
            for (int i = 0; i < num; i++)
            {
                SongModel temp = new SongModel();
                temp.SongImagePath = RandomString(10);
                temp.SongName = RandomString(6);
                temp.SongPath = RandomString(12);
                temp.ReleaseDate = new DateTime(2018, 3, 11);
                temp.Genre = RandomString(5);
                temp.AddedDate = DateTime.Today;
                ret.Add(temp);
                dbContext.Add(temp);
            }
            dbContext.SaveChanges();
            return ret;
        }


        public ICollection<UserModel> randomUsers(int num)
        {
            ICollection<UserModel> ret = new List<UserModel>();
            for (int i = 0; i < num; i++)
            {
                UserModel temp = new UserModel();
                temp.Username = RandomString(6);
                temp.FirstName = RandomString(5);
                temp.LastName = RandomString(5);
                temp.Email = RandomString(5);
                temp.Password = RandomString(5);
                temp.DateOfRegistration = RandomDay();
                temp.TimeOfLastLogin = RandomDay();
                ret.Add(temp);
                dbContext.Add(temp);
            }
            dbContext.SaveChanges();
            return ret;
        }

        public ICollection<ArtistModel> randomArtists(int num)
        {
            ICollection<ArtistModel> ret = new List<ArtistModel>();
            for (int i = 0; i < num; i++)
            {
                ArtistModel temp = new ArtistModel();
                temp.Username = RandomString(6);
                temp.FirstName = RandomString(5);
                temp.LastName = RandomString(5);
                temp.Email = RandomString(5);
                temp.Password = RandomString(5);
                temp.DateOfRegistration = RandomDay();
                temp.TimeOfLastLogin = RandomDay();
                temp.ArtistName = RandomString(5);
                temp.ArtistImage = RandomString(10);
                ret.Add(temp);
                dbContext.Add(temp);

            }
            dbContext.SaveChanges();
            return ret;
        }

        public ICollection<PlaylistModel> randomPlaylists(int num)
        {
            ICollection<PlaylistModel> ret = new List<PlaylistModel>();
            for (int i = 0; i < num; i++)
            {
                PlaylistModel temp = new PlaylistModel();
                temp.Public = true;
                temp.PlaylistImage = RandomString(10);
                temp.PlaylistName = RandomString(5);
                ret.Add(temp);
                dbContext.Add(temp);
            }
            dbContext.SaveChanges();
            return ret;
        }


        public ICollection<ShowModel> randomShows(int num)
        {
            ICollection<ShowModel> ret = new List<ShowModel>();
            for (int i = 0; i < num; i++)
            {
                ShowModel temp = new ShowModel();
                temp.ShowName = RandomString(6);
                temp.Country = RandomString(6);
                temp.City = RandomString(6);
                temp.StreetName = RandomString(6);
                temp.HouseNumber = random.Next(100);
                temp.ShowTime = RandomDay();
                ret.Add(temp);
                dbContext.Add(temp);
            }
            dbContext.SaveChanges();
            return ret;
        }

        public ICollection<Models.BridgeModel.ArtistShowModel> randomArtistsToShows(ICollection<ShowModel> shows, ICollection<ArtistModel> artists)
        {
            ICollection<Models.BridgeModel.ArtistShowModel> ret = new List<Models.BridgeModel.ArtistShowModel>();
            for (int i = 0; i < shows.Count; i++)
            {
                int randomNumArtists = random.Next(1, artists.Count);
                for (int j = 0; i < randomNumArtists; i++)
                {
                    int randomArtistIndex = random.Next(artists.Count);
                    if (!checkIfArtistAlreadyInShow(artists.ElementAt(randomArtistIndex).UserId, shows.ElementAt(i).ShowId))
                    {
                        Models.BridgeModel.ArtistShowModel temp = new Models.BridgeModel.ArtistShowModel();
                        temp.UserId = artists.ElementAt(randomArtistIndex).UserId;
                        temp.ShowId = shows.ElementAt(i).ShowId;
                        ret.Add(temp);
                        dbContext.Add(temp);
                    }
                }
            }
            dbContext.SaveChanges();
            return ret;
        }



        private bool checkIfArtistAlreadyInShow(int artistId, int showId)
        {
            ShowModel show = dbContext.Shows.SingleOrDefault(s => s.ShowId == showId);
            var check = show.ArtistList.SingleOrDefault(a => a.UserId == artistId);
            if (check == null) return false;
            return true;
        }

        private bool checkIfArtistAlreadyHaveSong(int artistId, int songId)
        {
            ArtistModel artist = dbContext.Artists.SingleOrDefault(a => a.UserId == artistId);
            var check = artist.SongList.SingleOrDefault(s => s.SongId == songId);
            if (check == null) return false;
            return true;
        }

        public ICollection<Models.BridgeModel.ArtistSongModel> randomArtistToSong(ICollection<SongModel> songs, ICollection<ArtistModel> artists)
        {
            ICollection<Models.BridgeModel.ArtistSongModel> ret = new List<Models.BridgeModel.ArtistSongModel>();
            for (int i = 0; i < songs.Count; i++)
            {
                int randomNumArtists = random.Next(1, 3);
                for (int j = 0; j < randomNumArtists; j++)
                {
                    Models.BridgeModel.ArtistSongModel temp = new Models.BridgeModel.ArtistSongModel();
                    temp.SongId = songs.ElementAt(i).SongId;
                    int rand = random.Next(0, artists.Count);
                    temp.UserId = artists.ElementAt(rand).UserId;
                    if (!checkIfArtistAlreadyHaveSong(temp.UserId,temp.SongId))
                    {
                        ret.Add(temp);
                        dbContext.Add(temp);
                    }
                }
            }
            dbContext.SaveChanges();
            return ret;
        }

        private bool checkIfSongAlreadyInPlaylist(int songId, int playlistId)
        {
            PlaylistModel playlist = dbContext.Playlists.SingleOrDefault(p => p.PlaylistId == playlistId);
            var check = playlist.Songs.SingleOrDefault(s => s.SongId == songId);
            if (check == null) return false;
            return true;
        }

        public ICollection<Models.BridgeModel.PlaylistSongModel> randomSongsToPlaylist(ICollection<PlaylistModel> playlists, ICollection<SongModel> songs)
        {
            ICollection<Models.BridgeModel.PlaylistSongModel> ret = new List<Models.BridgeModel.PlaylistSongModel>();
            for (int i = 0; i < songs.Count; i++)
            {
                int randNumPlaylistToAdd = random.Next(0, playlists.Count);
                for (int j = 0; j < randNumPlaylistToAdd; j++)
                {
                    int indexToAdd = random.Next(0, playlists.Count);
                    PlaylistModel curPlaylist = playlists.ElementAt(indexToAdd);
                    Models.BridgeModel.PlaylistSongModel temp = new Models.BridgeModel.PlaylistSongModel();
                    temp.PlaylistId = curPlaylist.PlaylistId;
                    temp.SongId = songs.ElementAt(i).SongId;
                    if (!checkIfSongAlreadyInPlaylist(temp.SongId, temp.PlaylistId))
                    {
                        ret.Add(temp);
                        dbContext.Add(temp);
                    }
                }
            }
            dbContext.SaveChanges();
            return ret;
        }



        public bool Index()
        {
            /*ArtistModel artistModel = new ArtistModel() { ArtistImage = "https://i.imgur.com/GkyKh.jpg", ArtistName = "Fake Artist #1", Email = "fakeartist@fake.com", DateOfRegistration = DateTime.UtcNow, FirstName = "Idodo", LastName = "Aloni", Password = "12312fdasd", TimeOfLastLogin = DateTime.Now, Username = "deadmau5" };
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

            PlaylistModel playlist1 = new PlaylistModel
            {
                PlaylistId = 1,
                PlaylistImage = "dfmn",
                PlaylistName = "workout",
                Public = true,
                UserModel = artistModel
            };

            PlaylistModel playlist2 = new PlaylistModel
            {
                PlaylistId = 2,
                PlaylistImage = "bvgn",
                PlaylistName = "workout",
                Public = true,
                UserModel = artistModel
            };

            Models.BridgeModel.PlaylistSongModel fir = new Models.BridgeModel.PlaylistSongModel() {
                Playlist = playlist1,
                PlaylistId = playlist1.PlaylistId,
                Song = songModel,
                SongId = songModel.SongId
            };

            Models.BridgeModel.PlaylistSongModel sec = new Models.BridgeModel.PlaylistSongModel()
            {
                Playlist = playlist1,
                PlaylistId = playlist1.PlaylistId,
                Song = songModel2,
                SongId = songModel2.SongId
            };

            Models.BridgeModel.PlaylistSongModel thir = new Models.BridgeModel.PlaylistSongModel()
            {
                Playlist = playlist1,
                PlaylistId = playlist1.PlaylistId,
                Song = songModel3,
                SongId = songModel3.SongId
            };






            Models.BridgeModel.PlaylistSongModel fir = new Models.BridgeModel.PlaylistSongModel()
            {
                Playlist = playlist1,
                PlaylistId = playlist1.PlaylistId,
                Song = songModel,
                SongId = songModel.SongId
            };

            Models.BridgeModel.PlaylistSongModel sec = new Models.BridgeModel.PlaylistSongModel()
            {
                Playlist = playlist1,
                PlaylistId = playlist1.PlaylistId,
                Song = songModel2,
                SongId = songModel2.SongId
            };

            Models.BridgeModel.PlaylistSongModel thir = new Models.BridgeModel.PlaylistSongModel()
            {
                Playlist = playlist1,
                PlaylistId = playlist1.PlaylistId,
                Song = songModel3,
                SongId = songModel3.SongId
            };

            dbContext.Add(songModel);
            dbContext.Add(songModel2);
            dbContext.Add(songModel3);
            dbContext.Add(artistModel);
            dbContext.Add(artistModel2);
            dbContext.Add(artistModel3);
            dbContext.SaveChanges();
            */

            ICollection<SongModel> songs = randomSongs(5);
            ICollection<ArtistModel> artists = randomArtists(3);
            ICollection<UserModel> users = randomUsers(3);
            ICollection<PlaylistModel> playlists = randomPlaylists(2);
            ICollection<Models.BridgeModel.ArtistSongModel> artistsong = randomArtistToSong(songs, artists);
            ICollection<Models.BridgeModel.PlaylistSongModel> playlistsong = randomSongsToPlaylist(playlists, songs);
            return true;
        }
    }
}
