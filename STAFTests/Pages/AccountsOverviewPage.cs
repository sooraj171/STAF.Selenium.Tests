using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using STAF.CF;
using System;
using System.Collections.Generic;

namespace STAFTests
{
    public class AccountsOverviewPage : PageBaseClass
    {
        private IWebDriver Driver;
        private TestContext context;
        /// <summary>
        /// Section to set the property value for objects
        /// </summary>
        #region ObjectIdentifierValues
        private string _headerTitle = "//h1[text()='Accounts Overview']";
        #endregion
        public AccountsOverviewPage(IWebDriver _driver, TestContext testContext):base(_driver,testContext)
        {
            Driver = _driver;
            context = testContext;
        }
        /// <summary>
        /// UserName
        /// </summary>
        public IWebElement headerTitle
        {
            get
            {
                return FindAppElement(By.XPath(_headerTitle));
            }
        }
       
    }
}
