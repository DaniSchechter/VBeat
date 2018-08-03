using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class ArtistModel : UserModel
    {
        public virtual List<SongModel> SongList { get; set; }
        public string ArtistName { get; set; }
        public string ArtistImage { get; set; }
        public virtual List<ShowModel> Shows { get; set; }
    }
}
