using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class ShowModel
    {
        public int ShowId { get; set; }
        public string ShowName { get; set; }
        public virtual List<ArtistModel> ArtistList { get; set; }
        public string Country { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public DateTime ShowTime { get; set; }
    }
}
