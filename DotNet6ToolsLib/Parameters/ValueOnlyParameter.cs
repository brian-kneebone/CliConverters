using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    public abstract class ValueOnlyParameter : ToolParameter
    {
        public static string GetStringValue(IList<string> args)
        {
            return args.Extract(0);
        }
    }

    public abstract class ValueOnlyParameter<V> : ValueOnlyParameter where V : class?
    {
        public V Value { get; private set; }

        protected ValueOnlyParameter(V value)
        {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }



    }
}
