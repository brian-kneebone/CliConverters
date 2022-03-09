using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Parameters
{
    internal class EmptyNameOnlyParameter : NameOnlyParameter
    {
        internal const string SHORT_NAME = null;

        internal const string LONG_NAME = null;

        internal EmptyNameOnlyParameter() : base(SHORT_NAME, LONG_NAME)
        {
        }
    }
}
