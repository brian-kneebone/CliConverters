using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net6CliTools.Loggers
{
    [TestClass]
    public class TestTextFileWriter
    {
        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestQuicklyOpeningAndClosingWithWrite()
        {
            var filename = "TextFileWriter_QuickOpenCloseWithWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.Start();
            writer.WriteLine("Test quickly opening & closing.");
            writer.Stop();
            writer.WaitUntilDisposed(5000);

            Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
            Assert.IsTrue(File.Exists(filename));
            Assert.IsTrue(File.ReadAllBytes(filename).Length > 0);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestQuicklyOpeningAndClosingNoWrite()
        {
            var filename = "TextFileWriter_QuickOpenCloseNoWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.Start();
            // NoWrite
            writer.Stop();
            writer.WaitUntilDisposed(5000);

            Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
            Assert.IsTrue(!File.Exists(filename));
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestErrorStoppingIdle()
        {
            var filename = "TextFileWriter_ErrorStoppingIdle_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);

            Assert.ThrowsException<InvalidOperationException>(() => {
                writer.Stop();
            });

            //writer.Stop();
            //writer.WaitUntilDisposed(5000);

            Assert.IsTrue(writer.State == TextFileWriterState.Idle);
            Assert.IsTrue(!File.Exists(filename));
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestErrorWritingIdle()
        {
            var filename = "TextFileWriter_ErrorWritingIdle_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);

            Assert.ThrowsException<InvalidOperationException>(() => {
                writer.WriteLine("Test writing while idle.");
            });

            //writer.Stop();
            //writer.WaitUntilDisposed(5000);

            Assert.IsTrue(writer.State == TextFileWriterState.Idle);
            Assert.IsTrue(!File.Exists(filename));
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestErrorStartingRunning()
        {
            var filename = "TextFileWriter_ErrorStartingRunning_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.Start();

            Assert.ThrowsException<InvalidOperationException>(() => { writer.Start(); });

            writer.Stop();
            writer.WaitUntilDisposed(5000);

            Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
            Assert.IsTrue(!File.Exists(filename));
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestMultithreadedWrite()
        {
            var filename = "TextFileWriter_MultithreadedWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.Start();
            // writer.StartAsync();
            // writer.WaitUntilRunning();

            var tasks = CreateThreadsThatErrorLog(writer, 200);
            StartThreadsThatErrorLog(tasks);
            WaitUntilThreadsThatErrorLogComplete(tasks, writer, 25);

            writer.Stop();
            writer.WaitUntilDisposed(5000);

            Assert.IsTrue(tasks.Count(t => t.Status == TaskStatus.RanToCompletion) == tasks.Length);
            Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
            Assert.IsTrue(File.Exists(filename));
            Assert.IsTrue(File.ReadAllBytes(filename).Length > 0);
        }

        private static Task[] CreateThreadsThatErrorLog(TextFileWriter writer, int numberOfThreads)
        {
            var tasks = new Task[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks[i] = new Task(() => { InitiateRandomizedErrorLogging(writer, 10, 50); });
            }

            return tasks;
        }

        private static void StartThreadsThatErrorLog(Task[] tasks)
        {
            foreach (var task in tasks)
                task.Start();
        }

        /// <param name="tasks">Array of Tasks</param>
        /// <param name="writer">Writer</param>
        /// <param name="waitBetweenStatusChecksInMs">Wait Between Status Checks</param>
        private static void WaitUntilThreadsThatErrorLogComplete(Task[] tasks, TextFileWriter writer, int waitBetweenStatusChecksInMs)
        {
            writer.WriteLine("Start Monitoring");

            var continueRunning = true;
            var random = new Random();

            while (continueRunning)
            {
                writer.WriteLine(" -> Thread Status:");

                var statusResults = tasks.Select(t => t.Status).ToArray();
                var statusTypes = Enum.GetNames<TaskStatus>();

                foreach (var statusType in statusTypes)
                {
                    var count = statusResults.Count(r => r.ToString() == statusType);
                    writer.WriteLine($" ---> {statusType}: {count}");
                }

                var numberDone = statusResults.Count(t => (t == TaskStatus.RanToCompletion) || (t == TaskStatus.Faulted) || (t == TaskStatus.Canceled));
                continueRunning = tasks.Length != numberDone;

                writer.WriteLine($" -> Tasks Done: {numberDone} of {tasks.Length})");
                writer.WriteLine($" -> Continue Running: {continueRunning}");

                if (continueRunning)
                    System.Threading.Thread.Sleep(random.Next(0, waitBetweenStatusChecksInMs));

            }

            writer.WriteLine("Stop Monitoring");

        }

        private static void InitiateRandomizedErrorLogging(TextFileWriter writer, int iterations, int maxWaitMs)
        {
            var threadId = Guid.NewGuid().ToString();
            var random = new Random();

            for (int i = 0; i < iterations; i++)
            {
                writer.WriteLine($"{threadId}: {i + 1}");
                System.Threading.Thread.Sleep(random.Next(0, maxWaitMs));
            }
        }
    }
}
