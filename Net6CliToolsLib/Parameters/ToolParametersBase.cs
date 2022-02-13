using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Net6CliTools.Parameters
{
    public abstract class ToolParametersBase
    {
        protected string[] Args => this._args;
        private readonly string[] _args;

        protected ToolParametersBase(string[] args)
        {
            this._args = args ?? throw new ArgumentNullException(nameof(args));
            var remaining = this.ParseArguments();
            this.ParseRemainingArguments(remaining);

        }

        protected abstract string[] ParseArguments();

        private void ParseRemainingArguments(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

        }

        



    }
}
