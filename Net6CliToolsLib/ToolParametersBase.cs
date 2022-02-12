using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Net6CliTools
{
    public abstract class ToolParametersBase
    {
        public FileInfo? Filename { get; private set; } = null;

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

            this.ParseFileNameIfPresent(args);
            
        }

        private void ParseFileNameIfPresent(string[] args)
        {
            if (args.Length == 0)
                return;

            this.Filename = new FileInfo(args[1]);

            if (this.Filename.Exists)
                throw new FileNotFoundException(args[1]);

        }



    }
}
