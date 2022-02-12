using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public static class EnumerableStringsExtensions
    {
        internal static int IndexOf(this IEnumerable<string> value, string? searchValue, StringComparison comparison)
        {
            for (int i = 0; i < value.Count(); i++)
            {
                if (value.ElementAt(i).Equals(searchValue, comparison))
                    return i;
            }

            return -1;
        }
    }
}
