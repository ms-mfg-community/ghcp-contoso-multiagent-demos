# Story Writing Standards Rubric

**For: AI Agent Story Evaluation**
**Scope: User Stories, Acceptance Criteria, Technical Specifications**
**Scoring: 1-10 Scale per Criterion**

---

## Purpose

This rubric enables systematic evaluation of user stories before implementation begins. Well-written stories reduce rework, prevent scope creep, and ensure AI agents have the context needed to implement correctly.

---

## Scoring Scale

| Score | Rating | Description |
|-------|--------|-------------|
| 10 | Exemplary | Ready for immediate implementation; serves as a template |
| 9 | Excellent | Implementation-ready; minor polish only |
| 8 | Very Good | Ready to implement; one or two small clarifications possible |
| 7 | Good | Mostly ready; minor gaps that won't block implementation |
| 6 | Satisfactory | Implementable but needs some refinement |
| 5 | Borderline | Can proceed but expect questions during implementation |
| 4 | Below Standard | Needs revision before implementation |
| 3 | Poor | Significant gaps; will cause implementation problems |
| 2 | Very Poor | Major rewrite needed; too vague or contradictory |
| 1 | Unacceptable | Cannot be implemented as written |

**Passing**: 6+ on all criteria | **Ready for Dev**: 7+ on all criteria

---

## Evaluation Criteria

### 1. Acceptance Criteria Quality (Weight: 25%)

**Question**: Can an implementer verify completion without asking follow-up questions?

| Score | Indicators |
|-------|------------|
| **9-10** | Every AC is specific, measurable, and independently verifiable. No ambiguity about what "done" means. Edge cases explicitly addressed. |
| **7-8** | ACs are clear and testable. Minor edge cases could be inferred. Implementation complete when ACs pass. |
| **5-6** | ACs present but some are vague. "User can do X" without specifying what X looks like. Some interpretation required. |
| **3-4** | ACs incomplete or contradictory. Significant ambiguity about expected behavior. Will need clarification. |
| **1-2** | No meaningful ACs, or ACs that are impossible to verify. "System works correctly." |

**What to check**:
- Can each AC be converted directly to a test case?
- Are success AND failure cases specified?
- Are boundary conditions defined?
- Is the expected outcome stated, not just the action?
- Are there ACs that overlap or contradict?

**Examples**:

```markdown
# Score 9: Specific, testable, complete
## Acceptance Criteria
- [ ] AC1: Search input field accepts 1-100 characters; displays validation error for empty or >100
- [ ] AC2: Search results display within 2 seconds for up to 10,000 records
- [ ] AC3: Results show student name, ID, and enrollment date in a sortable table
- [ ] AC4: No results displays "No students match your search" message
- [ ] AC5: Search is case-insensitive and matches partial strings

# Score 3: Vague, untestable
## Acceptance Criteria
- [ ] AC1: User can search for students
- [ ] AC2: Search should be fast
- [ ] AC3: Results are displayed nicely
```

---

### 2. Testability (Weight: 20%)

**Question**: Can automated tests be written directly from this story?

| Score | Indicators |
|-------|------------|
| **9-10** | Test cases are obvious from reading the story. Inputs, outputs, and expectations are explicit. Could generate test stubs automatically. |
| **7-8** | Most test scenarios clear. May need to infer one or two edge cases. |
| **5-6** | Happy path testable. Edge cases require assumptions. Some scenarios unclear. |
| **3-4** | Difficult to write tests without making significant assumptions about behavior. |
| **1-2** | Cannot write meaningful tests. Behavior too vague or undefined. |

**What to check**:
- Are inputs and expected outputs specified?
- Are error conditions and their responses defined?
- Can you write Given/When/Then scenarios?
- Are there UI behaviors that need E2E tests?
- Are performance or load requirements testable?

**Examples**:

```markdown
# Score 9: Test scenarios are obvious
**Given** a student search with the term "Smith"
**When** the search is executed
**Then** all students with "Smith" in their first or last name appear in results

**Given** a search with no matching results
**When** the search is executed
**Then** the message "No students match your search" is displayed

# Score 3: Cannot derive tests
"The search should find students and show them to the user."
```

---

### 3. Scope Clarity (Weight: 20%)

**Question**: Is it unambiguous what IS and IS NOT part of this story?

| Score | Indicators |
|-------|------------|
| **9-10** | Explicit boundaries. "In scope" and "out of scope" sections. No overlap with other stories. Single deliverable. |
| **7-8** | Scope is clear from context. Minor assumptions about boundaries are safe. |
| **5-6** | Scope mostly clear but some features could be interpreted as included or excluded. |
| **3-4** | Scope ambiguous. Could easily grow during implementation. Overlaps with other stories unclear. |
| **1-2** | Scope undefined. Epic disguised as a story. Multiple features bundled. |

**What to check**:
- Is this one story or multiple bundled together?
- Are there explicit "not included" items?
- Does this overlap with other stories?
- Is the story small enough to complete in one session?
- Are dependencies clearly stated?

**Examples**:

