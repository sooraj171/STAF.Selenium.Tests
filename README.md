# STAF.Selenium.Tests

**STAF.Selenium.Tests** is the official sample and reference implementation for the [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) NuGet package. It demonstrates how to use [STAF](https://github.com/sooraj171/STAF) (Simple Test Automation Framework) for UI automation, API tests, Excel validation, database helpers, reporting, accessibility, and more.

Anyone using this project can see working samples of every major STAF feature and quickly adopt the framework in their own test suites.

---

## Table of Contents

- [Features Covered](#features-covered)
- [MCP Agent (AI-Assisted Development)](#mcp-agent-ai-assisted-development)
- [AI-assisted development](#ai-assisted-development-cursor-vs-code-visual-studio)
  - [Skills quick chat sheet](#skills-quick-chat-sheet)
- [Documentation and architecture](#documentation-and-architecture)
- [Release Notes](#release-notes)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Test Samples Overview](#test-samples-overview)
- [Project Structure](#project-structure)
- [Running Tests](#running-tests)
- [License](#license)

---

## Features Covered

| STAF Feature | Description | Sample Location |
|-------------|-------------|-----------------|
| **TestBaseClass** | UI test base: WebDriver init, HTML reporting, cleanup | `GoogleSearchTest`, `ParaTests`, `ReportingSamplesTests`, `WebDriverExtensionsSamplesTests`, `BrowserOverrideSamplesTests` |
| **TestBaseAPI** | API test base: same reporting/cleanup, no browser | `APITests`, `ExcelTests`, `DatabaseSamplesTests`, `ProgrammaticReportSamplesTests` |
| **PageBaseClass** | Page Object with wait: `FindAppElement(By)`, `FindAppElement(parent, By, description)` | `LoginPage`, `AboutUsPage`, `AccountsOverviewPage` |
| **ReportResult** | UI step reporting: Pass, Fail, Warn, Info | `ReportingSamplesTests`, `Login`, `GoogleHome`, etc. |
| **ReportResultAPI** | API step reporting: Pass, Fail, Warn, Info | `APITests`, `ExcelTests`, `DatabaseSamplesTests` |
| **ReportElement** | Assert + report: ReportElementExists, IsDisplayed, IsEnabled | `ReportingSamplesTests`, `Login`, `AboutUs`, `AccountsOverview` |
| **HTML reporting** | Per-test and assembly summary (ResultTemplateFinal.html) | All tests via `AssemblyInit` |
| **ExcelDriver** | CompareFiles, GetExcelWorkbook, GetExcelCellData, SetExcelCellData, GetExcelRowCount, GetExcelColumnCount | `ExcelTests` |
| **DbHelper** | Connection from config, VerifyConnection, ExecuteScalar, ExecuteQuery, ExecuteNonQuery | `DatabaseSamplesTests` |
| **WebDriver extensions** | WaitForDocumentReady, getTotalTabsCount, CloseAllTabsExceptCurrent, waitForFindElement | `WebDriverExtensionsSamplesTests` |
| **Browser override** | SetChromeOptions, SetEdgeOptions, GetBrowserDriverObject | `BrowserOverrideSamplesTests` (commented examples) |
| **Accessibility (Axe)** | AnalyzePage, AnalyzePageAndSaveHtml, AnalyzeCssSelector, AnalyzeElement | `ParaTests` (LoginToApp) |
| **Configuration** | appsettings.json, run settings (browser, url, TestRunParameters) | `appsettings.json`, `testrunsetting.runsettings` |
| **Parallel execution** | MSTest Parallelize (Workers, Scope) | `testrunsetting.runsettings` |

---

## MCP Agent (AI-Assisted Development)

This repo includes an **MCP (Model Context Protocol) server** for Selenium + STAF. Use it with **Cursor** or **VS Code** to:

- **Control browsers** – Start Chrome/Edge/Firefox, navigate, click, type, take screenshots
- **Generate STAF code** – Produce C# Selenium tests (Page Object Model, ReportResult, TestBaseClass)

### Quick Start

1. **Clone and open** the solution in Cursor, VS Code, or **Visual Studio** (Professional / 2022 17.14+).
2. **MCP is preconfigured** for all supported editors:
   - **Cursor / VS Code:** `.cursor/mcp.json` and `.vscode/mcp.json` point to `MCPAgent/publish/mcp-sharp-staf-selenium.exe`.
   - **Visual Studio:** repo-root `.mcp.json` is picked up automatically (GitHub Copilot → Agent mode). Get latest and open the solution; no extra config needed.
3. **Restart** the editor (or reload Copilot in VS) so the **selenium-staf** server loads.
4. Use AI to run browser automation or generate STAF-style tests from natural language.

No .NET SDK is required to run the MCP server – the exe is self-contained. See [MCPAgent/README.md](MCPAgent/README.md) for details, tool list, and troubleshooting.

---

## AI-assisted development (Cursor, VS Code, Visual Studio)

Layered instructions keep **token use low** while enforcing STAF patterns (`TestBaseClass`, `FindAppElement`, `ReportResult`, no `Thread.Sleep`).

| Layer | Path | When it loads |
|-------|------|----------------|
| **Start here** | [START_HERE.md](START_HERE.md) · [INDEX.md](INDEX.md) | Cross-platform AI setup overview |
| **Agent entry (all tools)** | [AGENTS.md](AGENTS.md) | Reference for any AI agent; golden files + token discipline |
| **Master guide** | [docs/AI_GUIDE.md](docs/AI_GUIDE.md) · [docs/QUICK_START.md](docs/QUICK_START.md) | Full workflows and task-based entry |
| **Always-on** | [.cursor/rules/staf-selenium-framework.mdc](.cursor/rules/staf-selenium-framework.mdc) (Cursor) · [.github/copilot-instructions.md](.github/copilot-instructions.md) (Copilot) | Every chat in this repo |
| **File-scoped (Cursor)** | `.cursor/rules/staf-pages.mdc`, `staf-actions.mdc`, `staf-tests.mdc` | When editing matching `STAFTests/**` files |
| **Skills (Cursor)** | [.cursor/skills/MASTER.md](.cursor/skills/MASTER.md) | On-demand workflows: UI test, API test, page/action, context loading |
| **VS custom agents** | [.github/agents/](.github/agents/) | Visual Studio Copilot: **STAF UI Automation** / **STAF API Automation** |
| **Deep context** | [docs/ai-instructions.md](docs/ai-instructions.md) · [docs/ai-index.json](docs/ai-index.json) | Attach with `@` only when generating new types |
| **Setup & prompts** | [docs/ai-setup.md](docs/ai-setup.md) · [docs/ai-prompts.md](docs/ai-prompts.md) | Onboarding and copy-paste prompts |

| Tool | Primary file | How it is applied |
|------|--------------|-------------------|
| **Cursor** | `.cursor/rules/*.mdc` + `.cursor/skills/staf-*` + [.cursor/cursor.rules](.cursor/cursor.rules) | Rules auto-apply; skills discovered from `description` or invoked by name |
| **GitHub Copilot** (VS Code) | `.github/copilot-instructions.md` · [.vscode/README.md](.vscode/README.md) | Repo instructions when workspace root is this repository |
| **GitHub Copilot** (Visual Studio) | `.github/copilot-instructions.md` + `.github/agents/*.agent.md` | Repo instructions + specialized UI/API agents in agent picker |

### Skills quick chat sheet

Use this table to pick the right workflow. **Cursor** loads project skills from `.cursor/skills/` (Agent discovers them from your message, or type **`/`** in Chat and pick the skill name). **Copilot** has no skills folder—use the same **example prompt** column with `@workspace` and optional `@` files from [docs/ai-prompts.md](docs/ai-prompts.md).

| Skill | Use when you want to… | How to invoke (Cursor) | Example chat prompt | Copilot (VS Code / VS) — same intent |
|-------|------------------------|-------------------------|---------------------|--------------------------------------|
| **`staf-ui-test`** | Add or change a **UI** `[TestMethod]` (`TestBaseClass`, Action chains, `NavigateTo`) | `/staf-ui-test` or say *"use staf-ui-test skill"* | *Add a test in ParaTests: invalid login, use Login action only, no raw WebDriver.* | `@workspace` + `@STAFTests/Actions/Login.cs` — same prompt |
| **`staf-api-test`** | Add or change an **API** test (`TestBaseAPI`, `CreateRequests`, `ReportResultAPI`) | `/staf-api-test` | *Add APITests method: GET users page 1, assert DTO, ReportResultAPI Pass/Fail.* | `@workspace` + `@STAFTests/Tests/APITests.cs` |
| **`staf-page-action`** | Create a new **Page** (`*Page`) and **Action** flow (`PageBaseClass`, `FindAppElement`, fluent returns) | `/staf-page-action` | *Create RegisterPage + Register action like LoginPage/Login; update ai-index.json.* | `@workspace` + `@STAFTests/Pages/LoginPage.cs` + `@STAFTests/Actions/Login.cs` |
| **`staf-ai-context`** | **Save tokens** — which files to attach before a big codegen task | `/staf-ai-context` or ask *"what should I @ for a new page?"* | *I need a new Parabank screen test—what files should I attach, minimum context?* | Open [AGENTS.md](AGENTS.md) golden-files table; attach **one** golden `.cs` only |
| *(no skill)* | Rules only — small edit, refactor, explain code | Nothing extra; always-on rules apply | *Refactor this method to use ReportElementIsDisplayed.* | Repo instructions auto-apply; add *Follow STAF rules…* if output drifts |
| *(no skill)* | **Deep framework** detail (parallel, reporting map, few-shots) | `@docs/ai-instructions.md` | *@docs/ai-instructions.md Add Excel test following ExcelTests pattern.* | `@docs/ai-instructions.md` in Copilot Chat |
| *(no skill)* | Find class/file before coding | `@docs/ai-index.json` | *@docs/ai-index.json Where is AccountsOverview defined?* | Same `@docs/ai-index.json` |
| **MCP + STAF** | Drive browser, then generate STAF code | Enable **selenium-staf** MCP (see [MCP Agent](#mcp-agent-ai-assisted-development)) | *Use selenium-staf to open purl, then generate TestBaseClass test + PageBaseClass page.* | MCP in VS Code/VS per [MCPAgent/README.md](MCPAgent/README.md); same prompt in agent mode |

**Token tip:** For any row above, prefer **one** golden file (`Login.cs`, `APITests.cs`, …) over attaching the whole `STAFTests` folder. More prompts: [docs/ai-prompts.md](docs/ai-prompts.md) · Full setup: [docs/ai-setup.md](docs/ai-setup.md).

### How to use them (samples)

**Cursor — Chat / Composer**

You do not need to paste the rules file. Ask for work in natural language; Cursor already applies `staf-selenium-framework.mdc`.

Example prompts:

```text
Add a new UI test method in ParaTests that logs in with invalid credentials and uses ReportResult for each step. Use LoginPage and the existing action pattern—no raw WebDriver in the test.
```

```text
Create a new page class for the Parabank Register page under Pages/. Inherit PageBaseClass, use FindAppElement for locators, and add a short summary comment at the top.
```

```text
Refactor this test to use ReportElementIsDisplayed instead of Assert.IsTrue on element.Displayed, and keep ReportResult for the scenario steps.
```

**Cursor — MCP + instructions**

When the **selenium-staf** MCP server is enabled, you can combine runtime tools with the same rules:

```text
Use selenium-staf to open Chrome, go to the URL from run settings, then generate a STAF TestBaseClass test and a Page class that match our framework (FindAppElement, ReportResult).
```

**GitHub Copilot — VS Code**

1. Open the **folder** `STAF.Selenium.Tests` (not only a single file) so `.github/copilot-instructions.md` is part of the workspace.
2. In **inline chat** (Copilot Chat) or **Copilot Edits**, describe the task; Copilot uses the repo instructions automatically.

Example prompts:

```text
@workspace Add an API test method in APITests that calls the users endpoint and uses ReportResultAPI Pass and Fail only—follow TestBaseAPI and existing patterns in this file.
```

```text
In LoginPage.cs, add a method ClickForgotLogin that uses FindAppElement and does not instantiate the driver.
```

**GitHub Copilot — Visual Studio**

Use **Copilot Chat** or **agent mode** with the solution open. Reference the same style of prompts; repository instructions apply when the Git repo root contains `.github/copilot-instructions.md`.

**Tip:** If suggestions ignore STAF patterns, remind the model explicitly: *“Follow STAF rules: TestBaseClass, FindAppElement, ReportResult, no Thread.Sleep.”*

---

## Documentation and architecture

| Document | Purpose |
|----------|---------|
| [docs/STAF-Framework-User-Guide.html](docs/STAF-Framework-User-Guide.html) | End-user guide: benefits, features, **HTML reporting**, architecture figures (Mermaid), novelty. Open in a browser or print to PDF. |
| [docs/STAF-Framework-Architecture-and-User-Guide.pdf](docs/STAF-Framework-Architecture-and-User-Guide.pdf) | Same content as the user guide, **PDF with diagrams embedded** (regenerate from HTML if needed). |
| [docs/README-PDF.md](docs/README-PDF.md) | How to regenerate the PDF (`npm install` + `npm run generate-pdf` in `docs/`). |
| [docs/STAF-Framework-Architecture-and-Technical-Innovation.md](docs/STAF-Framework-Architecture-and-Technical-Innovation.md) | Formal architecture and technical innovation write-up (academic / legal style). |
| [docs/Technical-Architecture.md](docs/Technical-Architecture.md) | Full technical architecture, MCP integration, novelty, architect checklist. |
| [docs/Architecture-Diagram.md](docs/Architecture-Diagram.md) | Mermaid diagrams: system context, MCP tools, framework structure. |
| [docs/Architecture-Summary.md](docs/Architecture-Summary.md) | One-page architecture summary. |
| [AGENTS.md](AGENTS.md) | **AI assistants:** repo-root entry — rules, golden files, token discipline (Cursor + Copilot). |
| [docs/ai-setup.md](docs/ai-setup.md) | **AI assistants:** Cursor skills, file rules, MCP, VS Code Copilot tips. |
| [docs/ai-prompts.md](docs/ai-prompts.md) | **AI assistants:** copy-paste prompts for common automation tasks. |
| [docs/ai-instructions.md](docs/ai-instructions.md) | **AI assistants:** concise framework depth, few-shots (attach with `@` only when needed). |
| [docs/ai-index.json](docs/ai-index.json) | **AI assistants:** machine-readable map (class → file, relationships, golden example paths). |

If the PDF is not in your clone, open the HTML user guide in a browser and use **Print → Save as PDF**, or run `npm install` and `npm run generate-pdf` in the `docs/` folder (see [docs/README-PDF.md](docs/README-PDF.md)).

**AI index maintenance:** After adding or renaming pages, actions, or tests, update `docs/ai-index.json` and run `powershell -File tools/UpdateAiIndex.ps1` from the repo root to validate (or `pwsh` if you use PowerShell 7). Use `-Discover` to list classes under `STAFTests/Pages`, `Actions`, `Tests`, `Requests`, and `APIData`.

---

## Release Notes

The project has been **upgraded to .NET 10**. For details (target framework change, dependency updates), see **[RELEASE_NOTES.md](RELEASE_NOTES.md)**.

---

## Prerequisites

- **.NET 10 SDK**
- **Visual Studio 2022** (or later) or **VS Code** with C# extension
- **Chrome** or **Edge** (for UI tests)
- **MSTest** (included via package reference)

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/sooraj171/STAF.Selenium.Tests
   cd STAF.Selenium.Tests
   ```

2. **Restore and build**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run settings (optional)**  
   The runsettings file is set by default: the project file points to `STAFTests\testrunsetting.runsettings`, and `.vscode/settings.json` configures it for VS Code. If you run tests and see a message that run settings may not be set, configure explicitly:
   - **VS Code:** ensure `.vscode/settings.json` has `"dotnet.unitTests.runSettingsPath"` (already set in this repo).
   - **Visual Studio:** **Test** → **Configure Run Settings** → **Select Solution Wide runsettings File** → `STAFTests\testrunsetting.runsettings`.
   - **CLI:** `dotnet test --settings STAFTests\testrunsetting.runsettings` (or rely on the project default).

4. **Run tests**
   - From IDE: **Test Explorer** → select tests → **Run**
   - From CLI: `dotnet test --settings STAFTests\testrunsetting.runsettings`

---

## Configuration

### Run settings (`STAFTests\testrunsetting.runsettings`)

- **TestRunParameters**: `browser` (e.g. `chrome`), `driverPath`, `url`, `purl` (Parabank), `searchText`, `userName`, `password`, `project`
- **MSTest**: `Parallelize` (e.g. `Workers=4`, `Scope=MethodLevel`)
- **ResultsDirectory**: e.g. `.\TestResults`

### appsettings.json (`STAFTests\appsettings.json`)

- **ConnectionStrings**: `DefaultConnection` for DbHelper (e.g. SQL Server / LocalDB)
- **Email**: SmtpHost, SmtpPort, UseDefaultCred, Username, Password (optional; for emailing results)

---

## Test Samples Overview

### UI tests (TestBaseClass)

- **GoogleSearchTest** – Page Object flow: Google search → first result → LinkedIn (ReportResult, navigation).
- **ParaTests** – Parabank: login, invalid login, About Us; **AxeAccessibility** (AnalyzePageAndSaveHtml); **ReportElement** and **ReportResult**.
- **ReportingSamplesTests** – ReportResult Pass/Fail/Warn/Info; ReportElement (Exists, IsDisplayed, IsEnabled).
- **WebDriverExtensionsSamplesTests** – WaitForDocumentReady; single-tab flow (getTotalTabsCount/CloseAllTabsExceptCurrent documented in code).
- **BrowserOverrideSamplesTests** – Default browser run; commented examples for SetChromeOptions and GetBrowserDriverObject.

### API tests (TestBaseAPI)

- **APITests** – REST (reqres.in): verify user details; **ReportResultAPI** Pass/Fail/Warn/Info sample.
- **ExcelTests** – **ExcelDriver**: CompareFiles; GetExcelWorkbook, GetExcelCellData, SetExcelCellData, GetExcelRowCount, GetExcelColumnCount.
- **DatabaseSamplesTests** – **DbHelper**: VerifyConnection, ExecuteScalar (when DefaultConnection is configured).
- **ProgrammaticReportSamplesTests** – Documented use of TestReportGenerator and TestResultData for custom HTML reports.

### Pages and actions

- **Pages**: `LoginPage`, `AboutUsPage`, `AccountsOverviewPage` (PageBaseClass + FindAppElement); `GoogleHome`, `LinkedIn` (plain POM with ReportResult).
- **Actions**: `Login`, `AboutUs`, `AccountsOverview` – orchestrate pages and report steps.

---

## Project Structure

```
STAF.Selenium.Tests/
├── README.md
├── STAF.Selenium.Tests.sln
├── .mcp.json                  # MCP config for Visual Studio (selenium-staf; source-controlled)
├── nuget.config
├── AGENTS.md                  # AI agent entry (Cursor, Copilot, others)
├── .cursor/
│   ├── mcp.json               # Cursor MCP config (selenium-staf)
│   ├── rules/                 # Always-on + file-scoped STAF rules
│   └── skills/                # On-demand UI/API/page/context workflows
├── .github/copilot-instructions.md  # Copilot repo instructions (VS Code / VS)
├── .vscode/
│   ├── mcp.json               # VS Code MCP config (selenium-staf)
│   └── settings.json          # dotnet.unitTests.runSettingsPath → runsettings (default for VS Code)
├── docs/                      # Architecture docs, user guide (HTML/PDF), PDF build (see README-PDF.md)
│   ├── STAF-Framework-User-Guide.html
│   ├── STAF-Framework-Architecture-and-User-Guide.pdf
│   ├── STAF-Framework-Architecture-and-Technical-Innovation.md
│   ├── Technical-Architecture.md
│   ├── Architecture-Diagram.md
│   ├── Architecture-Summary.md
│   └── package.json           # Optional: npm run generate-pdf
├── MCPAgent/                  # MCP server for Selenium + STAF
│   ├── README.md
│   ├── publish/               # mcp-sharp-staf-selenium.exe (self-contained)
│   └── build-mcp-agent.ps1    # Rebuild script
└── STAFTests/
    ├── STAF.Selenium.Tests.csproj    # STAF.UI.API 4.4.0, MSTest, RestSharp, etc.
    ├── appsettings.json              # ConnectionStrings, Email
    ├── testrunsetting.runsettings     # Browser, URL, parallel, TestRunParameters
    ├── AssemblyInit.cs                # AssemblyInitialize/Cleanup → HTML summary
    ├── ResultTemplate.html
    ├── Actions/                      # Login, AboutUs, AccountsOverview
    ├── APIData/                      # DTOs for API tests
    ├── Pages/                        # Page objects (PageBaseClass and plain)
    ├── Requests/                     # REST client (CreateRequests)
    ├── TestData/                     # TestDataExcel1.xlsx
    └── Tests/                        # All test classes
```

---

## Running Tests

- **All tests**: `dotnet test` (runsettings file is used by default from the project). Or explicitly: `dotnet test --settings STAFTests\testrunsetting.runsettings`
- **Filter by class**: `dotnet test --filter "FullyQualifiedName~APITests"`
- **Filter by method**: `dotnet test --filter "FullyQualifiedName~Sample_ReportResult_Pass_Fail_Warn_Info"`

After the run, check **TestResults** (or path in run settings) for **ResultTemplateFinal.html** (assembly summary) and per-test HTML reports.

---

## License

This project is licensed under the **MIT License**.

**Copyright © 2026 Sooraj Ramachandran. All rights reserved.**

**Author:** Sooraj Ramachandran

Framework and package: [STAF](https://github.com/sooraj171/STAF) | [STAF.UI.API on NuGet](https://www.nuget.org/packages/STAF.UI.API)

*This software is provided "as is", without warranty of any kind, express or implied. In no event shall the author be liable for any claim, damages or other liability arising from the use of the software.*
