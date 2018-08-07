using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Session
{
    public class SessionFactory
    {
        public static SessionManager GetSessionManager()
        {
            return new EmptySessionFactory();
        }
    }

     class EmptySessionFactory : SessionManager
    {
        public ArtistModel GetArtistModel()
        {
            return null;
        }

        public UserModel GetUserModel()
        {
            VBeatDbContext vBeat = new VBeatDbContext();
            return vBeat.Users.First();
        }
    }
}
