using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Algorithm.Learning
{
    interface SuggestionAlgorithm
    {
        List<SongModel> Suggset(PlaylistModel playlist);
    }
}
