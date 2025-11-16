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
            axeAccessibility.AnalyzePageAndSaveHtml("C:\\repo\\sooraj171\\STAF\\STAFS\\test2.html");
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

    }
}
