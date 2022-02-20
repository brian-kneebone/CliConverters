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
        private const int TEST_ITERATIONS = 1;
        private const int _1000_MS = 1000;
        private const int _5000_MS = 5000;
        private const int _6000_MS = 6000;
        private const int _18000_MS = 18000;
        private const int _20000_MS = 20000;

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [Timeout(_20000_MS)]
        public void TestQuicklyOpeningAndClosingWithWrite()
        {
            for (int i = 0; i < TEST_ITERATIONS; i++)
            {
                var filename = "TextFileWriter_QuickOpenCloseWithWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
                var writer = new TextFileWriter(filename);
                writer.Start(_1000_MS);
                writer.WriteLine("Test quickly opening & closing.");
                writer.Stop();

                // Trying to figure out why file isn't writing!

                // Assert.IsTrue(writer.File.Exists(_18000_MS));
                var fileExists = writer.File.Exists(_18000_MS);

                var writerFilename = writer.File.Name;
                var writerQueueCount = writer.GetQueueCount();
                var writerState = writer.State;

                if (!fileExists)
                {
                    Assert.Fail($"File {writerFilename} not found with queue {writerQueueCount} and state {writerState}.");
                }

                Assert.IsTrue(File.ReadAllBytes(writerFilename).Length > 0);
                Assert.AreEqual(writerQueueCount, 0);
                Assert.AreEqual(writerState, TextFileWriterState.Disposed);
            }

        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [Timeout(_6000_MS)]
        public void TestQuicklyOpeningAndClosingNoWrite()
        {
            for (int i = 0; i < TEST_ITERATIONS; i++)
            {
                var filename = "TextFileWriter_QuickOpenCloseNoWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
                var writer = new TextFileWriter(filename);
                writer.Start(_1000_MS);
                // NoWrite
                writer.Stop();

                Assert.IsTrue(!writer.File.Exists(_5000_MS));
                Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
            }
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [Timeout(_6000_MS)]
        public void TestErrorStoppingIdle()
        {
            for (int i = 0; i < TEST_ITERATIONS; i++)
            {
                var filename = "TextFileWriter_ErrorStoppingIdle_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
                var writer = new TextFileWriter(filename);

                Assert.ThrowsException<InvalidOperationException>(() => {
                    writer.Stop();
                });

                Assert.IsTrue(!writer.File.Exists(_5000_MS));
                Assert.IsTrue(writer.State == TextFileWriterState.Idle);
            }
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [Timeout(_6000_MS)]
        public void TestErrorWritingIdle()
        {
            for (int i = 0; i < TEST_ITERATIONS; i++)
            {
                var filename = "TextFileWriter_ErrorWritingIdle_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
                var writer = new TextFileWriter(filename);

                Assert.ThrowsException<InvalidOperationException>(() => {
                    writer.WriteLine("Test writing while idle.");
                });

                Assert.IsTrue(!writer.File.Exists(_5000_MS));
                Assert.IsTrue(writer.State == TextFileWriterState.Idle);
            }

        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [Timeout(_6000_MS)]
        public void TestErrorStartingRunning()
        {
            for (int i = 0; i < TEST_ITERATIONS; i++)
            {
                var filename = "TextFileWriter_ErrorStartingRunning_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
                var writer = new TextFileWriter(filename);
                writer.Start(_1000_MS);

                Assert.ThrowsException<InvalidOperationException>(() => { writer.Start(_5000_MS); });

                writer.Stop();

                Assert.IsTrue(!writer.File.Exists(_5000_MS));
                Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
            }
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [Timeout(_20000_MS)]
        public void TestMultithreadedWrite()
        {
            for (int i = 0; i < TEST_ITERATIONS; i++)
            {
                var filename = "TextFileWriter_MultithreadedWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
                var writer = new TextFileWriter(filename);
                writer.Start(_1000_MS);
                // writer.StartAsync();
                // writer.WaitUntilRunning();

                var tasks = CreateThreadsThatErrorLog(writer, 200);
                StartThreadsThatErrorLog(tasks);
                WaitUntilThreadsThatErrorLogComplete(tasks, writer, 25);

                writer.Stop();

                Assert.IsTrue(writer.File.Exists(_5000_MS));
                Assert.IsTrue(tasks.Count(t => t.Status == TaskStatus.RanToCompletion) == tasks.Length);
                Assert.IsTrue(writer.State == TextFileWriterState.Disposed);
                Assert.IsTrue(File.ReadAllBytes(filename).Length > 0);
            }
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
