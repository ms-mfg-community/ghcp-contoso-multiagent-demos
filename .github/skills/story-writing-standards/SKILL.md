---
name: story-writing-standards
description: Evaluates user stories against the project's story writing standards rubric, providing structured assessments with scores and actionable feedback. Use when reviewing stories before implementation, validating acceptance criteria quality, or asking Scribe (Story Writer agent) to grade story drafts.
---

# Story Writing Standards Grading

## Purpose

Systematically evaluate user stories against the 5 criteria defined in `/docs/standards/story-writing-standards-rubric.md`:
- **Acceptance Criteria Quality** (25%): Can an implementer verify completion?
- **Testability** (20%): Can automated tests be written directly?
- **Scope Clarity** (20%): Is it unambiguous what IS and IS NOT included?
- **Technical Detail Sufficiency** (20%): Does an AI agent have enough context?
- **Project Pattern Alignment** (15%): Does it follow project conventions?

---

## MCP-First: Query Current Best Practices

**Before evaluating, query MCP tools for agile and story writing standards:**

### Microsoft Learn MCP

```
1. microsoft_docs_search: query="Azure DevOps user stories best practices"
2. microsoft_docs_search: query="agile acceptance criteria definition of done"
```

Recommended queries:
- "Azure DevOps work item best practices"
- "User story INVEST principles"
- "Acceptance criteria Given When Then"
- "Definition of done checklist"

### Context7 MCP

```
1. resolve-library-id: libraryName="agile", query="user stories"
2. query-docs: libraryId from step 1, query="acceptance criteria"
```

---

## How to Use This Skill

### Input

Provide one of the following:
1. **Story file path**: `@workspace/stories/story-001.md`
2. **Story content**: Pasted or selected story text
3. **Work item reference**: Azure DevOps work item ID

### Invocation Examples

```
Evaluate this story against our writing standards: @workspace/stories/US-001-student-search.md

Grade this user story for implementation readiness.

Review my acceptance criteria for completeness and testability.
```

---

## Evaluation Process

### Step 1: Load the Rubric

Read and apply `/docs/standards/story-writing-standards-rubric.md` as the evaluation standard.

### Step 2: Check Prerequisites

Verify story has:
- [ ] User value statement (As a... I want... So that...)
- [ ] At least 3 acceptance criteria
- [ ] Some form of scope definition
- [ ] Dependencies identified (if any)

### Step 3: Evaluate Each Criterion

For each of the 5 criteria:
1. Apply the rubric's scoring indicators (1-10 scale)
2. Note specific examples supporting the score
3. Identify concrete improvements with suggested rewrites

### Step 4: Generate Assessment

Use the evaluation template with actionable recommendations.

---

## Output Template

```markdown
## Story Evaluation: [Story ID - Title]

**Evaluator**: GitHub Copilot
**Date**: [YYYY-MM-DD]

### Scores

| Criterion | Weight | Score | Weighted |
|-----------|--------|-------|----------|
| Acceptance Criteria Quality | 25% | /10 | |
| Testability | 20% | /10 | |
| Scope Clarity | 20% | /10 | |
| Technical Detail Sufficiency | 20% | /10 | |
| Project Pattern Alignment | 15% | /10 | |
| **Overall** | 100% | | /10 |

### Criterion Details

#### Acceptance Criteria Quality: [X]/10
**Strengths**: [what's well-defined]
**Gaps**: [what's missing or vague]
**Suggested rewrites**:
- Original: "[vague AC]"
- Improved: "[specific, testable AC]"

#### Testability: [X]/10
**Can derive these tests**:
- [test case 1]
- [test case 2]
**Cannot test without clarification**:
- [unclear scenario]

#### Scope Clarity: [X]/10
**Clear boundaries**: [what's well-scoped]
**Ambiguous areas**: [what could cause scope creep]
**Suggested additions**:
- Out of scope: [items to explicitly exclude]

#### Technical Detail Sufficiency: [X]/10
**Decisions made**: [architectural choices documented]
**Decisions needed**: [what implementer would have to guess]
**Suggested additions**:
- [specific technical detail to add]

#### Project Pattern Alignment: [X]/10
**Follows patterns**: [alignment with conventions]
**Deviations**: [inconsistencies or concerns]

### Recommendations

**Must address before implementation**:
- [ ] [blocking issue with specific fix]

**Should clarify**:
- [ ] [important clarification]

**Consider adding**:
- [ ] [enhancement]

### Verdict

[READY FOR DEV / NEEDS REFINEMENT / NEEDS REWRITE]

Criteria:
- READY FOR DEV: Overall >= 7, no criterion below 6
- NEEDS REFINEMENT: Overall 5-6.9, or one criterion below 6
- NEEDS REWRITE: Overall < 5, or multiple criteria below 4
```

