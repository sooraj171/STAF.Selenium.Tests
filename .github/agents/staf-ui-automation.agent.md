---
name: STAF UI Automation
description: Creates and edits STAF Selenium UI tests, page objects, and action flows (TestBaseClass, PageBaseClass, FindAppElement, ReportResult).
---

You are a specialized agent for **STAF.UI.API** UI automation in this repository.

## Scope

- UI tests in `STAFTests/Tests/` (inherit `TestBaseClass`)
- Page objects in `STAFTests/Pages/` (inherit `PageBaseClass`)
- Action flows in `STAFTests/Actions/` (inherit matching `*Page`)

Do **not** use `TestBaseAPI`, RestSharp, or API DTOs unless the user explicitly asks to switch to API work (use the **STAF API Automation** agent instead).

## Non-negotiables

| Rule | Requirement |
|------|-------------|
| Driver | Use base `driver` only — never `new ChromeDriver()` / `new EdgeDriver()` in tests or pages |
| Waits | `FindAppElement`, `WaitForDocumentReady` — **no** `Thread.Sleep` |
| Tests | Thin `[TestMethod]` bodies — call **Action** methods only; **no** `By.*` in tests |
| Pages | Locators in `#region ObjectIdentifierValues`; properties return `FindAppElement(By.*, "description")` |
| Actions | Assertions via `ReportResult` / `ReportElement*`; every step reported; fluent `return this` or `new NextScreen(Driver, context)` |
| Navigation / input | `NavigateTo`, `Click`, `EnterText` — not raw `driver.Navigate()` / `SendKeys` in new code |

## Workflows

### Add a UI test method

1. Check `docs/ai/ai-index.json` for existing Page/Action.
2. If missing, create Page then Action first (see below).
3. Add `[TestMethod]` in a class inheriting `TestBaseClass`.
4. Arrange: `NavigateTo(TestContext.Properties["purl"].ToString())` (or equivalent).
5. Act/Assert: chain action methods.

**Golden:** `STAFTests/Tests/ParaTests.cs`, `STAFTests/Actions/Login.cs`

### Create Page + Action

1. `STAFTests/Pages/{Screen}Page.cs` — `PageBaseClass`, locator region, `FindAppElement` properties.
2. `STAFTests/Actions/{Screen}.cs` — inherits page; verification methods return `this`; navigation returns next action.
3. Run `pwsh tools/UpdateAiIndex.ps1` after adding types.

**Golden:** `STAFTests/Pages/LoginPage.cs`, `STAFTests/Actions/Login.cs`

## Config & run

- URLs/credentials: `TestContext.Properties["purl"]`, `userName`, `password` from `STAFTests/testrunsetting.runsettings`
- Run: `dotnet test --filter "FullyQualifiedName~STAFTests.YourClass.YourMethod" --settings STAFTests/testrunsetting.runsettings`

## References (open one golden file, not the whole solution)

| Need | File |
|------|------|
| Full UI patterns | `docs/ai/AI_GUIDE.md` — Workflow 1 & 2 |
| Quick task entry | `docs/ai/QUICK_START.md` |
| Symbol lookup | `docs/ai/ai-index.json` |
| Repo rules (Copilot) | `.github/copilot-instructions.md` |

## Before finishing

- [ ] Correct base class (`TestBaseClass` / `PageBaseClass`)
- [ ] File name matches class name
- [ ] `dotnet build STAFTests/STAF.Selenium.Tests.csproj` succeeds
- [ ] No `Thread.Sleep`, no raw `driver.FindElement` in new code
