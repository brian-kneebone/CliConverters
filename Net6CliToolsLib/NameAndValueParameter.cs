using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools
{
    public abstract class NameAndValueParameter : ToolParameter
    {
        public static string GetStringValue(IList<string> args, string? shortName, string longName)
        {
            var index = args.IndexOfNamedArgument(shortName, longName);
            return args.Extract(index);
        }
    }

    public abstract class NameAndValueParameter<V> : NameAndValueParameter where V : class
    {
        public abstract string? ShortName { get; protected set; }

        public abstract string LongName { get; protected set; }

        public abstract V Value { get; protected set; }

        public abstract V DefaultValue { get; }

        protected NameAndValueParameter(string? shortName, string longName, V value)
        {
            this.ShortName = shortName;
            this.LongName = longName ?? throw new ArgumentNullException(nameof(longName));

            if (this.IsRequired && value == null)
                throw new ArgumentNullException(nameof(value));

            this.Value = value ?? this.Value;
        }

    }
}
