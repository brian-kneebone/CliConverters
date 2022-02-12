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
            var shortIndex = (shortName == null) ? -1 : value.IndexOfCaseInsensitive($"-{shortName}");
            var longIndex = value.IndexOfCaseInsensitive($"--{longName}");

            if (shortIndex == -1 && longIndex == -1)
                return -1;
            else if (shortIndex == -1)
                return longIndex;
            else if (longIndex == -1)
                return shortIndex;
            else
                return Math.Min(shortIndex, longIndex);

        }

        internal static int IndexOfCaseInsensitive(this IList<string> value, string? searchValue)
        {
            if (searchValue == null)
                return -1;

            var loweredSearchValue = searchValue.ToLower();

            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].ToLower() == loweredSearchValue)
                    return i;
            }

            return -1;

        }

    }
}
