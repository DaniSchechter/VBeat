using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;

namespace VBeat.Models
{
    public class PlaylistModel
    {
        [Key]
        public int PlaylistId { get; set; }

        public virtual ICollection<PlaylistSongModel> Songs { get; } = new List<PlaylistSongModel>();

        [Required]
        [Display(Name = "Is Public")]
        public bool Public { get; set; }

        [Display(Name = "Playlist Image")]
        public string PlaylistImage { get; set; }

        [Display(Name = "Playlist Name")]
        public string PlaylistName { get; set; }

        virtual public UserModel UserModel { get; set; }
    }
}
