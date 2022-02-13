using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Loggers
{
    [TestClass]
    public class TestTextLogger
    {
        [TestMethod]
        public void TestQuicklyOpeningAndClosing()
        {
            var logger = TextFileLogger.Create("QuickOpenCloseTest", LogLevels.Debug);
            logger.Info("Test quickly opening & closing.");
            logger.Dispose();
            Assert.IsTrue(logger.State == LoggerStates.Closed);
        }
    }
}
