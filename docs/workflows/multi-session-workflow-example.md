# Example: Multi-Session Implementation Workflow

This document demonstrates a **multi-session workflow** for complex features that span multiple working sessions with handoffs between them.

> **Scenario:** Migrate the Contoso University grade display from letter grades to a comprehensive GPA system with calculations.

---

## Workflow Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           Multi-Session Workflow                             │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                             │
│  Session 1                Session 2                Session 3                │
│  ┌─────────────┐         ┌─────────────┐         ┌─────────────┐           │
│  │   Scout     │         │   Scribe    │         │   Builder   │           │
│  │  Analysis   │────────▶│   Stories   │────────▶│   Impl #1   │──────┐    │
│  └─────────────┘         └─────────────┘         └─────────────┘      │    │
│        │                       │                       │              │    │
│        ▼                       ▼                       ▼              │    │
│   handoff-001.md          handoff-002.md          handoff-003.md      │    │
│                                                                       │    │
│  ┌─────────────────────────────────────────────────────────────────┐ │    │
│  │                         Session 4                                │ │    │
│  │  ┌─────────────┐         ┌─────────────┐                        │ │    │
│  │  │   Builder   │         │    Sage     │                        │◀┘    │
│  │  │   Impl #2   │────────▶│    Docs     │                        │      │
│  │  └─────────────┘         └─────────────┘                        │      │
│  │        │                       │                                 │      │
│  │        ▼                       ▼                                 │      │
│  │   handoff-004.md          docs/gpa-feature.md                    │      │
│  └─────────────────────────────────────────────────────────────────┘      │
│                                                                             │
└─────────────────────────────────────────────────────────────────────────────┘
```

**Why multi-session?**
- GPA calculation affects multiple areas (models, controllers, views)
- Implementation too large for single context window
- Allows review/approval between phases
- Team members can take different sessions

---

## Session 1: Analysis Phase

### Session Prompt

```
You are starting a new multi-session workflow for the GPA Feature.

**Context:**
- Feature: Add GPA calculation and display to student details
- Current state: Application shows letter grades only
- Goal: Calculate and display cumulative GPA

**Your Task:**
1. Analyze the current grade implementation across the codebase
2. Identify all files that will need modification
3. Document your findings in docs/analysis/gpa-feature-analysis.md
4. Create a handoff document when complete

**Important:**
When your analysis is complete, use the `/create-handoff` prompt to document this session.
```

### Work Performed

Scout analyzes the codebase and creates:

**`docs/analysis/gpa-feature-analysis.md`:**

```markdown
# GPA Feature Analysis

## Current Implementation

### Grade Storage
- **Model**: `Enrollment.Grade` (nullable int, stores letter grade)
- **Values**: A=0, B=1, C=2, D=3, F=4 (enum in `Enrollment.cs`)

### Grade Display
- `Views/Enrollments/Index.cshtml`: Shows grade dropdown
- `Views/Students/Details.cshtml`: Lists enrollments with grades

### Identified Changes

| Component | Current | Required Change |
|-----------|---------|-----------------|
| Grade enum | Letter grades only | Add grade point mappings |
| Student model | No GPA property | Add calculated GPA property |
| Details view | Shows grades only | Add GPA display section |
| Extensions | None | New GradeExtensions.cs for calculations |

### Implementation Plan

**Story 1: Grade Point Mapping (S01)**
- Add GradeExtensions.cs with ToGradePoints()
- Update Grade enum documentation

**Story 2: GPA Calculation (S02)**
- Add GPA calculation logic
- Handle edge cases (no grades, incomplete courses)

**Story 3: Display GPA (S03)**
- Update Student Details view
- Add formatting and styling

### Risks
- NULL grades (in-progress courses) - need handling strategy
- Large student datasets - performance consideration for calculation
```

### Handoff Document

Use the `/create-handoff` prompt:

```
/create-handoff

