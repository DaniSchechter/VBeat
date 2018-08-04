using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class ArtistModel : UserModel
    {
        public virtual ICollection<SongModel> SongList { get; set; }

        [Required]
        [Display(Name ="Artist Name")]
        [StringLength(20 ,MinimumLength=3)]
        public string ArtistName { get; set; }

        public string ArtistImage { get; set; }

        public virtual ICollection<ShowModel> Shows { get; set; }
    }
}
