# STAF.Selenium.Tests

**STAF.Selenium.Tests** is the official sample and reference implementation for the [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) NuGet package. It demonstrates how to use [STAF](https://github.com/sooraj171/STAF) (Simple Test Automation Framework) for UI automation, API tests, Excel validation, database helpers, reporting, accessibility, and more.

Anyone using this project can see working samples of every major STAF feature and quickly adopt the framework in their own test suites.

---

## Table of Contents

- [Features Covered](#features-covered)
- [MCP Agent (AI-Assisted Development)](#mcp-agent-ai-assisted-development)
- [Cursor rules and GitHub Copilot instructions](#cursor-rules-and-github-copilot-instructions)
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

## Cursor rules and GitHub Copilot instructions

This repo ships **two instruction files** that tell AI assistants to follow STAF patterns (base classes, `FindAppElement`, `ReportResult`, no raw `Thread.Sleep`, Page Object workflow). They mirror each other so behavior stays consistent across tools.

| Tool | File | How it is applied |
|------|------|-------------------|
| **Cursor** | [.cursor/rules/staf-selenium-framework.mdc](.cursor/rules/staf-selenium-framework.mdc) | Cursor loads rules from `.cursor/rules/` for this workspace. This rule is set to **always apply** (`alwaysApply: true` in the front matter), so Chat, Composer, and Agent use it without you naming the file. |
| **GitHub Copilot** (VS Code / Visual Studio) | [.github/copilot-instructions.md](.github/copilot-instructions.md) | Copilot reads **repository instructions** from this path when you work inside this repo. Keep the repo open as the workspace root so Copilot picks it up. |

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
| [docs/ai-instructions.md](docs/ai-instructions.md) | **AI assistants:** concise framework rules, structure, few-shots, token discipline (use with `@` in Cursor). |
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
├── .cursor/mcp.json           # Cursor MCP config (selenium-staf)
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
