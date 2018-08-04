using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;

namespace VBeat.Models
{
    public class ShowModel
    {
        [Key]
        public int ShowId { get; set; }

        [Required]
        [Display(Name = "Show Name")]
        [StringLength(20, MinimumLength = 3)]
        public string ShowName { get; set; }
        public virtual ICollection<ArtistShowModel> ArtistList { get; } = new List<ArtistShowModel>();

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        [StringLength(20, MinimumLength = 3)]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "House Name")]
        public int HouseNumber { get; set; }

        [Required]
        [Display(Name = "Show Time")]
        //needs also hour
        public DateTime ShowTime { get; set; }
    }
}
