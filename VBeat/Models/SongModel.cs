using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;

namespace VBeat.Models
{
    public class SongModel
    {
        [Key]
        public int SongId { get; set; }

        [Required]
        [Display(Name = "Song Name")]
        public string SongName { get; set; }

        virtual public ICollection<ArtistSongModel> ArtistList { get; } = new List<ArtistSongModel>();

        [Required]
        public string Genre { get; set; }

        [Required]
        public string SongPath { get; set; }

        public string SongImagePath { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AddedDate { get; set; }
    }
}
