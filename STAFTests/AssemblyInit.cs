using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF;
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
                WarnIfRunSettingsNotSet(tc);
            }
            catch { }
        }

        /// <summary>
        /// If run settings file was not applied, TestRunParameters (e.g. url, browser) will be missing.
        /// Write a one-time message so the user knows to set the runsettings file.
        /// </summary>
        private static void WarnIfRunSettingsNotSet(TestContext tc)
        {
            bool hasBrowser = tc.Properties.ContainsKey("browser");
            bool hasUrl = tc.Properties.ContainsKey("url");
            if (!hasBrowser || !hasUrl)
            {
                tc.WriteLine("");
                tc.WriteLine("*** Run settings file may not be set. ***");
                tc.WriteLine("Tests expect TestRunParameters (browser, url, purl, etc.) from the runsettings file.");
                tc.WriteLine("  - In VS Code: ensure .vscode/settings.json has \"dotnet.unitTests.runSettingsPath\" pointing to STAFTests/testrunsetting.runsettings");
                tc.WriteLine("  - From CLI: dotnet test --settings STAFTests\\testrunsetting.runsettings");
                tc.WriteLine("  - In Visual Studio: Test → Configure Run Settings → Select Solution Wide runsettings File → STAFTests\\testrunsetting.runsettings");
                tc.WriteLine("");
            }
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
