using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using STAF.CF;

namespace STAFTests
{
    [TestClass]
    public class GlobalAssemblyInitialize: AssemblyInit
    {
        private static string resTestDir = "";

        [AssemblyInitialize]
        public static void Setup(TestContext tc)
        {
            try
            {
                AssemblyInitialize(tc);
            }
            catch { }
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            try
            {
                AssemblyCleanUp();
            }
            catch { }
        }

    }
}
