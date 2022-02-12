using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public abstract class ToolParameter
    {
        public abstract bool IsRequired { get; }
    }
}
