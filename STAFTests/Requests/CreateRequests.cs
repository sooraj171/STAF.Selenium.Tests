using Newtonsoft.Json;
using RestSharp;
using System;

namespace STAFTests
{
    class CreateRequests
    {
        private string strClient = "https://reqres.in";

        public ListOfUsersDTO GetUsers(int page = 2)
        {
            var options = new RestClientOptions(strClient)
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            };
            RestClient restClient = new RestClient(options);
            RestRequest restRequest = new RestRequest("/api/users", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Accept-Language", "en-US,en;q=0.9");
            restRequest.AddParameter("page", page);

            RestResponse response = restClient.ExecuteAsync(restRequest).GetAwaiter().GetResult();

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException(
                    $"API request failed: {(int)response.StatusCode} {response.StatusDescription}. " +
                    $"Content: {(response.Content?.Length > 200 ? response.Content.Substring(0, 200) + "..." : response.Content)}");
            }

            string content = response.Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("API returned empty response.");
            }

            if (content.TrimStart().StartsWith("<"))
            {
                throw new InvalidOperationException(
                    "API returned HTML instead of JSON (possible error page). " +
                    $"Content starts with: {(content.Length > 100 ? content.Substring(0, 100) + "..." : content)}");
            }

            ListOfUsersDTO users = JsonConvert.DeserializeObject<ListOfUsersDTO>(content);
            return users;
        }

        /// <summary>
        /// Gets users from DummyJSON API (https://dummyjson.com) - reliable public API without blocking.
        /// Used when reqres.in returns 403.
        /// </summary>
        public DummyJsonUsersResponse GetUsersFromDummyJson(int limit = 5)
        {
            var options = new RestClientOptions("https://dummyjson.com")
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
            };
            RestClient restClient = new RestClient(options);
            RestRequest restRequest = new RestRequest("/users", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("limit", limit);

            RestResponse response = restClient.ExecuteAsync(restRequest).GetAwaiter().GetResult();

            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException(
                    $"API request failed: {(int)response.StatusCode} {response.StatusDescription}.");
            }

            string content = response.Content;
            if (string.IsNullOrWhiteSpace(content) || content.TrimStart().StartsWith("<"))
            {
                throw new InvalidOperationException("API returned invalid response.");
            }

            return JsonConvert.DeserializeObject<DummyJsonUsersResponse>(content)
                ?? throw new InvalidOperationException("Failed to deserialize API response.");
        }
    }
}
