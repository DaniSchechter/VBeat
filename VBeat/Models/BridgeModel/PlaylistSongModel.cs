using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.BridgeModel
{
    public class PlaylistSongModel
    {
        public int SongId { get; set; }
        virtual public SongModel Song { get; set; }

        public int PlaylistId { get; set; }
        virtual public PlaylistModel Playlist { get; set; }
    }
}
