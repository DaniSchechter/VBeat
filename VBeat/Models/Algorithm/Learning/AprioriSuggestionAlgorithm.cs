using Accord.MachineLearning.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VBeat.Models.BridgeModel;

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

        private Apriori apriori = new Apriori(threshold: 2, confidence: 0.3);
        private AssociationRuleMatcher<int> classifier = null;

        private AprioriSuggestionAlgorithm()
        {

        }

        public void Train(List<PlaylistModel> playlistModelList)
        {
            // getting the songs ready for the machine learning algorithm
            SortedSet<int>[] songIdSetArray = new SortedSet<int>[playlistModelList.Count];
            for (int i = 0; i < songIdSetArray.Length; i++)
            {
                SortedSet<int> songSet = new SortedSet<int>();
                foreach (PlaylistSongModel psm in playlistModelList[i].Songs)
                {
                    songSet.Add(psm.SongId);
                }
                songIdSetArray[i] = songSet;
            }

            classifier = apriori.Learn(songIdSetArray);
        }

        public List<int> Suggset(PlaylistModel playlist)
        {
            if (classifier == null)
            {
                throw new NotTrainedException();
            }

            List<int> songIdList = new List<int>();
            foreach(PlaylistSongModel pms in playlist.Songs)
            {
                songIdList.Add(pms.SongId);
            }

            int[][] matches = classifier.Decide(songIdList.ToArray());
            List<int> resultSongIdList = new List<int>();
            for(int i = 0; i < matches.GetLength(0); i++)
            {
                for(int j = 0; j < matches.GetLength(1); j++)
                {
                    resultSongIdList.Add(matches[i][j]);
                }
            }

            return resultSongIdList;
        }

        class NotTrainedException: Exception
        {
        
        }
    }
}
