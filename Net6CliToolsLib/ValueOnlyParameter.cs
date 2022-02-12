using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public abstract class ValueOnlyParameter<V> : ToolParameter where V : class
    {
        public V Value { get; private set; }

        public abstract V DefaultValue { get; }

        protected ValueOnlyParameter(V value)
        {
            if (this.IsRequired && value == null)
                throw new ArgumentNullException(nameof(value));

            this.Value = value ?? this.DefaultValue;
        }

        protected ValueOnlyParameter(string value)
        {
            throw new NotImplementedException();
        }

    }
}
