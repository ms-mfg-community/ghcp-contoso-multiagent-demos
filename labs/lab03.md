# 3 - Writing Implementation Stories

In this lab you will break down an epic into actionable stories with acceptance criteria and agent prompts.

> Duration: 25-35 minutes

References:
- [User Story Best Practices](https://www.mountaingoatsoftware.com/agile/user-stories)
- [Acceptance Criteria Guidelines](https://www.altexsoft.com/blog/acceptance-criteria-purposes-formats-and-best-practices/)
- [INVEST Principles](https://www.agilealliance.org/glossary/invest/)

---

## 3.1 Story Structure

A user story is a unit of work that delivers measurable value. In multi-agent workflows, stories also serve as execution contexts—each story includes an agent assignment and a prompt that can be executed directly.

### Story Writing Standards Reference

This lab follows the story writing standards defined in the project:

| Resource | Path | Purpose |
|----------|------|---------|
| **Rubric** | `docs/standards/story-writing-standards-rubric.md` | Full scoring criteria |
| **Grading Skill** | `.github/skills/story-writing-standards/SKILL.md` | How to evaluate stories |
| **Grading Prompt** | `.github/prompts/grade-story.prompt.md` | Quick story evaluation |

Stories are evaluated on **5 criteria**:

| Criterion | Weight | Question |
|-----------|--------|----------|
| Acceptance Criteria Quality | 25% | Can an implementer verify completion? |
| Testability | 20% | Can automated tests be written directly? |
| Scope Clarity | 20% | Is it unambiguous what IS and IS NOT included? |
| Technical Detail Sufficiency | 20% | Does an AI agent have enough context? |
| Project Pattern Alignment | 15% | Does it follow project conventions? |

**Target**: Stories must score **7+** overall and **6+** on each criterion to be implementation-ready.

### Why Story Structure Matters

Well-structured stories enable:

| Benefit | How It Helps |
|---------|--------------|
| **Independent Execution** | Stories can be worked in parallel when dependencies allow |
| **Clear Completion** | Acceptance criteria define "done" objectively |
| **Agent Assignment** | Right agent for the task improves output quality |
| **Prompt Readiness** | Complete prompts enable immediate execution |

### Story Document Components

Every story follows this template:

```
┌─────────────────────────────────────────────────────┐
│                      STORY                          │
├─────────────────────────────────────────────────────┤
│  Summary                                            │
│  - What capability is being delivered?              │
│  - What value does it provide?                      │
├─────────────────────────────────────────────────────┤
│  Assigned Agent                                     │
│  - Which agent will execute this work?              │
│  - Why is this agent the right choice?              │
├─────────────────────────────────────────────────────┤
│  Acceptance Criteria                                │
│  - Testable conditions that define "done"           │
│  - Checkboxes for tracking completion               │
├─────────────────────────────────────────────────────┤
│  Dependencies                                       │
│  - Other stories that must complete first           │
│  - External systems or data requirements            │
├─────────────────────────────────────────────────────┤
│  Agent Prompt                                       │
│  - Complete, executable prompt for the agent        │
│  - Context, tasks, and expected output              │
└─────────────────────────────────────────────────────┘
```

### Writing Effective Acceptance Criteria

Acceptance criteria (AC) are the contract between requirement and implementation. They must be:

**Testable**: Can you verify it with a specific test?
```
❌ "The page should be fast"
✅ "Page load time is under 2 seconds"
```

**Specific**: Does it describe a single, clear behavior?
```
❌ "Grades work correctly"
✅ "Selecting a grade from the dropdown updates the enrollment record"
```

**Observable**: Can you see the result?
```
❌ "The system processes grades efficiently"
✅ "Grade update returns success response within 500ms"
```

### AC Formats

Two common formats work well:

**Checklist Format** (recommended for implementation stories):
```markdown
- [ ] AC1: Controller action `UpdateGrade` exists and accepts enrollment ID and grade
- [ ] AC2: Valid grade values (A, B, C, D, F, null) are accepted
- [ ] AC3: Invalid grade values return 400 Bad Request
- [ ] AC4: Successful update returns 200 OK
```

**Given-When-Then Format** (useful for behavior-focused stories):
```markdown
- [ ] AC1: Given an instructor viewing course enrollments, when they select a grade from the dropdown, then the enrollment record is updated
```

### INVEST Principles

Before writing stories, validate them against the INVEST principles:

| Principle | Question | Pass? |
|-----------|----------|-------|
| **I**ndependent | Can this story be completed without waiting for other stories? | |
| **N**egotiable | Can implementation details be discussed during development? | |
| **V**aluable | Does this deliver user or business value? | |
| **E**stimable | Can the team estimate the work required? | |
| **S**mall | Can this be completed in one sprint/session? | |
| **T**estable | Are there clear acceptance criteria to verify completion? | |

> **Reference**: The full INVEST checklist is in `docs/standards/story-writing-standards-rubric.md` (lines 179-190).

**Common INVEST violations:**
- **Not Independent**: "Story B requires Story A's database changes" → Split or sequence explicitly
- **Not Small**: "Implement user management" → Break into 5-10 focused stories
- **Not Testable**: "System should be fast" → Quantify: "Response time < 2 seconds"

### Agent Assignment Considerations

Choose the agent based on the work type:

| Agent | Best For | Example Stories |
|-------|----------|-----------------|
| **Scout** | Analysis, investigation, codebase exploration | "Analyze authentication patterns" |
| **Scribe** | Documentation, story creation, specs | "Document API endpoints" |
| **Builder** | Implementation, coding, bug fixes | "Add UpdateGrade action" |
| **Sage** | Planning, coordination, status reports | "Create sprint summary" |

### Crafting Effective Agent Prompts

A good prompt includes:

1. **Context**: What does the agent need to know?
   - Repository location
   - Related files
   - Background from previous work

2. **Tasks**: What specific actions should be taken?
   - Numbered, concrete steps
   - Clear deliverables

3. **Output**: What should the agent produce?
   - Expected artifacts
   - Verification steps

**Prompt Example:**
```
You are implementing the grade update feature for Contoso University.

**Context:**
- Repository: ~/Coding_Projects/ghcp-contoso-university-lab
- Controller: ContosoUniversity/Controllers/InstructorsController.cs
- This builds on the existing instructor management functionality

**Tasks:**
1. Add an UpdateGrade POST action to InstructorsController
2. Accept enrollmentId (int) and grade (string) parameters
3. Validate grade is one of: A, B, C, D, F, or null
4. Update the enrollment record and save changes

**Output:**
- Modified InstructorsController.cs with UpdateGrade action
- Verify compilation succeeds
- List the AC items completed
```

### Exercise: Evaluate Acceptance Criteria

Which set of AC is better for "Add grade dropdown to Instructor Index"?

**Set A:**
```
- [ ] Dropdown appears on the page
- [ ] Grades can be selected
- [ ] It works correctly
```

**Set B:**
```
- [ ] Grade dropdown renders for each enrollment row with options A, B, C, D, F
- [ ] Dropdown shows current grade as selected value (or empty if null)
- [ ] Selecting a new grade triggers form submission to UpdateGrade action
- [ ] Success response refreshes the enrollment display
```

<details>
<summary>Answer</summary>

**Set B** is superior because:
- **Specific**: Names exact options (A, B, C, D, F), specifies rendering location (each enrollment row)
- **Testable**: Each criterion can be verified with a specific test
- **Complete**: Covers initial state, interaction, and result

**Set A** fails because:
- "Appears on the page" - where? which page?
- "Can be selected" - what happens when selected?
- "Works correctly" - not testable, means nothing

</details>

---

## 3.2 Parallel Story Planning

Not all stories must wait for others. Understanding dependencies lets you identify which stories can be worked simultaneously.

### When Stories Can Be Parallel

Stories are parallel-safe when:

1. **No shared files**: Different areas of the codebase
2. **No data dependencies**: One doesn't need output from another
3. **No integration point**: They don't connect until a later story

### Identifying Dependencies

Use a dependency map to visualize story relationships:

```
┌─────────────────────────────────────────────────────────────────┐
│                     Grade Management Epic                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│   ┌──────────────┐                                              │
│   │   Backend    │                                              │
│   │  (Story A)   │──────────┐                                   │
│   │ UpdateGrade  │          │                                   │
│   │   action     │          ▼                                   │
│   └──────────────┘    ┌──────────────┐                         │
│                       │   Frontend   │                         │
│                       │  (Story B)   │                         │
│                       │  Grade UI    │                         │
│                       └──────────────┘                         │
│                                                                  │
│   ┌──────────────┐                                              │
│   │     GPA      │  (Independent - no dependencies)            │
│   │  (Story C)   │                                              │
│   │ Calculation  │                                              │
│   └──────────────┘                                              │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

In this map:
- **Story A (Backend)** has no dependencies—can start immediately
- **Story B (Frontend)** depends on Story A—the UI needs the action to call
- **Story C (GPA)** is independent—different files, different feature

### Parallel Execution Strategy

When you have multiple independent stories:

1. **Draft all stories first** using @Scribe
2. **Identify the dependency graph**
3. **Execute independent stories simultaneously**
4. **Execute dependent stories when predecessors complete**

```
@Scribe Create three stories for the grade management epic:
1. Backend: Add UpdateGrade controller action
2. Frontend: Add grade dropdown to Instructor Index
3. Enhancement: Add GPA calculation to Student Details

Mark dependencies between stories.
```

### Exercise: Map Dependencies

For the Grade Management epic, determine which stories can run in parallel:

| Story | Description | Dependencies |
|-------|-------------|--------------|
| S01 | Add UpdateGrade controller action | ? |
| S02 | Add grade dropdown to Instructor Index | ? |
| S03 | Add GPA calculation to Student Details | ? |
| S04 | Add input validation for grades | ? |

<details>
<summary>Dependency Analysis</summary>

| Story | Dependencies | Can Parallel With |
|-------|--------------|-------------------|
| S01 | None | S03 |
| S02 | S01 (needs action to call) | - |
| S03 | None | S01 |
| S04 | S01 (validates action input) | S03 |

**Parallel Groups:**
- **Group 1**: S01, S03 (no dependencies between them)
- **Group 2**: S02, S04 (both depend on S01)

**Execution Order:**
1. Execute S01 and S03 in parallel
2. When S01 completes, execute S02 and S04 in parallel

</details>

### Using Scribe for Batch Story Creation

Scribe can draft multiple stories in a single interaction:

```
@Scribe Create implementation stories for these features from the Grade
Management epic. For each story, provide:
- Summary
- Assigned agent
- Acceptance criteria (3-5 items)
- Dependencies
- Complete agent prompt

Features:
1. Backend controller action for updating grades
2. Frontend UI for grade selection
3. GPA calculation display

Use Builder as the assigned agent for all implementation stories.
```

<details>
<summary>Why Batch Creation Works</summary>

**Benefits:**
- Scribe sees all stories together, ensuring consistent scope
- Dependencies are identified during creation, not after
- Agent prompts reference related stories appropriately

**When to batch vs. individual:**
- **Batch**: Related stories from same epic, similar scope
- **Individual**: Complex stories needing detailed discussion

</details>

---

## 3.3 Creating Stories for Grade Update

Now let's create complete stories for the grade management feature. Each story will be ready for immediate execution with Builder.

### Story A: Backend - UpdateGrade Action

Let's use Scribe to create the backend story:

```
@Scribe Create a story for adding the UpdateGrade action to InstructorsController.

Context:
- Contoso University .NET MVC application
- Need a POST action that updates an enrollment's grade
- Should accept enrollment ID and grade value
- Return appropriate HTTP status codes

Include complete agent prompt for Builder.
```

<details>
<summary>Complete Story: Backend - UpdateGrade Action</summary>

```markdown
# Story A: Add UpdateGrade Controller Action

## Summary

Add a POST action to InstructorsController that allows updating a student's
grade for a specific enrollment. This enables the grade management feature
by providing the backend API endpoint.

## Assigned Agent

**Builder** - This is an implementation task requiring .NET/C# development.

## Acceptance Criteria

- [ ] AC1: `UpdateGrade` POST action exists in InstructorsController
- [ ] AC2: Action accepts `enrollmentId` (int) and `grade` (string) parameters
- [ ] AC3: Valid grades (A, B, C, D, F, or null/empty) update the enrollment
- [ ] AC4: Invalid enrollment ID returns 404 Not Found
- [ ] AC5: Successful update returns 200 OK with updated enrollment data
- [ ] AC6: Changes persist to database

## Dependencies

- None (first implementation story)

## Agent Prompt

You are implementing the grade update backend for Contoso University.

**Context:**
- Repository: ContosoUniversity MVC application
- File: ContosoUniversity/Controllers/InstructorsController.cs
- The Enrollment entity has a Grade property (nullable enum or string)
- DbContext is already injected into the controller

**Tasks:**
1. Open InstructorsController.cs and review existing action patterns
2. Add a new POST action named `UpdateGrade`:
   ```csharp
   [HttpPost]
   public async Task<IActionResult> UpdateGrade(int enrollmentId, string grade)
   ```
3. Implement the action to:
   - Find the enrollment by ID
   - Return 404 if not found
   - Validate grade is A, B, C, D, F, or null/empty
   - Update the grade property
   - Save changes to database
   - Return 200 with the updated enrollment

4. Ensure proper error handling for database operations

**Output:**
- Modified InstructorsController.cs
- Confirm compilation succeeds
- List completed AC items

**Acceptance Criteria to verify:**
- [ ] AC1: UpdateGrade action exists
- [ ] AC2: Parameters are correct
- [ ] AC3: Valid grades work
- [ ] AC4: Invalid ID returns 404
- [ ] AC5: Success returns 200
- [ ] AC6: Database persistence works
```

</details>

### Story B: Frontend - Grade Editing UI

Now the frontend story that depends on Story A:

```
@Scribe Create a story for adding grade editing UI to the Instructor Index page.

Context:
- Builds on Story A (UpdateGrade action must exist)
- Instructor Index shows enrollments for selected course
- Need grade dropdown for each enrollment row
- Should submit to UpdateGrade action

Include complete agent prompt for Builder.
```

<details>
<summary>Complete Story: Frontend - Grade Editing UI</summary>

```markdown
# Story B: Add Grade Editing UI to Instructor Index

## Summary

Add a grade selection dropdown to each enrollment row on the Instructor Index
page. When an instructor selects a new grade, submit to the UpdateGrade action
and refresh the display.

## Assigned Agent

**Builder** - This is a frontend implementation task requiring Razor/HTML/JavaScript.

## Acceptance Criteria

- [ ] AC1: Grade dropdown renders for each enrollment in the selected course
- [ ] AC2: Dropdown options include: A, B, C, D, F, and (No Grade)
- [ ] AC3: Current grade is pre-selected in dropdown
- [ ] AC4: Selecting a new grade submits to UpdateGrade action
- [ ] AC5: Successful update refreshes the enrollment display without full page reload
- [ ] AC6: Error response displays user-friendly message

## Dependencies

- Story A: Backend - UpdateGrade Action (provides the endpoint to call)

## Agent Prompt

You are implementing the grade editing UI for Contoso University.

**Context:**
- Repository: ContosoUniversity MVC application
- View: ContosoUniversity/Views/Instructors/Index.cshtml
- The UpdateGrade action (from Story A) is available at POST /Instructors/UpdateGrade
- Enrollments are displayed in a table when a course is selected

**Tasks:**
1. Open Views/Instructors/Index.cshtml and locate the enrollment display section
2. Replace the static grade display with a dropdown:
   ```html
   <select class="grade-dropdown" data-enrollment-id="@enrollment.EnrollmentID">
       <option value="">No Grade</option>
       <option value="A">A</option>
       <option value="B">B</option>
       <option value="C">C</option>
       <option value="D">D</option>
       <option value="F">F</option>
   </select>
   ```
3. Set the current grade as selected
4. Add JavaScript to handle dropdown change:
   - POST to /Instructors/UpdateGrade
   - Send enrollmentId and grade
   - On success, show brief confirmation
   - On error, show error message and revert selection
5. Style the dropdown to fit the existing table design

**Output:**
- Modified Index.cshtml with grade dropdown
- JavaScript for form submission (inline or separate file)
- Confirm the dropdown renders and submits correctly
- List completed AC items

**Acceptance Criteria to verify:**
- [ ] AC1: Dropdown renders per enrollment
- [ ] AC2: All grade options present
- [ ] AC3: Current grade selected
- [ ] AC4: Selection triggers submit
- [ ] AC5: Success updates display
- [ ] AC6: Errors handled gracefully
```

</details>

### Story C: GPA Calculation

This story is independent and can be worked in parallel with Story A:

```
@Scribe Create a story for adding GPA calculation to the Student Details page.

Context:
- Independent of the grade update feature
- Student Details page shows enrollments and grades
- Add GPA calculation based on completed courses
- Standard 4.0 scale (A=4, B=3, C=2, D=1, F=0)

Include complete agent prompt for Builder.
```

<details>
<summary>Complete Story: GPA Calculation</summary>

```markdown
# Story C: Add GPA Calculation to Student Details

## Summary

Add a calculated GPA display to the Student Details page. The GPA should be
computed from the student's completed courses using the standard 4.0 scale.

## Assigned Agent

**Builder** - This requires C# implementation and Razor view modification.

## Acceptance Criteria

- [ ] AC1: GradeExtensions class exists with GPA calculation logic
- [ ] AC2: Grade point mapping: A=4.0, B=3.0, C=2.0, D=1.0, F=0.0
- [ ] AC3: Only enrollments with grades are included in GPA
- [ ] AC4: GPA displays on Student Details page with 2 decimal precision
- [ ] AC5: Students with no graded courses show "N/A" instead of GPA
- [ ] AC6: Unit tests exist for GPA calculation logic

## Dependencies

- None (independent feature)

## Agent Prompt

You are implementing GPA calculation for Contoso University.

**Context:**
- Repository: ContosoUniversity MVC application
- View: ContosoUniversity/Views/Students/Details.cshtml
- The Student model has an Enrollments collection
- Each Enrollment has a Grade property

**Tasks:**
1. Create a new file ContosoUniversity/Extensions/GradeExtensions.cs:
   ```csharp
   public static class GradeExtensions
   {
       public static decimal? CalculateGPA(this IEnumerable<Enrollment> enrollments)
       {
           // Implementation here
       }

       public static decimal GetGradePoints(Grade? grade)
       {
           // A=4, B=3, C=2, D=1, F=0
       }
   }
   ```

2. Implement the GPA calculation:
   - Filter to enrollments with grades
   - Convert each grade to points
   - Calculate average
   - Return null if no graded courses

3. Modify Views/Students/Details.cshtml:
   - Add GPA display in the student info section
   - Format to 2 decimal places
   - Show "N/A" if GPA is null

4. Create unit tests in ContosoUniversity.Tests:
   - Test each grade point value
   - Test GPA calculation with multiple grades
   - Test null handling for no grades

**Output:**
- GradeExtensions.cs with calculation logic
- Modified Details.cshtml with GPA display
- Unit tests for GPA calculation
- List completed AC items

**Acceptance Criteria to verify:**
- [ ] AC1: GradeExtensions exists
- [ ] AC2: Grade points correct
- [ ] AC3: Only graded enrollments counted
- [ ] AC4: GPA displays correctly
- [ ] AC5: N/A for no grades
- [ ] AC6: Tests exist and pass
```

</details>

### Validating Stories Before Execution

Before executing stories with Builder, validate them against the story writing standards:

```
@Scribe Evaluate Story A (Backend - UpdateGrade Action) against our story
writing standards rubric at docs/standards/story-writing-standards-rubric.md.

Score each of the 5 criteria and provide:
1. Scores with specific observations
2. Overall verdict (READY FOR DEV / NEEDS REFINEMENT / NEEDS REWRITE)
3. Specific improvements if any criterion scores below 6
```

**Quality Gate Checklist:**

| Criterion | Minimum Score | Story A | Story B | Story C |
|-----------|---------------|---------|---------|---------|
| AC Quality | 6 | | | |
| Testability | 6 | | | |
| Scope Clarity | 6 | | | |
| Technical Detail | 6 | | | |
| Pattern Alignment | 6 | | | |
| **Overall** | **7** | | | |

> **Tip**: Use `.github/prompts/grade-story.prompt.md` for quick evaluations.

<details>
<summary>Example: Story A Evaluation</summary>

```markdown
## Story Evaluation: Story A - Backend UpdateGrade Action

| Criterion | Score | Notes |
|-----------|-------|-------|
| AC Quality | 8/10 | 6 specific, testable ACs covering happy path and errors |
| Testability | 8/10 | Can derive unit tests for each AC directly |
| Scope Clarity | 9/10 | Single action, clear boundaries, no scope creep risk |
| Technical Detail | 7/10 | Controller and parameters specified; DI approach implied |
| Pattern Alignment | 8/10 | Follows existing controller patterns |
| **Overall** | **8.0/10** | |

**Verdict**: READY FOR DEV

**Minor suggestions**:
- Consider adding AC for concurrent update handling
- Specify logging requirements for audit trail
```

</details>

**If a story needs refinement:**

```
@Scribe Story A scored 5/10 on Technical Detail Sufficiency.
Please enhance the story with:
1. Specific file paths to modify
2. Reference to similar existing implementations
3. Database/model details the implementer needs
```

### Executing the Stories

With all three stories defined, here's the execution plan:

```
┌─────────────────────────────────────────────────────────────────┐
│                     Execution Timeline                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  Phase 1 (Parallel):                                            │
│  ┌──────────────┐    ┌──────────────┐                          │
│  │   Story A    │    │   Story C    │                          │
│  │   Backend    │    │     GPA      │                          │
│  │  @Builder    │    │   @Builder   │                          │
│  └──────────────┘    └──────────────┘                          │
│         │                                                        │
│         ▼                                                        │
│  Phase 2:                                                        │
│  ┌──────────────┐                                               │
│  │   Story B    │                                               │
│  │   Frontend   │                                               │
│  │  @Builder    │                                               │
│  └──────────────┘                                               │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

To execute a story, invoke Builder with the complete prompt:

```
@Builder [paste the complete agent prompt from the story]
```

### Knowledge Check

1. Why is Story C independent while Story B depends on Story A?

<details>
<summary>Answer</summary>

**Story C (GPA)** is independent because:
- It modifies different files (GradeExtensions, Students/Details)
- It doesn't call or depend on UpdateGrade action
- It's a separate feature area (viewing vs. editing)

**Story B (Frontend)** depends on Story A because:
- The dropdown needs to POST to UpdateGrade action
- The action must exist before the UI can call it
- Testing the UI requires the backend to be functional

</details>

2. What makes a good acceptance criterion?

<details>
<summary>Answer</summary>

Good acceptance criteria are:
- **Testable**: Can write a specific test to verify
- **Specific**: Describes one clear behavior
- **Observable**: Can see the result
- **Independent**: Tests one thing, not compound conditions

</details>

---

## Summary

In this lab, you learned:

- **Story structure** includes Summary, Agent, AC, Dependencies, and Prompt
- **Acceptance criteria** must be testable, specific, and observable
- **INVEST principles** validate story structure before implementation
- **Agent assignment** matches the work type to agent expertise
- **Agent prompts** need context, tasks, and expected output
- **Parallel execution** is possible when stories don't share dependencies
- **Scribe** can draft multiple related stories efficiently
- **Story validation** ensures stories meet quality standards before execution
- **Builder** executes implementation prompts to deliver functionality

**Key takeaway:** A well-structured story with complete acceptance criteria and a ready-to-execute prompt enables efficient multi-agent workflows. Always validate stories against the rubric before execution—the time spent on quality gates pays off in cleaner implementation.

**Standards Reference:**
- Rubric: `docs/standards/story-writing-standards-rubric.md`
- Grading Skill: `.github/skills/story-writing-standards/SKILL.md`
- Grading Prompt: `.github/prompts/grade-story.prompt.md`

**Next:** In Lab 4, you'll execute these stories with Builder and observe the full development workflow.
