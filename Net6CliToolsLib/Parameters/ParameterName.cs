using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Parameters
{
    public class ParameterName
    {
        public string? ShortName { get; private set; }

        public string LongName { get; private set; }

        public ParameterName(string? shortName, string longName)
        {
            this.ShortName = shortName;
            this.LongName = longName ?? throw new ArgumentNullException(nameof(longName));
        }
    }
}
