using System;
using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(IEnumerable<T> list, Action<T> action)
        {
            foreach (var e in list)
            {
                action?.Invoke(e);
            }
        }
    }
}
