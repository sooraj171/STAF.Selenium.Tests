using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF.CF;

namespace STAFTests
{
    /// <summary>
    /// Sample for STAF programmatic HTML reports: TestReportGenerator and TestResultData.
    /// Use when you need custom report layout or data (test name, result, time, messages, screenshots).
    /// </summary>
    [TestClass]
    public class ProgrammaticReportSamplesTests : TestBaseAPI
    {
        [TestMethod]
        public void Sample_Programmatic_Report_Documentation()
        {
            // STAF provides TestReportGenerator and TestResultData for building custom HTML reports.
            // Example usage (when generating reports outside assembly summary):
            //   var data = new TestResultData { TestName = "...", Result = "Pass", ... };
            //   TestReportGenerator.GenerateReport(data, outputPath);
            // See STAF documentation for TestResultData properties and TestReportGenerator API.
            ReportResultAPI.ReportResultPass(TestContext, "ProgrammaticReport", "Programmatic report sample: use TestReportGenerator and TestResultData for custom HTML reports. See framework docs.");
        }
    }
}
