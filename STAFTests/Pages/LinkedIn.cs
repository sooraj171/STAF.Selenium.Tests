using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using STAF.CF;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace STAFTests
{
    public class LinkedInHome
    {
        private IWebDriver Driver;
        private TestContext context;
        public LinkedInHome(IWebDriver _driver, TestContext testContext)
        {
            Driver = _driver;
            context = testContext;
        }

        private IWebElement _divSearch;
        public IWebElement divSearch
        {
            get
            {
                try
                {
                    _divSearch = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists(By.CssSelector("h1[class*='top-card-layout']")));
                }
                catch
                {
                    _divSearch = null;
                }
                return _divSearch; 
            }
        }

        public LinkedInHome verifyLinkedInHomePageIsDispalyed()
        {
            string testName = "verifyLinkedInHomePageIsDispalyed";
            try
            {
                IWebElement element = divSearch;
                string actualTitle = Driver.Title;
                if (actualTitle.ToLower().Contains("linkedin"))
                {
                    ReportResult.ReportResultPass(Driver,context, testName, "LinkedIn Page is dispalyed.");
                }
                else
                {
                    ReportResult.ReportResultFail(Driver, context, testName, "LinkedIn Page is NOT dispalyed.");

                }
            }
            catch
            {
                ReportResult.ReportResultFail(Driver, context, testName, "LinkedIn Page is NOT dispalyed.");
                Assert.Fail("LinkedIn Page not displayed.");
            }
            return this;
        }

    }
}
