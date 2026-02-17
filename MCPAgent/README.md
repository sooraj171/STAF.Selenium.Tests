# MCP Agent – Selenium STAF

This folder contains the **MCP (Model Context Protocol) server** for Selenium WebDriver with STAF integration. It enables AI assistants (Cursor, VS Code with Copilot, Claude Desktop) to:

1. **Control browsers** – Start Chrome/Edge/Firefox, navigate, click, type, take screenshots, etc.
2. **Generate STAF code** – Produce C# Selenium tests using the STAF framework (Page Object Model, ReportResult, TestBaseClass).

## Contents

| Item | Description |
|------|-------------|
| `publish/` | Self-contained win-x64 build. Contains `mcp-sharp-staf-selenium.exe` and dependencies. |
| `mcp-config.example.json` | Example MCP configuration for reference. |
| `build-mcp-agent.ps1` | PowerShell script to rebuild the server from source (requires mcp-sharp-staf-selenium repo). |

## Quick Start

1. **Clone and open** the STAF.Selenium.Tests solution.
2. **MCP config is already set** – `.cursor/mcp.json` and `.vscode/mcp.json` at the workspace root point to `MCPAgent/publish/mcp-sharp-staf-selenium.exe`.
3. **Restart Cursor or VS Code** so the MCP server loads.
4. The **selenium-staf** server will appear in your AI tools.

No .NET SDK is required on the machine – the exe is self-contained.

## MCP Tools

### Runtime (browser automation)
- `start_browser`, `navigate`, `find_element`, `click_element`, `send_keys`
- `get_element_text`, `hover`, `take_screenshot`, `close_session`
- And more – see [mcp-sharp-staf-selenium](https://github.com/sooraj171/mcp-sharp-staf-selenium).

### Code generation
- `GenerateSeleniumStafCode` – Full STAF scenario (pages, actions, tests)
- `GenerateSeleniumSnippet` – Single-operation snippet
- `GeneratePageObjectSkeleton` – Page Object class from element list
- `GetSeleniumGuidance` – STAF/Selenium guidance
- `GetStafSeleniumFrameworkReference` – Framework reference

## Rebuilding the Server

If you have the [mcp-sharp-staf-selenium](https://github.com/sooraj171/mcp-sharp-staf-selenium) source locally:

```powershell
.\MCPAgent\build-mcp-agent.ps1
```

Or manually:

```bash
dotnet publish C:\path\to\mcp-sharp-staf-selenium\mcp-sharp-staf-selenium\mcp-sharp-staf-selenium.csproj -c Release -r win-x64 --self-contained true -o C:\path\to\STAF.Selenium.Tests\MCPAgent\publish
```

## MCP server selection in chat

As of early 2025, Cursor does not support a dedicated keyword (e.g. `@selenium-staf`) to force use of a specific MCP server in chat. The Composer Agent automatically selects relevant MCP tools from all configured servers. To increase use of the **selenium-staf** server:

- **Be explicit in your prompt**: e.g. *"Use the selenium-staf MCP tools to generate STAF Page Object code"* or *"Generate C# Selenium tests using STAF Page Object Model – create separate Pages, Actions, and Tests files"*.
- **Reference the framework**: Mention *STAF*, *Page Object Model*, or *STAF.Selenium.Tests* so the agent is more likely to use the selenium-staf tools.
- **Use file structure keywords**: Ask to *"create Page classes in Pages/ folder and tests in Tests/ folder"* – the guidance will steer the agent toward the correct structure.

A feature request for `@mcp serverName` exists in the Cursor community; check Cursor docs for future support.

## Troubleshooting

- **Server not loading**: Restart Cursor/VS Code after cloning.
- **Path issues**: If the relative path fails, use an absolute path in `.cursor/mcp.json` or `.vscode/mcp.json`:

  ```json
  "command": "C:/repo/cursor/STAF.Selenium.Tests/MCPAgent/publish/mcp-sharp-staf-selenium.exe"
  ```

- **Chrome/Edge not starting**: Ensure Chrome or Edge is installed; Selenium 4 auto-manages drivers.
