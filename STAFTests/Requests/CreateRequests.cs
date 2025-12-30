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
            RestRequest restRequest = new RestRequest("/api/users",Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            Task<RestResponse> response = restClient.ExecuteAsync(restRequest);
            string content = response.Result.Content;

            ListOfUsersDTO Users = JsonConvert.DeserializeObject<ListOfUsersDTO>(content);

            return Users;
        }
    }
}
