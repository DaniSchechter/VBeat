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

        public ICollection<SongModel> RandomSongs(int num)
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


        public ICollection<UserModel> RandomUsers(int num)
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

        public ICollection<ArtistModel> RandomArtists(int num)
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

        public ICollection<PlaylistModel> RandomPlaylists(int num)
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

        public ICollection<Models.BridgeModel.ArtistSongModel> RandomArtistToSong(ICollection<SongModel> songs, ICollection<ArtistModel> artists)
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

        public ICollection<Models.BridgeModel.PlaylistSongModel> RandomSongsToPlaylist(ICollection<PlaylistModel> playlists, ICollection<SongModel> songs)
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

            ICollection<SongModel> songs = RandomSongs(20);
            ICollection<ArtistModel> artists = RandomArtists(3);
            ICollection<UserModel> users = RandomUsers(3);
            ICollection<PlaylistModel> playlists = RandomPlaylists(2);
            ICollection<Models.BridgeModel.ArtistSongModel> artistsong = RandomArtistToSong(songs, artists);
            ICollection<Models.BridgeModel.PlaylistSongModel> playlistsong = RandomSongsToPlaylist(playlists, songs);
            return true;
        }
    }
}