Session information:
- Story/Task: GPA Feature - Analysis Phase
- Session Number: 1
- Agent Used: Scout (brownfield-analyst)
- Duration: ~20 minutes
- What Completed: [x] Analyzed grade implementation, [x] Created analysis document, [x] Identified 3 stories
- What Remains: [ ] Write stories, [ ] Implement feature, [ ] Document feature
- Files Modified: docs/analysis/gpa-feature-analysis.md (Created)
- Context Notes: Three stories identified. NULL grade handling needs decision before story 2.
```

**Generated `docs/handoffs/handoff-gpa-001.md`:**

```markdown
# Handoff GPA-001: Analysis Complete

## Session Metadata

| Field | Value |
|-------|-------|
| **Date/Time** | 2026-01-20 10:00 |
| **Session Number** | 1 |
| **Agent Used** | Scout (brownfield-analyst) |
| **Duration** | ~20 minutes |

---

## Current Task

**Task**: GPA Feature - Analysis Phase
**Status**: Completed

### What Was Completed

- [x] Analyzed current grade implementation (Enrollment model, views)
- [x] Identified all files requiring modification
- [x] Created analysis document at `docs/analysis/gpa-feature-analysis.md`
- [x] Defined 3-story implementation plan

### What Remains

- [ ] Write detailed user stories (Session 2)
- [ ] Implement GPA calculation (Sessions 3-4)
- [ ] Document the feature (Session 4)

### Blockers / Issues Encountered

**Decision Needed:** How should NULL grades (in-progress courses) be handled in GPA calculation?
- Option A: Exclude from calculation entirely
- Option B: Treat as 0.0 grade points
- Option C: Show "GPA (completed courses only)" distinction

---

## Next Task

**Task**: Write User Stories for GPA Feature
**Status**: Not Started

### Context for Next Session

Three stories identified in analysis:
1. **S01**: Grade Point Mapping - Add GradeExtensions.cs
2. **S02**: GPA Calculation Logic - Depends on NULL handling decision
3. **S03**: GPA Display - Update Student Details view

### Pre-Requisites Verified

- [x] Analysis document created
- [x] Files to modify identified
- [ ] NULL handling decision needed before S02

---

## Session Prompt

```
You are continuing work on the GPA Feature.

**Current State:**
- Workflow: GPA Feature (Multi-Session)
- Last completed: Session 1 - Analysis Phase
- Last handoff: docs/handoffs/handoff-gpa-001.md
- Next task: Write User Stories

**Decision Needed:**
How should NULL grades (in-progress courses) be handled?
- A: Exclude from calculation
- B: Treat as 0.0
- C: Show "completed courses only" distinction

**Your Task:**
1. Get decision on NULL handling from user
2. Write 3 user stories based on analysis at docs/analysis/gpa-feature-analysis.md
3. Create handoff document when complete

**Start by reading:** docs/analysis/gpa-feature-analysis.md
```

---

## Files Modified This Session

| File | Action | Notes |
|------|--------|-------|
| `docs/analysis/gpa-feature-analysis.md` | Created | Complete analysis of current implementation |
| `docs/handoffs/handoff-gpa-001.md` | Created | This handoff document |

---

## Notes for Future Sessions

- Grade enum uses int values 0-4, will need point mapping (A=4.0, B=3.0, etc.)
- Student Details already shows enrollments - good place to add GPA
- Consider adding GPA to Student list view in future enhancement
```

---

## Session 2: Story Writing Phase

### Session Prompt (from handoff-gpa-001.md)

```
You are continuing work on the GPA Feature.

**Current State:**
- Workflow: GPA Feature (Multi-Session)
- Last completed: Session 1 - Analysis Phase
- Last handoff: docs/handoffs/handoff-gpa-001.md
- Next task: Write User Stories

**Decision Needed:**
How should NULL grades (in-progress courses) be handled?
- A: Exclude from calculation
- B: Treat as 0.0
- C: Show "completed courses only" distinction

**Your Task:**
1. Get decision on NULL handling from user
2. Write 3 user stories based on analysis at docs/analysis/gpa-feature-analysis.md
3. Create handoff document when complete

**Start by reading:** docs/analysis/gpa-feature-analysis.md
```

### Work Performed

After user chooses **Option A** (exclude from calculation), Scribe writes stories.

**`stories/gpa-S01-grade-points.md`:**

```markdown
# GPA-S01: Grade Point Mapping

## User Story

