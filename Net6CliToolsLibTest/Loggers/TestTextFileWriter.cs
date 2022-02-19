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

    }
}
