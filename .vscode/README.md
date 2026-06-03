# VS Code GitHub Copilot Setup — STAF.Selenium.Tests

**Configuration and best practices for GitHub Copilot in VS Code.**

---

## Setup Checklist

- [ ] **GitHub Copilot Extension Installed** — Search "GitHub Copilot" in Extensions (Ctrl+Shift+X)
- [ ] **Signed into GitHub** — Command Palette (Ctrl+Shift+P) → "GitHub Copilot: Sign In"
- [ ] **C# Extension Installed** — "C# Dev Kit" by Microsoft for IntelliSense
- [ ] **Workspace Settings Configured** — `.vscode/settings.json` in place (see below)
- [ ] **Master Guide Available** — `docs/AI_GUIDE.md` for reference

---

## Configuration

### .vscode/settings.json

```json
{
  "dotnet.unitTests.runSettingsPath": "${workspaceFolder}/STAFTests/testrunsetting.runsettings",
  "[csharp]": {
	"editor.defaultFormatter": "ms-dotnettools.csharp",
	"editor.formatOnSave": true
  },
  "omnisharp.useRoslynAnalyzers": true
}
```

### .vscode/extensions.json

Recommended extensions (optional prompt to install):

```json
{
  "recommendations": [
	"GitHub.copilot",
	"GitHub.copilot-chat",
	"ms-dotnettools.csharp",
	"ms-dotnettools.vscode-dotnet-runtime",
	"ms-vscode.makefile-tools"
  ]
}
```

### .vscode/launch.json (Optional — for debugging tests)

```json
{
  "version": "0.2.0",
  "configurations": [
	{
	  "name": ".NET Core Attach",
	  "type": "coreclr",
	  "request": "attach",
	  "processId": "${command:pickProcess}"
	}
  ]
}
```

---

## Using GitHub Copilot in VS Code

### Copilot Chat (Ctrl+Shift+I)

**Best for:** Asking questions, generating code, explaining patterns.

1. **Open Copilot Chat:** `Ctrl+Shift+I` (or click chat icon in sidebar)
2. **Ask your question:**
   - *"Create a UI test for login flow using ParaTests.cs pattern"*
   - *"Generate a page object for the MyNewPage with these selectors"*
   - *"How do I structure an API test with RestSharp?"*
3. **Copilot generates code** (in chat window)
4. **Insert into file:** Click "Insert" or copy/paste

### Inline Copilot (Ctrl+Alt+\)

**Best for:** Quick completions while coding.

1. Type method or class name
2. Press `Ctrl+Alt+\` (or `Cmd+\` on Mac)
3. Copilot suggests code inline
4. Accept with `Tab`, reject with `Esc`

### File References

**In Copilot Chat, reference files for context:**

```
@filename.cs      - Include entire file
#docs/AI_GUIDE.md - Reference specific documentation
#function:MyMethod - Reference a function
```

---

## Prompts for Common Tasks

### Create UI Test

```
Create a new UI test in STAFTests/Tests/MyNewTests.cs that:
1. Navigates to the Parabank login page (use TestContext.Properties["purl"])
2. Logs in with valid credentials
3. Verifies the accounts overview page loads

Use the pattern from @STAFTests/Tests/ParaTests.cs and @STAFTests/Actions/Login.cs
Inherit TestBaseClass and call action methods only.
```

### Create Page + Action

```
Create a Page Object and Action flow in STAFTests:
1. Page: STAFTests/Pages/MyNewPage.cs (inherit PageBaseClass)
   - Elements: button with id "submit", text with class "message"
2. Action: STAFTests/Actions/MyNew.cs (inherit MyNewPage)
   - Method VerifyPageLoaded() - checks submit button displayed
   - Method ClickSubmit() - clicks button, returns NextPage class

Use the pattern from @STAFTests/Pages/LoginPage.cs and @STAFTests/Actions/Login.cs
Include ReportResult calls and XML comments.
```

### Create API Test

```
Create an API test:
1. Add request method in @STAFTests/Requests/CreateRequests.cs
   - Method: GetUsers(int page) - calls https://reqres.in/api/users?page={page}
   - Returns: RestResponse<UsersListDTO>
