using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;

namespace STAFTests
{
    /// <summary>
    /// Samples for STAF reporting: ReportResult (Pass/Fail/Warn/Info) and ReportElement extensions.
    /// </summary>
    [TestClass]
    public class ReportingSamplesTests : TestBaseClass
    {
        [TestMethod]
        public void Sample_ReportResult_Pass_Fail_Warn_Info()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["url"]?.ToString() ?? "https://google.com/");

            ReportResult.ReportResultPass(driver, TestContext, "Reporting", "Step 1: Page loaded.");
            ReportResult.ReportResultInfo(driver, TestContext, "Reporting", "Step 2: Info message for logging.");
            ReportResult.ReportResultWarn(driver, TestContext, "Reporting", "Step 3: Optional warning (e.g. non-critical).");
        }

        [TestMethod]
        public void Sample_ReportElement_Exists_IsDisplayed_IsEnabled()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["purl"]?.ToString() ?? "https://parabank.parasoft.com/");

            var loginPage = new LoginPage(driver, TestContext);
            var userNameField = loginPage.tbUserName;

            userNameField.ReportElementExists(driver, TestContext, "ReportElement", "Username field exists.", ProdceedFlag: true);
            userNameField.ReportElementIsDisplayed(driver, TestContext, "ReportElement", "Username field is displayed.", ProdceedFlag: true);
            userNameField.ReportElementIsEnabled(driver, TestContext, "ReportElement", "Username field is enabled.", ProdceedFlag: true);
        }
    }
}
