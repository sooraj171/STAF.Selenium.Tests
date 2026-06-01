---
name: staf-api-test
description: >-
  Creates or edits STAF API tests (TestBaseAPI, RestSharp, CreateRequests, DTOs in
  APIData, ReportResultAPI). Use for REST tests, reqres/DummyJSON, or API reporting samples.
---

# STAF API test

## Workflow

1. Check `STAFTests/Requests/` and `STAFTests/APIData/` for existing client/DTO.
2. Add or extend request method in `CreateRequests` (or new `*Requests` class).
3. Add DTO under `APIData/` if response shape is new.
4. `[TestMethod]` in class inheriting **`TestBaseAPI`** (e.g. `APITests.cs`).
5. Assert with MSTest `Assert.*` then `ReportResultAPI.ReportResultPass/Fail(TestContext, nameof(...), "msg")`.

## Checklist

- [ ] `TestBaseAPI` — not `TestBaseClass`
- [ ] No WebDriver / `driver` usage
- [ ] Fail path calls `ReportResultAPI.ReportResultFail` before `Assert.Fail` (match `APITests`)
- [ ] Update `docs/ai-index.json` for new request/DTO/test symbols

## Golden files

`STAFTests/Tests/APITests.cs`, `STAFTests/Requests/CreateRequests.cs`

Template: [reference.md](reference.md)
