using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace STAFTests
{
    class CreateRequests
    {
        private string strClient = "https://reqres.in";

        public ListOfUsersDTO GetUsers()
        {
            RestClient restClient = new RestClient(strClient);
            RestRequest restRequest = new RestRequest("/api/users?page=2",Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            Task<RestResponse> response = restClient.ExecuteAsync(restRequest);
            string content = response.Result.Content;

            ListOfUsersDTO Users = JsonConvert.DeserializeObject<ListOfUsersDTO>(content);

            return Users;
        }


        /*public string getExcelData(string strExpJsonColName)
        {
            Excel.Application xlApp;

            Excel.Workbook xlWorkBook;

            Excel.Worksheet xlWorkSheet;

            Excel.Range range;



            string str = "";

            int rCnt;

            int cCnt;

            int rw = 0;

            int cl = 0;



            xlApp = new Excel.Application();

            xlWorkBook = xlApp.Workbooks.Open(@"C:\Working\RNDProjects\APITestProject\DataLib\TestData\ExpectedJson.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);



            range = xlWorkSheet.UsedRange;

            rw = range.Rows.Count;

            cl = range.Columns.Count;





            for (rCnt = 1; rCnt <= rw; rCnt++)

            {

                //for (cCnt = 1; cCnt <= cl; cCnt++)

                //{

                str = (string)(range.Cells[rCnt, 1] as Excel.Range).Value2;



                if (str.ToLower().Trim() == strExpJsonColName.ToLower().Trim())

                {

                    str = (string)(range.Cells[rCnt, 2] as Excel.Range).Value2;

                    return str;

                }

                //}

            }



            xlWorkBook.Close(true, null, null);

            xlApp.Quit();



            _ = Marshal.ReleaseComObject(xlWorkSheet);

            _ = Marshal.ReleaseComObject(xlWorkBook);

            _ = Marshal.ReleaseComObject(xlApp);

            return str;

        }*/

    }
}
