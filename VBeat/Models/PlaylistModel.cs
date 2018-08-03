using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class PlaylistModel
    {
        public int PlaylistId { get; set; }
        public virtual List<SongModel> Songs { get; set; }
        public bool Public { get; set; }
        public string PlaylistImage { get; set; }
    }
}
