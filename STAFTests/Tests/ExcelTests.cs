using Microsoft.VisualStudio.TestTools.UnitTesting;
using STAF.CF.Excel;
using STAF.CF;
using System.Text;
using System.Xml;
using STAF.Utilities;
using System;
using System.Collections.Generic;

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

        [TestMethod]
        public void test111()
        {
            

            string xmlString = TRXParser.ReadTRXFile("C:\\Users\\soora\\OneDrive\\Desktop\\Result.trx");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);


            List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

            
            xmlDoc.LoadXml(xmlString);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");


            XmlNodeList unitTestResultNodes = xmlDoc.SelectNodes("/ns:TestRun/ns:Results/ns:UnitTestResult", namespaceManager);

            foreach (XmlNode unitTestResultNode in unitTestResultNodes)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                result["testName"] = unitTestResultNode.Attributes["testName"]?.Value;
                result["duration"] = unitTestResultNode.Attributes["duration"]?.Value;
                result["startTime"] = unitTestResultNode.Attributes["startTime"]?.Value;
                result["endTime"] = unitTestResultNode.Attributes["endTime"]?.Value;
                result["outcome"] = unitTestResultNode.Attributes["outcome"]?.Value;

                results.Add(result);
            }
        }

    }
}
