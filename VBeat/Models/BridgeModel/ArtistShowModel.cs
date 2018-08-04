using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.BridgeModel
{
    public class ArtistShowModel
    {
        [Key]
        public int UserId { get; set; }
        public ArtistModel Artist { get; set; }

        [Key]
        public int ShowId { get; set; }
        public ShowModel Show {get;set;}
    }
}
