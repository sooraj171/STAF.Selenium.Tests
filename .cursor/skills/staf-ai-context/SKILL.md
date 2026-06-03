---
name: staf-ai-context
description: >-
  Minimizes tokens when working in STAF.Selenium.Tests by choosing which docs and
  source files to attach. Use before large codegen, repo exploration, or when the
  user asks to reduce context or @-mentions.
---

# STAF context loading

## Default (most chats)

- **No extra files** — always-on rule + user message is enough for small edits.
- Copilot: `.github/copilot-instructions.md` | Cursor: `.cursor/rules/staf-selenium-framework.mdc`

## By task

| Task | Attach (`@`) |
|------|----------------|
| Locate symbol / file | `docs/ai/ai-index.json` only |
| New page/action/test | `ai-index.json` + **one** golden `.cs` from index `goldenExamples` |
| Framework depth (parallel, reporting map) | `docs/ai/ai-instructions.md` |
| UI test skill workflow | `.cursor/skills/staf-ui-test/SKILL.md` |
| API test skill workflow | `.cursor/skills/staf-api-test/SKILL.md` |
| Page/action skill workflow | `.cursor/skills/staf-page-action/SKILL.md` |

## Do not

- Paste entire `STAFTests/` or solution into context
- Load `docs/ai/ai-instructions.md` + all golden files together
- Read `MCPAgent/` DLL publish output for C# patterns

## After structural changes

1. Edit `docs/ai/ai-index.json`
2. `pwsh tools/UpdateAiIndex.ps1`

Human onboarding: [docs/ai/ai-setup.md](../../docs/ai/ai-setup.md) | Prompts: [docs/ai/ai-prompts.md](../../docs/ai/ai-prompts.md)
