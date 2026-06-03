# STAF.Selenium.Tests – Architecture Summary (One-Page)

**Full detail:** [details/Technical-Architecture.md](details/Technical-Architecture.md) | **Diagrams:** [details/Architecture-Diagram.md](details/Architecture-Diagram.md) | **Formal write-up:** [details/STAF-Framework-Architecture-and-Technical-Innovation.md](details/STAF-Framework-Architecture-and-Technical-Innovation.md)

---

## What This Is

- **STAF.Selenium.Tests** = A Selenium-based test automation framework (STAF) that runs UI tests, API tests, Excel/DB checks, and accessibility (Axe), with HTML reporting.
- **MCP Agent** = A separate, optional program (Model Context Protocol server) that runs inside AI-enabled editors (Cursor, VS Code). It lets the AI **control a browser** and **generate test code** that follows the framework’s conventions.

---

## How MCP Is Used

| Step | What Happens |
|------|----------------|
| 1 | Developer opens the repo in Cursor or VS Code. |
| 2 | Editor reads `.cursor/mcp.json` or `.vscode/mcp.json` and starts `MCPAgent/publish/mcp-sharp-staf-selenium.exe`. |
| 3 | Editor talks to this process over the **Model Context Protocol** (JSON-RPC over stdio). |
| 4 | When the user asks the AI to “open Chrome and take a screenshot” or “generate a STAF login test,” the AI can call **tools** provided by this process. |
| 5 | The MCP Agent either (a) drives a browser via Selenium, or (b) returns STAF-style C# code. The AI/editor may then create or edit files in the repo. |

The MCP Agent **does not** run tests, train on user data, or access source code except via the code it returns to the AI. The test framework runs independently via MSTest (`dotnet test` or IDE).

---

## Novelty (Distinctive Aspects)

1. **One MCP server, two roles:** Same server provides **browser automation** and **framework-aware code generation** (STAF Pages, Actions, Tests, ReportResult, FindAppElement).
2. **STAF-specific code generation:** Generated code matches the framework (PageBaseClass, ReportResult, TestBaseClass, folder layout), not generic Selenium snippets.
3. **Out-of-box integration:** Repo includes MCP config and a self-contained Agent exe; no extra SDK; works immediately in Cursor/VS Code.
4. **Clear separation:** MCP Agent is a separate process and codebase; the test project does not depend on it at build or test run time.

---

## Diagram (High Level)

```
User → IDE (AI + MCP client) → MCP Agent (selenium-staf)
                                    ├→ Browser (Selenium WebDriver)
                                    └→ Generated C# → STAF project (Pages/, Actions/, Tests/)
```

---

## Technical Architect Checklist

- **Architecture:** Documented with components and data flow; diagram in [Architecture-Diagram.md](Architecture-Diagram.md).
- **MCP:** Standard protocol (JSON-RPC over stdio); Agent is optional and separate from test execution.
- **Generated code:** Aligns with STAF patterns (PageBaseClass, ReportResult, TestBaseClass, ReportElement).
- **Reproducibility:** MCP config and published exe are in the repo; build script for Agent is in `MCPAgent/build-mcp-agent.ps1`.

---

*For full architecture, MCP tool list, and legal/technical review text, see [Technical-Architecture.md](Technical-Architecture.md).*
