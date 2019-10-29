using System.Globalization;
using System.Linq;
using HC2.Arcanastudio.Net.Models.Interfaces;

namespace HC2.Arcanastudio.Net.Models
{
    internal class Range : IRange
    {
        public double Start { get; set; }
        public double End { get; set; }
        public double Step { get; set; }

        public static Range FromString(string range)
        {
            var s = range.Split('(', ')');

            var values = s[1].Split(',');

            var v = values.Select(d => double.Parse(d, new NumberFormatInfo {NumberDecimalSeparator = "."})).ToList();

            return new Range{Start = v[0],End = v[1], Step = v[2]};
        }
    }
} 
