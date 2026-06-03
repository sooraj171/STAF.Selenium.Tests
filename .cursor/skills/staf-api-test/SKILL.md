---
name: staf-api-test
description: >-
  Creates or edits STAF API tests (TestBaseAPI, RestSharp, CreateRequests, DTOs in
  APIData, ReportResultAPI). Use for REST tests, reqres/DummyJSON, or API reporting samples.
---

# STAF API Test

## Quick Workflow

1. **Create Request Method** in `STAFTests/Requests/CreateRequests.cs`
   - Signature: `public async Task<RestResponse<T>> MethodName(params)`
   - Use `RestClient` (RestSharp v114+)
   - Return `await client.ExecuteAsync<T>(request)`

2. **Create DTO** in `STAFTests/APIData/{ResponseShape}DTO.cs`
   - Plain C# class with auto-properties
   - Use `[JsonPropertyName("field_name")]` if JSON keys differ

3. **Create Test Method** in `STAFTests/Tests/APITests.cs` inheriting `TestBaseAPI`
   - Arrange: set up test data
   - Act: call request method
   - Assert: check status code, content
   - Report: `ReportResultAPI.ReportResultPass/Fail(...)`

## Template

### Request Method

```csharp
public async Task<RestResponse<UsersListDTO>> GetUsersList(int page = 1)
{
    var client = new RestClient("https://reqres.in");
    var request = new RestRequest("/api/users", Method.Get);
    request.AddParameter("page", page);
    return await client.ExecuteAsync<UsersListDTO>(request);
}
```

### DTO

```csharp
using System.Text.Json.Serialization;

public class UsersListDTO
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("data")]
    public List<UserDTO> Data { get; set; }
}
```

### Test Method

```csharp
[TestMethod]
public async Task GetUsersList_Page1_ReturnsSuccess()
{
    var testName = nameof(GetUsersList_Page1_ReturnsSuccess);
    var requests = new CreateRequests();

    // Act
    var response = await requests.GetUsersList(page: 1);

    // Assert status code first
    if (response.StatusCode != System.Net.HttpStatusCode.OK)
    {
        ReportResultAPI.ReportResultFail(
            TestContext, testName, 
            $"Expected 200, got {response.StatusCode}");
        Assert.Fail($"Expected 200, got {response.StatusCode}");
    }

    // Assert content
    Assert.IsNotNull(response.Data?.Data, "Data should not be null");
    Assert.IsTrue(response.Data.Data.Count > 0, "Should have users");

    ReportResultAPI.ReportResultPass(TestContext, testName, "Users retrieved successfully");
}
```

## Checklist

- [ ] Test class inherits `TestBaseAPI` (not `TestBaseClass`)
- [ ] **No WebDriver usage** — no `driver` property or element interactions
- [ ] Request method: `async Task<RestResponse<DTO>>`
- [ ] Check **status code first** before parsing response body
- [ ] **Fail path:** `ReportResultAPI.ReportResultFail(...)` **before** `Assert.Fail(...)`
- [ ] **Pass path:** `ReportResultAPI.ReportResultPass(...)`
- [ ] DTO has `[JsonPropertyName]` attributes for JSON key mapping
- [ ] All public methods have XML comments
- [ ] Update `docs/ai-index.json`: `pwsh tools/UpdateAiIndex.ps1`

## Platforms

| Platform | How to Use | Setup |
|----------|-----------|-------|
| **Visual Studio** | GitHub Copilot chat | See `.github/copilot-instructions.md` |
| **VS Code** | Copilot Chat (`Ctrl+Shift+I`) | See `.vscode/README.md` |
| **Cursor** | Composer or Cmd+K | Reference this skill by name |

## Golden Files

- **Request:** `STAFTests/Requests/CreateRequests.cs` — All API methods
- **DTO:** `STAFTests/APIData/DummyJsonUsersDTO.cs`, `ListOfUsersDTO.cs` — Response shapes
- **Test:** `STAFTests/Tests/APITests.cs` — Multiple API test examples

## Full Guide

📖 **Master documentation:** [docs/AI_GUIDE.md](../../docs/AI_GUIDE.md#workflow-3-api-test)

Templates & examples: [reference.md](reference.md)
