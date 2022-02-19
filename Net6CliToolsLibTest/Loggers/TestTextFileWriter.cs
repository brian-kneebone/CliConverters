using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
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
            writer.StartAsync();
            writer.WriteLineAsync("Test quickly opening & closing.");
            writer.Stop();
            
            Assert.IsTrue(File.Exists(filename));
            Assert.IsTrue(File.ReadAllBytes(filename).Length > 0);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestQuicklyOpeningAndClosingNoWrite()
        {
            var filename = "TextFileWriter_QuickOpenCloseNoWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.StartAsync();
            // NoWrite
            writer.Stop();
            Assert.IsTrue(!File.Exists(filename));
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestMultithreadedWrite()
        {
            var filename = "TextFileWriter_MultithreadedWrite_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.StartAsync();

            var tasks = CreateThreadsThatErrorLog(writer, 10);
            StartThreadsThatErrorLog(tasks);
            WaitUntilThreadsThatErrorLogComplete(tasks, writer, 50);

            writer.Stop();

            Assert.IsTrue(tasks.Count(t => t.Status == TaskStatus.RanToCompletion) == tasks.Length);
            // Assert.IsTrue(writer.State == LoggerStates.Closed);
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

        private static void WaitUntilThreadsThatErrorLogComplete(Task[] tasks, TextFileWriter writer, int waitBetweenStatusChecksInMs)
        {
            writer.WriteLineAsync("Start Monitoring");

            var continueRunning = true;
            var random = new Random();

            while (continueRunning)
            {
                writer.WriteLineAsync(" -> Thread Status:");

                var statusResults = tasks.Select(t => t.Status).ToArray();
                var statusTypes = Enum.GetNames<TaskStatus>();

                foreach (var statusType in statusTypes)
                {
                    var count = statusResults.Count(r => r.ToString() == statusType);
                    writer.WriteLineAsync($" ---> {statusType}: {count}");
                }

                var numberDone = statusResults.Count(t => (t == TaskStatus.RanToCompletion) || (t == TaskStatus.Faulted) || (t == TaskStatus.Canceled));
                continueRunning = tasks.Length != numberDone;

                writer.WriteLineAsync($" -> Tasks Done: {numberDone} of {tasks.Length})");
                writer.WriteLineAsync($" -> Continue Running: {continueRunning}");

                if (continueRunning)
                    System.Threading.Thread.Sleep(random.Next(0, waitBetweenStatusChecksInMs));

            }

            writer.WriteLineAsync("Stop Monitoring");

        }

        private static void InitiateRandomizedErrorLogging(TextFileWriter writer, int iterations, int maxWaitMs)
        {
            var threadId = Guid.NewGuid().ToString();
            var random = new Random();

            for (int i = 0; i < iterations; i++)
            {
                writer.WriteLineAsync($"{threadId}: {i + 1}");
                System.Threading.Thread.Sleep(random.Next(0, maxWaitMs));
            }
        }
    }
}
