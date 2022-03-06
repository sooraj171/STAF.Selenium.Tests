using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;
using System.Reflection;

namespace STAFTests
{
    public class AccountsOverview : AccountsOverviewPage
    {
        private IWebDriver Driver;
        private TestContext context;
        public AccountsOverview(IWebDriver _driver, TestContext testContext):base(_driver,testContext)
        {
            Driver = _driver;
            context = testContext;
        }

        /// <summary>
        /// Verifying the accouont overview page is loaded
        /// </summary>
        /// <returns></returns>
        public AccountsOverview VerifyAccountsOverviewPageisLoaded()
        {
            
            headerTitle.ReportElementIsDisplayed(Driver, context, MethodBase.GetCurrentMethod().Name, "Accounts Overview Page Is Displayed.", false);
           
            return this;
        }


    }
}