```markdown
# Score 9: Crystal clear scope
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

# Score 3: Scope creep waiting to happen
## Description
Add search functionality with filters and export and maybe saved searches if time permits.
```

---

### 4. Technical Detail Sufficiency (Weight: 20%)

**Question**: Does an AI agent have enough context to implement without guessing architectural decisions?

| Score | Indicators |
|-------|------------|
| **9-10** | Architecture decisions made. File locations specified. Patterns to follow identified. Integration points clear. No ambiguity about "how." |
| **7-8** | Key technical decisions documented. Agent can infer remaining details from codebase patterns. |
| **5-6** | High-level approach clear but implementation details require exploration. Some architectural assumptions needed. |
| **3-4** | Technical approach unclear. Multiple valid implementations possible with different implications. |
| **1-2** | No technical guidance. Agent would have to make significant architectural decisions. |

**What to check**:
- Are file/class locations specified or inferable?
- Are API endpoints or contracts defined?
- Are database changes documented?
- Are existing patterns referenced?
- Are integration points with other systems clear?

**Examples**:

```markdown
# Score 9: Implementation path is clear
## Technical Notes
- Add `SearchAsync(string term)` method to `IStudentService`
- Implement in `StudentService.cs` following existing query patterns
- Create `Search.cshtml` view in `Views/Students/`
- Add `Search` action to `StudentController`
- Use existing `_StudentTablePartial.cshtml` for results display
- No database changes required (uses existing Student table)

## Patterns to Follow
- See `CourseController.Index()` for similar search implementation
- Use `ILogger` for search telemetry

# Score 3: No technical direction
## Technical Notes
Figure out the best way to implement search.
```

---

### 5. Project Pattern Alignment (Weight: 15%)

**Question**: Does this story align with established project conventions and architecture?

| Score | Indicators |
|-------|------------|
| **9-10** | Explicitly references project patterns. Follows established conventions. Consistent with existing features. References similar implementations. |
| **7-8** | Aligns with project patterns. Implementation would look like existing code. |
| **5-6** | Generally aligned but introduces minor variations from patterns. |
| **3-4** | Conflicts with established patterns or proposes new approaches without justification. |
| **1-2** | Contradicts project architecture. Would introduce inconsistency or technical debt. |

**What to check**:
- Does the story follow INVEST principles?
- Is the format consistent with other stories?
- Does it reference existing implementations as models?
- Are naming conventions followed?
- Does it align with the project's architecture (MVC, layers, etc.)?

**INVEST Principles**:
- **I**ndependent: Can be completed without other stories
- **N**egotiable: Details can be discussed during implementation
- **V**aluable: Delivers user or business value
- **E**stimable: Team can estimate the work
- **S**mall: Completable in one sprint/session
- **T**estable: Has clear acceptance criteria

---

## Evaluation Template

```markdown
## Story Evaluation: [Story ID - Title]

**Evaluator**: [agent name]
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
**Suggestions**: [specific improvements]

#### Testability: [X]/10
**Can derive these tests**: [list obvious test cases]
**Cannot test without clarification**: [list unclear scenarios]

#### Scope Clarity: [X]/10
**Clear boundaries**: [what's well-scoped]
**Ambiguous areas**: [what could cause scope creep]

#### Technical Detail Sufficiency: [X]/10
**Decisions made**: [architectural choices documented]
**Decisions needed**: [what agent would have to guess]

#### Project Pattern Alignment: [X]/10
**Follows patterns**: [alignment with conventions]
**Deviations**: [inconsistencies or concerns]

### Recommendations

**Must address before implementation**:
- [ ] [blocking issue]

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

## Story Quality Checklist

Quick pass/fail for story readiness:

### Must Have (blocking if missing)
- [ ] Clear user value statement (As a... I want... So that...)
- [ ] At least 3 specific, testable acceptance criteria
- [ ] Explicit scope boundaries
- [ ] Dependencies identified

### Should Have (refinement needed if missing)
- [ ] Technical implementation notes
- [ ] References to similar existing implementations
- [ ] Error/edge cases in acceptance criteria
- [ ] Out-of-scope items listed

### Nice to Have (polish)
- [ ] Test scenarios written in Given/When/Then
- [ ] Specific file paths or class names
- [ ] Performance requirements quantified
- [ ] Mockups or wireframes for UI changes

---

## Common Story Anti-Patterns

| Anti-Pattern | Example | Fix |
|--------------|---------|-----|
| **Vague AC** | "Search works well" | "Search returns results in <2s for up to 10k records" |
| **Hidden Epic** | "Implement student management" | Break into 5-10 independent stories |
| **Tech-only** | "Refactor database layer" | Add user value: "...so searches are faster" |
| **Gold-plating** | "Add search with AI suggestions, saved queries, and export" | One feature per story |
| **Missing Error Cases** | Only happy path ACs | Add: "When X fails, then Y message appears" |
| **Assumed Context** | "Update the form" (which form?) | "Update StudentCreate.cshtml form" |

---

**Remember**: A story that scores below 6 on any criterion will likely cause implementation delays, rework, or scope disputes. Invest in story quality upfront.
