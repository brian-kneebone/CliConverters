using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    public class InputFileParameter : ValueOnlyParameter<FileInfo?>
    {
        private InputFileParameter(FileInfo? value) : base(value)
        {
        }

        public static InputFileParameter TryCreate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new InputFileParameter(null);

            var inputFile = new FileInfo(value);

            if (!inputFile.Exists)
                throw new FileNotFoundException(nameof(value));

            return new InputFileParameter(inputFile);
        }

    }
}
