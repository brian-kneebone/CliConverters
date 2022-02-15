using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Loggers
{
    public class TextFileWriter
    {
        private DateTime? _start = null;

        private readonly object _queueLock = new object();
        private Queue<string> _queue = new Queue<string>();

        private readonly object _cancelLock = new object();

        private bool CancellationPending
        {
            get { lock (_cancelLock) { return this._cancellationPending; } }
            set { lock (_cancelLock) { this._cancellationPending = value; } }
        }

        private bool _cancellationPending = false;

        private readonly FileInfo? _file;
        private readonly FileStream? _stream;
        private readonly StreamWriter? _writer;

        public TextFileWriter(string? filename)
        {
            this._file = new FileInfo(filename ?? throw new ArgumentNullException(nameof(filename)));
            this._stream = this._file.OpenWrite();
            this._writer = new StreamWriter(this._stream);
        }

        private async void StartAsync()
        {
            var task = new Task(() => { this.LoopingWrite(); });
            // var awaiter = task.GetAwaiter();
            task.Start();
            await task;
        }

        public async void StopAsync()
        {
            var task = new Task(() => { this.CancellationPending = false; });
            // var awaiter = task.GetAwaiter();
            task.Start();
            await task;
        }

        public async void WriteAsync(string line)
        {
            var task = new Task(() => { this.Write(line); });
            // var awaiter = task.GetAwaiter();
            task.Start();
            await task;
        }

        private void Write(string line)
        {
            lock (this._queueLock)
                this._queue.Enqueue(line);
        }

        private void LoopingWrite()
        {
            while (!this.CancellationPending)
            {
                this.WriteAndFlushQueueIfNeeded();

                if (!this.CancellationPending)
                    Thread.Sleep(100); 
            }

            this.WriteStopIfNeeded();
        }

        private void WriteAndFlushQueueIfNeeded()
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
            if (!enqueuedMessagesFound || this._start.HasValue)
                return;

            this._start = DateTime.Now;
            this._writer?.WriteLine($"Start {this._file?.Name} @ {this._start?.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            this._writer?.WriteLine(string.Empty);
            this._writer?.Flush();
        }

        private void WriteStopIfNeeded()
        {
            if (!this._start.HasValue)
                return;

            var stop = DateTime.Now;
            var duration = stop - this._start.Value;
            this._writer?.WriteLine(string.Empty);
            this._writer?.WriteLine($"Stop {this._file?.Name} @ {this._start?.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            this._writer?.WriteLine($"Duration: {duration.ToString("dd HH:mm:ss.fff")}");
            this._writer?.Flush();
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
