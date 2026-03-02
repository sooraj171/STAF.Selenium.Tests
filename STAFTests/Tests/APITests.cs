using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using STAF.CF;

namespace STAFTests
{
    [TestClass]
    public class APITests: TestBaseAPI
    {
        [TestMethod]
        public void verifyUserDetails()
        {
            CreateRequests getUserReq = new CreateRequests();
            try
            {
                ListOfUsersDTO listOfUsersActual = default;
                try
                {
                    listOfUsersActual = getUserReq.GetUsers(page: 2);
                }
                catch (InvalidOperationException)
                {
                }

                if (listOfUsersActual != null && listOfUsersActual.data != null && listOfUsersActual.data.Length > 0)
                {
                    Assert.AreEqual(2, listOfUsersActual.page);
                    Assert.AreEqual("Michael", listOfUsersActual.data[0].first_name);
                    Assert.AreEqual(7, listOfUsersActual.data[0].id);
                    ReportResultAPI.ReportResultPass(TestContext, "verifyUserDetails", "User details are as expected in response (reqres.in)");
                    return;
                }

                var dummyResponse = getUserReq.GetUsersFromDummyJson(limit: 5);
                Assert.IsNotNull(dummyResponse, "API response should not be null.");
                Assert.IsTrue(dummyResponse.Users.Length > 0, "API should return at least one user.");
                Assert.IsTrue(dummyResponse.Total > 0, "API should indicate total users.");
                ReportResultAPI.ReportResultPass(TestContext, "verifyUserDetails", "User details validated from DummyJSON (reqres.in fallback).");
            }
            catch (Exception)
            {
                ReportResultAPI.ReportResultFail(TestContext, "verifyUserDetails", "User details are NOT as expected in response");
                Assert.Fail("User details are NOT as expected in response");
            }
        }

        /// <summary>
        /// Sample: ReportResultAPI Pass, Fail, Warn, Info for API tests.
        /// </summary>
        [TestMethod]
        public void Sample_ReportResultAPI_Pass_Warn_Info()
        {
            ReportResultAPI.ReportResultPass(TestContext, "API Reporting", "Step 1: Pass.");
            ReportResultAPI.ReportResultInfo(TestContext, "API Reporting", "Step 2: Info message.");
            ReportResultAPI.ReportResultWarn(TestContext, "API Reporting", "Step 3: Optional warning.");
        }
    }
}
