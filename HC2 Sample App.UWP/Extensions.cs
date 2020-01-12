using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HC2_Sample_App.UWP
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
        {
            foreach (var item in col)
            {
                action(item);
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> col, IEnumerable<T> listtoadd)
        { 
            foreach (var item in listtoadd)
            {
               col.Add(item);
            }
        }
    }
}
