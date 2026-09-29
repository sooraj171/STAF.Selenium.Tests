# VS Code

## Test run settings

`settings.json` points unit tests at `STAFTests/testrunsetting.runsettings`.

## GitHub Copilot

Repository instructions load from [.github/copilot-instructions.md](../.github/copilot-instructions.md).

For STAF test patterns and prompts, see [docs/README.md](../docs/README.md) and [docs/ai/QUICK_START.md](../docs/ai/QUICK_START.md).

## MCP

[.vscode/mcp.json](mcp.json) starts two stdio servers. See [MCPAgent/README.md](../MCPAgent/README.md).

| Server | Executable |
|--------|------------|
| **selenium-staf** | `MCPAgent/publish/mcp-sharp-staf-selenium.exe` — browser automation and STAF codegen |
| **azure-devops** | `MCPAgent/AzureDevOps/AzureDevOps.Mcp.Server.exe` — prompts for the organization (`ado_org`) and uses interactive sign-in |

Optional `ado_mcp_project` and `ado_mcp_team` values in that file limit the Azure DevOps server to one project or team.
