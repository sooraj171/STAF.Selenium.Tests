# Quick Start — STAF.Selenium.Tests

Get started in 5 minutes. Choose your platform and task.

---

## 🚀 What Do You Want to Do?

| Task | Time | Link |
|------|------|------|
| **Add a new test method** | 2 min | [Create UI Test](#create-ui-test) |
| **Create a new page/screen** | 5 min | [Create Page + Action](#create-page--action) |
| **Create an API test** | 5 min | [Create API Test](#create-api-test) |
| **Explore the framework** | 10 min | [Framework Overview](#framework-overview) |
| **Reference code patterns** | varies | [docs/AI_GUIDE.md](./AI_GUIDE.md) |

---

## Create UI Test

### Scenario
You have an existing page object (`LoginPage`) and action (`Login`). You want to add a new test method.

### Files
- Create in: `STAFTests/Tests/MyTests.cs`
- Reference: `STAFTests/Tests/ParaTests.cs` (golden example)

### Code

```csharp
[TestMethod]
public void LoginToApp_ValidCredentials_Success()
{
	// Arrange: Navigate to the app
	NavigateTo(TestContext.Properties["purl"].ToString());

	// Act & Assert: Call action methods (they handle assertions)
	new Login(driver, TestContext)
		.LoginToApplication(
			TestContext.Properties["userName"].ToString(),
			TestContext.Properties["password"].ToString())
		.VerifyAccountsOverviewPageisLoaded();
}
```

### Checklist
- [ ] Class inherits `TestBaseClass`
- [ ] Method decorated with `[TestMethod]`
- [ ] Calls action methods, not raw `driver.FindElement`
- [ ] No `By.*` selectors in test
- [ ] Test method name: `{Action}_{Scenario}_{Expected}`

### Run It

```powershell
dotnet test --filter "FullyQualifiedName~STAFTests.MyTests.LoginToApp_ValidCredentials_Success" `
	--settings STAFTests/testrunsetting.runsettings
```

### Next Steps

- **Explore:** Open `STAFTests/Tests/ParaTests.cs` to see more examples
- **Learn:** Read [docs/AI_GUIDE.md#workflow-1-ui-test](./AI_GUIDE.md#workflow-1-ui-test)
- **Ask AI:** Use GitHub Copilot or Cursor to generate similar tests

---

## Create Page + Action

### Scenario
You want to automate a new screen/page with multiple interactions.

### Files
1. Create: `STAFTests/Pages/MyNewPage.cs`
2. Create: `STAFTests/Actions/MyNew.cs`
3. Update: `docs/ai-index.json` (run `pwsh tools/UpdateAiIndex.ps1`)

### Step 1: Create Page

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;

namespace STAFTests
{
	public class MyNewPage : PageBaseClass
	{
		#region ObjectIdentifierValues
		private string _btnSubmit = "submit";
		private string _tbEmail = "email";
		#endregion

		public MyNewPage(IWebDriver driver, TestContext context) 
			: base(driver, context) { }

		public IWebElement btnSubmit => FindAppElement(By.Id(_btnSubmit));
		public IWebElement tbEmail => FindAppElement(By.Name(_tbEmail));
	}
}
```

### Step 2: Create Action

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF;

namespace STAFTests
{
	public class MyNew : MyNewPage
	{
		public MyNew(IWebDriver driver, TestContext context) 
			: base(driver, context) { }

		/// <summary>
		/// Verifies the page has loaded and key elements are visible
		/// </summary>
		public MyNew VerifyPageLoaded()
		{
			btnSubmit.ReportElementIsDisplayed(
				Driver, context, nameof(VerifyPageLoaded), 
				"Submit button visible", false);
			return this;
		}

		/// <summary>
		/// Enters email and clicks submit, navigates to next screen
		/// </summary>
		public NextScreenPage SubmitForm(string email)
		{
			var testName = nameof(SubmitForm);
			try
			{
				EnterText(tbEmail, email);
				Click(btnSubmit);
				ReportResult.ReportResultPass(Driver, context, testName, "Form submitted");
			}
			catch (Exception ex)
			{
				ReportResult.ReportResultFail(Driver, context, testName, $"Failed: {ex.Message}");
				Assert.Fail($"Failed: {ex.Message}");
			}
			return new NextScreenPage(Driver, context);
		}
	}
}
```

### Step 3: Update Symbol Index

```powershell
pwsh tools/UpdateAiIndex.ps1
```

### Step 4: Use in Tests

```csharp
[TestMethod]
public void MyNewScreen_SubmitForm_Success()
{
	NavigateTo(TestContext.Properties["purl"].ToString());

	new MyNew(driver, TestContext)
		.VerifyPageLoaded()
		.SubmitForm("user@example.com")
		.VerifyNextPageLoaded();
}
```

### Checklist
- [ ] Page inherits `PageBaseClass`
- [ ] Locators in `#region ObjectIdentifierValues`
- [ ] All element properties use `FindAppElement(By.*, selector, "description")`
- [ ] Action inherits page class
- [ ] All methods have XML comments
- [ ] Methods return `this` (stay) or `new NextPage(driver, context)` (navigate)
- [ ] All steps have `ReportResult` calls
- [ ] Updated `docs/ai-index.json`

### Next Steps

- **Explore:** Open `STAFTests/Pages/LoginPage.cs` and `STAFTests/Actions/Login.cs`
- **Learn:** Read [docs/AI_GUIDE.md#workflow-2-page--action](./AI_GUIDE.md#workflow-2-page--action)
- **Ask AI:** Use Copilot/Cursor to generate page + action pair

---

## Create API Test

### Scenario
You want to test a REST API endpoint (e.g., reqres.in, internal API).

### Files
1. Add method to: `STAFTests/Requests/CreateRequests.cs`
2. Create: `STAFTests/APIData/ResponseDTO.cs`
3. Add test to: `STAFTests/Tests/APITests.cs`

### Step 1: Create Request Method

```csharp
// In STAFTests/Requests/CreateRequests.cs
public async Task<RestResponse<UsersListDTO>> GetUsersList(int page = 1)
{
	var client = new RestClient("https://reqres.in");
	var request = new RestRequest("/api/users", Method.Get);
	request.AddParameter("page", page);
	return await client.ExecuteAsync<UsersListDTO>(request);
}
```

### Step 2: Create DTO

```csharp
// File: STAFTests/APIData/UsersListDTO.cs
using System.Text.Json.Serialization;

namespace STAFTests.APIData
{
	public class UsersListDTO
	{
		[JsonPropertyName("page")]
		public int Page { get; set; }

		[JsonPropertyName("per_page")]
		public int PerPage { get; set; }

		[JsonPropertyName("total")]
		public int Total { get; set; }

		[JsonPropertyName("data")]
		public List<UserDTO> Data { get; set; }
	}

	public class UserDTO
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("email")]
		public string Email { get; set; }
	}
}
```

### Step 3: Create Test Method

```csharp
// In STAFTests/Tests/APITests.cs
[TestMethod]
public async Task GetUsersList_Page1_ReturnsSuccess()
{
	var testName = nameof(GetUsersList_Page1_ReturnsSuccess);
	var requests = new CreateRequests();

	// Act
	var response = await requests.GetUsersList(page: 1);

	// Assert: Check status code first
	if (response.StatusCode != System.Net.HttpStatusCode.OK)
	{
		ReportResultAPI.ReportResultFail(
			TestContext, testName, 
			$"Expected 200, got {response.StatusCode}");
		Assert.Fail();
	}

	// Assert: Check content
	Assert.IsNotNull(response.Data?.Data, "Data should not be null");
	Assert.IsTrue(response.Data.Data.Count > 0, "Should have users");

	ReportResultAPI.ReportResultPass(TestContext, testName, "Users retrieved successfully");
}
```

### Checklist
- [ ] Test class inherits `TestBaseAPI` (not `TestBaseClass`)
- [ ] Request method: `public async Task<RestResponse<DTO>>`
- [ ] Request method added to `CreateRequests.cs`
- [ ] DTO class in `STAFTests/APIData/`
- [ ] DTO has `[JsonPropertyName]` attributes
- [ ] Test checks status code **before** parsing content
- [ ] Fail path: `ReportResultAPI.ReportResultFail(...)` → `Assert.Fail(...)`
- [ ] Pass path: `ReportResultAPI.ReportResultPass(...)`
- [ ] Updated `docs/ai-index.json`

### Run It

```powershell
dotnet test --filter "FullyQualifiedName~STAFTests.APITests.GetUsersList_Page1_ReturnsSuccess" `
	--settings STAFTests/testrunsetting.runsettings
```

### Next Steps

- **Explore:** Open `STAFTests/Tests/APITests.cs` and `STAFTests/Requests/CreateRequests.cs`
- **Learn:** Read [docs/AI_GUIDE.md#workflow-3-api-test](./AI_GUIDE.md#workflow-3-api-test)
- **Ask AI:** Use Copilot/Cursor to generate API tests

---

## Framework Overview

### Key Concepts

**Three base classes:**
1. **`TestBaseClass`** — for UI/browser automation tests
   - Has `driver` property (Selenium WebDriver)
   - Use for: navigating pages, interacting with UI, verifying state

2. **`TestBaseAPI`** — for REST API tests
   - No WebDriver; uses RestSharp
   - Use for: API calls, response validation

3. **`PageBaseClass`** — for page object definitions
   - Inherit once per screen/page
   - Contains locators + element properties
   - Action classes inherit from this

**Three workflow files:**

| Workflow | Pages | Actions | Tests | Purpose |
|----------|-------|---------|-------|---------|
| **Page + Action** | `STAFTests/Pages/*Page.cs` | `STAFTests/Actions/*.cs` | `STAFTests/Tests/*Tests.cs` | Full UI automation flow |
| **API** | N/A | N/A | `STAFTests/Tests/APITests.cs` | REST API testing |
| **Data-Driven** | Mixed | Mixed | `STAFTests/Tests/ExcelTests.cs` | Tests with Excel data |

### Code Rules

- ❌ **Don't:** Create new `IWebDriver()` in tests
- ✅ **Do:** Use inherited `driver` property
- ❌ **Don't:** Use `Thread.Sleep()`
- ✅ **Do:** Use `FindAppElement()` (auto-waits 10 seconds)
- ❌ **Don't:** Use `By.*` selectors directly in tests
- ✅ **Do:** Reference page properties instead
- ❌ **Don't:** Put assertions in tests
- ✅ **Do:** Put assertions in action methods (via `ReportResult`)

### File Naming

**File name = Class name** (always!)

| File | Class | Location |
|------|-------|----------|
| `LoginPage.cs` | `public class LoginPage` | `STAFTests/Pages/` |
| `Login.cs` | `public class Login` | `STAFTests/Actions/` |
| `ParaTests.cs` | `public class ParaTests` | `STAFTests/Tests/` |

### Example: Login Flow

```csharp
// User navigates to login page
NavigateTo(TestContext.Properties["purl"].ToString());

// User logs in using action class
var accountsPage = new Login(driver, TestContext)
	.EnterUserName("user123")
	.EnterPassword("pass123")
	.ClickLogin()
	.VerifyAccountsOverviewPageIsLoaded();

// Optional: Navigate to another screen
accountsPage.ClickLogout();
```

---

## 📚 Full Documentation

| Document | Best For | Link |
|----------|----------|------|
| **Master AI Guide** | Complete patterns, templates, examples | [docs/AI_GUIDE.md](./AI_GUIDE.md) |
| **VS GitHub Copilot** | Visual Studio users | [.github/copilot-instructions.md](../.github/copilot-instructions.md) |
| **VS Code Setup** | VS Code + Copilot users | [.vscode/README.md](../.vscode/README.md) |
| **Cursor Skills** | Cursor editor users | [.cursor/skills/MASTER.md](../.cursor/skills/MASTER.md) |
| **Cursor Rules** | Code generation consistency | [.cursor/cursor.rules](../.cursor/cursor.rules) |
| **Symbol Index** | Finding classes/methods | `docs/ai-index.json` |

---

## 💬 Using GitHub Copilot / Cursor

### Visual Studio
1. Open your test file
2. Press `Ctrl+Shift+I` (or click Copilot icon)
3. Ask: *"Create a UI test for login using ParaTests pattern"*
4. Copilot generates code based on `.github/copilot-instructions.md`

### VS Code
1. Open your test file
2. Press `Ctrl+Shift+I` (Copilot Chat)
3. Reference files: `@STAFTests/Tests/ParaTests.cs`
4. Ask: *"Create a similar test for..."*
5. Copilot uses `.github/copilot-instructions.md` + `.vscode/README.md`

### Cursor
1. Open your test file
2. Press `Cmd+K` (Composer) or `Cmd+L` (Chat)
3. Reference skill: *"staf-ui-test: Create..."*
4. Cursor uses `.cursor/skills/MASTER.md` + `.cursor/cursor.rules`

---

## 🔍 Troubleshooting

### Code Doesn't Compile
1. Check namespace: `namespace STAFTests { }`
2. Check `using` statements: `using OpenQA.Selenium;`, `using STAF;`
3. Check inheritance: Correct base class?
4. Run: `dotnet build STAFTests/STAF.Selenium.Tests.csproj`

### Test Fails
1. Check locators in browser DevTools
2. Check `appsettings.json` has correct URLs
3. Verify `testrunsetting.runsettings` configuration
4. Check test report in `TestResults/` folder

### Copilot Not Helping
1. Reference specific golden files: `@STAFTests/Actions/Login.cs`
2. Reference docs: `#docs/AI_GUIDE.md`
3. Ask step-by-step (one task at a time)
4. Include error messages in your prompt

---

## 🚀 Next Steps

1. **Pick a task** from the top of this page
2. **Follow the example code** — copy & adapt
3. **Check the checklist** — verify before running
4. **Run your test** — use the PowerShell command
5. **Reference golden files** — they're your template
6. **Ask AI if stuck** — Copilot/Cursor are trained on this codebase

---

## ❓ Questions?

| Question | Answer |
|----------|--------|
| **Where are the golden examples?** | `STAFTests/Tests/ParaTests.cs`, `STAFTests/Actions/Login.cs`, `STAFTests/Pages/LoginPage.cs` |
| **How do I run a specific test?** | Use `dotnet test --filter "FullyQualifiedName~Namespace.Class.Method"` |
| **What's the difference between Page and Action?** | **Page:** locators only. **Action:** user flows + assertions. Read [docs/AI_GUIDE.md](./AI_GUIDE.md) |
| **Can I use raw `driver.FindElement`?** | No. Always use `FindAppElement` or page properties. |
| **Do I need to update `ai-index.json`?** | Yes, after creating new pages/actions/requests. Run `pwsh tools/UpdateAiIndex.ps1` |
| **How long should a test method be?** | Thin — usually 3-5 lines. Logic goes in actions. |

---

**Last Updated:** 2026-05-31  
**Framework Version:** STAF.UI.API v4.4.0+  
**Target Framework:** .NET 10
