---
name: staf-page-action
description: >-
  Creates STAF page objects (PageBaseClass, FindAppElement, locator regions) and action
  flows (inherit page, ReportResult, fluent returns). Use when adding screens, POM
  classes, or Login-style flows.
---

# STAF Page + Action

## Quick Workflow

1. **Create Page** `{Screen}Page.cs` in `STAFTests/Pages/`
   - Inherit `PageBaseClass`
   - Define locators in `#region ObjectIdentifierValues`
   - Create properties returning `FindAppElement(By.*, selector, "description")`

2. **Create Action** `{Screen}.cs` in `STAFTests/Actions/`
   - Inherit the page class
   - Add methods: `DoSomething()` (user action), `VerifyXLoaded()` (check state)
   - Return `this` (stay) or `new NextScreen(driver, context)` (navigate)
   - Every step: `ReportResult.ReportResultPass/Fail(...)`

3. **Update symbol index** (once per session)
   - Run: `pwsh tools/UpdateAiIndex.ps1`

4. **Add tests** using skill **staf-ui-test**
   - Call action methods from test class inheriting `TestBaseClass`

## Templates

### Page (Minimal)

```csharp
public class MyScreenPage : PageBaseClass
{
    #region ObjectIdentifierValues
    private string _btnSubmit = "submit";
    private string _lblMessage = ".error-message";
    #endregion

    public MyScreenPage(IWebDriver driver, TestContext context) 
        : base(driver, context) { }

    public IWebElement btnSubmit => FindAppElement(By.Id(_btnSubmit), "Submit button");
    public IWebElement lblMessage => FindAppElement(By.CssSelector(_lblMessage), "Error message");
}
```

### Action (Verification)

```csharp
public class MyScreen : MyScreenPage
{
    public MyScreen(IWebDriver driver, TestContext context) 
        : base(driver, context) { }

    public MyScreen VerifyPageLoaded()
    {
        btnSubmit.ReportElementIsDisplayed(
            Driver, context, nameof(VerifyPageLoaded), 
            "Submit button visible", false);
        return this;
    }
}
```

### Action (Navigation)

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
        ReportResult.ReportResultFail(Driver, context, testName, $"Failed: {ex.Message}");
        Assert.Fail($"Failed: {ex.Message}");
    }
    return new NextScreen(Driver, context);
}
```

## Checklist

- [ ] Page inherits `PageBaseClass`, action inherits page
- [ ] Locators in `#region ObjectIdentifierValues` as private strings
- [ ] All element properties use `FindAppElement(By.*, selector, "description")`
- [ ] Action methods have **descriptive names**: `VerifyPageLoaded()`, `ClickSubmit()`, `EnterUserName()`
- [ ] Every step calls `ReportResult.ReportResultPass/Fail(...)` with `nameof(CurrentMethod)`
- [ ] Methods return `this` (fluent on same page) or `new NextPage(driver, context)` (navigate)
- [ ] Public methods have XML comments: `/// <summary>`
- [ ] No `Thread.Sleep`, no raw `driver.FindElement`, no hardcoded WebDriver
- [ ] File naming matches class naming: `LoginPage.cs` → `public class LoginPage`

## Platforms

| Platform | How to Use | Setup |
|----------|-----------|-------|
| **Visual Studio** | GitHub Copilot chat | See `.github/copilot-instructions.md` |
| **VS Code** | Copilot Chat (`Ctrl+Shift+I`) | See `.vscode/README.md` |
| **Cursor** | Composer or Cmd+K | Reference this skill by name |

## Golden Files

- **Page:** `STAFTests/Pages/LoginPage.cs` — Locator structure, XML comments
- **Action (verification):** `STAFTests/Actions/AboutUs.cs` — Verify page state, return `this`
- **Action (flow):** `STAFTests/Actions/Login.cs` — User actions, fluent chains, navigate to next

## Full Guide

📖 **Master documentation:** [docs/AI_GUIDE.md](../../docs/AI_GUIDE.md#workflow-2-page--action)

Templates & examples: [reference.md](reference.md)
