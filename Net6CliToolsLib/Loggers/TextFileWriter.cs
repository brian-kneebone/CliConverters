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
        private Queue<string> _queue = new();

        private readonly object _stateLock = new object();

        public TextFileWriterState State
        {
            get { lock (_stateLock) { return this._state; } }
            private set { lock (_stateLock) { this._state = value; } }
        }

        private TextFileWriterState _state = TextFileWriterState.Idle;

        public FileInfo File => this._file;
        private readonly FileInfo _file;
        
        private FileStream? _stream;
        private StreamWriter? _writer;

        public TextFileWriter(string? filename)
        {
            this._file = new FileInfo(filename ?? throw new ArgumentNullException(nameof(filename)));
        }

        private async void StartAsync()
        {
            this._state = TextFileWriterState.Running;

            var task = new Task(() => {
                this._start = DateTime.Now;
                this.LoopingWrite();
            });
            
            task.Start();
            await task;
        }

        public void Start()
        {
            if (this.State != TextFileWriterState.Idle)
                throw new InvalidOperationException($"{this.GetType().Name} is {this.State} and cannot start asynchronously while not in a {TextFileWriterState.Idle} state.");

            this.StartAsync();
            this.WaitUntilRunning();
        }

        private void WaitUntilRunning()
        {
            switch (this.State)
            {
                case TextFileWriterState.Running:
                    return;

                case TextFileWriterState.Idle:
                    while (this.State == TextFileWriterState.Idle) Thread.Sleep(50);
                    return;

                case TextFileWriterState.Stopping:
                case TextFileWriterState.Disposed:
                default:
                    throw new InvalidOperationException($"{this.GetType().Name} is {this.State} and cannot wait until in a {TextFileWriterState.Running} state.");
            }

        }

        public void WaitUntilDisposed(int? waitInMilliseconds = null)
        {
            var waitStarted = DateTime.Now;

            while (this._state != TextFileWriterState.Disposed)
            {
                if (waitInMilliseconds.HasValue)
                {
                    var duration = DateTime.Now - waitStarted;

                    if (duration.TotalMilliseconds > waitInMilliseconds.Value)
                        throw new TimeoutException($"{this.GetType().Name} failed to dispose in {waitInMilliseconds.Value} milliseconds with state {this._state}.");
                }

                Thread.Sleep(50);
            }
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
            if (this._state != TextFileWriterState.Running)
                throw new InvalidOperationException($"{this.GetType().Name} is {this.State} and cannot stop while not in a {TextFileWriterState.Running} state.");

            this.State = TextFileWriterState.Stopping;

            while(this.State != TextFileWriterState.Disposed)
                Thread.Sleep(250);
        }

        public void Dispose()
        {
            if (this.State == TextFileWriterState.Running)
                this.Stop();

            GC.SuppressFinalize(this);
        }

        public void WriteLine(string line)
        {
            if (this._state != TextFileWriterState.Running)
                throw new InvalidOperationException($"{this.GetType().Name} is {this.State} and cannot write while not in a {TextFileWriterState.Running} state.");

            var task = new Task(() => {

                lock (this._queueLock)
                    this._queue.Enqueue(line);

            });

            task.Start();
        }

        private void LoopingWrite()
        {
            while (this._state == TextFileWriterState.Running)
            {
                this.WriteIfNeeded();

                if (this._state == TextFileWriterState.Running)
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
            this._state = TextFileWriterState.Disposed;
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