2. Create DTO: STAFTests/APIData/UsersListDTO.cs for response shape
3. Create test in STAFTests/Tests/APITests.cs
   - Test method: GetUsers_Page1_ReturnsSuccess
   - Check status 200, assert data not null
   - Use ReportResultAPI

Pattern from @STAFTests/Tests/APITests.cs and @STAFTests/Requests/CreateRequests.cs
```

### Refactor Test Away from Raw WebDriver

```
Refactor this test to use Page Object Model and action classes:
[Include test code]

Create:
1. Page class extracting locators
2. Action class with flow methods
3. Thin test calling action methods only

Reference docs/AI_GUIDE.md#workflow-2-page--action for patterns.
```

---

## Keyboard Shortcuts

| Action | Shortcut | Notes |
|--------|----------|-------|
| Open Copilot Chat | `Ctrl+Shift+I` | Main interface for questions/generation |
| Inline Suggestion | `Ctrl+Alt+\` | Quick code completion |
| Trigger Suggestion | `Ctrl+Space` | Show Copilot completions at cursor |
| Accept Suggestion | `Tab` | Accept inline suggestion |
| Reject Suggestion | `Esc` | Dismiss inline suggestion |
| Command Palette | `Ctrl+Shift+P` | Access all commands |
| Go to Definition | `F12` or `Ctrl+Click` | Jump to method/class definition |
| Find References | `Shift+F12` | See all usages of symbol |
| Format Document | `Shift+Alt+F` | Auto-format C# code |

---

## Best Practices

### 1. Reference Golden Examples

Always point Copilot to working code:

```
"Use the pattern from @STAFTests/Actions/Login.cs"
"Reference @STAFTests/Pages/LoginPage.cs for locator structure"
```

### 2. Include File Context

When asking for code generation, provide full file path and context:

```
"Create in STAFTests/Tests/MyNewTests.cs inheriting TestBaseClass"
"Add to STAFTests/Requests/CreateRequests.cs using RestSharp"
```

### 3. Reference Documentation

Point to specific sections in `docs/AI_GUIDE.md`:

```
"Follow the pattern in docs/AI_GUIDE.md#workflow-1-ui-test"
"Use ReportResult as shown in docs/AI_GUIDE.md#code-patterns"
```

### 4. Specify Constraints

Be explicit about rules:

```
"Don't use Thread.Sleep, use FindAppElement instead"
"No raw driver.FindElement — use page properties only"
"Every step must call ReportResult or ReportElement*"
```

### 5. Ask for Validation

Get Copilot to verify code against checklist:

```
"Generate the code. Then verify it meets the checklist in docs/AI_GUIDE.md"
"Check that all methods have XML comments and inheritance is correct"
```

---

## Testing in VS Code

### Run Test from Explorer

1. **Open Test Explorer:** `Ctrl+Shift+T` (or click icon in Activity Bar)
2. **Find your test** in the tree
3. **Right-click** → "Run Test" or "Debug Test"

### Run Test from Command Palette

```powershell
# Open terminal in VS Code: Ctrl+`
dotnet test --filter "FullyQualifiedName~STAFTests.ParaTests.LoginToApp_ValidCredentials_Success" `
	--settings STAFTests/testrunsetting.runsettings
```

### Build Solution

```powershell
# Ctrl+` to open terminal
dotnet build STAFTests/STAF.Selenium.Tests.csproj
```

---

## Troubleshooting Copilot

### Copilot Chat Not Appearing

1. Install "GitHub Copilot" extension from VS Code Marketplace
2. Ensure you're signed into GitHub (Command Palette → "GitHub Copilot: Sign In")
3. Reload VS Code window (`Ctrl+Shift+P` → "Developer: Reload Window")

### Copilot Ignoring Context

1. **Use `@` references** explicitly: `@STAFTests/Actions/Login.cs`
2. **Include file paths** in your prompt
3. **Reference docs** with `#docs/AI_GUIDE.md`
4. **Keep prompts focused** on one task at a time

### Generated Code Not Compiling

1. **Check namespaces** — must be `namespace STAFTests { }`
2. **Verify usings** — ensure `using OpenQA.Selenium;`, `using STAF;`, etc.
3. **Test inheritance** — `TestBaseClass` (UI) vs `TestBaseAPI` (API)
4. **Run `dotnet build`** to see exact errors
5. **Ask Copilot to fix** — paste error and ask for solution

