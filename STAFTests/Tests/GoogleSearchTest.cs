using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF.CF;

namespace STAFTests
{
    [TestClass]
    public class GoogleSearchTest: TestBaseClass
    {
        //Implementing Pqge Object in Simple way

        [TestMethod]
        public void VerifyGoogleSearch()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["url"].ToString());
            GoogleHome pgG = new GoogleHome(driver, TestContext);
            pgG.verifyGoogleHomePageIsDispalyed()
                .enterSearchTerm(TestContext.Properties["searchText"].ToString())
                .selectFirstItemFromResult()
                .verifyLinkedInHomePageIsDispalyed()
                ;
        }

        [TestMethod]
        public void VerifyAmelyaSearch()
        {
            driver.Navigate().GoToUrl(TestContext.Properties["url"].ToString());
            GoogleHome pgG = new GoogleHome(driver, TestContext);
            pgG.verifyGoogleHomePageIsDispalyed()
                .enterSearchTerm("Amelya")
                .selectFirstItemFromResult()
                //.verifyLinkedInHomePageIsDispalyed() //uncomment tihs line to see screen shot on failure
                ;
        }


    }
}
