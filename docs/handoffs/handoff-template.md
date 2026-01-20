# Handoff Document Template

> Copy this template when creating a new handoff document.
> Name handoff files sequentially: `handoff-001.md`, `handoff-002.md`, etc.

---

# Handoff [NNN]: [Brief Description]

## Session Metadata

| Field | Value |
|-------|-------|
| **Date/Time** | YYYY-MM-DD HH:MM |
| **Session Number** | N |
| **Agent Used** | [Agent name - e.g., General-Purpose, Explore, Dev] |
| **Duration** | ~X minutes |

---

## Current Story

**Story**: [Story ID and Title with link]
**Status**: [In Progress / Completed / Blocked]

### What Was Completed

- [ ] List of completed items
- [ ] With checkboxes for clarity

### What Remains

- [ ] Remaining items
- [ ] If story is complete, write "N/A - Story Complete"

### Blockers / Issues Encountered

> Describe any problems encountered and how they were resolved or why they remain blockers.

None / [Description of blockers]

---

## Next Story

**Story**: [Next Story ID and Title with link]
**Status**: Not Started

### Context for Next Session

> Provide any context the next agent/session needs to know before starting.

### Pre-Requisites Verified

- [ ] Previous story dependencies met
- [ ] Required files exist
- [ ] No blocking issues

---

## Session Prompt

> Copy this prompt to start the next session. It should be self-contained and actionable.

```
You are continuing work on the Multi-Agent Orchestration Lab.

**Current State:**
- Epic: docs/epics/epic-01-lab-creation.md
- Last completed: [Story ID]
- Next story: [Story ID and Title]

**Your Task:**
Read the story file at docs/stories/[story-file].md and execute the agent prompt contained within.

**Important:**
1. Read the story's Acceptance Criteria carefully
2. Follow the agent prompt exactly
3. Update the story's Session Notes when complete
4. Create a new handoff document if session ends before completion

**Start by reading:** docs/stories/[story-file].md
```

---

## Epic Progress Update

> Update the epic's progress tracker with this session's changes.

| Story | Previous Status | New Status |
|-------|-----------------|------------|
| S[X] | [Status] | [Status] |

---

## Files Modified This Session

| File | Action | Notes |
|------|--------|-------|
| path/to/file | Created/Modified/Deleted | Brief note |

---

## Notes for Future Sessions

> Any observations, lessons learned, or recommendations for future work.
