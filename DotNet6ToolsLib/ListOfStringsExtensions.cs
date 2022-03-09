using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public static class ListOfStringsExtensions
    {
        internal static string Extract(this IList<string> value, int index)
        {
            if (value.ElementAtOrDefault(index) == null)
                throw new IndexOutOfRangeException($"Extraction index {index} is out of range for list of size {value.Count}.");

            var returnValue = value.First();
            value.RemoveAt(0);
            return returnValue;
        }

        internal static int IndexOfNamedArgument(this IList<string> value, string? shortName, string longName)
        {
            var shortIndex = (shortName == null) ? -1 : value.IndexOf($"-{shortName}", StringComparison.InvariantCultureIgnoreCase);
            var longIndex = value.IndexOf($"--{longName}", StringComparison.InvariantCultureIgnoreCase);

            if (shortIndex == -1 && longIndex == -1)
                return -1;
            else if (shortIndex == -1)
                return longIndex;
            else if (longIndex == -1)
                return shortIndex;
            else
                return Math.Min(shortIndex, longIndex);

        }

        

    }
}
