using System;
using System.Collections.Generic;
using System.Linq;

namespace Venn.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Replace<T>(this IList<T> aList, IList<T> bList, Func<T,T,bool> comparer)
        {
            aList.ToList().ForEach(itemA =>
            {
                bList.ToList().ForEach(itemB => 
                {
                    if(comparer(itemA, itemB))
                    {
                        aList[aList.IndexOf(itemA)] = itemB;
                    }
                });
            });

            return aList;
        }
    }

}
