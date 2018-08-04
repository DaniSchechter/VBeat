using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.BridgeModel
{
    public class ArtistSongModel
    {
        public int UserId { get; set; }
        public ArtistModel Artist { get; set; }

        public int SongId { get; set; }
        public SongModel Song { get; set; }
        
    }
}
