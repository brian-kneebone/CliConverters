using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools
{
    public static class EnumerableStringsExtensions
    {
        public static int IndexOf(this IEnumerable<string> value, string? searchValue, StringComparison comparison)
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
