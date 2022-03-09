using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools
{
    public static class FileInfoExtensions
    {
        public static bool Exists(this FileInfo value, int timeoutInMilliseconds)
        {
            if (value.Exists)
                return true;

            var started = DateTime.Now;
            var duration = TimeSpan.Zero;

            while (duration.TotalMilliseconds < timeoutInMilliseconds)
            {
                if (value.Exists)
                    return true;

                duration = DateTime.Now - started;
                Thread.Sleep(50);
            }

            return value.Exists;
        }
    }
}
