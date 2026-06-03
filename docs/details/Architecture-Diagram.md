# STAF.Selenium.Tests – Architecture Diagram

This file contains the **high-level architecture diagram** for the STAF.Selenium.Tests framework and MCP Agent integration. It can be rendered in any Markdown viewer that supports Mermaid (e.g., GitHub, VS Code with Mermaid extension, or [mermaid.live](https://mermaid.live)).

---

## System Context: User → IDE → MCP Agent → Browser / Framework

```mermaid
flowchart TB
    subgraph User["User / Developer"]
        Prompt["Natural language prompt\n(e.g. 'Generate STAF login test'\nor 'Open Chrome and go to URL')"]
    end

    subgraph IDE["AI-Enabled IDE (Cursor / VS Code)"]
        LLM["AI Model (e.g. Composer)"]
        MCPClient["MCP Client"]
    end

    subgraph MCPAgent["MCP Agent (selenium-staf)"]
        direction TB
        Runtime["Runtime tools\n(start_browser, navigate,\nclick_element, send_keys,\ntake_screenshot, close_session)"]
        CodeGen["Code-generation tools\n(GenerateSeleniumStafCode,\nGeneratePageObjectSkeleton,\nGetStafSeleniumFrameworkReference)"]
    end

    subgraph RuntimeEnv["Runtime Environment"]
        Browser["Browser\n(Chrome / Edge / Firefox)"]
    end

    subgraph Framework["STAF.Selenium.Tests Framework"]
        Tests["Tests/\n(TestBaseClass)"]
        Pages["Pages/\n(PageBaseClass, FindAppElement)"]
        Actions["Actions/\n(ReportResult, ReportElement)"]
        AssemblyInit["AssemblyInit\n(HTML reporting)"]
    end

    Prompt --> LLM
    LLM --> MCPClient
    MCPClient -->|"JSON-RPC over stdio"| MCPAgent
    MCPAgent -->|"Selenium WebDriver"| Browser
    MCPAgent -->|"Generated C# files"| Framework
    LLM -->|"Edits / creates files"| Framework
    Tests --> Pages
    Tests --> Actions
    Pages --> Actions
    AssemblyInit --> Tests
```

---

## MCP Agent Internal View (Tools)

```mermaid
flowchart LR
    subgraph MCPAgent["MCP Agent Process"]
        MCPProtocol["MCP Protocol\n(JSON-RPC stdio)"]
        Runtime["Runtime Tools"]
        CodeGen["Code-Gen Tools"]
    end

    subgraph RuntimeTools["Runtime Tools"]
        T1["start_browser"]
        T2["navigate"]
        T3["click_element"]
        T4["send_keys"]
        T5["take_screenshot"]
        T6["close_session"]
    end

    subgraph CodeGenTools["Code-Gen Tools"]
        G1["GenerateSeleniumStafCode"]
        G2["GeneratePageObjectSkeleton"]
        G3["GenerateSeleniumSnippet"]
        G4["GetStafSeleniumFrameworkReference"]
    end

    MCPProtocol --> Runtime
    MCPProtocol --> CodeGen
    Runtime --> RuntimeTools
    CodeGen --> CodeGenTools
    RuntimeTools --> Selenium["Selenium WebDriver"]
    CodeGenTools --> Output["STAF C# code"]
```

---

## STAF Framework Structure (Test Project)

```mermaid
flowchart TB
    subgraph STAFTests["STAFTests Project"]
        AssemblyInit["AssemblyInit\n(AssemblyInitialize/Cleanup)\n→ HTML summary"]
        TestBase["TestBaseClass / TestBaseAPI"]
        Tests["Tests/\nParaTests, GoogleSearchTest,\nAPITests, ExcelTests, ..."]
        Pages["Pages/\nLoginPage, AboutUsPage,\nGoogleHome, ..."]
        Actions["Actions/\nLogin, AboutUs,\nAccountsOverview"]
    end

    subgraph STAFUIAPI["STAF.UI.API (NuGet)"]
        PageBase["PageBaseClass\nFindAppElement"]
        Report["ReportResult\nReportElement"]
        Reporting["HTML reporting\nExcelDriver, DbHelper\nAxe accessibility"]
    end

    AssemblyInit --> Tests
    TestBase --> Tests
    Tests --> Pages
    Tests --> Actions
    Pages --> PageBase
    Actions --> Report
    Tests --> Reporting
```

---

## Summary

| Layer | Responsibility |
|-------|----------------|
| **User** | Asks the AI (in natural language) to automate the browser or generate STAF tests. |
| **IDE (Cursor/VS Code)** | Runs the AI model and MCP client; starts the MCP Agent via `.cursor/mcp.json` / `.vscode/mcp.json`. |
| **MCP Agent** | Exposes tools: (1) runtime—control browser via Selenium; (2) code-gen—return STAF-conformant C#. Communicates with IDE over MCP (JSON-RPC over stdio). |
| **Browser** | Started and controlled by the MCP Agent when runtime tools are used. |
| **STAF.Selenium.Tests** | MSTest project using STAF.UI.API; receives generated or hand-written Pages, Actions, and Tests; runs via `dotnet test` or IDE. |

For full technical architecture and novelty, see [Technical-Architecture.md](Technical-Architecture.md).
