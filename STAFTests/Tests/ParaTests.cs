using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SATF.Accessibility;
using STAF.CF;

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

            AxeAccessibility axeAccessibility = new AxeAccessibility(driver);
            string filePath = DirectoryUtils.BaseDirectory;
            axeAccessibility.AnalyzePageAndSaveHtml(filePath+"\\test2.html");
            Login pgG = new Login(driver, TestContext);
            pgG.LoginToApplication(TestContext.Properties["userName"].ToString(), TestContext.Properties["password"].ToString())
                .VerifyAccountsOverviewPageisLoaded()
                
                ;
        }

        /// <summary>
        /// Expects an error message for invalid credentials. Fails when ParaBank incorrectly accepts
        /// the login (known demo-app issue); that failure is expected and documents the app defect.
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

    }
}
