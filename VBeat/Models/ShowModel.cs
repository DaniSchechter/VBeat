using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;
using VBeat.Models.Validations;

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
        [StringLength(15)]
        public string Country { get; set; }

        [Required]
        [StringLength(15)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        [StringLength(20)]
        public string StreetName { get; set; }

        [Required]
        [Display(Name = "House #")]
        [PositiveNumber]
        public int HouseNumber { get; set; }

        [Required]
        [Display(Name = "Show Time")]
        [FutureDate]
        //needs also hour
        public DateTime ShowTime { get; set; }

        [Required]
        [Display(Name = "Show Image Path")]
        public string ShowImagePath { get; set; }
    }
}
