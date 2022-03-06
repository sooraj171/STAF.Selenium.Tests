using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;


namespace STAFTests
{
    public class LoginPage: PageBaseClass
    {
        private IWebDriver Driver;
        private TestContext context;
        /// <summary>
        /// Section to set the property value for objects
        /// </summary>
        #region ObjectIdentifierValues
        private string _tbUserName = "username";
        private string _tbPassword = "password";
        private string _btnLogin = "//input[@type='submit']";
        private string _lblError = "#rightPanel p.error";
        private string _divHeadPanel = "headerPanel";
        #endregion
        public LoginPage(IWebDriver _driver, TestContext testContext):base(_driver,testContext)
        {
            Driver = _driver;
            context = testContext;
        }
        /// <summary>
        /// UserName
        /// </summary>
        public IWebElement tbUserName
        {
            get
            {
                return FindAppElement(By.Name(_tbUserName));
            }
            
        }
        /// <summary>
        /// password
        /// </summary>
        public IWebElement tbPassword
        {
            get
            {
                return FindAppElement(By.Name(_tbPassword));
            }
        }

        /// <summary>
        /// password
        /// </summary>
        public IWebElement btnLogin
        {
            get
            {
                return FindAppElement(By.XPath(_btnLogin));
            }
        }

        /// <summary>
        /// Error label
        /// </summary>
        public IWebElement lblError
        {
            get
            {
                return FindAppElement(By.CssSelector(_lblError));
            }
        }

        /// <summary>
        /// Error label
        /// </summary>
        public IWebElement divHeadPanel
        {
            get
            {
                return FindAppElement(By.Id(_divHeadPanel));
            }
        }

        /// <summary>
        /// Error label
        /// </summary>
        public IWebElement lnkAboutUs
        {
            get
            {
                return FindAppElement(divHeadPanel,By.LinkText("about"),"Link About US");
            }
        }
    }
}
