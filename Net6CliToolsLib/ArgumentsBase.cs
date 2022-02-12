using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Net6CliTools
{
    public abstract class ArgumentsBase
    {
        public FileInfo? Filename { get; private set; } = null;

        protected string[] Args => this._args;
        private readonly string[] _args;

        protected ArgumentsBase(string[] args)
        {
            this._args = args ?? throw new ArgumentNullException(nameof(args));
            var remaining = this.ParseArguments();
            this.ParseRemainingCommonArguments(remaining);

        }

        protected abstract string[] ParseArguments();

        private void ParseRemainingCommonArguments(string[] args)
        {
            if (args == null || args.Length == 0)
                return;

            if (args.Length == 1)
                this.ParseFileName(args[0]);
        }

        private void ParseFileName(string filename)
        {
            this.Filename = new FileInfo(filename);

            if (this.Filename.Exists)
                throw new FileNotFoundException(nameof(filename));
        }


    }
}
