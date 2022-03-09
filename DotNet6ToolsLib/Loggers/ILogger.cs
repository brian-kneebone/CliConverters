using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Loggers
{
    public interface ILogger : IDisposable
    {
        LoggerStates State { get; }

        void Info(string message);

        void Warn(string message);
        
        void Error(string message, Exception? ex = null);

        void Debug(string message);

    }
}
