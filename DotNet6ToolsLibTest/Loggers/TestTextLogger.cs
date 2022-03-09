using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Loggers
{
    [TestClass]
    public class TestTextLogger
    {
        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestQuicklyOpeningAndClosing()
        {
            var logger = TextFileLogger.Create("QuickOpenCloseTest", LogLevels.Debug);
            logger.Info("Test quickly opening & closing.");
            logger.Dispose();
            Assert.IsTrue(logger.State == LoggerStates.Closed);
            Assert.IsTrue(File.Exists(logger.Filename));
            Assert.IsTrue(File.ReadAllBytes(logger.Filename ?? throw new NullReferenceException()).Length > 0);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void TestMultithreading()
        {
            var logger = TextFileLogger.Create("MultithreadingTest", LogLevels.Debug);

            var tasks = CreateThreadsThatErrorLog(logger, 10);
            StartThreadsThatErrorLog(tasks);
            WaitUntilThreadsThatErrorLogComplete(tasks, logger, 50);
            
            logger.Dispose();

            Assert.IsTrue(logger.State == LoggerStates.Closed);
            Assert.IsTrue(File.Exists(logger.Filename));
            Assert.IsTrue(File.ReadAllBytes(logger.Filename ?? throw new NullReferenceException()).Length > 0);
        }

        private static Task[] CreateThreadsThatErrorLog(ITextFileLogger logger, int numberOfThreads)
        {
            var tasks = new Task[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks[i] = new Task(() => { InitiateRandomizedErrorLogging(logger, 10, 50); });
            }

            return tasks;
        }

        private static void StartThreadsThatErrorLog(Task[] tasks)
        {
            foreach (var task in tasks)
                task.Start();
        }

        private static void WaitUntilThreadsThatErrorLogComplete(Task[] tasks, ITextFileLogger logger, int waitBetweenStatusChecksInMs)
        {
            logger.Error("Start Monitoring");
            
            var continueRunning = true;
            var random = new Random();

            while (continueRunning)
            {
                logger.Error(" -> Thread Status:");

                var statusResults = tasks.Select(t => t.Status).ToArray();
                var statusTypes = Enum.GetNames<TaskStatus>();

                foreach (var statusType in statusTypes)
                {
                    var count = statusResults.Count(r => r.ToString() == statusType);
                    logger.Error($" ---> {statusType}: {count}");
                }

                var numberDone = statusResults.Count(t => (t == TaskStatus.RanToCompletion) || (t == TaskStatus.Faulted) || (t == TaskStatus.Canceled));
                continueRunning = tasks.Length != numberDone;

                logger.Error($" -> Tasks Done: {numberDone} of {tasks.Length})");
                logger.Error($" -> Continue Running: {continueRunning}");

                if (continueRunning)
                    System.Threading.Thread.Sleep(random.Next(0, waitBetweenStatusChecksInMs));

            }

            logger.Error("Stop Monitoring");

        }

        private static void InitiateRandomizedErrorLogging(ITextFileLogger logger, int iterations, int maxWaitMs)
        {
            var threadId = Guid.NewGuid().ToString();
            var random = new Random();

            for (int i = 0; i < iterations; i++)
            {
                logger.Error($"{threadId}: {i + 1}");
                System.Threading.Thread.Sleep(random.Next(0, maxWaitMs));
            }
        }

    }
}
