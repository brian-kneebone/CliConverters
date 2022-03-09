using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Parameters
{
    public abstract class NameAndValueParameter : ToolParameter
    {
        public ParameterName Name { get; private set; }

        protected NameAndValueParameter(string? shortName, string longName)
        {
            this.Name = new ParameterName(shortName, longName);
        }

        public static string GetStringValue(IList<string> args, string? shortName, string longName)
        {
            var index = args.IndexOfNamedArgument(shortName, longName);
            return args.Extract(index);
        }

    }

    public abstract class NameAndValueParameter<V> : NameAndValueParameter where V : class?
    {
        public abstract V Value { get; protected set; }

        protected NameAndValueParameter(string? shortName, string longName, V value) : base(shortName, longName)
        {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

    }
}
