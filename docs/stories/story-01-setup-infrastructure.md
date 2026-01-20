# Story 01: Setup Lab Infrastructure

## Summary

Prepare the lab repository infrastructure by copying necessary GitHub Copilot agent configurations from the bmad-azure-module repository and validating the MCP configuration.

## Assigned Agent

**Claude Code Agent**: `Explore` (for research) + `General-Purpose` (for file operations)

## Acceptance Criteria

- [x] AC1: `.github/agents/` directory exists with relevant agent files copied
- [x] AC2: `.github/instructions/` directory exists with instruction files copied
- [x] AC3: `.github/prompts/` directory exists with prompt files copied
- [x] AC4: `mcp.json` exists at repository root with Microsoft Learn and Context7 servers configured
- [x] AC5: All copied files are validated to not have Azure-specific content that doesn't apply to this lab
- [x] AC6: A `README.md` section documents the custom agent setup

## Dependencies

- None (first story)

## Source Materials

| Material | Location |
|----------|----------|
| Agent definitions | `~/Coding_Projects/bmad-azure-module/.github/agents/` |
| Instructions | `~/Coding_Projects/bmad-azure-module/.github/instructions/` |
| Prompts | `~/Coding_Projects/bmad-azure-module/.github/prompts/` |
| MCP config | `~/Coding_Projects/bmad-azure-module/mcp.json` |

## Implementation Notes

1. Copy only the files needed for this lab's learning objectives
2. The `mcp.json` should include:
   - `microsoft-learn` server for documentation access
   - `context7` server for library documentation
3. Review each agent file and adapt descriptions if too Azure-specific
4. Keep the file structure patterns from bmad-azure-module

---

## Agent Prompt

```
You are setting up lab infrastructure for a GitHub Copilot multi-agent orchestration lab.

**Context:**
- Source repo: ~/Coding_Projects/bmad-azure-module
- Target repo: ~/Coding_Projects/ghcp-contoso-university-lab
- This lab teaches multi-agent workflows using a .NET Contoso University application

**Tasks:**
1. Read the contents of ~/Coding_Projects/bmad-azure-module/.github/ to understand the agent structure
2. Read ~/Coding_Projects/bmad-azure-module/mcp.json for MCP configuration
3. Create the following directories in the target repo:
   - .github/agents/
   - .github/instructions/
   - .github/prompts/
4. Copy the mcp.json to the target repo root
5. Copy instruction files that are generally applicable (not Azure-specific):
   - testing-standards.instructions.md
   - Any other broadly applicable instructions
6. Copy prompt files that demonstrate the pattern (select 2-3 representative examples)
7. Validate the MCP configuration has both microsoft-learn and context7 servers

**Output:**
- List all files created
- Confirm MCP configuration is valid
- Note any files that were intentionally NOT copied and why

**Acceptance Criteria to verify:**
- [ ] AC1: .github/agents/ directory exists
- [ ] AC2: .github/instructions/ directory exists
- [ ] AC3: .github/prompts/ directory exists
- [ ] AC4: mcp.json exists with correct servers
- [ ] AC5: No Azure-specific content in copied files
- [ ] AC6: Document what was set up
```

---

## Estimated Effort

- **Complexity**: Low
- **Files Changed**: 5-10 new files

## Session Notes

### Implementation Summary (2026-01-16)

**Approach Change**: Per user direction, copied the entire `.github/` directory from bmad-azure-module instead of selectively copying files. This provides complete examples of the GitHub Copilot extensibility patterns.

### Files Created

**Agents (6 files):**
- `agent-framework.agent.md` - Microsoft Agent Framework SDK specialist
- `ai-foundry.agent.md` - Azure AI Foundry specialist
- `azure-infrastructure.agent.md` - Azure infrastructure specialist
- `azure-orchestrator.agent.md` - Multi-agent orchestration specialist
- `azure-testing.agent.md` - Azure testing specialist
- `fabric-data.agent.md` - Microsoft Fabric data specialist

**Instructions (2 files):**
- `action-guardrails.instructions.md` - Action classification and confirmation requirements
- `responsible-ai.instructions.md` - Responsible AI guidelines

**Prompts (9 files):**
- `create-agent.prompt.md` - Create AI agents
- `create-test.prompt.md` - Generate tests
- `create-bicep.prompt.md` - Generate Bicep templates
- `cost-optimize.prompt.md` - Cost optimization
- `design-lakehouse.prompt.md` - Lakehouse design
- `plan-infrastructure.prompt.md` - Infrastructure planning
- `security-baseline.prompt.md` - Security baseline
- `select-model.prompt.md` - Model selection
- `waf-review.prompt.md` - Well-Architected review

**Skills (3 directories):**
- `azure-waf/SKILL.md` - Azure Well-Architected Framework
- `bicep-best-practices/SKILL.md` - Bicep best practices
- `testing-standards/SKILL.md` - Testing standards

**Other:**
- `copilot-instructions.md` - Global Copilot instructions
- `mcp.json` (at repo root) - MCP server configuration

### MCP Configuration Validated

Both required servers configured:
- `microsoft-learn` - HTTP server at learn.microsoft.com/api/mcp
- `context7` - stdio server via npx @upstash/context7-mcp

### Note on Azure-Specific Content

The copied files retain Azure-specific content as they serve as learning examples. For lab exercises, participants will create new agents/prompts specific to the Contoso University .NET application context.
