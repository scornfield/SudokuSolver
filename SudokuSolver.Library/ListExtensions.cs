﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cornfield.SudokuSolver.Library
{
    public static class ListExtensions
    {
        public static List<List<T>> GetAllCombos<T>(this IEnumerable<T> initialList)
        {
            var ret = new List<List<T>>();

            // The final number of sets will be 2^N (or 2^N - 1 if skipping empty set)
            int setCount = Convert.ToInt32(Math.Pow(2, initialList.Count()));

            // Start at 1 if you do not want the empty set
            for (int mask = 0; mask < setCount; mask++)
            {
                var nestedList = new List<T>();
                for (int j = 0; j < initialList.Count(); j++)
                {
                    // Each position in the initial list maps to a bit here
                    var pos = 1 << j;
                    if ((mask & pos) == pos) { nestedList.Add(initialList.ElementAt(j)); }
                }
                ret.Add(nestedList);
            }
            return ret;
        }

        // Returns true if list 1 contains all of the items in list 2, false otherwise
        public static bool ContainsAll<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            if (list1.Intersect(list2).Count() == list2.Count()) return true;
            return false;
        }

        // Returns true if list 1 contains any of the items in list 2
        public static bool ContainsAny<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            return list2.Any(x => list1.Contains(x));
        }
    }
}
