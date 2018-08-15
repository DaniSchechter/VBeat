using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;
using VBeat.Models.Validations;

namespace VBeat.Models
{      
    public enum enum_Genre { Blues , Country, Electronic, Folk, Hip_hop, Jazz, Latin, Pop, Rock, Classic, Other};
    public class SongModel
    {
        [Key]
        public int SongId { get; set; }

        [Required]
        [Display(Name = "Song Name")]
        public string SongName { get; set; }

        [Required]
        [Display(Name = "Song Genre")]
        public String Genre { get; set; }

        [Required]
        [Display(Name  ="URL To MP3")]
        public string SongPath { get; set; }

        [Required, Display(Name = "Song Image URL")]
        public string SongImagePath { get; set; }

        [Required, Display(Name ="Release Date"),DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), PastDate]
        public DateTime ReleaseDate { get; set; }

        [Required,Display(Name = "Added To Site Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), PastDate]
        public DateTime AddedDate { get; set; }

        virtual public ICollection<PlaylistSongModel> Playlists { get; } = new List<PlaylistSongModel>();
        virtual public ICollection<ArtistSongModel> ArtistList { get; } = new List<ArtistSongModel>();
    }
}
