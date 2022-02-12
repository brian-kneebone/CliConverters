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
//        [TestMethod]
//        public void MayBeArgumentNameTest()
//        {

//            // True if single hyphen and single char for short argument name

//            Assert.IsFalse("a".MightBeArgumentName());
            
            
            
//            Assert.IsFalse("arg1".MightBeArgumentName());


//#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
//#pragma warning disable CS8604 // Possible null reference argument.

//            string nullString = null;
//            var nullIsFalse = nullString.MightBeArgumentName();

//#pragma warning restore CS8604 // Possible null reference argument.
//#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

//            // False if arguments are only null or white space

//            Assert.IsFalse(nullIsFalse);
//            Assert.IsFalse("\t".MightBeArgumentName());
//            Assert.IsFalse("\r".MightBeArgumentName());
//            Assert.IsFalse("\n".MightBeArgumentName());
//            Assert.IsFalse("\r\n".MightBeArgumentName());
//            Assert.IsFalse("\n".MightBeArgumentName());
//            Assert.IsFalse("\r\n".MightBeArgumentName());
//            Assert.IsFalse(" ".MightBeArgumentName());
//            Assert.IsFalse("".MightBeArgumentName());

//            // False if no argument prefixes

//            Assert.IsFalse("a".MightBeArgumentName());
//            Assert.IsFalse("arg1".MightBeArgumentName());

//            // Valid argument prefixes but null or white space for names but valid args

//            Assert.IsFalse("-".MightBeArgumentName());
//            Assert.IsFalse("--".MightBeArgumentName());
//            Assert.IsFalse(nullIsFalse);

//            // Invalid arguments entirely as only null or white space

//            Assert.IsFalse(nullIsFalse);
//            Assert.IsFalse("\t".MightBeArgumentName());
//            Assert.IsFalse("\r".MightBeArgumentName());
//            Assert.IsFalse("\n".MightBeArgumentName());
//            Assert.IsFalse("\r\n".MightBeArgumentName());
//            Assert.IsFalse("\n".MightBeArgumentName());
//            Assert.IsFalse("\r\n".MightBeArgumentName());
//            Assert.IsFalse(" ".MightBeArgumentName());
//            Assert.IsFalse("".MightBeArgumentName());




//            Assert.IsFalse("-\t".MightBeArgumentName());
//            Assert.IsFalse("-\r".MightBeArgumentName());
//            Assert.IsFalse("-\n".MightBeArgumentName());
//            Assert.IsFalse("-\r\n".MightBeArgumentName());
//            Assert.IsFalse("-\n".MightBeArgumentName());
//            Assert.IsFalse("-\r\n".MightBeArgumentName());
//            Assert.IsFalse("- ".MightBeArgumentName());

//            Assert.IsFalse("--\t".MightBeArgumentName());
//            Assert.IsFalse("--\r".MightBeArgumentName());
//            Assert.IsFalse("--\n".MightBeArgumentName());
//            Assert.IsFalse("--\r\n".MightBeArgumentName());
//            Assert.IsFalse("--\n".MightBeArgumentName());
//            Assert.IsFalse("--\r\n".MightBeArgumentName());
//            Assert.IsFalse("-- ".MightBeArgumentName());

//            // Valid prefixes with invalid names

//            Assert.IsFalse("-arg1".MightBeArgumentName());     // Must be one char only with short prefix indicator
//            Assert.IsFalse("---arg1".MightBeArgumentName());   // Must never have leading - chars in name with long prefix
//            Assert.IsFalse("--arg1-".MightBeArgumentName());   // Must never have leading - chars in name with long prefix

//            // Valid short names

//            Assert.IsFalse("-".MightBeArgumentName());
//            Assert.IsTrue("-a".MightBeArgumentName());
//            Assert.IsFalse("-ar".MightBeArgumentName());
//            Assert.IsFalse("- a".MightBeArgumentName());





//            Assert.IsTrue("--arg1".MightBeArgumentName());

            
//            Assert.IsFalse("---arg1".MightBeArgumentName());
//            Assert.IsFalse("-- -arg1".MightBeArgumentName());

//            Assert.IsTrue("--arg1".MightBeArgumentName());

//            Assert.IsFalse("- arg1".MightBeArgumentName());
//            Assert.IsFalse("-- arg1".MightBeArgumentName());

//            Assert.IsFalse("-arg1-sdfsdf".MightBeArgumentName());
//            Assert.IsTrue("--arg1-sdfsdf".MightBeArgumentName());
//            Assert.IsFalse("--arg1 sdfsdf".MightBeArgumentName());
//        }
    }
}
