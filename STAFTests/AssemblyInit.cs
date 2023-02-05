using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF.CF;

namespace STAFTests
{
    [TestClass]
    public class GlobalAssemblyInitialize: AssemblyInit
    {

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
