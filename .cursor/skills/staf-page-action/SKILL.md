---
name: staf-page-action
description: >-
  Creates STAF page objects (PageBaseClass, FindAppElement, locator regions) and action
  flows (inherit page, ReportResult, fluent returns). Use when adding screens, POM
  classes, or Login-style flows.
---

# STAF page + action

## Order

1. **Page** `{Screen}Page.cs` in `STAFTests/Pages/`
2. **Action** `{Screen}.cs` in `STAFTests/Actions/` inheriting the page
3. Wire into existing fluent chain (return `new NextScreen(driver, context)`)
4. Update `docs/ai-index.json`; run `pwsh tools/UpdateAiIndex.ps1`

## Page rules

- `PageBaseClass`, `#region ObjectIdentifierValues`, properties → `FindAppElement`
- Scoped: `FindAppElement(parent, By..., "description")`

## Action rules

- Inherit page; use page properties for elements
- `ReportResult` for steps; `ReportElement*` for checks
- `nameof(CurrentMethod)` for report step names

## Golden files

`LoginPage.cs`, `Login.cs`, `AboutUs.cs`

Templates: [reference.md](reference.md)
