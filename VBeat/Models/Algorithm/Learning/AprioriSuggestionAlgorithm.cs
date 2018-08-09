using Accord.MachineLearning.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Algorithm.Learning
{
    public class AprioriSuggestionAlgorithm : SuggestionAlgorithm
    {
        private static AprioriSuggestionAlgorithm instance = null;
        public static AprioriSuggestionAlgorithm GetInstance()
        {
            if (instance == null)
            {
                instance = new AprioriSuggestionAlgorithm();
            }
            return instance;
        }

        private AprioriSuggestionAlgorithm()
        {

        }
        public void Train()
        {

        }

        public List<SongModel> Suggset(PlaylistModel playlist)
        {
        
            
        }
    }
}
