# STAF.Selenium.Tests

Official sample and reference solution for [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) — UI automation (Selenium), REST API tests, Excel and database helpers, HTML reporting, and accessibility checks using [STAF](https://github.com/sooraj171/STAF).

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022+, or VS Code with C# Dev Kit
- Chrome or Edge (UI tests)

---

## Quick start

```bash
git clone https://github.com/sooraj171/STAF.Selenium.Tests
cd STAF.Selenium.Tests
dotnet restore
dotnet build
dotnet test --settings STAFTests/testrunsetting.runsettings
```

**Run settings:** The project defaults to `STAFTests/testrunsetting.runsettings`. In Visual Studio, use **Test → Configure Run Settings → Select Solution Wide runsettings File** if tests do not pick it up automatically.

**Configuration:** URLs, browser, and credentials are in `STAFTests/testrunsetting.runsettings` (`purl`, `userName`, `password`, …) and `STAFTests/appsettings.json` (database connection, optional email).

After a run, open **TestResults** for per-test HTML reports and the assembly summary (`ResultTemplateFinal.html`).

---

## What’s in the solution

| Area | Examples |
|------|----------|
| **UI tests** | `ParaTests`, `GoogleSearchTest`, `ReportingSamplesTests`, `WebDriverExtensionsSamplesTests` |
| **API / data** | `APITests`, `ExcelTests`, `DatabaseSamplesTests` |
| **Page objects** | `LoginPage`, `AboutUsPage`, `AccountsOverviewPage` (`PageBaseClass`, `FindAppElement`) |
| **Actions** | `Login`, `AboutUs`, `AccountsOverview` — fluent flows with `ReportResult` |
| **Reporting** | `ReportResult`, `ReportElement*`, `ReportResultAPI` |
| **Accessibility** | Axe in `ParaTests.LoginToApp` |
| **Parallel runs** | MSTest `Parallelize` in run settings |

See [RELEASE_NOTES.md](RELEASE_NOTES.md) for framework and .NET 10 upgrade notes.

**AI index maintenance:** After adding or renaming pages, actions, or tests, update `docs/ai-index.json` and run `powershell -File tools/UpdateAiIndex.ps1` from the repo root to validate (or `pwsh` if you use PowerShell 7). Use `-Discover` to list classes under `STAFTests/Pages`, `Actions`, `Tests`, `Requests`, and `APIData`.

---

## Project layout

```
STAF.Selenium.Tests/
├── README.md
├── AGENTS.md                 # AI assistant entry (Cursor, Copilot)
├── STAF.Selenium.Tests.sln
├── STAFTests/                # Test project (Pages, Actions, Tests, Requests, APIData)
├── docs/                     # User guide, architecture summary, AI docs
├── MCPAgent/                 # Optional MCP server for AI + browser tools
├── .github/                  # Copilot instructions and VS custom agents
├── .cursor/                  # Cursor rules and skills
└── .vscode/                  # VS Code settings and MCP config
```

---

## Documentation

| Document | Audience |
|----------|----------|
| [docs/README.md](docs/README.md) | Documentation hub |
| [docs/STAF-Framework-User-Guide.html](docs/STAF-Framework-User-Guide.html) | End-user framework guide (browser / PDF) |
| [docs/Architecture-Summary.md](docs/Architecture-Summary.md) | One-page architecture and MCP overview |
| [docs/ai/QUICK_START.md](docs/ai/QUICK_START.md) | AI: add UI, page/action, or API tests |
| [docs/details/](docs/details/) | Extended / maintainer reference (optional) |

---

## AI-assisted development

STAF patterns are enforced via repo instructions so generated code uses `TestBaseClass`, `FindAppElement`, and `ReportResult` — not raw `Thread.Sleep` or ad-hoc WebDriver code.

| Tool | Entry |
|------|--------|
| **All agents** | [AGENTS.md](AGENTS.md) |
| **GitHub Copilot** | [.github/copilot-instructions.md](.github/copilot-instructions.md) |
| **Visual Studio agents** | [.github/agents/](.github/agents/) — **STAF UI Automation**, **STAF API Automation** |
| **Cursor** | `.cursor/rules/`, `.cursor/skills/` |
| **MCP (browser + codegen)** | [MCPAgent/README.md](MCPAgent/README.md) |

Symbol index: `docs/ai/ai-index.json` — refresh with `pwsh tools/UpdateAiIndex.ps1` after adding types.

---

## Running tests

```bash
# All tests (runsettings from project)
dotnet test

# Explicit runsettings
dotnet test --settings STAFTests/testrunsetting.runsettings

# Filter
dotnet test --filter "ClassName~ParaTests" --settings STAFTests/testrunsetting.runsettings
```

---

## License

MIT License — Copyright © 2026 Sooraj Ramachandran.

[STAF](https://github.com/sooraj171/STAF) · [STAF.UI.API on NuGet](https://www.nuget.org/packages/STAF.UI.API)
