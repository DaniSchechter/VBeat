using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.BridgeModel
{
    public class ArtistSongModel
    {
        public int UserId { get; set; }
        virtual public ArtistModel Artist { get; set; }

        public int SongId { get; set; }
        virtual public SongModel Song { get; set; }
    }
}
