using System;
using System.Collections.Generic;

namespace processAI1.Board
{
    class DuplicateKeyComparer<TKey>
        :
            IComparer<TKey> where TKey : IComparable
    {

        public int Compare(TKey x, TKey y)
        {
            if (x.CompareTo(y) <= 0)
                return 1;
            else
                return -1;
        }
    }
}
