using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Loggers
{
    public class TextFileLogger : ITextFileLogger
    {
        private readonly object _lock = new object();

        public LoggerStates State 
        { 
            get { lock (_lock) { return this._state; } }
            private set { lock (_lock) { this._state = value; } } 
        }
        private LoggerStates _state = LoggerStates.Unopened;

        private readonly string _filenamePrefix;
        private readonly LogLevels _level = LogLevels.None;

        public string? Filename => this._filename;
        private string? _filename = null;

        private FileStream? _stream = null;
        private StreamWriter? _writer = null;

        private TextFileLogger(string filenamePrefix, LogLevels? level = LogLevels.None)
        {
            if (string.IsNullOrEmpty(filenamePrefix))
                throw new ArgumentNullException(nameof(filenamePrefix));    

            this._filenamePrefix = filenamePrefix ?? throw new ArgumentNullException(nameof(filenamePrefix));

            if (level.HasValue)
                this._level = level.Value;
        }

        public void Debug(string message)
        {
            var prefix = TextFileLogger.GetStringPrefix("DEBUG");
            
            switch (this._level)
            {
                case LogLevels.Debug:
                case LogLevels.Info:
                case LogLevels.Warn:
                case LogLevels.Error:
                    this.Write(prefix, message);
                    break;

                case LogLevels.None:
                    // Do Nothing
                    break;

                default:
                    throw new NotImplementedException($"Log level {this._level} not implemented.");
            }
        }

        public void Info(string message)
        {
            var prefix = TextFileLogger.GetStringPrefix("DEBUG");

            switch (this._level)
            {
                case LogLevels.Debug:
                case LogLevels.Info:
                    this.Write(prefix, message);
                    break;

                case LogLevels.Warn:
                case LogLevels.Error:
                case LogLevels.None:
                    // Do Nothing
                    break;

                default:
                    throw new NotImplementedException($"Log level {this._level} not implemented.");
            }
        }

        public void Warn(string message)
        {
            var prefix = TextFileLogger.GetStringPrefix("DEBUG");

            switch (this._level)
            {
                case LogLevels.Debug:
                case LogLevels.Info:
                case LogLevels.Warn:
                    this.Write(prefix, message);
                    break;

                case LogLevels.Error:
                case LogLevels.None:
                    // Do Nothing
                    break;

                default:
                    throw new NotImplementedException($"Log level {this._level} not implemented.");
            }
        }

        public void Error(string message, Exception? exception = null)
        {
            var prefix = TextFileLogger.GetStringPrefix("DEBUG");

            switch (this._level)
            {
                case LogLevels.Debug:
                case LogLevels.Info:
                case LogLevels.Warn:
                case LogLevels.Error:
                    this.Write(prefix, message, exception);
                    break;

                case LogLevels.None:
                    // Do Nothing
                    break;

                default:
                    throw new NotImplementedException($"Log level {this._level} not implemented.");
            }
        }

        private void Write(string prefix, string message, Exception? error = null)
        {
            this.OpenIfNeeded();

            switch (this.State)
            {
                case LoggerStates.Unopened:
                    // Wait until opened, assuming another thread called this method entering the OpenIfNeeded() method.
                    while (this.State != LoggerStates.Open) Thread.Sleep(1000);
                    break;

                case LoggerStates.Opening:
                    // Wait until opened, already in the openening state.
                    while (this.State != LoggerStates.Open) Thread.Sleep(1000);
                    break;

                case LoggerStates.Open:
                    // Continue, ready to write!
                    break;

                case LoggerStates.Closing:
                    throw new InvalidOperationException("Cannot open text file logger when closing.");

                case LoggerStates.Closed:
                    throw new InvalidOperationException("Cannot open text file logger when closed.");

                default:
                    throw new NotImplementedException($"Cannot write with text file logger state {this.State}.");
            }

            this._writer?.WriteLine(prefix + message);

            if (error != null)
                this._writer?.WriteLine(error.ToString());

            this._writer?.Flush();
        }

        public void Dispose()
        {
            DateTime now = DateTime.Now;

            switch (this.State)
            {
                case LoggerStates.Open:
                    // Continue the method.
                    break;

                case LoggerStates.Unopened:
                case LoggerStates.Closed:
                    // Nothing to close, already there.
                    return;

                case LoggerStates.Opening:
                    // Wait until the state is opened to continue the method.
                    while (this.State != LoggerStates.Open)
                        Thread.Sleep(1000);
                    break;

                case LoggerStates.Closing:
                    // Wait until the state is closed to exit the method.
                    while (this.State != LoggerStates.Closed)
                        Thread.Sleep(1000);
                    return;

                default:
                    throw new NotImplementedException($"Cannot close text file logger while this state is {this.State}.");
            }

            this.Close(now);
            GC.SuppressFinalize(this);
        }

        private void Close(DateTime now)
        {
            try
            {
                this.State = LoggerStates.Closing;
                this._writer?.Write($"Close: {this._filename} @ {now.ToString("yyyy-MM-dd @ HH:mm:ss.fff")}");
                this._writer?.Flush();
                this._writer?.Close();
                this._stream?.Close();
            }
            catch
            {
                this.State = LoggerStates.Closed;
            }
        }

        private void OpenIfNeeded()
        {
            var now = DateTime.Now;

            switch (this.State)
            {
                case LoggerStates.Open:
                    // Nothing to open, already there.
                    return;

                case LoggerStates.Unopened:
                    // Continue the method.
                    break;

                case LoggerStates.Opening:
                    // Wait until the state is opented to continue the method.
                    while (this.State != LoggerStates.Open)
                        Thread.Sleep(1000);
                    break;

                case LoggerStates.Closing:
                    throw new InvalidOperationException("Cannot open text file logger when closing.");

                case LoggerStates.Closed:
                    throw new InvalidOperationException("Cannot open text file logger when closed.");

                default:
                    throw new NotImplementedException($"Cannot open text file logger when state is {this.State}.");
            }

            this.Open(now);

        }

        private void Open(DateTime now)
        {
            this.State = LoggerStates.Opening;

            this._filename = this._filenamePrefix + "_" + now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var file = new FileInfo(this._filename);

            if (file.Exists)
                throw new IOException($"Cannot create log file '{this._filename}' since it exists already.");

            this._stream = file.OpenWrite();
            this._writer = new StreamWriter(this._stream);
            this._writer.Write($"Open: {this._filename} @ {now.ToString("yyyy-MM-dd @ HH:mm:ss.fff")}");
            this._writer.Flush();

            this.State = LoggerStates.Open;
        }

        private static string GetStringPrefix(string type)
        {
            return DateTime.Now.ToString($"yyyy-MM-dd-HH-mm-ss.fff [{type}] ");
        }

        public static ITextFileLogger Create(string filenamePrefix, LogLevels? level = LogLevels.None)
        {
            return new TextFileLogger(filenamePrefix, level);
        }

    }
}
