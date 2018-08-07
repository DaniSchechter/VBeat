using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.BridgeModel
{
    public class ArtistShowModel
    {
        public int UserId { get; set; }
        virtual public ArtistModel Artist { get; set; }

        public int ShowId { get; set; }
        virtual public ShowModel Show {get;set;}
    }
}
