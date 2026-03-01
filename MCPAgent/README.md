# MCP Sharp STAF Selenium – Extension

Self-contained MCP server build for easy reuse. Copy this **extension** folder into your test project and reference the exe from your MCP configuration. The server uses **stdio** (stdin/stdout) as the transport—no network port or URL.

## Contents

- **publish/** – Published build (win-x64, self-contained, .NET 10). Populated by running the rebuild script or publish command below.
  - **mcp-sharp-staf-selenium.exe** – MCP server; launch this exe and the MCP client communicates via stdio.

## Usage

### 1. Copy extension into your project

Copy the entire `extension` folder into your test project root:

```
YourTestProject/
├── extension/
│   └── publish/
│       └── mcp-sharp-staf-selenium.exe
├── YourTests.csproj
└── ...
```

### 2. Add MCP configuration

Point your MCP client to the exe. The server uses **stdio** transport (stdin/stdout).

**Cursor** – Add to `.cursor/mcp.json` or Cursor Settings → MCP:

```json
{
  "mcpServers": {
    "selenium-staf": {
      "command": "extension/publish/mcp-sharp-staf-selenium.exe",
      "args": []
    }
  }
}
```

**Visual Studio (Professional / 2022 17.14+)** – This repo includes a root-level **`.mcp.json`** so Visual Studio discovers the MCP server automatically when you open the solution (GitHub Copilot → Agent mode). No extra setup needed after get latest. If the agent does not start:

- Ensure the solution is opened from the repo root (folder that contains `STAF.Selenium.Tests.sln` and `MCPAgent/`).
- In Visual Studio: **GitHub Copilot Chat** → mode **Agent** → enable the **selenium-staf** tools when prompted.
- Optional: add or merge the same server into `%USERPROFILE%\.mcp.json` for a user-wide config, or into `.vs/mcp.json` (repo `.vs` folder, user-specific) if you need a different path (e.g. absolute path to the exe).

**Claude Desktop** – Add to `claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "selenium-staf": {
      "command": "extension/publish/mcp-sharp-staf-selenium.exe",
      "args": []
    }
  }
}
```

Use an absolute path if needed:

```json
"command": "C:/path/to/YourTestProject/extension/publish/mcp-sharp-staf-selenium.exe"
```

## Rebuilding (get latest from main project)

To refresh `extension/publish` with the latest build from `mcp-sharp-staf-selenium.csproj`:

**From repo root:**

```powershell
.\extension\rebuild.ps1
```

Or with cmd:

```cmd
extension\rebuild.cmd
```

Or manually:

```bash
dotnet publish mcp-sharp-staf-selenium/mcp-sharp-staf-selenium.csproj -c Release -r win-x64 --self-contained true -o extension/publish
```

Requires .NET 10 SDK. After rebuilding, copy the updated `extension` folder (including `publish/`) into your test project.
