using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Loggers
{
    public class TextFileWriter : IDisposable
    {
        private DateTime? _start = null;

        private readonly object _queueLock = new object();
        private Queue<string> _queue = new Queue<string>();

        private readonly object _stateLock = new object();

        public TextFileWriterState State
        {
            get { lock (_stateLock) { return this._state; } }
            private set { lock (_stateLock) { this._state = value; } }
        }

        private TextFileWriterState _state = TextFileWriterState.Idle;

        private readonly FileInfo? _file;
        private FileStream? _stream;
        private StreamWriter? _writer;

        public TextFileWriter(string? filename)
        {
            this._file = new FileInfo(filename ?? throw new ArgumentNullException(nameof(filename)));
        }

        public async void StartAsync()
        {
            var task = new Task(() => { this.Start(); });
            // var awaiter = task.GetAwaiter();
            task.Start();
            await task;
        }

        private void Start()
        {
            this._start = DateTime.Now;
            this.State = TextFileWriterState.Running;
            this.LoopingWrite();
        }

        //public async Task StopAsync()
        //{
        //    var task = new Task(() => { this.Stop(); });
        //    // var awaiter = task.GetAwaiter();
        //    task.Start();
        //    await task;
        //}

        public void Stop()
        {
            this.State = TextFileWriterState.Stopping;

            while(this.State != TextFileWriterState.Disposed)
                Thread.Sleep(250);
        }

        public void Dispose()
        {
            this.Stop();
            GC.SuppressFinalize(this);
        }

        public async void WriteLineAsync(string line)
        {
            var task = new Task(() => { this.WriteLine(line); });
            // var awaiter = task.GetAwaiter();
            task.Start();
            await task;
        }

        private void WriteLine(string line)
        {
            lock (this._queueLock)
                this._queue.Enqueue(line);
        }

        private void LoopingWrite()
        {
            while (this.State == TextFileWriterState.Running)
            {
                this.WriteIfNeeded();

                if (this.State == TextFileWriterState.Running)
                    Thread.Sleep(100); 
            }

            this.WriteIfNeeded();
            this.WriteStop();
        }

        private void WriteIfNeeded()
        {
            var count = this.GetQueueCount();

            this.WriteStartIfNeeded(count > 0);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    this._writer?.WriteLine(this.DequeueLine());
                }

                if (count > 0)
                    this._writer?.Flush();
            }
        }

        private void WriteStartIfNeeded(bool enqueuedMessagesFound)
        {
            if (!enqueuedMessagesFound || (this._writer != null))
                return;

            if (this._file == null)
                throw new NullReferenceException(nameof(this._file));

            this._stream = this._file.OpenWrite();
            this._writer = new StreamWriter(this._stream);

            this._writer.WriteLine($"Start {this._file?.Name} @ {this._start?.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            this._writer.WriteLine(string.Empty);
            this._writer.Flush();
        }

        private void WriteStop()
        {
            if (!this._start.HasValue)
            {
                this.State = TextFileWriterState.Disposed;
                return;
            }

            var stop = DateTime.Now;
            var duration = stop - this._start.Value;
            this._writer?.WriteLine(string.Empty);
            this._writer?.WriteLine($"Stop {this._file?.Name} @ {this._start?.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            this._writer?.WriteLine($"Duration: {duration}");
            this._writer?.Flush();

            this._writer?.Dispose();
            this._stream?.Dispose();
            this.State = TextFileWriterState.Disposed;
        }

        private int GetQueueCount()
        {
            lock (this._queueLock)
                return this._queue.Count;
        }

        private string DequeueLine()
        {
            lock (this._queueLock)
                return this._queue.Dequeue();
        }

    }
}
