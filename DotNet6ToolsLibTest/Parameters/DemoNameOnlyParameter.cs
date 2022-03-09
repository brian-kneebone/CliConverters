using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    internal class DemoNameOnlyParameter : NameOnlyParameter
    {
        internal DemoNameOnlyParameter(string? shortName, string longName) : base(shortName, longName)
        {

        }
    }
}
