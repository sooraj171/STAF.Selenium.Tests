# GitHub Copilot — STAF.Selenium.Tests

**Master reference:** [docs/AI_GUIDE.md](../docs/AI_GUIDE.md) — Single source of truth for all platforms (Visual Studio, VS Code, Cursor).

---

## Platform-Specific Entry Points

| Tool | Where Instructions Load | What to Read |
|------|---|---|
| **Visual Studio** | `.github/copilot-instructions.md` (this file) | **Quick Rules** below + [Full Guide](../docs/AI_GUIDE.md) |
| **VS Code** | `.vscode/README.md` + `.github/copilot-instructions.md` | [Quick Rules](#quick-rules) + [Full Guide](../docs/AI_GUIDE.md) |
| **Cursor** | `.cursor/skills/` + `.cursor/cursor.rules` | [MASTER.md](.cursor/skills/MASTER.md) + [Full Guide](../docs/AI_GUIDE.md) |

---

## Quick Rules

### Framework Basics

- **UI Tests:** inherit `TestBaseClass` (has `driver` property, UI helpers)
- **API Tests:** inherit `TestBaseAPI` (no WebDriver, REST/data calls)
- **Pages:** inherit `PageBaseClass`, use `FindAppElement(By.*, selector)` only
- **Actions:** inherit Page, contain flows, return `this` (stay) or `new NextScreen(driver, context)` (navigate)

### Critical Constraints

- **No `new IWebDriver()`** in tests/pages — use inherited `driver` property
- **No `Thread.Sleep`** — use `FindAppElement()` (auto-waits) or `WaitForDocumentReady()`
- **No raw `By.` selectors in tests** — reference page properties only
- **Assertions in actions**, not tests — tests stay thin, call action methods

### Core Methods

| Action | UI Pattern | API Pattern |
|--------|-----------|-------------|
| **Navigate** | `NavigateTo(url)` | N/A |
| **Find Element** | `FindAppElement(By.*, "desc")` | N/A |
| **Click/Enter** | `Click(elem)`, `EnterText(elem, text)` | N/A |
| **Report Step** | `ReportResult.ReportResultPass/Fail(...)` | `ReportResultAPI.ReportResultPass/Fail(...)` |
| **Check Element** | `elem.ReportElementIsDisplayed/Enabled/Has Value(...)` | `Assert.AreEqual/IsNotNull(...)` |

### File Naming = Class Naming

- File: `LoginPage.cs` → Class: `LoginPage` (inherits `PageBaseClass`)
- File: `Login.cs` → Class: `Login` (inherits `LoginPage`)
- File: `ParaTests.cs` → Class: `ParaTests` (inherits `TestBaseClass`)

---

## Three Core Workflows

### 1️⃣ Create UI Test

**Where:** `STAFTests/Tests/{TestClass}.cs`  
**What:** New `[TestMethod]` in class inheriting `TestBaseClass`

```csharp
[TestMethod]
public void LoginToApp_ValidCredentials_Success()
{
    NavigateTo(TestContext.Properties["purl"].ToString());
    new Login(driver, TestContext)
        .LoginToApplication(TestContext.Properties["userName"].ToString(),
                           TestContext.Properties["password"].ToString())
        .VerifyAccountsOverviewPageisLoaded();
}
```

**Golden file:** `STAFTests/Tests/ParaTests.cs`  
**Full details:** [Workflow 1: UI Test](../docs/AI_GUIDE.md#workflow-1-ui-test)

---

### 2️⃣ Create Page + Action

**Where:** `STAFTests/Pages/{Screen}Page.cs` + `STAFTests/Actions/{Screen}.cs`  
**What:** New screen object + action flow (reusable in multiple tests)

**Order:**
1. Create `*Page.cs` in `STAFTests/Pages/`
2. Create `*.cs` (inherits `*Page`) in `STAFTests/Actions/`
3. Update `docs/ai-index.json` (run `pwsh tools/UpdateAiIndex.ps1`)

**Page template:**

```csharp
public class MyScreenPage : PageBaseClass
{
    #region ObjectIdentifierValues
    private string _btnSubmit = "submit";
    #endregion

    public MyScreenPage(IWebDriver driver, TestContext context) 
        : base(driver, context) { }

    public IWebElement btnSubmit => FindAppElement(By.Id(_btnSubmit));
}
```

**Action template (verification):**

```csharp
public class MyScreen : MyScreenPage
{
    public MyScreen(IWebDriver driver, TestContext context) : base(driver, context) { }

    public MyScreen VerifyPageLoaded()
    {
        btnSubmit.ReportElementIsDisplayed(Driver, context, nameof(VerifyPageLoaded), 
                                           "Submit button visible", false);
        return this;
    }
}
```

**Action template (navigation):**

```csharp
public NextScreen ClickSubmit()
{
    Click(btnSubmit);
    ReportResult.ReportResultPass(Driver, context, nameof(ClickSubmit), "Clicked submit");
    return new NextScreen(Driver, context);
}
```

**Golden files:** `STAFTests/Pages/LoginPage.cs` + `STAFTests/Actions/Login.cs`  
**Full details:** [Workflow 2: Page + Action](../docs/AI_GUIDE.md#workflow-2-page--action)

---

### 3️⃣ Create API Test

**Where:** `STAFTests/Requests/CreateRequests.cs` + `STAFTests/APIData/*.cs` + `STAFTests/Tests/APITests.cs`

**Steps:**
1. Add request method to `CreateRequests.cs` (returns `RestResponse<T>`)
2. Create DTO in `APIData/` (response shape)
3. Add `[TestMethod]` in `APITests.cs` (inherits `TestBaseAPI`)

**Request template:**

```csharp
public async Task<RestResponse<UsersDTO>> GetUsers(int page = 1)
{
    var client = new RestClient("https://api.example.com");
    var request = new RestRequest("/users", Method.Get);
    request.AddParameter("page", page);
    return await client.ExecuteAsync<UsersDTO>(request);
}
```

**DTO template:**

```csharp
public class UsersDTO
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("data")]
    public List<UserDTO> Data { get; set; }
}
```

**Test template:**

```csharp
[TestMethod]
public async Task GetUsers_Page1_ReturnsUsersSuccessfully()
{
    var response = await new CreateRequests().GetUsers(page: 1);

    if (response.StatusCode != HttpStatusCode.OK)
    {
        ReportResultAPI.ReportResultFail(TestContext, nameof(GetUsers_Page1_ReturnsUsersSuccessfully),
                                         $"Expected 200, got {response.StatusCode}");
        Assert.Fail();
    }

    Assert.IsNotNull(response.Data?.Data, "Data should not be null");
    ReportResultAPI.ReportResultPass(TestContext, nameof(GetUsers_Page1_ReturnsUsersSuccessfully),
                                     "Users retrieved successfully");
}
```

**Golden files:** `STAFTests/Requests/CreateRequests.cs` + `STAFTests/APIData/*.cs` + `STAFTests/Tests/APITests.cs`  
**Full details:** [Workflow 3: API Test](../docs/AI_GUIDE.md#workflow-3-api-test)

---

## Testing & Running

```powershell
# Run specific test (most reliable)
dotnet test --filter "FullyQualifiedName~STAFTests.ParaTests.LoginToApp_ValidCredentials_Success" `
    --settings STAFTests/testrunsetting.runsettings

# Run test class
dotnet test --filter "ClassName~ParaTests" --settings STAFTests/testrunsetting.runsettings

# Run all tests
dotnet test --settings STAFTests/testrunsetting.runsettings
```

---

## Resource Map

| Need | Open | Notes |
|------|------|-------|
| **Full patterns & workflows** | [docs/AI_GUIDE.md](../docs/AI_GUIDE.md) | Single source of truth; covers all 3 workflows with templates |
| **Quick start (new users)** | [docs/QUICK_START.md](../docs/QUICK_START.md) | Platform-specific navigation |
| **Symbol reference** | `docs/ai-index.json` | Generated; run `pwsh tools/UpdateAiIndex.ps1` to update |
| **Cursor skills** | [.cursor/skills/MASTER.md](.cursor/skills/MASTER.md) | Cursor-specific skill index (VS Code, Cursor) |
| **VS Code setup** | [.vscode/README.md](.vscode/README.md) | VS Code GitHub Copilot configuration |
| **VS custom agents** | [agents/](agents/) | STAF UI & API specialized Copilot agents |
| **Golden examples** | See [File Structure](../docs/AI_GUIDE.md#file-structure) | `LoginPage.cs`, `Login.cs`, `ParaTests.cs`, `CreateRequests.cs`, `APITests.cs` |

---

## IDE-Specific Tips

### Visual Studio (GitHub Copilot)

- **This file** (`.github/copilot-instructions.md`) is auto-loaded by VS GitHub Copilot
- **Custom agents** (VS 2026 18.4+): `.github/agents/staf-ui-automation.agent.md` (UI) and `staf-api-automation.agent.md` (API) — pick from the agent picker or `@staf-ui-automation` / `@staf-api-automation`
- Ask Copilot: *"Create a Page Object for..."* or *"Generate a UI test using..."*
- Reference patterns: *"Use the pattern from LoginPage.cs and Login.cs"*
- Refer to [docs/AI_GUIDE.md](../docs/AI_GUIDE.md) for deep dives

### VS Code (GitHub Copilot)

- Check [.vscode/README.md](.vscode/README.md) for Copilot setup
- Both `.github/copilot-instructions.md` and `.cursor/skills/` are available
- Use Copilot Chat (`Ctrl+Shift+I`) for code generation

### Cursor (AI Editor)

- Cursor reads `.cursor/skills/MASTER.md` and `.cursor/cursor.rules`
- Skills are available via Composer or Cmd+K
- Full guide at [.cursor/skills/MASTER.md](.cursor/skills/MASTER.md)

---

## Checklists

### Before Creating New Code

- [ ] Check `docs/ai-index.json` — reuse existing pages/actions/requests
- [ ] Identify workflow: UI Test, Page+Action, or API Test
- [ ] Reference golden file(s) from [docs/AI_GUIDE.md](../docs/AI_GUIDE.md#workflow-1-ui-test)
- [ ] Follow file naming (name = class name)
- [ ] Add step reporting (`ReportResult` / `ReportElement*` / `ReportResultAPI`)
- [ ] No `Thread.Sleep`, no raw `driver.FindElement`, no hardcoded WebDriver creation

### After Creating New Code

- [ ] Inherits correct base class (`TestBaseClass`, `PageBaseClass`, `TestBaseAPI`)
- [ ] All public methods have XML comments
- [ ] Test passes locally: `dotnet test --filter "FullyQualifiedName~YourTest" --settings STAFTests/testrunsetting.runsettings`
- [ ] Update `docs/ai-index.json` (if new page/action/request)
- [ ] No build errors: `dotnet build STAFTests/STAF.Selenium.Tests.csproj`

---

**Last Updated:** 2026-05-31  
**Framework Version:** STAF.UI.API v4.4.0+  
**Target Framework:** .NET 10  
**Applies To:** Visual Studio, VS Code, Cursor
