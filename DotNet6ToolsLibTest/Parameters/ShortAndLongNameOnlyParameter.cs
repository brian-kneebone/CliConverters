using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    internal class ShortAndLongNameOnlyParameter : NameOnlyParameter
    {
        internal const string SHORT_NAME = "d";

        internal const string LONG_NAME = "dummy-arg-name-d";

        internal ShortAndLongNameOnlyParameter() : base(SHORT_NAME, LONG_NAME)
        {
        }


    }
}
