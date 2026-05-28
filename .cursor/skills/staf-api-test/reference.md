# API test template

```csharp
[TestMethod]
public void verifySomething()
{
    var client = new CreateRequests();
    try
    {
        var actual = client.GetUsers(page: 1);
        Assert.IsNotNull(actual);
        ReportResultAPI.ReportResultPass(TestContext, nameof(verifySomething), "OK");
    }
    catch (Exception)
    {
        ReportResultAPI.ReportResultFail(TestContext, nameof(verifySomething), "Failed");
        Assert.Fail("...");
    }
}
```

Request method sketch:

```csharp
public MyDto GetResource()
{
    var client = new RestClient(new RestClientOptions(baseUrl) { UserAgent = "..." });
    var request = new RestRequest("/path", Method.Get);
    var response = client.ExecuteAsync(request).GetAwaiter().GetResult();
    if (!response.IsSuccessful) throw new InvalidOperationException(...);
    return JsonConvert.DeserializeObject<MyDto>(response.Content);
}
```
