using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6CliTools.Test
{
    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void MayBeArgumentNameTest()
        {
            string? nullString = null;

            Assert.IsFalse(nullString?.MightBeArgumentName());
            Assert.IsFalse("sdfsdf".MightBeArgumentName());
            Assert.IsFalse("".MightBeArgumentName());

            Assert.IsFalse("-".MightBeArgumentName());
            Assert.IsFalse("--".MightBeArgumentName());

            Assert.IsTrue("-arg1".MightBeArgumentName());
            Assert.IsTrue("--arg1".MightBeArgumentName());
            
            Assert.IsFalse("---arg1".MightBeArgumentName());
            Assert.IsFalse("-- -arg1".MightBeArgumentName());

            Assert.IsTrue("-arg1".MightBeArgumentName());
            Assert.IsTrue("--arg1".MightBeArgumentName());

            Assert.IsFalse("- arg1".MightBeArgumentName());
            Assert.IsFalse("-- arg1".MightBeArgumentName());

            Assert.IsFalse("-arg1-sdfsdf".MightBeArgumentName());
            Assert.IsTrue("--arg1-sdfsdf".MightBeArgumentName());
            Assert.IsFalse("--arg1 sdfsdf".MightBeArgumentName());
        }
    }
}
