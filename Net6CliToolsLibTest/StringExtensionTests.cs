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

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

            string nullString = null;
            var nullIsFalse = nullString.MightBeArgumentName();

#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            Assert.IsFalse(nullIsFalse);
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
