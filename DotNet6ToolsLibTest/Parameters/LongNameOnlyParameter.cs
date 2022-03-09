using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    internal class LongNameOnlyParameter : NameOnlyParameter
    {
        internal const string SHORT_NAME = null;

        internal const string LONG_NAME = "dummy-arg-name-b";

        internal LongNameOnlyParameter() : base(SHORT_NAME, LONG_NAME)
        {
        }
    }
}