**As a** developer implementing GPA features
**I want** a reliable method to convert letter grades to grade points
**So that** GPA calculations are consistent across the application

## Acceptance Criteria

- [ ] **AC1**: GradeExtensions.ToGradePoints() returns 4.0 for A, 3.0 for B, 2.0 for C, 1.0 for D, 0.0 for F
- [ ] **AC2**: ToGradePoints() returns null for null Grade input (in-progress courses)
- [ ] **AC3**: Extension method is accessible from any code referencing ContosoUniversity.Models

## Technical Details

**New File:** `Extensions/GradeExtensions.cs`

```csharp
public static class GradeExtensions
{
    public static decimal? ToGradePoints(this Grade? grade)
    {
        return grade switch
        {
            Grade.A => 4.0m,
            Grade.B => 3.0m,
            Grade.C => 2.0m,
            Grade.D => 1.0m,
            Grade.F => 0.0m,
            _ => null
        };
    }
}
```

## Tests Required

- [ ] Each grade value maps to correct points
- [ ] Null input returns null output
```

(Similar stories created for S02 and S03)

### Handoff Document

**Generated `docs/handoffs/handoff-gpa-002.md`:**

```markdown
# Handoff GPA-002: Stories Written

## Session Metadata

| Field | Value |
|-------|-------|
| **Date/Time** | 2026-01-20 14:00 |
| **Session Number** | 2 |
| **Agent Used** | Scribe (story-writer) |
| **Duration** | ~25 minutes |

---

## Current Task

**Task**: Write User Stories for GPA Feature
**Status**: Completed

### What Was Completed

- [x] Confirmed NULL handling decision: Exclude from calculation (Option A)
- [x] Created GPA-S01: Grade Point Mapping
- [x] Created GPA-S02: GPA Calculation Logic
- [x] Created GPA-S03: GPA Display
- [x] Graded all stories using story-writing-standards skill (all READY FOR DEV)

### What Remains

- [ ] Implement GPA-S01 (Session 3)
- [ ] Implement GPA-S02 (Session 3 or 4)
- [ ] Implement GPA-S03 (Session 4)
- [ ] Document the feature (Session 4)

### Blockers / Issues Encountered

None - All stories passed quality gate.

---

## Next Task

**Task**: Implement GPA-S01 (Grade Point Mapping)
**Status**: Not Started

### Context for Next Session

- Start with S01 (foundational - other stories depend on it)
- S02 can follow immediately if time permits
- S03 should wait until S02 tests pass

### Pre-Requisites Verified

- [x] Stories written and graded
- [x] NULL handling decision documented
- [x] Dependencies clear (S01 → S02 → S03)

---

## Session Prompt

```
You are continuing work on the GPA Feature.

**Current State:**
- Workflow: GPA Feature (Multi-Session)
- Last completed: Session 2 - Stories Written
- Last handoff: docs/handoffs/handoff-gpa-002.md
- Next task: Implement GPA-S01 and GPA-S02

**Your Task:**
1. Implement GPA-S01 (Grade Point Mapping) per stories/gpa-S01-grade-points.md
2. If time permits, implement GPA-S02 (GPA Calculation)
3. Run tests and verify all acceptance criteria
4. Create handoff document when complete

**Start by reading:** stories/gpa-S01-grade-points.md
```

---

## Files Modified This Session

| File | Action | Notes |
|------|--------|-------|
| `stories/gpa-S01-grade-points.md` | Created | Grade point extension story |
| `stories/gpa-S02-gpa-calculation.md` | Created | GPA calculation story |
| `stories/gpa-S03-gpa-display.md` | Created | GPA display story |
| `docs/handoffs/handoff-gpa-002.md` | Created | This handoff document |
```

---

## Session 3: Implementation Phase (Part 1)

### Session Prompt (from handoff-gpa-002.md)

```
You are continuing work on the GPA Feature.

**Current State:**
- Workflow: GPA Feature (Multi-Session)
- Last completed: Session 2 - Stories Written
- Last handoff: docs/handoffs/handoff-gpa-002.md
- Next task: Implement GPA-S01 and GPA-S02

