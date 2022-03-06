using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;

namespace STAFTests
{
    public class AboutUsPage : PageBaseClass
    {
        private IWebDriver Driver;
        private TestContext context;
        /// <summary>
        /// Section to set the property value for objects
        /// </summary>
        #region ObjectIdentifierValues
        private string _headerTitle = "//h1[text()='ParaSoft Demo Website']";
        #endregion
        public AboutUsPage(IWebDriver _driver, TestContext testContext):base(_driver,testContext)
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
