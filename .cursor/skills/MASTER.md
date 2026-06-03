# STAF.Selenium.Tests — Cursor Skills Master Index

**Unified Cursor skills reference for STAF.UI.API automation.** Links to all workflows and examples.

---

## Quick Navigation

| Skill | When to Use | File | Details |
|-------|---|---|---|
| **UI Test** | Adding test methods to existing actions | [staf-ui-test/SKILL.md](./staf-ui-test/SKILL.md) | Quick workflow for `[TestMethod]` in test classes |
| **Page + Action** | Creating new screen objects and flows | [staf-page-action/SKILL.md](./staf-page-action/SKILL.md) | Full POM pattern: Page → Action → Test chain |
| **API Test** | REST API testing with RestSharp | [staf-api-test/SKILL.md](./staf-api-test/SKILL.md) | Requests → DTOs → Test methods |

---

## Golden Files (by Workflow)

### UI Test
- **Test:** `STAFTests/Tests/ParaTests.cs` — Login flow with multiple scenarios
- **Action:** `STAFTests/Actions/Login.cs` — Valid + invalid login, fluent returns
- **Pattern:** Thin test calling action methods only

### Page + Action
- **Page:** `STAFTests/Pages/LoginPage.cs` — Locator definitions, XML docs
- **Action:** `STAFTests/Actions/Login.cs` + `STAFTests/Actions/AboutUs.cs` — Verification + navigation
- **Pattern:** PageBaseClass → Action methods → Fluent chain returns

### API Test
- **Request:** `STAFTests/Requests/CreateRequests.cs` — Async RestSharp methods
- **DTO:** `STAFTests/APIData/DummyJsonUsersDTO.cs` — Response shapes
- **Test:** `STAFTests/Tests/APITests.cs` — MSTest + ReportResultAPI
- **Pattern:** Request → Assert → Report

---

## Master Documentation

**Single source of truth:** [docs/ai/AI_GUIDE.md](../../docs/ai/AI_GUIDE.md)

Contains:
- Full workflow descriptions (UI Test, Page+Action, API Test)
- Code patterns (navigation, finders, reporting, fluent chains)
- File structure reference
- Testing commands
- Framework class reference
- AI prompt examples

**Quick start:** [docs/ai/QUICK_START.md](../../docs/ai/QUICK_START.md)

---

## Skill Details

### 1. UI Test (`staf-ui-test`)

**When:** Adding a new test method.  
**Base Class:** `TestBaseClass`  
**Files Created:** `STAFTests/Tests/{TestName}Tests.cs`

```csharp
[TestMethod]
public void LoginToApp_ValidCredentials_Success()
{
	NavigateTo(TestContext.Properties["purl"].ToString());
	new Login(driver, TestContext)
		.LoginToApplication(user, pwd)
		.VerifyAccountsOverviewPageisLoaded();
}
```