**Your Task:**
1. Implement GPA-S01 (Grade Point Mapping) per stories/gpa-S01-grade-points.md
2. If time permits, implement GPA-S02 (GPA Calculation)
3. Run tests and verify all acceptance criteria
4. Create handoff document when complete

**Start by reading:** stories/gpa-S01-grade-points.md
```

### Work Performed

Builder implements S01 completely and S02 partially:

- Created `Extensions/GradeExtensions.cs`
- Added tests in `GradeExtensionsTests.cs`
- Started S02 implementation but ran out of context

### Handoff Document

**Generated `docs/handoffs/handoff-gpa-003.md`:**

```markdown
# Handoff GPA-003: S01 Complete, S02 In Progress

## Session Metadata

| Field | Value |
|-------|-------|
| **Date/Time** | 2026-01-21 09:00 |
| **Session Number** | 3 |
| **Agent Used** | Builder (senior-dev) |
| **Duration** | ~30 minutes |

---

## Current Task

**Task**: Implement GPA-S01 and GPA-S02
**Status**: S01 Complete, S02 In Progress

### What Was Completed

- [x] Created GradeExtensions.cs with ToGradePoints()
- [x] All S01 acceptance criteria verified
- [x] S01 tests written and passing
- [x] Graded S01 code: 8.5/10 PASS
- [x] Started S02: Added CalculateGPA method signature

### What Remains

- [ ] Complete S02 implementation (calculation logic)
- [ ] Write S02 tests
- [ ] Grade S02 code
- [ ] Implement S03 (Display)
- [ ] Document the feature

### Blockers / Issues Encountered

**Context limit reached** - Session ended before S02 completion.

Partial S02 progress:
- Method signature in GradeExtensions.cs
- Core calculation logic NOT implemented
- Tests NOT written

---

## Next Task

**Task**: Complete GPA-S02 and Implement GPA-S03
**Status**: In Progress

### Context for Next Session

**S02 Status:**
```csharp
// IN PROGRESS - logic not implemented
public static decimal? CalculateGPA(this IEnumerable<Enrollment> enrollments)
{
    // TODO: Implement calculation
    throw new NotImplementedException();
}
```

**S02 Requirements:**
- Calculate average of grade points for enrollments with non-null grades
- Return null if no graded enrollments
- Handle empty enrollment collection

### Pre-Requisites Verified

- [x] S01 complete and tested
- [x] GradeExtensions.cs exists and works
- [ ] S02 needs completion before S03

---

## Session Prompt

```
You are continuing work on the GPA Feature.

**Current State:**
- Workflow: GPA Feature (Multi-Session)
- Last completed: Session 3 - S01 Complete, S02 Partial
- Last handoff: docs/handoffs/handoff-gpa-003.md
- Current task: Complete GPA-S02 and implement GPA-S03

**In Progress Code:**
GradeExtensions.cs has CalculateGPA stub that throws NotImplementedException.

**Your Task:**
1. Complete GPA-S02 implementation per stories/gpa-S02-gpa-calculation.md
2. Write and run S02 tests
3. Implement GPA-S03 per stories/gpa-S03-gpa-display.md
4. Document the feature with Sage
5. Create final handoff document

**Start by reading:** Extensions/GradeExtensions.cs (current state)
```

---

## Files Modified This Session

| File | Action | Notes |
|------|--------|-------|
| `Extensions/GradeExtensions.cs` | Created | ToGradePoints() complete, CalculateGPA() stub |
| `Tests/GradeExtensionsTests.cs` | Created | S01 tests passing |
| `docs/handoffs/handoff-gpa-003.md` | Created | This handoff document |
```

---

## Session 4: Final Implementation & Documentation

### Session Prompt (from handoff-gpa-003.md)

```
You are continuing work on the GPA Feature.

**Current State:**
- Workflow: GPA Feature (Multi-Session)
- Last completed: Session 3 - S01 Complete, S02 Partial
- Last handoff: docs/handoffs/handoff-gpa-003.md
- Current task: Complete GPA-S02 and implement GPA-S03

**In Progress Code:**
GradeExtensions.cs has CalculateGPA stub that throws NotImplementedException.

**Your Task:**
1. Complete GPA-S02 implementation per stories/gpa-S02-gpa-calculation.md
2. Write and run S02 tests
3. Implement GPA-S03 per stories/gpa-S03-gpa-display.md
4. Document the feature with Sage
5. Create final handoff document

