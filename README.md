# STAF.Selenium.Tests

**STAF.Selenium.Tests** is the official sample and reference implementation for the [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) NuGet package. It demonstrates how to use [STAF](https://github.com/sooraj171/STAF) (Simple Test Automation Framework) for UI automation, API tests, Excel validation, database helpers, reporting, accessibility, and more.

Anyone using this project can see working samples of every major STAF feature and quickly adopt the framework in their own test suites.

---

## Table of Contents

- [Features Covered](#features-covered)
- [MCP Agent (AI-Assisted Development)](#mcp-agent-ai-assisted-development)
- [Technical Architecture](#technical-architecture)
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

1. **Clone and open** the solution in Cursor or VS Code.
2. **MCP is preconfigured** – `.cursor/mcp.json` and `.vscode/mcp.json` point to `MCPAgent/publish/mcp-sharp-staf-selenium.exe`.
3. **Restart** Cursor or VS Code so the **selenium-staf** server loads.
4. Use AI to run browser automation or generate STAF-style tests from natural language.

No .NET SDK is required to run the MCP server – the exe is self-contained. See [MCPAgent/README.md](MCPAgent/README.md) for details, tool list, and troubleshooting.

---

## Technical Architecture

For **technical architecture**, **MCP Agent usage**, and **novelty summary** (suitable for attorney or technical architect review), see:

- **[docs/Technical-Architecture.md](docs/Technical-Architecture.md)** – Full architecture, MCP integration, novelty, and architect checklist.
- **[docs/Architecture-Diagram.md](docs/Architecture-Diagram.md)** – Mermaid diagrams (system context, MCP tools, framework structure).
- **[docs/Architecture-Summary.md](docs/Architecture-Summary.md)** – One-page summary.

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

3. **Configure run settings** (Visual Studio)  
   **Test** → **Configure Run Settings** → **Select Solution Wide runsettings File** → choose `STAFTests\testrunsetting.runsettings`.

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
├── nuget.config
├── .cursor/mcp.json           # Cursor MCP config (selenium-staf)
├── .vscode/mcp.json           # VS Code MCP config (selenium-staf)
├── MCPAgent/                  # MCP server for Selenium + STAF
│   ├── README.md
│   ├── publish/               # mcp-sharp-staf-selenium.exe (self-contained)
│   └── build-mcp-agent.ps1    # Rebuild script
└── STAFTests/
    ├── STAF.Selenium.Tests.csproj    # STAF.UI.API 4.3.3, MSTest, RestSharp, etc.
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

- **All tests**: `dotnet test --settings STAFTests\testrunsetting.runsettings`
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
