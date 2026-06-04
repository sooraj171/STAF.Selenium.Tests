using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using STAF.CF;

namespace STAFTests
{
    public class GoogleHome
    {
        private IWebDriver Driver;
        private TestContext context;
        public GoogleHome(IWebDriver _driver, TestContext testContext)
        {
            Driver = _driver;
            context = testContext;
        }

        private IWebElement _tbSearch;
        public IWebElement tbSearch
        {
            get
            {
                try
                {
                    _tbSearch = Driver.FindElement(By.Name("q"));
                    //_tbSearch = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists(By.Name("q")));
                }
                catch
                {
                    _tbSearch = null;
                }
                return _tbSearch; 
            }
        }

        public GoogleHome verifyGoogleHomePageIsDispalyed()
        {
            string testName = "verifyGoogleHomePageIsDispalyed";
            try
            {
                IWebElement temp = tbSearch;
                if (temp != null && temp.Displayed)
                {
                    ReportResult.ReportResultPass(Driver,context, testName, "Google Home Page is dispalyed.");
                }
                else
                {
                    ReportResult.ReportResultFail(Driver, context, testName, "Google Home Page is NOT dispalyed.");
                }
            }
            catch
            {
                ReportResult.ReportResultFail(Driver, context, testName, "Google Home Page is NOT dispalyed.");
                Assert.Fail("Google Home Page not displayed.");
            }
            return this;
        }

        public GoogleHome enterSearchTerm(string strSearchTerm)
        {
            string testName = "enterSearchTerm";
            try
            {
                IWebElement temp = tbSearch;
                if (temp != null && temp.Displayed)
                {
                    tbSearch.SendKeys(strSearchTerm+ Keys.Enter);
                    
                    ReportResult.ReportResultPass(Driver, context, testName, "Enter Search Term as: "+ strSearchTerm);
                }
                else
                {
                    ReportResult.ReportResultFail(Driver, context, testName, "Not able to enter the search term.");
                }
            }
            catch
            {
                ReportResult.ReportResultFail(Driver, context, testName, "Not able to enter the search term.");
                Assert.Fail("Not able to enter the search term.");
            }
            return this;
        }

        public LinkedInHome selectFirstItemFromResult()
        {
            string testName = "selectFirstItemFromResult";
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
                IWebElement firstLink = wait.Until(driver =>
                {
                    var resultLinks = driver.FindElements(By.CssSelector("div#search a h3, div#rso a h3"));
                    foreach (var heading in resultLinks)
                    {
                        try
                        {
                            var link = heading.FindElement(By.XPath("./ancestor::a[1]"));
                            if (link.Displayed)
                            {
                                return link;
                            }
                        }
                        catch (NoSuchElementException)
                        {
                            // try next heading
                        }
                    }

                    return null;
                });

                firstLink.Click();
                ReportResult.ReportResultPass(Driver, context, testName, "Clicked on the first item from search result.");
            }
            catch
            {
                ReportResult.ReportResultFail(Driver,context,testName, "Not able to enter the search term.");
                Assert.Fail("Not able to enter the search term.");
            }
            return new LinkedInHome(Driver, context);
        }
    }
}
