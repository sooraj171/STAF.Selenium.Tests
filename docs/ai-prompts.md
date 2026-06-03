# Copy-paste prompts (STAF)

Use with Cursor Chat/Agent or Copilot Chat. Rules apply automatically; attach **one** golden file only when generating code.

## UI test (Parabank-style)

```text
Add a [TestMethod] in ParaTests that <scenario>. Use TestBaseClass, NavigateTo(TestContext.Properties["purl"]), compose Login/AccountsOverview actions only—no raw FindElement in the test. Report via existing action methods.
```

## New page + action

```text
Create {Screen}Page under STAFTests/Pages/ (PageBaseClass, #region ObjectIdentifierValues, FindAppElement properties) and {Screen} action under Actions/ (inherit page, ReportResult + ReportElement*, fluent return). Mirror LoginPage.cs and Login.cs. Update docs/ai-index.json.
```

## API test

```text
Add a [TestMethod] in APITests: call CreateRequests (or new method in Requests/), assert on DTO, use ReportResultAPI Pass/Fail. TestBaseAPI only. Follow verifyUserDetails pattern.
```

## Refactor assertion

```text
Replace Assert on element.Displayed with ReportElementIsDisplayed(Driver, TestContext, nameof(...), "...", false). Keep scenario steps on ReportResult.
```

## MCP + codegen

```text
Use selenium-staf to open Chrome at purl from run settings, confirm login form, then generate STAF TestBaseClass test + PageBaseClass page matching our repo (FindAppElement, no Thread.Sleep).
```

## Token-saving reminder (any tool)

```text
Before coding: check docs/ai-index.json for existing Page/Action. Reuse Login/AboutUs patterns. Do not read the entire solution.
```
