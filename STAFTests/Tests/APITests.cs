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
            ListOfUsersDTO listOfUsersActual = getUserReq.GetUsers();
            try
            {
                if (listOfUsersActual != null)
                {
                    Assert.AreEqual(2, listOfUsersActual.page);
                    Assert.AreEqual("Michael", listOfUsersActual.data[0].first_name);
                    Assert.AreEqual(7, listOfUsersActual.data[0].id);
                    ReportResultAPI.ReportResultPass(TestContext, "verifyUserDetails", "User details are as expected in response");
                }
                else
                {
                    ReportResultAPI.ReportResultFail(TestContext, "verifyUserDetails", "User details are NOT as expected in response");
                }
            }
            catch (Exception)
            {
                ReportResultAPI.ReportResultFail(TestContext, "verifyUserDetails", "User details are NOT as expected in response");
                Assert.Fail("User details are NOT as expected in response");
            }
        }
    }
}
