using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    [TestClass]
    public class TestIndividualParameters
    {
        [TestMethod]
        public void TestNameOnlyParameters()
        {
            #pragma warning disable CA1806
            Assert.ThrowsException<ArgumentNullException>(new Action(() => { new EmptyNameOnlyParameter(); }));
            Assert.ThrowsException<ArgumentNullException>(new Action(() => { new ShortNameOnlyParameter(); }));
            #pragma warning restore CA1806

            Assert.IsNotNull(new LongNameOnlyParameter());
            Assert.IsNotNull(new ShortAndLongNameOnlyParameter());
        }
    }
}
