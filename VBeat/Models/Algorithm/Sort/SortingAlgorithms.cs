using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Algorithm.Sort
{
    public class SortingAlgorithms
    {
        public static ICollection<T> BubbleSort<T>(ICollection<T> list, Comparer<T> comparer)
        {
            T temp;
            T[] arr = list.ToArray<T>();
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if (comparer.Compare(arr[sort], arr[sort + 1])  > 0)
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                    }
                }
            }

            return new List<T>(arr);
        }
    }
}
