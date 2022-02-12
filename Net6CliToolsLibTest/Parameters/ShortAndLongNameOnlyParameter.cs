using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Test.Parameters
{
    internal class ShortAndLongNameOnlyParameter : NameOnlyParameter
    {
        internal const string SHORT_NAME = "d";

        internal const string LONG_NAME = "dummy-arg-name-d";

        public override bool IsRequired => true;

        private ShortAndLongNameOnlyParameter(string? shortName, string longName) : base(shortName, longName)
        {
        }


    }
}
