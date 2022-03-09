using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Loggers
{
    public enum TextFileWriterState
    {
        Idle,
        Running,
        Stopping,
        Disposed
    }
}
