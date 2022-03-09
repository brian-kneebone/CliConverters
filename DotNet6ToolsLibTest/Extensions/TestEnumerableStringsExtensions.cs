using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Extensions
{
    [TestClass]
    public class TestEnumerableStringsExtensions
    {
        public static List<string> TestList = new List<string> { "abc123", "ABC123", "DEF321" };

        [TestMethod]
        public void TestInvariantCultureCaseSensitive()
        {
            Assert.AreEqual(-1, TestList.IndexOf("none", StringComparison.InvariantCulture));
            Assert.AreEqual(-1, TestList.IndexOf("AbC123", StringComparison.InvariantCulture));
            Assert.AreEqual(0, TestList.IndexOf("abc123", StringComparison.InvariantCulture));
            Assert.AreEqual(1, TestList.IndexOf("ABC123", StringComparison.InvariantCulture));
            Assert.AreEqual(-1, TestList.IndexOf("def321", StringComparison.InvariantCulture));
        }

        [TestMethod]
        public void TestInvariantCultureCaseInSensitive()
        {
            Assert.AreEqual(-1, TestList.IndexOf("none", StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(0, TestList.IndexOf("AbC123", StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(0, TestList.IndexOf("abc123", StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(0, TestList.IndexOf("ABC123", StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(2, TestList.IndexOf("def321", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
