---
description: "Create a handoff document to preserve context between sessions for multi-session workflows"
---

# Create Handoff Document

Generate a structured handoff document following the project template at `docs/handoffs/handoff-template.md`.

## Purpose

Handoff documents preserve context when work spans multiple sessions. They enable:
- Any agent to continue work seamlessly
- Complete audit trail of progress
- Copy-paste session prompts for continuity

## Input Required

Provide the following information:

| Field | Description | Example |
|-------|-------------|---------|
| **Story/Task** | Current story being worked on | `story-01-setup-infrastructure.md` |
| **Session Number** | Which session in the sequence | `2` |
| **Agent Used** | Agent that performed the work | `General-Purpose` |
| **Duration** | Approximate session length | `~20 minutes` |
| **What Completed** | Checklist of completed items | `[x] Created files`, `[x] Validated config` |
| **What Remains** | Remaining work items | `[ ] Write tests`, `[ ] Update docs` |
| **Files Modified** | Files created/changed/deleted | `path/to/file.md - Created` |
| **Next Story** | Story to work on next (if applicable) | `story-02-create-lab-structure.md` |
| **Context Notes** | Important context for next session | Key findings, blockers, recommendations |

## Process

1. **Read the handoff template** at `docs/handoffs/handoff-template.md`
2. **Determine handoff number** by checking existing files in `docs/handoffs/`
3. **Gather session information** from the inputs provided
4. **Generate the handoff document** following template structure
5. **Write the handoff file** to `docs/handoffs/handoff-NNN.md`

## Output Format

The generated handoff must include all template sections:

```markdown
# Handoff [NNN]: [Brief Description]

## Session Metadata
| Field | Value |
|-------|-------|
| **Date/Time** | YYYY-MM-DD HH:MM |
| **Session Number** | N |
| **Agent Used** | [Agent name] |
| **Duration** | ~X minutes |

## Current Story
**Story**: [Story ID and Title with link]
**Status**: [In Progress / Completed / Blocked]

### What Was Completed
- [x] Completed item 1
- [x] Completed item 2

### What Remains
- [ ] Remaining item 1
- [ ] Remaining item 2

### Blockers / Issues Encountered
None / [Description]

## Next Story
**Story**: [Next Story ID and Title with link]
**Status**: Not Started

### Context for Next Session
[Important context, findings, or recommendations]

### Pre-Requisites Verified
- [ ] Previous story dependencies met
- [ ] Required files exist
- [ ] No blocking issues

## Session Prompt
[Copy-paste prompt for next session - self-contained and actionable]

## Epic Progress Update
| Story | Previous Status | New Status |
|-------|-----------------|------------|
| S[X] | [Status] | [Status] |

## Files Modified This Session
| File | Action | Notes |
|------|--------|-------|
| path/to/file | Created/Modified/Deleted | Brief note |

## Notes for Future Sessions
[Observations, lessons learned, recommendations]
```

## Session Prompt Guidelines

The session prompt section is critical. It must be:

1. **Self-contained** - No external context needed
2. **Actionable** - Clear first step to take
3. **Specific** - References exact files and story IDs

Example session prompt:
```
You are continuing work on the Multi-Agent Orchestration Lab.

**Current State:**
- Epic: docs/epics/epic-01-lab-creation.md
- Last completed: [Story ID]
- Next story: [Story ID and Title]

**Your Task:**
Read the story file at docs/stories/[story-file].md and execute the agent prompt.

**Important:**
1. Read the story's Acceptance Criteria carefully
2. Follow the agent prompt exactly
3. Update the story's Session Notes when complete
4. Create a new handoff document if session ends before completion

**Start by reading:** docs/stories/[story-file].md
```

## Naming Convention

Handoff files follow sequential naming:
- `handoff-001.md`, `handoff-002.md`, etc.
- For workflow-specific handoffs: `handoff-[workflow]-NNN.md`
  - Example: `handoff-expansion-001.md`

## Reference

- Template: `docs/handoffs/handoff-template.md`
- Example: `docs/handoffs/handoff-001.md`
