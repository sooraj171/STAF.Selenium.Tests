# STAF Framework: Architecture and Technical Innovation

**Document Title:** STAF Framework — Architecture Description and Technical Innovations  
**Document Version:** 4.4.0  
**Last Updated:** February 2026  
**Classification:** Technical and Architectural Documentation  
---

## 1. Purpose and Scope

This document provides a formal description of the **STAF (Simple Test Automation Framework)** architecture and the technical innovations it introduces. It is structured to support:

- **Academic evaluation** of the framework’s design, novelty, and contribution to test automation practice.
- **Legal and compliance review** of system boundaries, data flow, and distinctive aspects relevant to intellectual property or contractual assessment.

The framework is distributed as the **STAF.UI.API** NuGet package ([NuGet Gallery — STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API)) and is documented in the **STAF** repository ([GitHub — sooraj171/STAF](https://github.com/sooraj171/STAF)). The reference implementation and MCP Agent integration are provided by the **STAF.Selenium.Tests** project ([GitHub — STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests)).

---

## 2. Framework Overview

### 2.1 Definition and Positioning

**STAF** is a production-oriented .NET test automation framework for:

- **UI automation** — Selenium WebDriver–based testing with a Page Object Model and structured reporting.
- **API automation** — REST-style tests with the same reporting and lifecycle as UI tests.
- **Supplementary capabilities** — Excel comparison and validation (ClosedXML), SQL Server helpers (DbHelper), and accessibility scanning (Deque Axe-core).

The framework targets **.NET 10** and uses **MSTest** as the test platform. It is delivered as a single NuGet package, **STAF.UI.API**, which encapsulates base classes, reporting, browser management, and optional integrations (Excel, database, accessibility).

### 2.2 Principal Capabilities (Summary)

| Capability | Description |
|------------|-------------|
| **Base classes** | TestBaseClass (UI), TestBaseAPI (API/Excel/DB), PageBaseClass (element location and waits). |
| **Browser support** | Chrome, Edge; local or remote WebDriver; overridable options and driver creation. |
| **HTML reporting** | Per-test step reporting (Pass/Fail/Warn/Info) and assembly-level summary (e.g., ResultTemplateFinal.html). |
| **Parallel execution** | Parallel-safe result accumulation; MSTest parallelization (e.g., worker count, method-level scope). |
| **Excel** | Workbook/sheet comparison; get/set cell data; row/column counts via ClosedXML. |
| **Database** | DbHelper: connection strings from configuration; execute query, scalar, and non-query operations. |
| **Accessibility** | Axe-core integration: full-page and scoped scans with configurable rules and HTML reports. |
| **Configuration** | appsettings.json and run settings (browser, driver path, URL, test parameters). |

This consolidated stack distinguishes STAF from ad hoc Selenium scripts and from frameworks that address only UI or only API testing without a unified reporting and lifecycle model.

---

## 3. System Architecture

### 3.1 Architectural Layers

The system is composed of the following layers, with clear separation of responsibilities:

| Layer | Responsibility |
|-------|----------------|
| **User / Developer** | Issues natural-language requests to the AI (e.g., “Generate a STAF login test” or “Open Chrome and navigate to URL”). |
| **AI-Enabled IDE** | Hosts the AI model (e.g., Cursor Composer) and the MCP client; invokes the MCP Agent per editor configuration. |
| **MCP Agent** | Exposes two tool categories: (1) **runtime** — control a browser via Selenium; (2) **code-generation** — produce STAF-conformant C# (Pages, Actions, Tests). Communicates with the IDE over the Model Context Protocol (JSON-RPC over stdio). |
| **Browser** | Started and controlled by the MCP Agent when runtime tools are used; not required for code-generation tools. |
| **STAF Test Project** | MSTest project consuming STAF.UI.API; contains Pages, Actions, and Tests; executes via `dotnet test` or the IDE; does not depend on the MCP Agent at build or run time. |

### 3.2 High-Level Data Flow

1. The **user** submits a prompt in the IDE.
2. The **IDE (AI + MCP client)** may call **tools** provided by the **MCP Agent**.
3. The **MCP Agent** runs as a **separate process**, started by the IDE via configuration (e.g., `.cursor/mcp.json`, `.vscode/mcp.json`), and communicates over **stdio** using the Model Context Protocol.
4. **Runtime tools:** The Agent can drive a browser (start, navigate, click, type, screenshot) via Selenium WebDriver.
5. **Code-generation tools:** The Agent returns **STAF-conformant** C# (e.g., PageBaseClass, FindAppElement, ReportResult, TestBaseClass). The AI/IDE may then create or edit files in the repository.
6. **Test execution:** The STAF test project runs under MSTest independently of the MCP Agent. The Agent does not execute tests.

Diagrams and a more detailed component view are in [Architecture-Diagram.md](Architecture-Diagram.md) and [Technical-Architecture.md](Technical-Architecture.md).

### 3.3 MCP Agent Integration

The **MCP (Model Context Protocol) Agent** (“selenium-staf”) is:

- A **separate executable** (e.g., `mcp-sharp-staf-selenium.exe`), supplied in the STAF.Selenium.Tests repository.
- **Optional:** The test framework builds and runs without it.
- **Configurable** via `.cursor/mcp.json` or `.vscode/mcp.json`; no additional SDK is required to run the Agent.
- **Bounded:** It does not execute the test suite, does not access source code except through the code it returns to the AI, and does not store or train on user data.

This design supports both technical audit (clear boundaries, standard protocol) and compliance review (no implicit data collection or test execution by the Agent).

---

## 4. Technical Innovations and Novelty

The following subsections describe the **distinctive technical contributions** of the framework. They are formulated so that:

- A **professor or technical reviewer** can assess architectural and methodological novelty.
- An **attorney or compliance officer** can understand scope and boundaries for IP or contractual purposes.

### 4.1 Dual-Purpose MCP Server: Runtime Automation and Framework-Aware Code Generation

**Innovation:** A **single** MCP server provides both:

1. **Runtime browser automation tools** — start browser, navigate, click, type, capture screenshots, etc., via Selenium WebDriver.
2. **Framework-aware code-generation tools** — generation of C# that conforms to STAF patterns (Page Object Model, ReportResult, ReportElement, TestBaseClass, folder layout).

**Significance:** The AI assistant can alternate between “drive the application in a browser” and “generate tests that conform to the project’s framework” without changing servers or context. This supports a unified workflow: explore in the browser, then generate conformant code from the same MCP endpoint. To the best of the author’s knowledge, this combination—one MCP server offering both runtime Selenium control and framework-specific test code generation—is not commonly provided together in open-source or commercial Selenium/.NET offerings as a single, out-of-box integration.

**Reference:** [NuGet — STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API); [GitHub — STAF](https://github.com/sooraj171/STAF); [Architecture-Summary.md](Architecture-Summary.md).

---

### 4.2 STAF-Specific Code Generation (Non-Generic Output)

**Innovation:** Code-generation tools do not produce generic Selenium snippets. They produce **STAF-specific** artifacts:

- **Page classes** using `PageBaseClass` and `FindAppElement(By)` / `FindAppElement(parent, By, description)`.
- **Action classes** using `ReportResult.ReportResultPass` / `ReportResultFail` and `ReportElement` extensions (e.g., `ReportElementIsDisplayed`).
- **Test classes** inheriting `TestBaseClass`, using the base `driver` and `TestContext`, and adhering to the project layout (e.g., Pages/, Actions/, Tests/).

**Significance:** Generated code integrates directly with the existing framework: HTML reporting, assembly summary, and parallel execution. This reduces manual refactoring and preserves consistent patterns. The novelty lies in the **targeting** of code generation to a specific framework’s conventions and APIs, rather than generic Selenium or C# snippets.

**Reference:** [Technical-Architecture.md](Technical-Architecture.md) § 4.2; STAF.UI.API key components (PageBaseClass, ReportResult, ReportElement).

---

### 4.3 Out-of-Box AI Integration

**Innovation:** The reference implementation (STAF.Selenium.Tests) ships with:

1. **Preconfigured MCP** — `.cursor/mcp.json` and `.vscode/mcp.json` point to the included Agent executable.
2. **Self-contained MCP Agent** — A published executable in `MCPAgent/publish/` that runs without requiring a .NET SDK on the host.
3. **No mandatory extra SDK** — The developer clones the repository, opens it in Cursor or VS Code, and can use AI-driven browser control and STAF code generation without additional setup.

**Significance:** The “MCP + STAF” integration is a **first-class, reproducible** part of the framework’s distribution. Adoption cost is lowered and the integration is versioned with the repository (same commit includes framework and Agent binary). This contrasts with approaches that require separate installation or custom integration work.

**Reference:** [Architecture-Summary.md](Architecture-Summary.md); [Technical-Architecture.md](Technical-Architecture.md) § 4.3.

---

### 4.4 Separation of Concerns: Test Framework vs. MCP Agent

**Innovation:** The MCP Agent is:

- A **separate process** — invoked by the IDE, not by the test runner.
- A **separate codebase** — developed and built independently (e.g., mcp-sharp-staf-selenium repository).
- **Not a build or runtime dependency** of the STAF test project — tests run via MSTest with or without the IDE or MCP.

**Significance:** From an architectural and compliance perspective:

- The test framework remains a standard .NET/MSTest project; MCP is **additive and optional**.
- Data sent to the Agent is limited to what the user explicitly requests (e.g., “navigate to this URL,” “generate a login test”).
- The protocol (MCP over stdio) is well-defined and auditable; the Agent does not execute tests or read arbitrary files.

**Reference:** [Technical-Architecture.md](Technical-Architecture.md) § 4.4, § 3.2.

---

### 4.5 Unified Stack: UI, API, Reporting, and Accessibility

**Innovation:** The same solution and NuGet package provide:

- **TestBaseClass** (UI) and **TestBaseAPI** (API/Excel/DB).
- **ReportResult** and **ReportResultAPI** for step-level reporting.
- **ReportElement** for assertion-and-report in one call.
- **HTML reporting** (per-test and assembly summary via AssemblyInit).
- **Axe accessibility** (e.g., AnalyzePage, AnalyzePageAndSaveHtml, scoped and configurable scans).

The MCP code-generation tools are designed to produce code that fits this stack (e.g., ReportResult in Actions, TestBaseClass in Tests).

**Significance:** The novelty is not only “MCP + Selenium” but **MCP + a full STAF stack** with consistent patterns and reporting across UI, API, and accessibility. This enables AI-assisted development across multiple test types within one framework and one repository.

**Reference:** [NuGet — STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) (features and key components); [GitHub — STAF](https://github.com/sooraj171/STAF).

---

## 5. Key Components and Design Rationale

### 5.1 Test and Page Base Classes

| Class | Purpose |
|-------|---------|
| **TestBaseClass** | Base for UI tests: initializes WebDriver from TestContext/run settings, sets up per-test HTML result file, cleans up and contributes to assembly summary. |
| **TestBaseAPI** | Base for API (and Excel/DB) tests: same lifecycle and reporting as UI tests, without a browser. |
| **PageBaseClass** | Wraps element location with wait: FindAppElement(By), FindAppElement(By, description), FindAppElement(parent, By, description). |

**Design rationale:** A single abstraction for element lookup and waiting ensures consistent behavior and reporting; tests and pages do not instantiate the driver directly, preserving a single point of control and cleanup.

### 5.2 Reporting and Assertions

| Component | Purpose |
|-----------|---------|
| **ReportResult** | Step-level reporting for UI tests: ReportResultPass/Fail/Warn/Info(driver, TestContext, moduleName, description [, exception]). |
| **ReportResultAPI** | Same for API tests (no driver parameter). |
| **ReportElement** (extensions) | Assert and report in one call: ReportElementExists, ReportElementIsDisplayed, ReportElementIsEnabled (with optional proceed-on-fail). |

**Design rationale:** Assertions and reporting are combined so that failures are both validated and recorded in the HTML report without duplicate code.

### 5.3 Browser and Driver

**BrowserDriver** creates IWebDriver for Chrome or Edge, local or remote. SetChromeOptions(), SetEdgeOptions(), or GetBrowserDriverObject() can be overridden to customize behavior. The framework does not require tests or pages to construct the driver, supporting consistent configuration and lifecycle management.

---

## 6. Integration, Distribution, and Reproducibility

- **Distribution:** The framework is distributed as **STAF.UI.API** on NuGet ([STAF.UI.API 4.4.0](https://www.nuget.org/packages/STAF.UI.API)); the sample project and MCP Agent are in **STAF.Selenium.Tests** ([GitHub](https://github.com/sooraj171/STAF.Selenium.Tests)).
- **Reproducibility:** MCP configuration and the published Agent executable are included in the repository; a build script (e.g., `MCPAgent/build-mcp-agent.ps1`) documents how to rebuild the Agent.
- **License:** The project is licensed under the **MIT License**. Author: Sooraj Ramachandran. Copyright (c) 2026 Sooraj Ramachandran. All rights reserved.

---

## 7. Summary for Review

The following table summarizes points that a **technical reviewer (e.g., professor)** or **legal/compliance reviewer (e.g., attorney)** may wish to verify.

| Criterion | Status | Notes |
|-----------|--------|--------|
| **Architecture clarity** | Met | Layered design; user → IDE → MCP Agent → browser / generated code → framework. Diagrams in Architecture-Diagram.md and Technical-Architecture.md. |
| **MCP usage** | Met | MCP Agent is a separate process; JSON-RPC over stdio; no build/run-time coupling between STAF tests and MCP. |
| **Framework consistency** | Met | Generated code follows PageBaseClass, FindAppElement, ReportResult, ReportElement, TestBaseClass, and folder layout. |
| **Reproducibility** | Met | MCP config and published Agent are in the repo; build script for the Agent is provided. |
| **Security / boundaries** | Met | Agent does not execute tests or read arbitrary files; it only responds to tool invocations (browser control or code generation). |
| **Novelty** | Described | Single MCP server for runtime + STAF-aware code generation (§ 4.1); STAF-specific code output (§ 4.2); out-of-box IDE integration (§ 4.3); separation of test framework and Agent (§ 4.4); unified UI/API/reporting/accessibility stack (§ 4.5). |

---

## 8. References

| Reference | URL or Location |
|-----------|-----------------|
| STAF (GitHub) | [https://github.com/sooraj171/STAF](https://github.com/sooraj171/STAF) |
| STAF.UI.API (NuGet) | [https://www.nuget.org/packages/STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API) |
| STAF.Selenium.Tests (sample + MCP) | [https://github.com/sooraj171/STAF.Selenium.Tests](https://github.com/sooraj171/STAF.Selenium.Tests) |
| Architecture Diagram | [Architecture-Diagram.md](Architecture-Diagram.md) |
| Architecture Summary | [Architecture-Summary.md](Architecture-Summary.md) |
| Technical Architecture | [Technical-Architecture.md](Technical-Architecture.md) |
| Model Context Protocol | [modelcontextprotocol.io](https://modelcontextprotocol.io/) (specification) |

---

*This document is part of the STAF.Selenium.Tests project. For questions or updates, refer to the repository maintainers.*
