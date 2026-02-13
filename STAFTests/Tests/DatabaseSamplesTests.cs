using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF;
using STAF.CF;

namespace STAFTests
{
    /// <summary>
    /// Samples for STAF Database (DbHelper). Configure ConnectionStrings:DefaultConnection in appsettings.json.
    /// Framework methods: DbHelper.GetConnectionString(name), OpenConnection(name), VerifyConnection(name),
    /// ExecuteQuery(sql[, connName, params]), ExecuteScalar, ExecuteNonQuery.
    /// </summary>
    [TestClass]
    public class DatabaseSamplesTests : TestBaseAPI
    {
        [TestMethod]
        public void Sample_DbHelper_VerifyConnection_WhenConfigured()
        {
            try
            {
                bool verified = DbHelper.VerifyConnection("DefaultConnection");
                if (verified)
                {
                    ReportResultAPI.ReportResultPass(TestContext, "DbHelper", "Database connection verified successfully.");
                }
                else
                {
                    ReportResultAPI.ReportResultWarn(TestContext, "DbHelper", "Connection not available. Set ConnectionStrings:DefaultConnection in appsettings.json to run DB samples.");
                }
            }
            catch (System.Exception ex)
            {
                ReportResultAPI.ReportResultWarn(TestContext, "DbHelper", $"Database not available: {ex.Message}. Configure appsettings.json for full DB samples.");
            }
        }

        [TestMethod]
        public void Sample_DbHelper_ExecuteScalar_WhenConfigured()
        {
            try
            {
                if (!DbHelper.VerifyConnection("DefaultConnection"))
                {
                    ReportResultAPI.ReportResultInfo(TestContext, "DbHelper", "Skipping ExecuteScalar: no connection. Configure DefaultConnection to run.");
                    return;
                }

                var result = DbHelper.ExecuteScalar<object>("SELECT 1 AS Value", "DefaultConnection");
                Assert.IsNotNull(result);
                ReportResultAPI.ReportResultPass(TestContext, "DbHelper", $"ExecuteScalar returned: {result}.");
            }
            catch (System.Exception ex)
            {
                ReportResultAPI.ReportResultWarn(TestContext, "DbHelper", $"ExecuteScalar sample skipped: {ex.Message}.");
            }
        }
    }
}
