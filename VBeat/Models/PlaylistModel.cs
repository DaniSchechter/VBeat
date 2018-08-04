using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models
{
    public class PlaylistModel
    {
        [Key]
        public int PlaylistId { get; set; }
        public virtual ICollection<SongModel> Songs { get; set; }
        [Required]
        [Display(Name = "Is Public")]
        public bool Public { get; set; }
        public string PlaylistImage { get; set; }
    }
}
