using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;
using System.Reflection;

namespace STAFTests
{
    public class AboutUs : AboutUsPage
    {
        private IWebDriver Driver;
        private TestContext context;
        public AboutUs(IWebDriver _driver, TestContext testContext):base(_driver,testContext)
        {
            Driver = _driver;
            context = testContext;
        }

        /// <summary>
        /// Verify About us page is displayed
        /// </summary>
        /// <returns></returns>
        public AboutUs VerifyAboutUsPageisLoaded()
        {
         
            headerTitle.ReportElementIsDisplayed(Driver, context, nameof(VerifyAboutUsPageisLoaded), "About Us Page Is Displayed: ", false);
            
            return this;
        }


    }
}
