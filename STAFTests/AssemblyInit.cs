using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using STAF.CF;

namespace STAFTests
{
    [TestClass]
    class AssemblyInit
    {
        private static string resTestDir = "";

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext tc)
        {
            try
            {
                Console.WriteLine("Before all tests");
                var driverProcess = Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver");

                foreach (var process in driverProcess)
                {
                    process.Kill();
                }
                resTestDir = tc.TestDir;
                string locPath = tc.DeploymentDirectory;

                Environment.SetEnvironmentVariable("OverallFailFlag", "No");
                Environment.SetEnvironmentVariable("resultbodyfinal", "");
            }
            catch { }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            try
            {
                closeAllBrowser();

                StreamWriter writer;
                string overallResult = DirectoryUtils.BaseDirectory + "\\ResultTemplate.html";
                writer = new StreamWriter(File.Open(overallResult, FileMode.Append, FileAccess.Write, FileShare.Write));
                writer.WriteLine(Environment.GetEnvironmentVariable("resultbodyfinal"));
                writer.Flush();
                writer.Close();

                File.Copy(DirectoryUtils.BaseDirectory + "\\ResultTemplate.html", resTestDir + @"\ResultTemplateFinal.html");
                //CommonAction.SendEmail("sooraj171@hotmail.com", "sooraj171@gmail.com", "Auto Test Result - Google", Environment.GetEnvironmentVariable("resultbody"), resTestDir + @"\ResultTemplateFinal.html");
                if (Environment.GetEnvironmentVariable("OverallFailFlag").ToLower() == "yes")
                {
                    Assert.Fail("Some Test Cases failed in execution");
                }
            }
            catch { }
        }

        public static void closeAllBrowser()
        {
            var driverP = Process.GetProcesses().Where(pr => pr.ProcessName == "chromedriver");
            foreach (var process in driverP)
            {
                process.Kill();
            }
        }
    }
}
