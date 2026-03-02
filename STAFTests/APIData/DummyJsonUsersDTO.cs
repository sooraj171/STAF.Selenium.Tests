using System;
using Newtonsoft.Json;

namespace STAFTests
{
    public class DummyJsonUsersResponse
    {
        [JsonProperty("users")]
        public DummyJsonUser[] Users { get; set; } = Array.Empty<DummyJsonUser>();

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class DummyJsonUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
    }
}
