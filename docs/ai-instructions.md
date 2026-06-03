# STAF.Selenium.Tests — AI context (token-efficient)

Attach with `@docs/ai-instructions.md` **only** when generating Pages, Actions, or Tests. For lighter loads use [AGENTS.md](../AGENTS.md) or [ai-index.json](ai-index.json).

| Tool | Always-on | Skills / setup |
|------|-----------|----------------|
| Cursor | `.cursor/rules/staf-selenium-framework.mdc` | `.cursor/skills/staf-*`, [ai-setup.md](ai-setup.md) |
| Copilot | `.github/copilot-instructions.md` | [ai-prompts.md](ai-prompts.md) |

---

## 1. Framework overview

- **Stack:** C# / **.NET 10**, **MSTest**, Selenium via **[STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API)** (NuGet; version in `STAFTests/STAF.Selenium.Tests.csproj`).
- **UI tests:** inherit `TestBaseClass` (`STAF.CF`). **API / Excel / DB:** inherit `TestBaseAPI`.
- **Pages (canonical POM):** inherit `PageBaseClass`; locate with `FindAppElement`.
- **Assembly:** `GlobalAssemblyInitialize : AssemblyInit` — HTML / assembly reporting; run settings via `RunSettingsFilePath` → `STAFTests/testrunsetting.runsettings`.
- **Parallel:** `[assembly: Parallelize(...)]` in `AssemblyInit.cs` + `<MSTest><Parallelize>` in runsettings — prefer **method-level**, no shared mutable static state.

---

## 2. Project structure

| Path | Purpose |
|------|---------|
| `STAFTests/Pages/` | `*Page` + `PageBaseClass` + `FindAppElement`; some **plain** classes (`GoogleHome`, `LinkedInHome`, `ExcelClass`) — prefer `PageBaseClass` for new work |
| `STAFTests/Actions/` | Flow classes extending `*Page`; `ReportResult` / `ReportElement*`; fluent `return this` / next screen |
| `STAFTests/Tests/` | `[TestClass]` / `[TestMethod]` |
| `STAFTests/Requests/` | REST helpers (e.g. `CreateRequests`) |
| `STAFTests/APIData/` | DTOs for API tests |
| `docs/ai-index.json` | Machine index: symbols → files (for selective `@` context) |

---

## 3. Mandatory rules (strict)

- **No** `new` browser/driver in tests or pages — use base `driver` / constructors `(driver, TestContext)` only.
- **No** `Thread.Sleep` — `FindAppElement`, `WaitForDocumentReady`, etc. from STAF.
- **New page objects:** `PageBaseClass` + **only** `FindAppElement(By)` / `FindAppElement(parent, By, description)` for elements.
- **Navigation / click / type in generated code:** prefer framework helpers (`NavigateTo`, `Click`, `EnterText`) per Cursor rules; **note:** some samples still use `driver.Navigate()` / `SendKeys` — **treat rules as canonical** for new code.
- **Assertions:** `ReportElement*`, `ReportResult` (UI); `ReportResultAPI` (API) — align with existing `Actions/*.cs` and `APITests`.

---

## 4. Page objects (`PageBaseClass`)

- Locator strings in `#region ObjectIdentifierValues` (private fields).
- **Properties** return `IWebElement` via `FindAppElement(...)`.
- **Scoped search:** `FindAppElement(parentElement, By..., "description")` when element is under a container.
- **Ctor:** `public FooPage(IWebDriver driver, TestContext ctx) : base(driver, ctx)`.

---

## 5. Actions / flows

- **Inherit** the page: `public class Login : LoginPage`.
- **Fluent API:** return `this` or `new NextScreen(driver, context)` after steps.
- **Reporting:** `ReportResult.ReportResultPass/Fail(Driver, context, nameof(MethodName), "msg")`; elements: `someElement.ReportElementIsDisplayed(Driver, context, nameof(...), "msg", false)`.
- Keep **test methods thin** — compose calls on Action classes.

---

## 6. Tests

- `[TestClass]`; UI: `TestBaseClass`; API/Excel/DB: `TestBaseAPI`.
- **Config:** `TestContext.Properties["key"]` — keys must match `testrunsetting.runsettings` `<TestRunParameters>` (e.g. `url`, `purl`, `browser`, `userName`, `password`).
- **Run:** `dotnet test --settings STAFTests/testrunsetting.runsettings` (or IDE path to same file).
- **Parallel-safe:** avoid static fields mutated by tests; unique temp files if writing artifacts.

---

## 7. Driver and browser overrides

- Use **`driver`** from `TestBaseClass`.
- Optional overrides (see commented samples in `WebDriverExtensionsSamplesTests`, `ParaTests`): `SetChromeOptions`, `GetBrowserDriverObject` — **API surface is on the base package**; follow samples, do not invent members.

---

## 8. Reporting (quick map)

| API | When |
|-----|------|
| `ReportResult.*` | UI steps / screenshots path via framework |
| `ReportResultAPI.*` | API / no driver |
| `ReportElement*` / extensions on `IWebElement` | Assert + report in one step |

---

## 9. Parallel execution

- Default: **method-level**, multiple workers (assembly + runsettings).
- Avoid **global** counters, shared Excel paths, or fixed screenshot filenames without `TestContext.TestName` / unique IDs.

---

## 10. Naming

- Pages: `{Screen}Page.cs`, class `{Screen}Page`.
- Actions: noun matching screen or flow (`Login`, `AboutUs`, `AccountsOverview`).
- Tests: `{Area}Tests` or `*SamplesTests` for demos.

---

## 11. Few-shot snippets

**Page property (canonical):**

```csharp
public IWebElement btnSubmit => FindAppElement(By.CssSelector(_btnSubmit));
```

**Test method (target pattern — prefer framework NavigateTo if available):**

```csharp
[TestMethod]
public void CanOpenParabank()
{
    NavigateTo(TestContext.Properties["purl"].ToString());
    var login = new Login(driver, TestContext);
    login.LoginToApplication(
        TestContext.Properties["userName"].ToString(),
        TestContext.Properties["password"].ToString())
        .VerifyAccountsOverviewPageisLoaded();
}
```

**Golden references in repo:** `STAFTests/Actions/Login.cs`, `STAFTests/Pages/LoginPage.cs`, `STAFTests/Tests/ParaTests.cs`, `STAFTests/Tests/APITests.cs`.

---

## Token discipline

- Default chat: rely on `.cursor/rules` only.
- For **new** POM/test: `@docs/ai-index.json` + `@docs/ai-instructions.md` + **one** existing Action + **one** Test — not the whole solution.
