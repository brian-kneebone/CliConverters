using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public abstract class ValueOnlyParameter : ToolParameter
    {
        public static string GetStringValue(IList<string> args)
        {
            return args.Extract(0);
        }
    }

    public abstract class ValueOnlyParameter<V> : ValueOnlyParameter where V : class
    {
        public V Value { get; private set; }

        public abstract V DefaultValue { get; }

        protected ValueOnlyParameter(V value)
        {
            if (this.IsRequired && value == null)
                throw new ArgumentNullException(nameof(value));

            this.Value = value ?? this.DefaultValue;
        }



    }
}
