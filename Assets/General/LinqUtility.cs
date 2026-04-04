using System;
using System.Collections.Generic;

namespace BenScr.UnityStack
{
    public static class LinqUtility
    {
        public static bool Contains<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item)) return true;
            }

            return false;
        }

        public static T Find<T>(IEnumerable<T> items, Func<T, bool> predicate, T defaultValue = default)
        {
            foreach (var item in items)
            {
                if (predicate(item)) return item;
            }

            return defaultValue;
        }

        public static T Find<T>(IEnumerable<T> items, T obj, T defaultValue = default)
        {
            foreach (var item in items)
            {
                if (item.Equals(obj)) return item;
            }

            return defaultValue;
        }

        public static int IndexOf<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return i;
                i++;
            }

            return -1;
        }

        public static int Count<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            int i = 0;

            foreach (var item in items)
            {
                if (predicate(item)) i++;
            }

            return i;
        }
    }
}