### Test Fails at Runtime

1. **Check element locators** — use browser DevTools to verify selectors
2. **Verify URLs** in `appsettings.json` and `testrunsetting.runsettings`
3. **Look at test report** — check `TestResults/` folder for detailed logs
4. **Debug with breakpoints** — set breakpoints and use F5 to debug

---

## Documentation References

| Document | Purpose | Path |
|----------|---------|------|
| **Master AI Guide** | Comprehensive patterns, workflows, templates | `docs/AI_GUIDE.md` |
| **Quick Start** | Quick platform navigation | `docs/QUICK_START.md` |
| **Copilot Instructions** | VS/Cursor rules (also useful for VS Code) | `.github/copilot-instructions.md` |
| **Cursor Skills** | Same skills for all platforms | `.cursor/skills/` |
| **Symbol Index** | Class/method reference for agents | `docs/ai-index.json` |

---

## Copilot Tips by Task

### Adding a Test Method

> Reference **UI Test Workflow** from docs/AI_GUIDE.md#workflow-1-ui-test
>
> **Prompt:**
> ```
> Add a new test method to @STAFTests/Tests/ParaTests.cs
> Test: LoginToApp_InvalidCredentials_ErrorDisplayed
> Pattern: Navigate → call Login.LoginToApplicationInvalid("bad", "bad") → verify error
> Inherit TestBaseClass, call action methods only, use TestContext.Properties for URLs
> ```

### Creating a Page Object

> Reference **Page + Action Workflow** from docs/AI_GUIDE.md#workflow-2-page--action
>
> **Prompt:**
> ```
> Create @STAFTests/Pages/MyNewPage.cs inheriting PageBaseClass
> Locators:
> - Button: id="submit"
> - Message: class="error-msg"
> - Input: name="email"
> Use FindAppElement for all, include XML comments
> Then create @STAFTests/Actions/MyNew.cs with VerifyPageLoaded() and other methods
> ```

### Creating an API Test

> Reference **API Test Workflow** from docs/AI_GUIDE.md#workflow-3-api-test
>
> **Prompt:**
> ```
> Create API test for GitHub users endpoint https://api.github.com/users/{username}
> 1. Add method GetUser(string username) to @STAFTests/Requests/CreateRequests.cs
> 2. Create DTO @STAFTests/APIData/GitHubUserDTO.cs
> 3. Create test GetUser_ValidUsername_ReturnsSuccess in APITests.cs
> Use RestSharp, async/await, ReportResultAPI
> Pattern from @STAFTests/Tests/APITests.cs
> ```

---

## Extending Copilot Capabilities

### Use Custom Instructions

If you have organization-specific patterns, create a `.vscode/copilot-settings.md`:

```markdown
# STAF Custom Patterns

## Naming Conventions
- Page classes: `{ScreenName}Page` inherits `PageBaseClass`
- Action classes: `{ScreenName}` inherits `{ScreenName}Page`
- Test classes: `{Feature}Tests` inherits `TestBaseClass`

## Reporting Pattern
All steps must call ReportResult or ReportElement*.
Format: `ReportResult.ReportResultPass(Driver, context, nameof(MethodName), "message")`

## Element Finders
Prefer: id > name > css > xpath
Always include description: FindAppElement(By.Id("id"), "description")
```

---

## Getting Help

### Copilot Chat Questions

- *"Explain the STAF framework structure"*
- *"Show me an example of Page Object Model in this codebase"*
- *"How do I create a fluent action chain?"*
- *"What's the difference between TestBaseClass and TestBaseAPI?"*

### Reference Documentation

- **Full Guide:** `docs/AI_GUIDE.md`
- **Quick Start:** `docs/QUICK_START.md`
- **Visual Studio Rules:** `.github/copilot-instructions.md`
- **Cursor Skills:** `.cursor/skills/MASTER.md`

---

**Last Updated:** 2026-05-31  
**Framework Version:** STAF.UI.API v4.4.0+  
**Target Framework:** .NET 10  
**Applies To:** VS Code with GitHub Copilot
