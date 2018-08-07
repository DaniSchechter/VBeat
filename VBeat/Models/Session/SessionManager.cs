using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Session
{
    interface SessionManager
    {
        UserModel GetUserModel();
        ArtistModel GetArtistModel();
    }
}
