# STAF.Selenium.Tests — AI Code Generation Guide

**Master documentation for Visual Studio, VS Code, and Cursor.** Single source of truth for STAF.UI.API framework code generation.

---

## Table of Contents

1. [Quick Start](#quick-start)
2. [Framework Overview](#framework-overview)
3. [Three Core Workflows](#three-core-workflows)
   - [UI Test](#workflow-1-ui-test)
   - [Page + Action](#workflow-2-page--action)
   - [API Test](#workflow-3-api-test)
4. [Code Patterns](#code-patterns)
5. [File Structure](#file-structure)
6. [Testing & Validation](#testing--validation)
7. [References](#references)

---

## Quick Start

### Choose Your Task

| Task | Skill/Workflow | Entry Point |
|------|---|---|
| Create new UI test | [UI Test](#workflow-1-ui-test) | `STAFTests/Tests/` |
| Create page object + action flow | [Page + Action](#workflow-2-page--action) | `STAFTests/Pages/` → `STAFTests/Actions/` |
| Create API test | [API Test](#workflow-3-api-test) | `STAFTests/Tests/APITests.cs` |
| Refactor test away from raw WebDriver | [Page + Action](#workflow-2-page--action) | Any test file |

### Quick Rules

- **UI Tests** use `TestBaseClass` (has `driver` property)
- **API Tests** use `TestBaseAPI` (no WebDriver)
- **No `Thread.Sleep`** — use `FindAppElement`, `WaitForDocumentReady`
- **Page Objects** inherit `PageBaseClass`, use `FindAppElement` only
- **Actions** inherit Page, contain test flows, return fluent chains
- **Assertions** via `ReportResult` / `ReportElement*` (UI) or `ReportResultAPI` (API)
- **Locators** prefer `id` > `name` > `css` > `xpath`; always add descriptions
- **File name = Class name**: `LoginPage.cs` → `public class LoginPage`, `Login.cs` → `public class Login`

---

## Framework Overview

### STAF.UI.API (v4.4.0+)

Manages **UI tests** (Selenium WebDriver), **API tests** (RestSharp), and **data extraction** (Excel/Database).

| Component | Base Class | Usage | Key Methods |
|-----------|-----------|-------|------|
| **UI Tests** | `TestBaseClass` | Web app automation | `driver`, `FindAppElement`, `NavigateTo`, `Click`, `EnterText` |
| **Pages** | `PageBaseClass` | Locator definitions | `FindAppElement(By.*, selector)`, properties return `IWebElement` |
| **Actions** | *(inherit Page)* | Test flows, assertions | `ReportResult`, `ReportElement*`, fluent returns |
| **API Tests** | `TestBaseAPI` | REST API calls | `ReportResultAPI`, MSTest `Assert.*` |
| **Requests** | *(plain classes)* | HTTP client methods | RestSharp `RestClient`, DTOs |
| **DTOs** | *(plain classes)* | Response shapes | Auto-properties, decorators (`[JsonProperty]`) |

### TestContext Properties

```csharp
// appsettings.json → Loaded in AssemblyInit.cs
TestContext.Properties["purl"]      // Parabank URL (http://localhost:8090/parabank)
TestContext.Properties["gurl"]      // Google URL
TestContext.Properties["userName"]  // Test user
TestContext.Properties["password"]  // Test password
TestContext.Properties["apiurl"]    // API base URL
```

---

## Three Core Workflows

### Workflow 1: UI Test

**When?** Adding a new test method to an existing action flow.  
**Where?** `STAFTests/Tests/{TestClassName}.cs`  
**Returns?** Test class inheriting `TestBaseClass`.

#### Checklist

- [ ] Inherits `TestBaseClass` (not `TestBaseAPI`)
- [ ] Uses `driver` from base — never constructed new
- [ ] Calls existing **Action methods** only; no `By.*` in test
- [ ] **Arrange:** `NavigateTo(...)` or action that navigates
- [ ] **Act/Assert:** chain Action methods; they return fluent chains or `this`
- [ ] No `Thread.Sleep` — use `FindAppElement` (built-in waits)
- [ ] Test name: `[MethodName]_[Intent]` e.g. `LoginToApp_ValidCredentials_Success`
- [ ] Assertions done in **Action methods** via `ReportResult`; test is thin
- [ ] Optional: Axe accessibility checks (see `ParaTests.LoginToApp` pattern)

#### Template

```csharp
[TestMethod]
public void LoginToApp_ValidCredentials_Success()
{
	// Arrange
	NavigateTo(TestContext.Properties["purl"].ToString());

	// Act & Assert (chained actions handle assertions)
	new Login(driver, TestContext)
		.LoginToApplication(
			TestContext.Properties["userName"].ToString(),
			TestContext.Properties["password"].ToString())
		.VerifyAccountsOverviewPageisLoaded();
}
```

**Negative flow** (stays on same action/page):

```csharp
[TestMethod]
public void LoginToApp_InvalidCredentials_ErrorDisplayed()
{
	NavigateTo(TestContext.Properties["purl"].ToString());

	new Login(driver, TestContext)
		.LoginToApplicationInvalid("bad", "bad")
		.VerifyInvalidUserMessageIsDisplayed();
}
```

#### Command to Run

```powershell
dotnet test --filter "FullyQualifiedName~YourNamespace.YourClass.YourMethod" --settings STAFTests/testrunsetting.runsettings
```

---

### Workflow 2: Page + Action

**When?** Creating a new screen/page object or refactoring tests to use POM.  
**Where?** `STAFTests/Pages/{ScreenName}Page.cs` → `STAFTests/Actions/{ScreenName}.cs`  
**Returns?** Reusable page + action pair for test flows.

#### Order & Checklist

1. **Create Page** in `STAFTests/Pages/`
   - [ ] Inherits `PageBaseClass`
   - [ ] Constructor: `public ScreenNamePage(IWebDriver driver, TestContext context) : base(driver, context) { }`
   - [ ] `#region ObjectIdentifierValues` section with string selectors
   - [ ] Properties return `FindAppElement(By.*, selector, "description")`
   - [ ] Scoped finds: `FindAppElement(parentElement, By.*, "description")`

2. **Create Action** in `STAFTests/Actions/`
   - [ ] Inherits the page class
   - [ ] Constructor passes `driver` and `context` to base
   - [ ] Methods for test steps: `DoSomething()`, `VerifyXLoaded()`
   - [ ] Use page properties for element references
   - [ ] `ReportResult` for every step; `ReportElement*` for checks
   - [ ] Return `this` (same action) or `new NextScreen(driver, context)` (navigate to next)
   - [ ] Naming: `nameof(CurrentMethod)` for report step names

3. **Wire into flow**
   - [ ] Return chain calls navigate to next action: `.ThenClickNext().VerifyNextLoaded()`
   - [ ] Update `docs/ai/ai-index.json` (run `pwsh tools/UpdateAiIndex.ps1`)

#### Page Template

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF.CF;

namespace STAFTests
{
	public class MyScreenPage : PageBaseClass
	{
		#region ObjectIdentifierValues
		private string _btnSubmit = "submit";
		private string _lblMessage = ".message";
		#endregion

		public MyScreenPage(IWebDriver driver, TestContext context) 
			: base(driver, context) { }

		public IWebElement btnSubmit => FindAppElement(By.Id(_btnSubmit));
		public IWebElement lblMessage => FindAppElement(By.CssSelector(_lblMessage));
	}
}
```

#### Action Template (Verification)

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using STAF;

namespace STAFTests
{
	public class MyScreen : MyScreenPage
	{
		public MyScreen(IWebDriver driver, TestContext context) 
			: base(driver, context) { }

		public MyScreen VerifyMyScreenLoaded()
		{
			btnSubmit.ReportElementIsDisplayed(
				Driver, context, nameof(VerifyMyScreenLoaded), 
				"Submit button visible", false);
			return this;
		}
	}
}
```

#### Action Template (Flow with Navigation)

```csharp
public NextScreen ClickSubmit()
{
	var testName = nameof(ClickSubmit);
	try
	{
		Click(btnSubmit);
		ReportResult.ReportResultPass(Driver, context, testName, "Clicked submit");
	}
	catch (Exception ex)
	{
		ReportResult.ReportResultFail(Driver, context, testName, $"Failed to click: {ex.Message}");
		Assert.Fail($"Click failed: {ex.Message}");
	}
	return new NextScreen(Driver, context);
}
```

#### Golden Examples

- **Page:** `STAFTests/Pages/LoginPage.cs`
- **Action (verification):** `STAFTests/Actions/AboutUs.cs`
- **Action (flow):** `STAFTests/Actions/Login.cs` (contains both `LoginToApplication` → navigate + `VerifyInvalidUserMessageIsDisplayed` → stay)

---

### Workflow 3: API Test

**When?** Testing REST APIs (reqres.in, DummyJSON, internal APIs).  
**Where?** `STAFTests/Tests/APITests.cs` (or new `*APITests.cs`)  
**Returns?** API tests with request methods + DTOs.

#### Order & Checklist

1. **Create/Update Request Method** in `STAFTests/Requests/CreateRequests.cs`
   - [ ] Use `RestClient` (RestSharp v114+)
   - [ ] Method signature: `public async Task<RestResponse<T>> MethodName(params)`
   - [ ] Build `RestRequest`, set content-type, headers, body
   - [ ] Return `await client.ExecuteAsync<T>(request)`
   - [ ] Document with XML comments

2. **Create DTO** in `STAFTests/APIData/`
   - [ ] Plain C# class with auto-properties
   - [ ] Use `[JsonProperty("field_name")]` if JSON keys differ from C# names
   - [ ] Example: `DummyJsonUsersDTO.cs`

3. **Add Test Method** in test class inheriting `TestBaseAPI`
   - [ ] **Arrange:** Set up test data, URLs
   - [ ] **Act:** Call request method, capture response
   - [ ] **Assert:** Check status code, content
   - [ ] On **fail**: Call `ReportResultAPI.ReportResultFail(...)` **before** `Assert.Fail(...)`
   - [ ] On **pass**: Call `ReportResultAPI.ReportResultPass(...)`
   - [ ] Test name: `[MethodName]_[Scenario]_[Expected]`

#### Checklist

- [ ] Test class inherits `TestBaseAPI` (not `TestBaseClass`)
- [ ] No WebDriver usage (`driver` not available)
- [ ] Response status checked first (200, 201, 400, 404, etc.)
- [ ] Fail path: `ReportResultAPI.ReportResultFail(...)` → `Assert.Fail(...)`
- [ ] Pass path: `ReportResultAPI.ReportResultPass(...)`
- [ ] Update `docs/ai/ai-index.json` for new request/DTO/test symbols

#### Request Method Template

```csharp
public async Task<RestResponse<UsersListDTO>> GetUsersList(int page = 1)
{
	var client = new RestClient("https://reqres.in");
	var request = new RestRequest("/api/users", Method.Get);
	request.AddParameter("page", page);
	return await client.ExecuteAsync<UsersListDTO>(request);
}
```

#### DTO Template

```csharp
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

		[JsonPropertyName("total_pages")]
		public int TotalPages { get; set; }
	}
}
```

#### Test Method Template

```csharp
[TestMethod]
public async Task GetUsersList_Page1_ReturnsUsersSuccessfully()
{
	var testName = nameof(GetUsersList_Page1_ReturnsUsersSuccessfully);

	// Arrange
	var requests = new CreateRequests();

	// Act
	var response = await requests.GetUsersList(page: 1);

	// Assert
	if (response.StatusCode != System.Net.HttpStatusCode.OK)
	{
		ReportResultAPI.ReportResultFail(
			TestContext, testName, 
			$"Expected 200, got {response.StatusCode}");
		Assert.Fail($"Expected 200, got {response.StatusCode}");
	}

	var data = response.Data;
	Assert.IsNotNull(data.Data, "Data should not be null");
	Assert.IsTrue(data.Data.Count > 0, "Should have users");

	ReportResultAPI.ReportResultPass(TestContext, testName, "Users retrieved successfully");
}
```

#### Golden Examples

- **Requests:** `STAFTests/Requests/CreateRequests.cs`
- **DTOs:** `STAFTests/APIData/DummyJsonUsersDTO.cs`, `ListOfUsersDTO.cs`
- **Tests:** `STAFTests/Tests/APITests.cs`

---

## Code Patterns

### Navigation & Page Transitions

```csharp
// Enter test
NavigateTo(TestContext.Properties["purl"].ToString());

// Action returns next screen
new Login(driver, TestContext)
	.LoginToApplication(user, pwd)  // Returns AccountsOverview
	.VerifyAccountsOverviewPageisLoaded();

// Or explicit navigation
public AccountsOverview ClickLogout()
{
	Click(btnLogout);
	ReportResult.ReportResultPass(Driver, context, nameof(ClickLogout), "Logged out");
	return new AccountsOverview(Driver, context);
}
```

### Element Finders

**From Page** (standard):

```csharp
public IWebElement tbUserName => FindAppElement(By.Name("username"));

// Scoped (within parent):
public IWebElement lvAccount => FindAppElement(parentElement, By.XPath("./div[@class='account']"));
```

**Locator Priority:**

1. `By.Id("id-value")` — Most stable
2. `By.Name("name")` — Form fields
3. `By.CssSelector(".class")` — Flexible, readable
4. `By.XPath("//tag[@attr='val']")` — Last resort, brittle

### Step Reporting (UI)

```csharp
// Action step
var testName = nameof(LoginToApplication);
try
{
	EnterUserName(user);
	EnterPassword(pwd);
	ClickLogin();
	ReportResult.ReportResultPass(Driver, context, testName, "Logged in successfully");
}
catch (Exception ex)
{
	ReportResult.ReportResultFail(Driver, context, testName, $"Login failed: {ex.Message}");
	Assert.Fail($"Login failed: {ex.Message}");
}

// Verification step (check element properties)
btnSubmit.ReportElementIsDisplayed(Driver, context, nameof(VerifyPageLoaded), "Submit button", false);
btnSubmit.ReportElementIsEnabled(Driver, context, nameof(VerifyPageLoaded), "Submit button enabled", false);
btnSubmit.ReportElementHasValue(Driver, context, nameof(VerifyPageLoaded), "Submit button value", "true", false);
```

### Step Reporting (API)

```csharp
var testName = nameof(GetUsersList_Page1_ReturnsUsersSuccessfully);

var response = await requests.GetUsersList(page: 1);

if (response.StatusCode != HttpStatusCode.OK)
{
	ReportResultAPI.ReportResultFail(TestContext, testName, 
		$"Expected 200, got {response.StatusCode}");
	Assert.Fail();
}

// Assertions
Assert.IsNotNull(response.Data, "Data should not be null");

ReportResultAPI.ReportResultPass(TestContext, testName, "Users retrieved");
```

### Waits & Synchronization

```csharp
// Automatic (built into FindAppElement)
var element = FindAppElement(By.Id("myElement"));  // Waits up to 10 seconds

// Explicit page load
WaitForDocumentReady();  // Waits for document.readyState == 'complete'

// Never use:
// Thread.Sleep(5000);  // ❌ Bad practice
```

### Fluent Returns

```csharp
// Stay on same page/action
public MyAction DoSomething()
{
	// ... steps ...
	return this;
}

// Chainable
new Login(driver, context)
	.LoginToApplication(user, pwd)
	.VerifyAccountsOverviewPageisLoaded();
```

---

## File Structure

```
STAFTests/
├── Pages/
│   ├── LoginPage.cs
│   ├── AccountsOverviewPage.cs
│   ├── AboutUsPage.cs
│   └── *Page.cs                    (One per screen)
│
├── Actions/
│   ├── Login.cs                    (inherits LoginPage)
│   ├── AccountsOverview.cs         (inherits AccountsOverviewPage)
│   ├── AboutUs.cs                  (inherits AboutUsPage)
│   └── *.cs                        (One per flow)
│
├── Tests/
│   ├── ParaTests.cs                (Parabank UI tests)
│   ├── APITests.cs                 (REST API tests)
│   ├── GoogleSearchTest.cs         (Google UI test)
│   ├── ExcelTests.cs               (Data-driven with Excel)
│   └── *Tests.cs                   (Test classes)
│
├── Requests/
│   └── CreateRequests.cs           (All request methods)
│
├── APIData/
│   ├── DummyJsonUsersDTO.cs
│   └── *.cs                        (DTOs for API responses)
│
├── appsettings.json                (Test URLs, credentials)
├── testrunsetting.runsettings      (Test run configuration)
└── *.cs                            (Helpers, extensions)

docs/
├── README.md                       (Documentation hub)
├── Architecture-Summary.md
├── STAF-Framework-User-Guide.html
├── ai/
│   ├── AI_GUIDE.md                 (This file — master reference)
│   ├── QUICK_START.md
│   └── ai-index.json
└── details/                        (Extended / maintainer reference)

.github/
├── copilot-instructions.md
└── agents/                         (VS custom agents: UI & API)

.cursor/
├── skills/
│   ├── MASTER.md                   (Cursor skill index)
│   ├── staf-ui-test/
│   │   └── SKILL.md
│   ├── staf-api-test/
│   │   └── SKILL.md
│   └── staf-page-action/
│       └── SKILL.md
└── cursor.rules                    (Cursor-specific rules)

.vscode/
├── README.md                       (VS Code Copilot setup)
└── settings.json
```

---

## Testing & Validation

### Run Specific Test

```powershell
# By full method name (most reliable)
dotnet test --filter "FullyQualifiedName~STAFTests.ParaTests.LoginToApp_ValidCredentials_Success" `
	--settings STAFTests/testrunsetting.runsettings

# By test class
dotnet test --filter "ClassName~ParaTests" --settings STAFTests/testrunsetting.runsettings

# All tests
dotnet test --settings STAFTests/testrunsetting.runsettings
```

### Verify Code Generation

1. **Page Object:**
   - Inherits `PageBaseClass`
   - Properties use `FindAppElement`
   - Selectors in `#region ObjectIdentifierValues`

2. **Action:**
   - Inherits page class
   - Methods return `this` or next action
   - Every step has `ReportResult` / `ReportElement*`
   - No `Thread.Sleep`

3. **Test:**
   - Inherits `TestBaseClass` (UI) or `TestBaseAPI` (API)
   - Calls action methods, not `driver.FindElement`
   - AAA pattern: Arrange, Act, Assert
   - Thin — assertions delegated to actions

### Build & Compile

```powershell
dotnet build STAFTests/STAF.Selenium.Tests.csproj
```

If errors, check:
- Namespace consistency (e.g., `namespace STAFTests { }`)
- Missing `using` statements (`OpenQA.Selenium`, `STAF`, `STAF.CF`)
- Test methods decorated with `[TestMethod]`
- No hardcoded `IWebDriver` instantiation

---

## References

### Official Links

- **STAF.UI.API NuGet:** https://www.nuget.org/packages/STAF.UI.API
- **STAF GitHub:** https://github.com/sooraj171/STAF.Selenium.Tests
- **Selenium WebDriver:** https://www.selenium.dev/documentation/webdriver/
- **RestSharp:** https://restsharp.dev/

### Key Classes

| Class | Namespace | Purpose |
|-------|-----------|---------|
| `TestBaseClass` | `STAF` | Base for UI tests; provides `driver`, `context`, helpers |
| `TestBaseAPI` | `STAF` | Base for API tests; no WebDriver |
| `PageBaseClass` | `STAF.CF` | Base for page objects; provides `FindAppElement` |
| `ReportResult` | `STAF` | UI step reporting (pass/fail) |
| `ReportResultAPI` | `STAF` | API step reporting |
| `RestClient` | `RestSharp` | HTTP client for API calls |

### Documentation Files

- `docs/ai/AI_GUIDE.md` — **This file** (comprehensive patterns & workflows)
- `docs/ai/QUICK_START.md` — Quick platform-specific navigation
- `.github/copilot-instructions.md` — Visual Studio GitHub Copilot rules
- `.cursor/skills/MASTER.md` — Cursor skill index
- `.vscode/README.md` — VS Code Copilot setup

---

## Tips for Code Generation (AI Agents)

### When Using GitHub Copilot / Cursor

1. **Specify the workflow** in your prompt: *"Create a UI test for..."* vs. *"Create a page object for..."*
2. **Reference a golden example**: *"Use the pattern from `LoginPage.cs` and `Login.cs`"*
3. **Indicate file location**: *"In `STAFTests/Actions/`, create..."*
4. **Ask for method signature first** if unsure of pattern
5. **Iterate**: Start with template, then refine with feedback

### Prompts by Scenario

**Create UI Test:**
> Create a new test method in `STAFTests/Tests/MyNewTests.cs` that inherits `TestBaseClass`. The test should navigate to the Parabank login page, log in with valid credentials, and verify the accounts overview loads. Use the pattern from `ParaTests.LoginToApp`.

**Create Page Object:**
> Create a new page class `MyNewPage.cs` in `STAFTests/Pages/` that inherits `PageBaseClass`. Include properties for: button with id "submit", link with text "Next", and input field with name "email". Use `FindAppElement` for all locators and add descriptions.

**Create API Test:**
> Create an async method in `STAFTests/Requests/CreateRequests.cs` that calls the GitHub users API endpoint (`https://api.github.com/users/{username}`). Return a `RestResponse<UserDTO>`. Then add a test in `APITests.cs` that calls this method and verifies the response status and user data.

---

**Last Updated:** 2026-05-31  
**Framework Version:** STAF.UI.API v4.4.0+  
**Target Framework:** .NET 10  
**Platforms:** Visual Studio, VS Code, Cursor
