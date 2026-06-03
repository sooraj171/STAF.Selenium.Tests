# Documentation

Reference material for **STAF.Selenium.Tests** — the sample solution for [STAF.UI.API](https://www.nuget.org/packages/STAF.UI.API).

## User guide

| Resource | Description |
|----------|-------------|
| [STAF-Framework-User-Guide.html](STAF-Framework-User-Guide.html) | Framework overview, reporting, and architecture (open in a browser) |
| [STAF-Framework-Architecture-and-User-Guide.pdf](STAF-Framework-Architecture-and-User-Guide.pdf) | Same content as PDF |
| [Architecture-Summary.md](Architecture-Summary.md) | One-page architecture and MCP overview |

## AI-assisted test development

Use these when generating tests with Cursor, GitHub Copilot, or Visual Studio agents.

| Resource | Description |
|----------|-------------|
| [ai/QUICK_START.md](ai/QUICK_START.md) | Task-based entry (UI test, page/action, API test) |
| [ai/AI_GUIDE.md](ai/AI_GUIDE.md) | Full patterns, templates, and checklists |
| [ai/ai-setup.md](ai/ai-setup.md) | Editor setup (Cursor skills, MCP, Copilot) |
| [ai/ai-prompts.md](ai/ai-prompts.md) | Copy-paste prompts |
| [ai/ai-instructions.md](ai/ai-instructions.md) | Deep framework notes (attach only when needed) |
| [ai/ai-index.json](ai/ai-index.json) | Class → file map for agents |

Repo entry points: [AGENTS.md](../AGENTS.md) · [.github/copilot-instructions.md](../.github/copilot-instructions.md) · [.github/agents/](../.github/agents/)

After adding pages, actions, or tests: `pwsh tools/UpdateAiIndex.ps1`

## Extended reference

Maintainer and deep-dive material: [details/](details/)
