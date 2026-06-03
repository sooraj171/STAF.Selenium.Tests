# AI agents — STAF.Selenium.Tests

C# / MSTest UI and API automation on **[STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API)**. Use this file in **Cursor**, **GitHub Copilot (VS Code / Visual Studio)**, and other agents that read repo-root instructions.

## Non-negotiables

| Rule | UI | API / Excel / DB |
|------|----|------------------|
| Test base | `TestBaseClass` | `TestBaseAPI` |
| Driver | Use base `driver` only — never `new` browser in tests/pages | N/A |
| Waits | `FindAppElement`, `WaitForDocumentReady` — **no** `Thread.Sleep` | N/A |
| Pages | `PageBaseClass` + `FindAppElement` | N/A |
| Navigation / input | `NavigateTo`, `Click`, `EnterText` (not raw `driver.Navigate()` / `SendKeys` in **new** code) | N/A |
| Reporting | `ReportResult`, `ReportElement*` | `ReportResultAPI` |
| Tests | Thin methods — call **Actions**, not raw locators | `Requests/` + `APIData/` DTOs |

## Layout

| Folder | Role |
|--------|------|
| `STAFTests/Pages/` | Locators + `IWebElement` properties (`*Page`) |
| `STAFTests/Actions/` | Flows; inherit page; fluent returns |
| `STAFTests/Tests/` | `[TestMethod]` only |
| `STAFTests/Requests/` | REST helpers |
| `STAFTests/APIData/` | Response DTOs |

Config: `TestContext.Properties["key"]` ↔ `STAFTests/testrunsetting.runsettings` (`url`, `purl`, `browser`, `userName`, `password`, …).

Run: `dotnet test --settings STAFTests/testrunsetting.runsettings`

## Golden examples (open these, not the whole solution)

| Task | Files |
|------|--------|
| Page + action + UI test | `LoginPage.cs`, `Actions/Login.cs`, `Tests/ParaTests.cs` |
| API test | `Requests/CreateRequests.cs`, `Tests/APITests.cs` |
| Reporting | `Tests/ReportingSamplesTests.cs` |

## Token discipline

1. **Default:** rely on `.cursor/rules/staf-selenium-framework.mdc` or `.github/copilot-instructions.md` — do not paste full `docs/AI_GUIDE.md` or `docs/ai-instructions.md` unless generating new types.
2. **New page/action/test:** add `docs/ai-index.json` (symbol lookup) + **one** golden file from the table above.
3. **Deep detail:** `docs/AI_GUIDE.md` (workflows/templates) or `docs/ai-instructions.md` (parallel/reporting few-shots) — attach one file only.
4. **Cursor skills** (optional, on-demand): `.cursor/skills/staf-*` — index [`.cursor/skills/MASTER.md`](.cursor/skills/MASTER.md); see [docs/ai-setup.md](docs/ai-setup.md).

After adding types: update `docs/ai-index.json`; validate with `pwsh tools/UpdateAiIndex.ps1` (`-Discover` to list classes).

## Tool-specific setup

| Tool | Instructions |
|------|----------------|
| **Cursor** | `.cursor/rules/*.mdc` (always-on + file-scoped); skills in `.cursor/skills/`; MCP: `.cursor/mcp.json` |
| **VS Code Copilot** | `.github/copilot-instructions.md`; MCP: `.vscode/mcp.json` |
| **Visual Studio Copilot** | `.github/copilot-instructions.md`; **custom agents** in `.github/agents/*.agent.md`; MCP: `.mcp.json` |

### Visual Studio custom agents (UI & API)

Pick an agent from the Copilot agent picker (VS 2026 18.4+) or type `@staf-ui-automation` / `@staf-api-automation`:

| Agent | File | Use for |
|-------|------|---------|
| **STAF UI Automation** | [.github/agents/staf-ui-automation.agent.md](.github/agents/staf-ui-automation.agent.md) | UI tests, pages, actions |
| **STAF API Automation** | [.github/agents/staf-api-automation.agent.md](.github/agents/staf-api-automation.agent.md) | REST tests, requests, DTOs |

Master guide: [docs/AI_GUIDE.md](docs/AI_GUIDE.md) · Onboarding: [START_HERE.md](START_HERE.md) · Index: [INDEX.md](INDEX.md)

MCP server: `MCPAgent/publish/mcp-sharp-staf-selenium.exe` — see [MCPAgent/README.md](MCPAgent/README.md).

## Prompt shortcuts

Copy from [docs/ai-prompts.md](docs/ai-prompts.md) for common tasks (new UI test, API test, page class, refactor to ReportElement).