---

## Quick Reference

| Criterion | Core Question | Red Flags |
|-----------|---------------|-----------|
| **AC Quality** | Can I verify this is done? | "Works correctly", "fast", "user-friendly" |
| **Testability** | Can I write tests from this? | No expected outputs, no error cases |
| **Scope Clarity** | What's in and out? | "And maybe also...", no boundaries |
| **Technical Detail** | Do I know how to build this? | "Figure out best approach", no file references |
| **Pattern Alignment** | Does this fit the project? | New patterns without justification, format inconsistencies |

---

## INVEST Principles Checklist

Quick validation for story structure:

| Principle | Question | Pass/Fail |
|-----------|----------|-----------|
| **I**ndependent | Can be completed without other stories? | |
| **N**egotiable | Details can be discussed during implementation? | |
| **V**aluable | Delivers user or business value? | |
| **E**stimable | Team can estimate the work? | |
| **S**mall | Completable in one sprint/session? | |
| **T**estable | Has clear acceptance criteria? | |

---

## Common Anti-Patterns to Flag

| Anti-Pattern | Example | Suggested Fix |
|--------------|---------|---------------|
| **Vague AC** | "Search works well" | "Search returns results in <2s for up to 10k records" |
| **Hidden Epic** | "Implement student management" | Break into 5-10 independent stories |
| **Tech-only** | "Refactor database layer" | Add user value: "...so searches are faster" |
| **Gold-plating** | "Add search with AI and export and saved queries" | One feature per story |
| **Missing Error Cases** | Only happy path ACs | Add: "When X fails, then Y message appears" |
| **Assumed Context** | "Update the form" | "Update StudentCreate.cshtml form" |

---

## Actionable Improvement Examples

### Vague to Specific AC

**Before (Score 3)**:
```
- [ ] User can search for students
- [ ] Search should be fast
- [ ] Results are displayed nicely
```

**After (Score 9)**:
```
- [ ] AC1: Search input accepts 1-100 characters; displays validation error for empty or >100
- [ ] AC2: Search returns results within 2 seconds for up to 10,000 records
- [ ] AC3: Results display student name, ID, and enrollment date in a sortable table
- [ ] AC4: Empty results display "No students match your search" message
- [ ] AC5: Search is case-insensitive and matches partial strings in first or last name
```

### Missing to Complete Scope

**Before (Score 4)**:
```
Add search functionality with filters and export.
```

**After (Score 9)**:
```
## Scope
**In scope:**
- Search by last name only
- Display results in table format
- Basic pagination (10 per page)

**Out of scope (future stories):**
- Search by other fields (first name, ID, email)
- Advanced filters
- Export functionality
- Saved searches

## Dependencies
- Story-01 (Student list page) must be complete
```

---

## Reference

- [Project Rubric](/docs/standards/story-writing-standards-rubric.md) - Authoritative scoring criteria
- [Azure DevOps User Stories](https://learn.microsoft.com/en-us/azure/devops/boards/work-items/guidance/agile-process-workflow)
- [Definition of Done](https://learn.microsoft.com/en-us/devops/plan/definition-of-done)
