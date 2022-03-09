using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet6Tools.Parameters
{
    [TestClass]
    public class TestParameterNames
    {
        [TestMethod]
        public void Test()
        {
            Assert.IsNotNull(new ParameterName(null, "test"));
            Assert.IsNotNull(new ParameterName("t", "test"));

            #pragma warning disable CA1806
            #pragma warning disable CS8625
            Assert.ThrowsException<ArgumentNullException>(new Action(() => { new ParameterName(null, null); }));
            Assert.ThrowsException<ArgumentNullException>(new Action(() => { new ParameterName("t", null); }));
            #pragma warning restore CS8625
            #pragma warning restore CA1806

        }
    }
}
