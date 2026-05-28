---
name: staf-ui-test
description: >-
  Creates or edits STAF UI tests (TestBaseClass, MSTest, Action classes, NavigateTo,
  ReportResult). Use when adding UI test methods, Parabank/Google flows, or refactoring
  tests away from raw WebDriver.
---

# STAF UI test

## Workflow

1. Check `docs/ai-index.json` for existing Page/Action.
2. If missing → use skill **staf-page-action** (or create Page then Action first).
3. Add `[TestMethod]` in `STAFTests/Tests/` : `TestBaseClass`.
4. Arrange: `NavigateTo(TestContext.Properties["purl"|"url"].ToString())` (or action that navigates).
5. Act/Assert: chain Action methods only; no `By.*` in test.
6. Run: `dotnet test --filter "FullyQualifiedName~YourMethod" --settings STAFTests/testrunsetting.runsettings`

## Checklist

- [ ] Inherits `TestBaseClass`
- [ ] Uses `driver` from base — never constructed in test
- [ ] No `Thread.Sleep`
- [ ] No duplicate methods on existing Actions
- [ ] Optional Axe: see `ParaTests.LoginToApp` pattern

## Golden file

Open **one**: `STAFTests/Tests/ParaTests.cs` + `STAFTests/Actions/Login.cs`

Templates: [reference.md](reference.md)
