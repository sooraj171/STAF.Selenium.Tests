using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF.CF.Excel;
using STAF.CF;
using System.Text;


namespace STAFTests
{
    [TestClass]
    public class ExcelTests : TestBaseAPI
    {
        
        /// <summary>
        /// Navigating to About us screen
        /// </summary>
        [TestMethod]
        public void CompareExcel()
        {
            ExcelDriver excel= new ExcelDriver();
            string file1 = DirectoryUtils.BaseDirectory + "\\TestData\\TestDataExcel1.xlsx";
            string file2 = DirectoryUtils.BaseDirectory + "\\TestData\\TestDataExcel1.xlsx"; // Assuming a second file for comparison
            ExcelCompareStatus res = excel.CompareFiles(file1, file2, 1,1);
            StringBuilder stringBuilder= new StringBuilder();
            res.Messages.ForEach(p => stringBuilder.AppendLine(p.ToString()));
            if (res.IsMatching)
            {
                ReportResultAPI.ReportResultPass(TestContext, "CompareExcel", stringBuilder.ToString());
            }
            else
            {
                ReportResultAPI.ReportResultFail(TestContext, "CompareExcel", stringBuilder.ToString());
            }

        }

    }
}
