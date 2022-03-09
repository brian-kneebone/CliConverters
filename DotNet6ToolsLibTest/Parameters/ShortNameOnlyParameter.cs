using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Parameters
{
    internal class ShortNameOnlyParameter : NameOnlyParameter
    {
        internal const string SHORT_NAME = "c";

        internal const string LONG_NAME = null;

        internal ShortNameOnlyParameter() : base(SHORT_NAME, LONG_NAME)
        {
        }

    }
}
