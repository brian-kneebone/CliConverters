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

            Assert.IsFalse(nullString?.MightBeAnArgumentName());
            Assert.IsFalse("sdfsdf".MightBeAnArgumentName());
            Assert.IsFalse("".MightBeAnArgumentName());

            Assert.IsFalse("-".MightBeAnArgumentName());
            Assert.IsFalse("--".MightBeAnArgumentName());

            Assert.IsTrue("-arg1".MightBeAnArgumentName());
            Assert.IsTrue("--arg1".MightBeAnArgumentName());
            
            Assert.IsFalse("---arg1".MightBeAnArgumentName());
            Assert.IsFalse("-- -arg1".MightBeAnArgumentName());

            Assert.IsTrue("-arg1".MightBeAnArgumentName());
            Assert.IsTrue("--arg1".MightBeAnArgumentName());

            Assert.IsFalse("- arg1".MightBeAnArgumentName());
            Assert.IsFalse("-- arg1".MightBeAnArgumentName());

            Assert.IsFalse("-arg1-sdfsdf".MightBeAnArgumentName());
            Assert.IsTrue("--arg1-sdfsdf".MightBeAnArgumentName());
            Assert.IsFalse("--arg1 sdfsdf".MightBeAnArgumentName());
        }
    }
}