**Start by reading:** Extensions/GradeExtensions.cs (current state)
```

### Work Performed

- Completed S02 implementation
- Wrote and passed S02 tests
- Implemented S03 (view changes)
- Sage documented the feature
- All code graded and passing

### Final Handoff Document

**Generated `docs/handoffs/handoff-gpa-004.md`:**

```markdown
# Handoff GPA-004: Feature Complete

## Session Metadata

| Field | Value |
|-------|-------|
| **Date/Time** | 2026-01-21 14:00 |
| **Session Number** | 4 |
| **Agent Used** | Builder + Sage |
| **Duration** | ~35 minutes |

---

## Current Task

**Task**: Complete GPA Feature
**Status**: Completed

### What Was Completed

- [x] Completed S02 CalculateGPA implementation
- [x] S02 tests written and passing
- [x] S02 code graded: 8/10 PASS
- [x] S03 implemented (Details view updated)
- [x] S03 code graded: 8.5/10 PASS
- [x] Feature documentation created by Sage
- [x] All acceptance criteria verified

### What Remains

N/A - Feature Complete

### Blockers / Issues Encountered

None - All work completed successfully.

---

## Feature Summary

| Story | Status | Code Grade |
|-------|--------|------------|
| GPA-S01: Grade Point Mapping | Complete | 8.5/10 PASS |
| GPA-S02: GPA Calculation | Complete | 8/10 PASS |
| GPA-S03: GPA Display | Complete | 8.5/10 PASS |

---

## Files Modified (All Sessions)

| File | Action | Notes |
|------|--------|-------|
| `Extensions/GradeExtensions.cs` | Created | ToGradePoints() and CalculateGPA() |
| `Views/Students/Details.cshtml` | Modified | Added GPA display section |
| `Tests/GradeExtensionsTests.cs` | Created | All calculation tests |
| `docs/features/gpa-calculation.md` | Created | Feature documentation |
| `docs/analysis/gpa-feature-analysis.md` | Created | Initial analysis |
| `stories/gpa-S01-grade-points.md` | Created | Story file |
| `stories/gpa-S02-gpa-calculation.md` | Created | Story file |
| `stories/gpa-S03-gpa-display.md` | Created | Story file |

---

## Workflow Complete

This multi-session workflow is complete. The GPA feature has been:
- Analyzed (Session 1)
- Planned as stories (Session 2)
- Implemented and tested (Sessions 3-4)
- Documented (Session 4)

All code passed quality gates per coding-standards-grading skill.
```

---

## Key Takeaways

### When to Use Multi-Session Workflows

| Indicator | Single-Session | Multi-Session |
|-----------|----------------|---------------|
| Task scope | Fits in one context window | Too large for one session |
| Duration | < 30 minutes | Hours to days |
| Review needs | None or at end | Between phases |
| Dependencies | Self-contained | External decisions needed |
| Collaboration | Solo | Team or async |

### Handoff Best Practices

1. **Session prompts are critical** - They must be self-contained and actionable
2. **Document partial work explicitly** - Note what's done vs. what's stub/incomplete
3. **Capture decisions** - Future sessions need to know WHY, not just WHAT
4. **Use quality gates** - Grade stories and code before proceeding

### Using `/create-handoff`

The `/create-handoff` prompt (at `.github/prompts/create-handoff.prompt.md`) automates handoff creation:

```
/create-handoff

Session information:
- Story/Task: [current work]
- Session Number: [N]
- Agent Used: [agent name]
- Duration: [~X minutes]
- What Completed: [checklist]
- What Remains: [checklist]
- Files Modified: [list]
- Context Notes: [important info for next session]
```

---

## Related Documents

- [Sequential Workflow Example](./sequential-workflow-example.md)
- [Handoff Template](../handoffs/handoff-template.md)
- [Create Handoff Prompt](/.github/prompts/create-handoff.prompt.md)
- [Coding Standards Grading](/.github/skills/coding-standards-grading/SKILL.md)
- [Story Writing Standards](/.github/skills/story-writing-standards/SKILL.md)
