# AI setup — Cursor & VS Code

Token-efficient guidance for **STAF.Selenium.Tests**. Full agent entry: [AGENTS.md](../../AGENTS.md).

## What loads automatically

| Editor | Always-on | On-demand |
|--------|-----------|-----------|
| **Cursor** | `.cursor/rules/staf-selenium-framework.mdc` | File rules when editing matching paths; `@docs/ai/ai-instructions.md`; skills below |
| **VS Code / VS (Copilot)** | `.github/copilot-instructions.md` | `@docs/ai/ai-instructions.md`, `@docs/ai/ai-index.json`, golden `.cs` files |

Open the **repo root** as the workspace folder so paths resolve.

## Cursor skills (project)

Skills live in `.cursor/skills/`. Cursor discovers them from the `description` field; you can also type `/` and the skill name.

| Skill | Use when |
|-------|----------|
| `staf-ui-test` | New or changing UI `[TestMethod]` |
| `staf-api-test` | REST / `TestBaseAPI` tests |
| `staf-page-action` | New `*Page` or `Actions/*` flow |
| `staf-ai-context` | Choosing which files to attach (`@`) to save tokens |

Each skill links to a small `reference.md` with copy-paste templates.

## File-scoped rules (Cursor only)

| Rule | Applies when editing |
|------|----------------------|
| `staf-pages.mdc` | `STAFTests/Pages/**/*.cs` |
| `staf-actions.mdc` | `STAFTests/Actions/**/*.cs` |
| `staf-tests.mdc` | `STAFTests/Tests/**/*.cs` |

These add **deltas** on top of the always-on framework rule (no duplicate wall of text).

## MCP (browser + codegen)

1. Restart editor after clone.
2. Confirm **selenium-staf** in MCP panel (config: `.cursor/mcp.json` or `.vscode/mcp.json`).
3. Combine MCP with instructions: *"Use selenium-staf to open purl, then generate a TestBaseClass test using Login action pattern."*

## VS Code Copilot tips

- Use **@workspace** for repo-wide questions; attach **one** golden file for codegen (e.g. `@STAFTests/Actions/Login.cs`).
- If output drifts to raw Selenium: *"Follow STAF: TestBaseClass, FindAppElement, ReportResult, no Thread.Sleep."*
- Repository instructions path: `.github/copilot-instructions.md` (mirrors Cursor always-on rule).

## Maintaining the index

```powershell
pwsh tools/UpdateAiIndex.ps1          # validate ai-index.json
pwsh tools/UpdateAiIndex.ps1 -Discover  # list classes to add to index
```

Update `docs/ai/ai-index.json` when you add pages, actions, tests, requests, or DTOs.
