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
        public void TestQuicklyOpeningAndClosing()
        {
            var filename = "TextFileWriter_QuickOpenCloseTest_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log";
            var writer = new TextFileWriter(filename);
            writer.StartAsync();
            writer.WriteAsync("Test quickly opening & closing.");
            writer.StopAsync();
            // writer.Dispose();
            // Assert.IsTrue(logger.State == LoggerStates.Closed);



            Assert.IsTrue(File.Exists(filename));
            Assert.IsTrue(File.ReadAllBytes(filename).Length > 0);
        }
    }
}
