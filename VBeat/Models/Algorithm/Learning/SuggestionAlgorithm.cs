using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Algorithm.Learning
{
    interface SuggestionAlgorithm
    {
        List<int> Suggset(PlaylistModel playlist);
    }
}
