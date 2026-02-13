using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;

namespace STAFTests
{
    /// <summary>
    /// Samples for STAF WebDriver extensions: WaitForDocumentReady, waitForFindElement,
    /// CloseAllTabsExceptCurrent, getTotalTabsCount (CommonAction namespace).
    /// </summary>
    [TestClass]
    public class WebDriverExtensionsSamplesTests : TestBaseClass
    {
        [TestMethod]
        public void Sample_WaitForDocumentReady_AfterNavigate()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["url"]?.ToString() ?? "https://google.com/");
            driver.WaitForDocumentReady();

            ReportResult.ReportResultPass(driver, TestContext, "WebDriverExtensions", "Page loaded and document ready.");
        }

        /// <summary>
        /// STAF also provides: getTotalTabsCount(), CloseAllTabsExceptCurrent(),
        /// waitForFindElement(By, timeout), waitForElementExist/NotExist, WaitForElementDisapper.
        /// See framework documentation for usage.
        /// </summary>
        [TestMethod]
        public void Sample_SingleTab_Navigate_And_Ready()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["url"]?.ToString() ?? "https://google.com/");
            driver.WaitForDocumentReady();
            ReportResult.ReportResultInfo(driver, TestContext, "WebDriverExtensions", "Single tab; document ready. Use getTotalTabsCount/CloseAllTabsExceptCurrent when working with multiple tabs.");
        }
    }

    /// <summary>
    /// Sample: Override SetChromeOptions and GetBrowserDriverObject for custom browser/driver setup.
    /// Uncomment the overrides in a copy of this class to use custom options (e.g. start-maximized, headless).
    /// </summary>
    [TestClass]
    public class BrowserOverrideSamplesTests : TestBaseClass
    {
        // Example: customize Chrome options (uncomment and add: using OpenQA.Selenium.Chrome;)
        // protected override ChromeOptions SetChromeOptions()
        // {
        //     var options = new ChromeOptions();
        //     options.AddArguments("start-maximized");
        //     options.AddArguments("--incognito");
        //     return options;
        // }

        // Example: override driver creation (uncomment to use)
        // public override IWebDriver GetBrowserDriverObject(string brwType, string driverPath = "", bool isRemote = false)
        // {
        //     return base.GetBrowserDriverObject(brwType, driverPath, isRemote);
        // }

        [TestMethod]
        public void Sample_DefaultBrowser_Opens_Successfully()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["url"]?.ToString() ?? "https://google.com/");
            driver.WaitForDocumentReady();
            ReportResult.ReportResultPass(driver, TestContext, "BrowserOverride", "Default browser opened and page loaded.");
        }
    }
}
