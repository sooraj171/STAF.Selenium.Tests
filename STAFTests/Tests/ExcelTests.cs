using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF.CF.Excel;
using STAF.CF;
using System;
using System.IO;
using System.Text;

namespace STAFTests
{
    /// <summary>
    /// Samples for STAF ExcelDriver: CompareFiles, GetExcelWorkbook, GetExcelCellData,
    /// SetExcelCellData, GetExcelRowCount, GetExcelColumnCount.
    /// Each test uses a temp copy of the Excel file to avoid file locks when tests run in parallel.
    /// </summary>
    [TestClass]
    public class ExcelTests : TestBaseAPI
    {
        [TestMethod]
        public void CompareExcel()
        {
            var excel = new ExcelDriver();
            string basePath = DirectoryUtils.BaseDirectory + "\\TestData\\TestDataExcel1.xlsx";
            string tempPath = Path.Combine(Path.GetTempPath(), "STAF_Compare_" + Guid.NewGuid().ToString("N")[..8] + ".xlsx");
            File.Copy(basePath, tempPath);
            try
            {
                ExcelCompareStatus res = excel.CompareFiles(tempPath, tempPath, 1, 1);
            var sb = new StringBuilder();
            res.Messages.ForEach(p => sb.AppendLine(p.ToString()));
            if (res.IsMatching)
                ReportResultAPI.ReportResultPass(TestContext, "CompareExcel", sb.ToString());
            else
                ReportResultAPI.ReportResultFail(TestContext, "CompareExcel", sb.ToString());
            }
            finally
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
            }
        }

        [TestMethod]
        public void Sample_Excel_GetWorkbook_CellData_RowColumnCount()
        {
            var excel = new ExcelDriver();
            string sourcePath = DirectoryUtils.BaseDirectory + "\\TestData\\TestDataExcel1.xlsx";
            string filePath = Path.Combine(Path.GetTempPath(), "STAF_GetWorkbook_" + Guid.NewGuid().ToString("N")[..8] + ".xlsx");
            File.Copy(sourcePath, filePath);
            try
            {
            var workbook = excel.GetExcelWorkbook(filePath);
            Assert.IsNotNull(workbook, "Workbook should be loaded.");

            int rowCount = excel.GetExcelRowCount(workbook, 1);
            int colCount = excel.GetExcelColumnCount(workbook.Worksheet(1));
            ReportResultAPI.ReportResultInfo(TestContext, "Excel", $"Sheet 1: rows={rowCount}, columns={colCount}.");

            string cellValue = excel.GetExcelCellData(workbook, 1, 1, 1);
            ReportResultAPI.ReportResultPass(TestContext, "Excel", $"GetExcelCellData(1,1,1) = '{cellValue}'.");
            }
            finally
            {
                try { if (File.Exists(filePath)) File.Delete(filePath); } catch { }
            }
        }

        [TestMethod]
        public void Sample_Excel_SetCellData_And_ReadBack()
        {
            var excel = new ExcelDriver();
            string sourcePath = DirectoryUtils.BaseDirectory + "\\TestData\\TestDataExcel1.xlsx";
            string tempPath = Path.Combine(Path.GetTempPath(), "STAF_SetCell_" + Guid.NewGuid().ToString("N")[..8] + ".xlsx");
            File.Copy(sourcePath, tempPath);
            try
            {
                var workbook = excel.GetExcelWorkbook(tempPath);

                string testValue = "STAF_Sample_" + DateTime.Now.ToString("HHmmss");
                excel.SetExcelCellData(workbook, 1, 1, 1, testValue);
                string readBack = excel.GetExcelCellData(workbook, 1, 1, 1);
                Assert.AreEqual(testValue, readBack, "SetExcelCellData / GetExcelCellData should match.");
                ReportResultAPI.ReportResultPass(TestContext, "Excel", $"SetExcelCellData and read-back verified: '{readBack}'.");
            }
            finally
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
            }
        }
    }
}
