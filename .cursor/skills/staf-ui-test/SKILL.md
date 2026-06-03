---
name: staf-ui-test
description: >-
  Creates or edits STAF UI tests (TestBaseClass, MSTest, Action classes, NavigateTo,
  ReportResult). Use when adding UI test methods, Parabank/Google flows, or refactoring
  tests away from raw WebDriver.
---

# STAF UI Test

## Quick Workflow

1. **Check** `docs/ai-index.json` for existing Page/Action
2. **If missing** → use skill **staf-page-action** first (create Page → Action)
3. **Add `[TestMethod]`** in `STAFTests/Tests/{Feature}Tests.cs` inheriting `TestBaseClass`
4. **Arrange:** `NavigateTo(TestContext.Properties["purl"].ToString())` or action that navigates
5. **Act/Assert:** Chain action methods only; no `By.*` selectors in test
6. **Test:** `dotnet test --filter "FullyQualifiedName~YourNamespace.ClassName.MethodName" --settings STAFTests/testrunsetting.runsettings`

## Template

```csharp
[TestMethod]
public void LoginToApp_ValidCredentials_Success()
{
    // Arrange
    NavigateTo(TestContext.Properties["purl"].ToString());

    // Act & Assert (actions handle assertions via ReportResult)
    new Login(driver, TestContext)
        .LoginToApplication(
            TestContext.Properties["userName"].ToString(),
            TestContext.Properties["password"].ToString())
        .VerifyAccountsOverviewPageisLoaded();
}
```

## Checklist

- [ ] Inherits `TestBaseClass` (not custom base, not `TestBaseAPI`)
- [ ] Uses `driver` from base — **never constructed** in test
- [ ] No `Thread.Sleep` — use `FindAppElement` (auto-waits)
- [ ] **No raw `By.*` selectors** — reference page properties only
- [ ] Assertions delegated to action methods (via `ReportResult`)
- [ ] Test is thin — primarily calls action methods
- [ ] Test method name: `{Action}_{Scenario}_{Expected}` (e.g., `LoginToApp_ValidCredentials_Success`)
- [ ] No duplicate method names on actions

## Platforms

| Platform | How to Use | Setup |
|----------|-----------|-------|
| **Visual Studio** | GitHub Copilot chat | See `.github/copilot-instructions.md` |
| **VS Code** | Copilot Chat (`Ctrl+Shift+I`) | See `.vscode/README.md` |
| **Cursor** | Composer or Cmd+K | Reference this skill by name |

## Golden Files

- **Test:** `STAFTests/Tests/ParaTests.cs` — Parabank login flows
- **Action:** `STAFTests/Actions/Login.cs` — Valid/invalid login, fluent returns
- **Page:** `STAFTests/Pages/LoginPage.cs` — Element locators

## Full Guide

📖 **Master documentation:** [docs/AI_GUIDE.md](../../docs/AI_GUIDE.md#workflow-1-ui-test)

Templates & examples: [reference.md](reference.md)
