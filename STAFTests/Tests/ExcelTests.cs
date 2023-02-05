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
            ExcelCompareStatus res = excel.CompareFiles("C:\\Users\\soora\\Downloads\\TestDataRealEstate1.xlsx", "C:\\Users\\soora\\Downloads\\TestDataRealEstate2.xlsx",1,1);
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
