using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public static class ArgumentValidator
    {
        public static bool MightBeArgumentName(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (!value.StartsWith("-") && !value.StartsWith("--"))
                return false;

            throw new NotImplementedException();
        }
    }
}
