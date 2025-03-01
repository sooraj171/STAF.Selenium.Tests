using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using STAF.CF;
using STAFTests.Tests;
using System;

namespace STAFTests
{
    [TestClass]
    public class ParaTests : TestBaseClass
    {
        // Implementing Page Object Model in Second way

        //protected override ChromeOptions SetChromeOptions()
        //{
        //    ChromeOptions options = new ChromeOptions();
        //    options.AddArguments("start-maximized");
        //    options.AddArguments("--incognito");
        //    return options;
        //}
        //public override IWebDriver GetBrowserDriverObject(string brwType, string driverPath = "", bool isRemote = false)
        //{
        //    return base.GetBrowserDriverObject(brwType, driverPath, isRemote);
            
        //}
        /// <summary>
        /// Test to verify users are logged into account overview screen
        /// </summary>
        [TestMethod]
        public void LoginToApp()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["purl"].ToString());
            Login pgG = new Login(driver, TestContext);
            pgG.LoginToApplication(TestContext.Properties["userName"].ToString(), TestContext.Properties["password"].ToString())
                .VerifyAccountsOverviewPageisLoaded()
                
                ;
        }

        /// <summary>
        /// Test will fail if login is success.
        /// </summary>
        [TestMethod]
        public void LoginToAppWithInvalidId()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["purl"].ToString());
            Login pgG = new Login(driver, TestContext);
            pgG.LoginToApplicationInvalid(TestContext.Properties["userName"].ToString(), "erridval")
                .VerifyInvalidUserMessageIsDisplayed()

                ;
        }

        /// <summary>
        /// Navigating to About us screen
        /// </summary>
        [TestMethod]
        public void NavigateToAboutUs()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["purl"].ToString());
            Login pgG = new Login(driver, TestContext);
            pgG.ClickAboutUs()
                .VerifyAboutUsPageisLoaded();
                
        }

        [TestMethod]
        public void TestAIElementGet()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["purl"].ToString());

            // Get page source
            string pageSource = driver.PageSource;

            // Define the display text to find the element next to
            string searchText = "Username";

            // Extract only relevant part
            string un = AiElementGet.ExtractRelevantHtml(pageSource, "Username");
            string pwd = AiElementGet.ExtractRelevantHtml(pageSource, "Password","input");

            // Send to Ollama for processing
            string locator1 = AiElementGet.FindElementLocatorUsingOllama(un, "Username");
            string locator2 = AiElementGet.FindElementLocatorUsingOllama(un, "Password");


            if (!string.IsNullOrEmpty(locator1))
            {
                IWebElement element = driver.FindElement(By.XPath(locator1));
                element.SendKeys(searchText);

                element = driver.FindElement(By.XPath(locator2));
                element.SendKeys("test");
                Console.WriteLine("Element found: " + element.TagName);
            }

        }



    }
}