**See:** [staf-ui-test/SKILL.md](./staf-ui-test/SKILL.md) | [docs/ai/AI_GUIDE.md#workflow-1-ui-test](../../docs/ai/AI_GUIDE.md#workflow-1-ui-test)

---

### 2. Page + Action (`staf-page-action`)

**When:** Creating a new screen / POM.  
**Base Classes:** `PageBaseClass` (page), inherits page (action)  
**Files Created:** 
- `STAFTests/Pages/{Screen}Page.cs`
- `STAFTests/Actions/{Screen}.cs`

**Order:**
1. Page first (locators)
2. Action second (flows)
3. Update `docs/ai/ai-index.json`

```csharp
// Page
public class LoginPage : PageBaseClass
{
	#region ObjectIdentifierValues
	private string _tbUserName = "username";
	#endregion

	public IWebElement tbUserName => FindAppElement(By.Name(_tbUserName));
}

// Action
public class Login : LoginPage
{
	public Login VerifyPageLoaded()
	{
		tbUserName.ReportElementIsDisplayed(Driver, context, nameof(VerifyPageLoaded), "Username field", false);
		return this;
	}
}
```

**See:** [staf-page-action/SKILL.md](./staf-page-action/SKILL.md) | [docs/ai/AI_GUIDE.md#workflow-2-page--action](../../docs/ai/AI_GUIDE.md#workflow-2-page--action)

---

### 3. API Test (`staf-api-test`)

**When:** Testing REST APIs.  
**Base Class:** `TestBaseAPI`  
**Files Created:**
- `STAFTests/Requests/CreateRequests.cs` (or extend)
- `STAFTests/APIData/{ResponseShape}DTO.cs`
- `STAFTests/Tests/{Name}APITests.cs` (or extend `APITests.cs`)

**Order:**
1. Request method (RestSharp async)
2. DTO (response shape)
3. Test method (arrange, act, assert + report)

```csharp
// Request
public async Task<RestResponse<UsersDTO>> GetUsers(int page = 1)
{
	var client = new RestClient("https://api.example.com");
	var request = new RestRequest("/users", Method.Get);
	request.AddParameter("page", page);
	return await client.ExecuteAsync<UsersDTO>(request);
}

// DTO
public class UsersDTO
{
	[JsonPropertyName("page")]
	public int Page { get; set; }
}

// Test
[TestMethod]
public async Task GetUsers_Page1_ReturnsSuccess()
{
	var response = await new CreateRequests().GetUsers(page: 1);

	if (response.StatusCode != HttpStatusCode.OK)
	{
		ReportResultAPI.ReportResultFail(TestContext, nameof(GetUsers_Page1_ReturnsSuccess), "Expected 200");
		Assert.Fail();
	}

	Assert.IsNotNull(response.Data);
	ReportResultAPI.ReportResultPass(TestContext, nameof(GetUsers_Page1_ReturnsSuccess), "Success");
}
```

**See:** [staf-api-test/SKILL.md](./staf-api-test/SKILL.md) | [docs/ai/AI_GUIDE.md#workflow-3-api-test](../../docs/ai/AI_GUIDE.md#workflow-3-api-test)

---

## Workflow Decision Tree

```
Starting a new task?
│
├─ "Create a test method"
│  └─→ UI Test Skill (staf-ui-test)
│      └─ Check: Page/Action already exists?
│         ├─ Yes: Just add [TestMethod] call existing actions
│         └─ No: First create Page+Action (see below)
│
├─ "Create a new screen / page object"
│  └─→ Page + Action Skill (staf-page-action)
│      └─ Create Page → Create Action → Update ai-index.json
│         └─ Then create tests (staf-ui-test)
│
├─ "Test a REST API"
│  └─→ API Test Skill (staf-api-test)
│      └─ Create Request → Create DTO → Create Test
│         └─ Report with ReportResultAPI
│
└─ "Refactor existing test away from raw WebDriver"
   └─→ Page + Action Skill (staf-page-action)
	   └─ Extract locators → Create Page
		  └─ Create Action with flow methods
			 └─ Replace test with thin action calls
```

---

## Key Constraints (All Skills)

- ❌ **No `new IWebDriver()`** in tests/pages — use inherited `driver`
- ❌ **No `Thread.Sleep(...)`** — use `FindAppElement` (auto-waits 10s)
- ❌ **No raw `By.*` in tests** — reference page properties only
- ✅ **Assertions in actions**, not tests — tests call action methods
- ✅ **File name = Class name** — `LoginPage.cs` → `class LoginPage`
- ✅ **Every step reports** — `ReportResult` (UI) or `ReportResultAPI` (API)
- ✅ **Fluent returns** — return `this` or `new NextScreen(driver, context)`

---

## Testing Commands

```powershell
# Run specific test
dotnet test --filter "FullyQualifiedName~STAFTests.ParaTests.LoginToApp_ValidCredentials_Success" `
	--settings STAFTests/testrunsetting.runsettings

# Run test class
dotnet test --filter "ClassName~MyTests" --settings STAFTests/testrunsetting.runsettings

# Build
dotnet build STAFTests/STAF.Selenium.Tests.csproj
```

---

## Cursor-Specific Tips

### Using Skills in Composer/Cmd+K

1. **Reference a skill explicitly:**
   - *"Using the staf-ui-test skill, create..."*
   - *"Based on staf-page-action, add..."*

2. **Combine with golden files:**
   - *"Use the pattern from `LoginPage.cs` and `Login.cs`"*

3. **Ask for checklist:**
   - *"Create a UI test. Before finishing, verify against the staf-ui-test checklist."*

4. **Cross-reference the guide:**
   - *"Review docs/ai/AI_GUIDE.md#code-patterns for element finders"*

---

## Updating Files After Generation

### After Creating New Code

1. **Update `docs/ai/ai-index.json`**
   - Run: `pwsh tools/UpdateAiIndex.ps1`
   - Regenerates symbol index for agents

2. **Verify XML comments**
   - Public methods should have `/// <summary>` blocks

3. **Test locally**
   - Use commands above; verify pass/fail

4. **Commit & push**
   - Files auto-update when merged to main

---

## Platform-Specific Notes

### Visual Studio

- This repository also has `.github/copilot-instructions.md` for VS GitHub Copilot
- Reference the same `docs/ai/AI_GUIDE.md` when using VS
- All skills work identically across platforms

### VS Code

- Reference `.vscode/README.md` for Copilot setup
- Same skills as Cursor; use `Ctrl+Shift+I` for Copilot Chat

### Cursor

- Cursor reads this folder (`.cursor/skills/`) automatically
- Use Cmd+K or Composer; reference skills by name
- Also reads `.cursor/cursor.rules` for consistency rules

---

## Resource Links

| Resource | Path | Purpose |
|----------|------|---------|
| Master AI Guide | [docs/ai/AI_GUIDE.md](../../docs/ai/AI_GUIDE.md) | Comprehensive patterns, workflows, templates |
| Quick Start | [docs/ai/QUICK_START.md](../../docs/ai/QUICK_START.md) | Platform navigation for new users |
| Symbol Index | `docs/ai/ai-index.json` | Generated class/method reference |
| Cursor Rules | [.cursor/cursor.rules](.././cursor.rules) | Consistency rules for Cursor |
| VS Code Setup | [.vscode/README.md](.././../vscode/README.md) | GitHub Copilot config for VS Code |
| VS Instructions | [.github/copilot-instructions.md](.././../github/copilot-instructions.md) | Visual Studio GitHub Copilot rules |
| NuGet Package | [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) | Framework documentation |

---

## Troubleshooting

### Skill Not Triggering

1. Check skill `.md` files are in `.cursor/skills/{skill-name}/`
2. Ensure SKILL.md has proper frontmatter (name, description)
3. Restart Cursor or reload the workspace
4. Try typing skill name explicitly: *"staf-ui-test: Create..."*

### Generated Code Not Compiling

1. Check namespaces match existing code (`namespace STAFTests { }`)
2. Verify `using` statements are present (OpenQA.Selenium, STAF, STAF.CF)
3. Ensure inheritance is correct (TestBaseClass vs TestBaseAPI)
4. Run `dotnet build` to get exact errors

### Test Fails at Runtime

1. Verify element locators are correct (use browser DevTools)
2. Check `testrunsetting.runsettings` has correct URL properties
3. Ensure `FindAppElement` is used, not `driver.FindElement`
4. Review `ReportResult` calls — they log errors

---

**Last Updated:** 2026-05-31  
**Framework Version:** STAF.UI.API v4.4.0+  
**Target Framework:** .NET 10  
**Applies To:** Cursor, VS Code, Visual Studio
