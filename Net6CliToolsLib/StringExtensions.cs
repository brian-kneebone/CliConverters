using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public static class StringExtensions
    {
        public static bool MightBeArgumentName(this string value)
        {
            if (value == null)
                return false;

            if (value.StartsWith("-") || value.StartsWith("--"))
                return true;

            return false;
        }
    }
}
