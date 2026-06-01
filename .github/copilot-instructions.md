# GitHub Copilot — STAF.Selenium.Tests

Repository instructions for **VS Code** and **Visual Studio**. Mirror of Cursor always-on rules; extended guide: [AGENTS.md](../AGENTS.md).

## Framework

- UI tests: **TestBaseClass** | API/Excel/DB: **TestBaseAPI**
- No `new` driver in tests/pages; no **Thread.Sleep** — use `FindAppElement`, `WaitForDocumentReady`
- Pages: **PageBaseClass** + `FindAppElement` only (new work — not plain POM like `GoogleHome`)
- New code: **NavigateTo**, **Click**, **EnterText**; assertions via **ReportResult** / **ReportElement*** / **ReportResultAPI**

## Workflow

1. Find page in `STAFTests/Pages/` → else create `*Page`
2. Add flow in `STAFTests/Actions/` (inherit page)
3. Thin test in `STAFTests/Tests/` calling actions only
4. Reuse existing methods; AAA; stable locators (id, data-testid, CSS)

## Token discipline

| Need | Open |
|------|------|
| Quick rules | This file (auto-loaded) |
| Agent overview | `AGENTS.md` |
| Class → file | `docs/ai-index.json` |
| Few-shots / parallel | `docs/ai-instructions.md` |
| Copy-paste prompts | `docs/ai-prompts.md` |

Golden: `LoginPage.cs` + `Actions/Login.cs` + `Tests/ParaTests.cs` (UI) | `Requests/CreateRequests.cs` + `Tests/APITests.cs` (API).

**Cursor users:** project skills in `.cursor/skills/` and file rules in `.cursor/rules/` — see [docs/ai-setup.md](../docs/ai-setup.md).
