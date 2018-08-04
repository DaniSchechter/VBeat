using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class SongModel
    {
        [Key]
        public int SongId { get; set; }
        public string SongName { get; set; }
        virtual public ICollection<ArtistModel> ArtistList { get; set; }
        public string Genre { get; set; }
        public string SongPath { get; set; }
        public string SongImagePath { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
