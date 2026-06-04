using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using STAF.CF;
using STAF;
using System;

namespace STAFTests
{
    public class Login : LoginPage
    {
        private IWebDriver Driver;
        private TestContext context;
        public Login(IWebDriver _driver, TestContext testContext):base(_driver,testContext)
        {
            Driver = _driver;
            context = testContext;
        }

        /// <summary>
        /// Login to app with Invalid User
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public Login LoginToApplicationInvalid(string strUser,string strPwd)
        {
            string testName =  nameof(LoginToApplicationInvalid);
            try
            {
                IWebElement temp = tbUserName;
                if (temp != null && temp.Displayed)
                {
                    EnterUserName(strUser);
                    EnterPassword(strPwd);
                    ClickLogin();
                }
                else
                {
                    ReportResult.ReportResultFail(Driver, context, testName, "Not able to login to the application.");
                }
            }
            catch
            {
                ReportResult.ReportResultFail(Driver, context, testName, "Not able to login to the application.");
                Assert.Fail("Not able to login to the application.");
            }
            return this;
        }

        /// <summary>
        /// Login to App with Valid Credentials
        /// </summary>
        /// <param name="strUser"></param>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public AccountsOverview LoginToApplication(string strUser, string strPwd)
        {
            string testName = nameof(LoginToApplication);
            try
            {
                IWebElement temp = tbUserName;
                if (temp != null && temp.Displayed)
                {
                    EnterUserName(strUser);
                    EnterPassword(strPwd);
                    ClickLogin();
                    WaitForAccountsOverview();
                    ReportResult.ReportResultPass(Driver, context, testName, "Logged Into the Application.");
                }
                else
                {
                    ReportResult.ReportResultFail(Driver, context, testName, "Login page is NOT dispalyed.");
                }
            }
            catch
            {
                ReportResult.ReportResultFail(Driver, context, testName, "Login page is NOT dispalyed.");
                Assert.Fail("Login page not displayed.");
            }
            return new AccountsOverview(Driver, context);
        }

        /// <summary>
        /// Verifying Invalid user error message is displyed
        /// </summary>
        /// <returns></returns>
        public Login VerifyInvalidUserMessageIsDisplayed()
        {
            lblError.ReportElementIsDisplayed(Driver, context, nameof(VerifyInvalidUserMessageIsDisplayed), "Verify Invalid User Message IsDisplayed");
            return this;
        }

        /// <summary>
        /// Enter Username field value
        /// </summary>
        /// <param name="strUser"></param>
        public void EnterUserName(string strUser)
        {
            tbUserName.SendKeys(strUser);
        }

        /// <summary>
        /// Enter Password field value
        /// </summary>
        /// <param name="strUser"></param>
        public void EnterPassword(string strPwd)
        {
            tbPassword.SendKeys(strPwd);
        }

        /// <summary>
        /// Click Login Button
        /// </summary>
        public void ClickLogin()
        {
            btnLogin.Click();
        }

        private void WaitForAccountsOverview()
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(15)).Until(driver =>
                driver.Url.Contains("overview", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Click Login Button
        /// </summary>
        /// <param name="strUser"></param>
        public bool VerifyError()
        {
            return lblError.Displayed;
        }

        /// <summary>
        /// Click About Us
        /// </summary>
        public AboutUs ClickAboutUs()
        {
            lnkAboutUs.Click();
            return new AboutUs(Driver, context);
        }

        

    }
}
