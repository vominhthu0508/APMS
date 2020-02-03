using System;
using System.Collections.Generic;

namespace XT.Utilities
{
    public static class RandomSelector
    {
        public static Random Rand = new Random();

        public static List<T> Deal<T>(IList<T> items, int needed)
        {
            var selected = new List<T>();

            var available = items.Count;

            if (available > 0 && available >= needed)
            {
                while (selected.Count < needed)
                {
                    if (Rand.NextDouble() < (double)needed / available)
                    {
                        selected.Add(items[available - 1]);
                        needed--;
                    }
                    available--;
                }
            }
            return selected;
        }
    }
}