using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    public abstract class NameOnlyParameter : ToolParameter
    {
        public ParameterName Name { get; private set; }

        protected NameOnlyParameter(string? shortName, string longName)
        {
            this.Name = new ParameterName(shortName, longName);
        }

    }
}
