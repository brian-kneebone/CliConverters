using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Loggers
{
    public enum TextFileWriterState
    {
        Idle,
        Running,
        Stopping,
        Disposed
    }
}
