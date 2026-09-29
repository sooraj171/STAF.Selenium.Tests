# MCP servers

This folder ships two local MCP servers. Both use **stdio** (stdin/stdout). No port or URL is required. This repo already points Cursor, VS Code, and Visual Studio at them.

| Server | Executable | Role |
|--------|------------|------|
| **selenium-staf** | `MCPAgent/publish/mcp-sharp-staf-selenium.exe` | Browser automation and STAF-aware code generation |
| **azure-devops** | `MCPAgent/AzureDevOps/AzureDevOps.Mcp.Server.exe` | Azure DevOps tools (work items, repos, pipelines, and the other domains enabled by `-d all`) |

## Checked-in configuration

| Editor | File |
|--------|------|
| Cursor | [.cursor/mcp.json](../.cursor/mcp.json) |
| VS Code | [.vscode/mcp.json](../.vscode/mcp.json) |
| Visual Studio | [.mcp.json](../.mcp.json) |

Open the solution from the repo root (the folder that contains `STAF.Selenium.Tests.sln` and `MCPAgent/`).

**selenium-staf** (all three files):

```json
"command": "MCPAgent/publish/mcp-sharp-staf-selenium.exe"
```

**azure-devops** prompts for the organization (`ado_org`), signs in interactively, and enables every domain:

```json
"command": "MCPAgent/AzureDevOps/AzureDevOps.Mcp.Server.exe",
"args": ["${input:ado_org}", "-a", "interactive", "-d", "all"]
```

Optional environment values in the same config scope the server to one project or team. Leave them empty to choose later:

- `ado_mcp_project`
- `ado_mcp_team`

### Cursor

Config is in `.cursor/mcp.json` (`mcpServers`). On first use, approve **selenium-staf** and **azure-devops**, and enter the Azure DevOps organization when prompted (for example `contoso`).

### VS Code

Config is in `.vscode/mcp.json` (`servers`). Start or enable both servers from the MCP panel. The `ado_org` input asks for the organization name.

### Visual Studio (Copilot Agent mode)

Config is in the repo-root `.mcp.json` (`servers`). GitHub Copilot discovers it when the solution is opened from the repo root.

- **GitHub Copilot Chat** → mode **Agent** → enable **selenium-staf** and **azure-devops** when prompted.
- Optional: merge the same servers into `%USERPROFILE%\.mcp.json` for a user-wide config, or into `.vs/mcp.json` if you need an absolute path to an exe.

## Using the servers in another project

Copy `MCPAgent/publish/` for **selenium-staf**, and `MCPAgent/AzureDevOps/` for **azure-devops**. Point `command` at those exes. Use an absolute path when the client does not run from the repo root:

```json
"command": "C:/path/to/YourTestProject/MCPAgent/publish/mcp-sharp-staf-selenium.exe"
```

Example fragments:

- [mcp-config.example.json](mcp-config.example.json) — selenium-staf
- [AzureDevOps/mcp-config.example.json](AzureDevOps/mcp-config.example.json) — azure-devops

## What is not in this folder

The **selenium-staf** source lives in [mcp-sharp-staf-selenium](https://github.com/sooraj171/mcp-sharp-staf-selenium). This repo includes the published Windows executable only. There is no rebuild script here; replace `MCPAgent/publish/` with a new publish output when you rebuild that project.

The test project does not reference either server at build or `dotnet test` time.
