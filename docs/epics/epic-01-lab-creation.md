# Epic 01: Multi-Agent Orchestration Lab Creation

## Overview

Create a comprehensive lab that teaches developers how to use multi-agent orchestration techniques with GitHub Copilot custom agents. The lab will guide learners through creating a brownfield epic for the Contoso University application, writing implementation stories, and executing them using AI-assisted development workflows.

## Learning Objectives

By completing this lab, learners will understand:

1. **Multi-Agent Orchestration**
   - Sequential task execution (one agent completes before next begins)
   - Parallel task execution (multiple agents working simultaneously)

2. **Workflow Patterns**
   - Single-session workflows (complete in one conversation)
   - Multi-session workflows (handoffs between sessions)

3. **GitHub Copilot Custom Agents**
   - Creating `.agent.md` files with custom personalities and expertise
   - Writing `.instructions.md` files for coding standards
   - Building `.prompt.md` files for reusable task commands
   - Configuring MCP servers via `mcp.json`

## Feature Context

The lab recreates a feature from the Contoso University application:
- **Grade Update Feature**: Instructors can update student grades directly from the instructor index page
- **GPA Calculation**: Students can view their calculated GPA on their details page
- **Database Flexibility**: Support for both SQLite and SQL Server

## Stories

| ID | Story | Agent | Status |
|----|-------|-------|--------|
| S01 | [Setup Lab Infrastructure](../stories/story-01-setup-infrastructure.md) | Explore Agent | ✅ Complete |
| S02 | [Create Lab Structure](../stories/story-02-create-lab-structure.md) | General-Purpose Agent | ✅ Complete |
| S03 | [Configure GitHub Copilot Agents](../stories/story-03-configure-copilot-agents.md) | General-Purpose Agent | ✅ Complete |
| S04 | [Create Custom PM/Doc Agent](../stories/story-04-create-pm-doc-agent.md) | General-Purpose Agent | ✅ Complete |
| S05 | [Write Lab Module 1: Introduction](../stories/story-05-lab-module-intro.md) | General-Purpose Agent | ✅ Complete |
| S06 | [Write Lab Module 2: Brownfield Epic Creation](../stories/story-06-lab-module-epic.md) | General-Purpose Agent | ✅ Complete |
| S07 | [Write Lab Module 3: Story Creation](../stories/story-07-lab-module-stories.md) | General-Purpose Agent | ✅ Complete |
| S08 | [Write Lab Module 4: Implementation](../stories/story-08-lab-module-implementation.md) | General-Purpose Agent | ✅ Complete |
| S09 | [Validate and Test Lab](../stories/story-09-validate-lab.md) | Dev Agent | ✅ Complete |

---

## Progress Tracker

### Current Status: ✅ COMPLETE

| Phase | Stories | Status | Handoff |
|-------|---------|--------|---------|
| **Phase 1: Infrastructure** | S01 | ✅ Complete | [handoff-001](../handoffs/handoff-001.md) |
| **Phase 2: Lab Structure** | S02, S03, S04 | ✅ Complete | - |
| **Phase 3: Lab Content** | S05, S06, S07, S08 | ✅ Complete | - |
| **Phase 4: Validation** | S09 | ✅ Complete | - |

### Session History

| Session | Date/Time | Stories Completed | Notes |
|---------|-----------|-------------------|-------|
| 1 | 2026-01-16 | Initial Planning | Created epic and 9 story documents |
| 2 | 2026-01-16 | S01 | Setup lab infrastructure from bmad-azure-module |
| 3 | 2026-01-16 | S02 | Created labs/ folder structure, README.md |
| 4 | 2026-01-16 | S03 | Created Scout, Scribe, Builder agents |
| 5 | 2026-01-16 | S04 | Created Sage (pm-doc-writer) agent |
| 6 | 2026-01-16 | S05 | Created Lab 01 - Introduction (287 lines) |
| 7 | 2026-01-16 | S06 | Created Lab 02 - Epic Creation (417 lines) |
| 8 | 2026-01-16 | S07 | Created Lab 03 - Story Creation (685 lines) |
| 9 | 2026-01-16 | S08 | Created Lab 04 - Implementation (956 lines) |
| 10 | 2026-01-16 | S09 | Final validation - all 8 ACs pass |

---

## Technical Requirements

### Source Materials

1. **Lab Structure Reference**: `~/Coding_Projects/gh-abcs-actions`
   - Follow lab numbering pattern (lab01.md, lab02.md, etc.)
   - Use collapsible `<details>` sections for solutions
   - Include duration estimates and references

2. **GitHub Copilot Agents Source**: `~/Coding_Projects/bmad-azure-module`
   - `.github/agents/` - 6 custom agent definitions
   - `.github/instructions/` - 5 instruction files
   - `.github/prompts/` - 10 prompt files
   - `mcp.json` - MCP server configuration

3. **Feature Source**: `~/Coding_Projects/ghcp-contoso-university`
   - Branch: `brownfield-feature-advanced` (target feature)
   - Branch: `local-testing` (baseline)
   - Diff shows: Grade update, GPA calculation, SQLite support

### Files to Create

```
ghcp-contoso-university-lab/
├── .github/
│   ├── agents/           # Custom Copilot agents
│   ├── instructions/     # Coding standards
│   └── prompts/          # Reusable prompts
├── mcp.json              # MCP configuration
├── labs/
│   ├── setup.md          # Initial setup
│   ├── lab01.md          # Introduction to multi-agent
│   ├── lab02.md          # Brownfield epic creation
│   ├── lab03.md          # Story creation workflow
│   └── lab04.md          # Implementation with agents
└── docs/
    ├── epics/            # This epic
    ├── stories/          # Implementation stories
    └── handoffs/         # Session handoffs
```

### MCP Configuration Requirements

Based on research, the `mcp.json` needs:
- Microsoft Learn MCP server for Azure/Copilot documentation
- Context7 MCP server for library documentation

---

## Definition of Done

- [x] All 9 stories completed and validated
- [x] Lab can be completed end-to-end by a learner
- [x] GitHub Copilot custom agents functional
- [x] All lab modules have working examples
- [x] Handoff documents capture complete session history

---

## Notes for AI Agents

When picking up this epic:
1. Check the Progress Tracker for current status
2. Read the latest handoff document for context
3. Identify the next story to work on
4. Follow the story's AC and agent prompt exactly
5. Update the Progress Tracker when completing stories
6. Create a new handoff document at session end
