# 4 - Executing Stories with Agents

In this lab you will implement stories using multi-agent orchestration and learn to manage session handoffs.

> Duration: 30-45 minutes

References:
- [GitHub Copilot Chat](https://docs.github.com/en/copilot/using-github-copilot/asking-github-copilot-questions-in-your-ide)
- [Context Management Best Practices](https://docs.github.com/en/copilot/customizing-copilot)
- [Session Continuity Patterns](https://docs.github.com/en/copilot/using-github-copilot/best-practices-for-using-github-copilot)

---

## 4.1 Single-Session Implementation

Some stories are small enough to complete in a single conversation with an agent. Understanding when to use single-session implementation helps you work efficiently.

### When to Use Single-Session

Single-session implementation works best when:

| Criteria | Example |
|----------|---------|
| **Small scope** | Add one method, modify one view |
| **Clear requirements** | Well-defined acceptance criteria |
| **Self-contained** | No dependencies on other in-progress work |
| **Quick verification** | Can test completion immediately |

### The Single-Session Workflow

```
┌─────────────────────────────────────────────────────────────────┐
│                  Single-Session Workflow                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│   ┌──────────────┐                                              │
│   │  Load Story  │  Read the story file, understand AC          │
│   └──────────────┘                                              │
│          │                                                       │
│          ▼                                                       │
│   ┌──────────────┐                                              │
│   │Execute Prompt│  Invoke @Builder with the agent prompt       │
│   └──────────────┘                                              │
│          │                                                       │
│          ▼                                                       │
│   ┌──────────────┐                                              │
│   │ Verify AC    │  Check each criterion is met                 │
│   └──────────────┘                                              │
│          │                                                       │
│          ▼                                                       │
│   ┌──────────────┐                                              │
│   │Mark Complete │  Update story status, close story            │
│   └──────────────┘                                              │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### Exercise: Implement GradeExtensions

The GPA Calculation story (Story C from Lab 3) is perfect for single-session implementation. It's self-contained—new file, isolated functionality, clear acceptance criteria.

**Step 1: Load the story**

Open `docs/stories/story-gpa-calculation.md` (or reference from Lab 3) and review:
- Acceptance criteria
- Agent prompt
- Expected output

**Step 2: Invoke Builder**

```
@Builder Create the GradeExtensions class for Contoso University.

**Context:**
- Repository: ContosoUniversity MVC application
- Create new file: ContosoUniversity/Extensions/GradeExtensions.cs
- The Grade enum exists with values: A, B, C, D, F

**Tasks:**
1. Create GradeExtensions.cs with a static class
2. Add ToGradePoints extension method:
   - A = 4.0, B = 3.0, C = 2.0, D = 1.0, F = 0.0
   - Return decimal for precision
3. Add CalculateGPA extension method for IEnumerable<Enrollment>:
   - Filter to enrollments with grades
   - Calculate average of grade points
   - Return null if no graded enrollments

**Output:**
- Complete GradeExtensions.cs file
- Confirm it compiles
```

**Step 3: Observe Builder's response**

Watch Builder:
- Create the file in the correct location
- Implement both methods
- Follow C# conventions matching the codebase

<details>
<summary>Expected Implementation</summary>

```csharp
using ContosoUniversity.Models;

namespace ContosoUniversity.Extensions
{
    public static class GradeExtensions
    {
        public static decimal ToGradePoints(this Grade grade)
        {
            return grade switch
            {
                Grade.A => 4.0m,
                Grade.B => 3.0m,
                Grade.C => 2.0m,
                Grade.D => 1.0m,
                Grade.F => 0.0m,
                _ => 0.0m
            };
        }

        public static decimal? CalculateGPA(this IEnumerable<Enrollment> enrollments)
        {
            var gradedEnrollments = enrollments
                .Where(e => e.Grade.HasValue)
                .ToList();

            if (!gradedEnrollments.Any())
                return null;

            return gradedEnrollments
                .Average(e => e.Grade!.Value.ToGradePoints());
        }
    }
}
```

</details>

**Step 4: Verify Acceptance Criteria**

Check each criterion from the story:

| AC | Criterion | Verification |
|----|-----------|--------------|
| AC1 | GradeExtensions class exists | File created in Extensions folder |
| AC2 | Grade point mapping correct | A=4, B=3, C=2, D=1, F=0 |
| AC3 | Only graded enrollments counted | Where clause filters nulls |
| AC4 | Returns null for no grades | Check for empty list handling |

**Step 5: Mark Complete**

Update the story file:
```markdown
## Status: Complete

### Completion Notes
- GradeExtensions.cs created in ContosoUniversity/Extensions/
- All AC verified
- Compiles successfully
- Completed: [date] by @Builder
```

### Knowledge Check

1. What makes Story C (GPA Calculation) suitable for single-session implementation?

<details>
<summary>Answer</summary>

**Story C is single-session suitable because:**
- **Small scope**: One new file, two methods
- **Self-contained**: No dependencies on other stories
- **Clear output**: Can verify by compilation
- **Isolated functionality**: Doesn't touch existing code

Stories involving multiple files, complex integrations, or requiring debugging typically need multi-session.

</details>

---

## 4.2 Multi-Session Handoffs

Larger stories often require multiple conversations. Context limits, time breaks, or complex debugging can interrupt work. Handoff documents ensure continuity between sessions.

### When Handoffs Are Needed

| Situation | Why Handoff Needed |
|-----------|-------------------|
| **Context limit reached** | Agent needs fresh context to continue |
| **Time break** | Resuming work tomorrow or later |
| **Complex debugging** | Need to research before continuing |
| **Review checkpoint** | Want human review before proceeding |
| **Agent switch** | Different agent needed for next phase |

### Handoff Document Structure

A handoff document captures everything needed to resume work:

```
┌─────────────────────────────────────────────────────────────────┐
│                     HANDOFF DOCUMENT                             │
├─────────────────────────────────────────────────────────────────┤
│  Session Info                                                    │
│  - Date/time                                                     │
│  - Story being worked                                            │
│  - Agent used                                                    │
├─────────────────────────────────────────────────────────────────┤
│  Current Status                                                  │
│  - What was the goal?                                            │
│  - What was completed?                                           │
│  - What remains?                                                 │
├─────────────────────────────────────────────────────────────────┤
│  Context                                                         │
│  - Files modified (with paths)                                   │
│  - Key decisions made                                            │
│  - Problems encountered                                          │
├─────────────────────────────────────────────────────────────────┤
│  Next Session Prompt                                             │
│  - Ready-to-use prompt for resuming                              │
│  - Includes all necessary context                                │
├─────────────────────────────────────────────────────────────────┤
│  Blockers (if any)                                               │
│  - What's preventing progress                                    │
│  - What needs to be resolved                                     │
└─────────────────────────────────────────────────────────────────┘
```

### Handoff Document Template

Use this template for all handoffs:

```markdown
# Handoff: [Story Name]

## Session Info
- **Date**: YYYY-MM-DD HH:MM
- **Story**: [Story ID and title]
- **Agent**: @Builder / @Scout / @Scribe / @Sage

## Status Summary
**Goal**: [What we were trying to accomplish]

**Completed**:
- [ ] Item 1
- [x] Item 2 (done)
- [x] Item 3 (done)

**Remaining**:
- [ ] Item 4
- [ ] Item 5

## Work Done This Session

### Files Modified
| File | Change |
|------|--------|
| `path/to/file.cs` | Added UpdateGrade action |
| `path/to/view.cshtml` | Added grade dropdown |

### Key Decisions
1. [Decision 1 and rationale]
2. [Decision 2 and rationale]

### Problems Encountered
- [Problem 1]: [How resolved or status]
- [Problem 2]: [How resolved or status]

## Context for Next Session

[Paragraph explaining current state and what's important to know]

## Next Session Prompt

```
@Builder Continue implementing [Story Name].

**Previous Session Summary:**
[Brief summary of what was done]

**Current State:**
- [File 1] has [change]
- [File 2] needs [remaining work]

**Remaining Tasks:**
1. [Task 1]
2. [Task 2]

**Acceptance Criteria to complete:**
- [ ] [Remaining AC 1]
- [ ] [Remaining AC 2]
```

## Blockers
- [ ] None / [Description of blocker]
```

### Exercise: Create a Handoff Mid-Implementation

Let's practice creating a handoff document for the Backend story (Story A) as if we completed half the work.

**Scenario**: You've added the `UpdateGrade` action signature but haven't implemented the body or tested it.

**Step 1: Document current state**

```markdown
# Handoff: Story A - Backend UpdateGrade Action

## Session Info
- **Date**: 2024-01-15 14:30
- **Story**: S01 - Add UpdateGrade Controller Action
- **Agent**: @Builder

## Status Summary
**Goal**: Add UpdateGrade POST action to InstructorsController

**Completed**:
- [x] AC1: UpdateGrade action exists (signature only)
- [x] AC2: Parameters are correct (enrollmentId, grade)

**Remaining**:
- [ ] AC3: Valid grades update the enrollment
- [ ] AC4: Invalid enrollment ID returns 404
- [ ] AC5: Successful update returns 200
- [ ] AC6: Database persistence works

## Work Done This Session

### Files Modified
| File | Change |
|------|--------|
| `Controllers/InstructorsController.cs` | Added UpdateGrade signature |

### Key Decisions
1. Using `[HttpPost]` attribute for the action
2. Returning `IActionResult` for flexible responses

### Problems Encountered
- None yet

## Context for Next Session

The UpdateGrade action exists but has no implementation. The method
signature follows the existing patterns in InstructorsController.
Next session needs to implement the body: find enrollment, validate
grade, update, save, return appropriate status codes.

## Next Session Prompt

```
@Builder Continue implementing the UpdateGrade action.

**Previous Session:**
Added the action signature to InstructorsController.cs

**Current State:**
- UpdateGrade action exists with (int enrollmentId, string grade) params
- Method body is empty/placeholder

**Remaining Tasks:**
1. Find enrollment by ID, return 404 if not found
2. Validate grade is A, B, C, D, F, or null
3. Update enrollment.Grade property
4. Save changes via DbContext
5. Return 200 with updated enrollment

**Acceptance Criteria to complete:**
- [ ] AC3: Valid grades update the enrollment
- [ ] AC4: Invalid enrollment ID returns 404
- [ ] AC5: Successful update returns 200
- [ ] AC6: Database persistence works
```

## Blockers
- None
```

**Step 2: Save the handoff**

Create the file: `docs/handoffs/handoff-2024-01-15-story-a.md`

**Step 3: Resume in new session**

When ready to continue, copy the "Next Session Prompt" and invoke Builder:

```
@Builder Continue implementing the UpdateGrade action.

**Previous Session:**
Added the action signature to InstructorsController.cs

[... rest of prompt ...]
```

### Handoff Best Practices

| Practice | Why It Matters |
|----------|---------------|
| **Be specific about files** | Full paths prevent confusion |
| **Include rationale for decisions** | Future sessions understand why |
| **Make prompts copy-pasteable** | Reduces friction resuming |
| **Track AC explicitly** | Clear what's done vs remaining |
| **Note blockers prominently** | Don't repeat failed approaches |

### Using Sage for Handoff Creation

Sage can help create handoff documents:

```
@Sage Create a handoff document for Story A - Backend UpdateGrade Action.

Current state:
- UpdateGrade action signature added
- Implementation not started
- AC1 and AC2 complete, AC3-6 remaining

Include a prompt for @Builder to continue the work.
```

<details>
<summary>Why Use Sage for Handoffs?</summary>

**Benefits:**
- Sage understands project management conventions
- Consistent formatting across handoffs
- Includes all necessary sections automatically
- Generates effective continuation prompts

**When to write manually:**
- Quick, simple handoffs
- Debugging sessions with complex context
- When you need specific technical details

</details>

---

## 4.3 Implementing with @Builder

Now let's execute a complete story with Builder, observing the full implementation workflow.

### Preparing for Implementation

Before invoking Builder, ensure you have:

1. **The complete story document** with all acceptance criteria
2. **The agent prompt** ready to execute
3. **Any prerequisite stories** completed
4. **The codebase** in a clean state (committed, no pending changes)

### Executing Story A: Backend UpdateGrade Action

**Step 1: Verify prerequisites**

This story has no dependencies (first in the sequence). Verify the codebase builds:

```bash
dotnet build
```

**Step 2: Invoke Builder with the full prompt**

```
@Builder You are implementing the grade update backend for Contoso University.

**Context:**
- Repository: ContosoUniversity MVC application
- File: ContosoUniversity/Controllers/InstructorsController.cs
- The Enrollment entity has a Grade property (nullable enum)
- DbContext is already injected into the controller

**Tasks:**
1. Add IRepository<Enrollment> or use DbContext.Enrollments for data access
2. Add a new POST action named UpdateGrade:
   ```csharp
   [HttpPost]
   public async Task<IActionResult> UpdateGrade(int enrollmentId, string grade)
   ```
3. Implement the action to:
   - Find the enrollment by ID using GetQueryable with Include for eager loading
   - Return 404 if not found
   - Validate grade is A, B, C, D, F, or null/empty
   - Update the grade property
   - Save changes to database
   - Return 200 with the updated enrollment

**Output:**
- Modified InstructorsController.cs with UpdateGrade action
- Confirm compilation succeeds
- List completed AC items
```

**Step 3: Monitor Builder's actions**

Watch as Builder:
- Opens InstructorsController.cs
- Analyzes existing patterns (how other actions work)
- Adds necessary using statements
- Implements the UpdateGrade action
- Follows the existing code style

**Step 4: Verify each acceptance criterion**

| AC | Expected | How to Verify |
|----|----------|---------------|
| AC1 | Action exists | Search for `public async Task<IActionResult> UpdateGrade` |
| AC2 | Correct parameters | Check signature has `int enrollmentId, string grade` |
| AC3 | Valid grades work | Review validation logic in action body |
| AC4 | 404 for invalid ID | Check for `NotFound()` return |
| AC5 | 200 on success | Check for `Ok()` or similar return |
| AC6 | Database persistence | Check for `SaveChangesAsync()` call |

<details>
<summary>Expected Implementation</summary>

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateGrade(int enrollmentId, string grade)
{
    var enrollment = await _context.Enrollments
        .Include(e => e.Student)
        .Include(e => e.Course)
        .FirstOrDefaultAsync(e => e.EnrollmentID == enrollmentId);

    if (enrollment == null)
    {
        return NotFound();
    }

    // Validate and parse grade
    if (string.IsNullOrEmpty(grade))
    {
        enrollment.Grade = null;
    }
    else if (Enum.TryParse<Grade>(grade, out var parsedGrade))
    {
        enrollment.Grade = parsedGrade;
    }
    else
    {
        return BadRequest("Invalid grade value. Valid values are: A, B, C, D, F, or empty.");
    }

    await _context.SaveChangesAsync();

    return Ok(new {
        EnrollmentId = enrollment.EnrollmentID,
        Grade = enrollment.Grade?.ToString() ?? "No Grade",
        StudentName = enrollment.Student.FullName,
        CourseName = enrollment.Course.Title
    });
}
```

</details>

**Step 5: Handle issues**

If Builder encounters problems:

1. **Compilation error**: Ask Builder to read the error and fix it
2. **Missing dependency**: Builder may need to add a using statement or inject a service
3. **Pattern mismatch**: Point Builder to an existing action to follow as template

Example correction prompt:

```
@Builder The UpdateGrade action is missing the ValidateAntiForgeryToken
attribute that other POST actions have. Please add it to maintain consistency
with the existing patterns.
```

**Step 6: Mark story complete**

Once all AC are verified:

```markdown
# Story A: Add UpdateGrade Controller Action

## Status: Complete

### Acceptance Criteria
- [x] AC1: UpdateGrade POST action exists
- [x] AC2: Accepts enrollmentId and grade parameters
- [x] AC3: Valid grades update the enrollment
- [x] AC4: Invalid enrollment ID returns 404
- [x] AC5: Successful update returns 200 OK
- [x] AC6: Changes persist to database

### Completion Notes
- Implemented in InstructorsController.cs
- Follows existing action patterns
- Includes validation for grade values
- Completed: 2024-01-15 by @Builder
```

### Exercise: Execute Story B (Frontend)

Now that Story A is complete, execute Story B which depends on it.

```
@Builder You are implementing the grade editing UI for Contoso University.

**Context:**
- Repository: ContosoUniversity MVC application
- View: ContosoUniversity/Views/Instructors/Index.cshtml
- The UpdateGrade action (from Story A) is available at POST /Instructors/UpdateGrade
- Enrollments are displayed in a table when a course is selected

**Tasks:**
1. Open Views/Instructors/Index.cshtml and locate the enrollment display section
2. Replace the static grade display with a form containing a dropdown:
   ```html
   <form asp-action="UpdateGrade" method="post">
       <input type="hidden" name="enrollmentId" value="@enrollment.EnrollmentID" />
       <select name="grade" onchange="this.form.submit()">
           <option value="">No Grade</option>
           <option value="A">A</option>
           <option value="B">B</option>
           <option value="C">C</option>
           <option value="D">D</option>
           <option value="F">F</option>
       </select>
   </form>
   ```
3. Set the current grade as selected using @(enrollment.Grade == Grade.A ? "selected" : "")
4. Style the dropdown to fit the existing table design

**Output:**
- Modified Index.cshtml with grade dropdown
- Confirm the dropdown renders when viewing instructor courses
- List completed AC items
```

<details>
<summary>Verification Steps</summary>

1. **Build the project**: `dotnet build`
2. **Run the application**: `dotnet run`
3. **Navigate to Instructors Index**: `/Instructors`
4. **Select an instructor and course**
5. **Verify dropdown appears** for each enrollment
6. **Select a grade and verify** it saves (page refreshes with updated value)

</details>

### Knowledge Check

1. Why do we verify the codebase builds before starting implementation?

<details>
<summary>Answer</summary>

**Reasons to build first:**
- Confirms starting state is clean
- Any errors during implementation are from new changes
- Prevents confusion about pre-existing issues
- Establishes a known-good baseline

If the build fails before you start, fix that first.

</details>

2. What should you do if Builder's implementation doesn't match the existing code style?

<details>
<summary>Answer</summary>

**Correction approach:**
1. Point Builder to a specific file/method as a style reference
2. Ask explicitly: "Please match the pattern used in [specific action]"
3. Be specific about what's different (naming, formatting, attribute usage)

Example: "@Builder The UpdateGrade action should follow the same async pattern as the Edit action. Please use async/await consistently."

</details>

---

## 4.4 Progress Tracking

Maintaining visibility into epic progress ensures nothing is forgotten and enables effective handoffs between sessions.

### Story Status Updates

After completing each story, update its status:

```markdown
## Status: Complete | In Progress | Blocked | Not Started
```

**Status definitions:**

| Status | Meaning | Actions |
|--------|---------|---------|
| **Not Started** | Work hasn't begun | Load prompt and start |
| **In Progress** | Actively being worked | Continue or create handoff |
| **Blocked** | Cannot proceed | Document blocker, get help |
| **Complete** | All AC verified | Mark checkboxes, add notes |

### Epic Progress Tracker

Maintain a progress tracker in your epic document:

```markdown
## Progress Tracker

### Phase 1: Backend Foundation
| Story | Status | Completed | Notes |
|-------|--------|-----------|-------|
| S01 - UpdateGrade Action | Complete | 2024-01-15 | @Builder |
| S04 - Database Flexibility | Not Started | - | - |

### Phase 2: Frontend Implementation
| Story | Status | Completed | Notes |
|-------|--------|-----------|-------|
| S02 - Grade Dropdown UI | In Progress | - | Handoff created |

### Phase 3: Enhancement
| Story | Status | Completed | Notes |
|-------|--------|-----------|-------|
| S03 - GPA Calculation | Complete | 2024-01-15 | @Builder |
```

### Updating the Epic After Story Completion

When a story completes:

1. **Update story status table**
   ```markdown
   | S01 | Grade Update Backend | Complete | 2024-01-15 |
   ```

2. **Check the Definition of Done item**
   ```markdown
   ## Definition of Done
   - [x] Instructors can update grades from Index page
   - [ ] Grade changes persist to database
   ```

3. **Add session note**
   ```markdown
   ### Session History
   - 2024-01-15: Completed S01 (Backend) and S03 (GPA) with @Builder
   - 2024-01-16: Started S02 (Frontend), created handoff
   ```

### Exercise: Update Epic Progress

Given this scenario:
- Story A (Backend) - Completed today
- Story B (Frontend) - Started, created handoff
- Story C (GPA) - Completed today
- Story D (Database) - Not started

Update the epic progress tracker:

<details>
<summary>Solution</summary>

```markdown
## Progress Tracker

### Current Sprint: Grade Management Enhancement

| ID | Story | Agent | Status | Date | Notes |
|----|-------|-------|--------|------|-------|
| S01 | UpdateGrade Action | @Builder | Complete | 2024-01-15 | Backend endpoint ready |
| S02 | Grade Dropdown UI | @Builder | In Progress | - | Handoff: handoff-2024-01-15-story-b.md |
| S03 | GPA Calculation | @Builder | Complete | 2024-01-15 | GradeExtensions.cs created |
| S04 | Database Flexibility | @Builder | Not Started | - | Blocked by S02 completion |

### Session History

#### 2024-01-15
- **Agent**: @Builder
- **Completed**: S01 (Backend), S03 (GPA)
- **Started**: S02 (Frontend)
- **Handoff**: Created for S02, dropdown partially implemented
- **Next Session**: Complete S02 frontend integration

### Definition of Done
- [x] Backend UpdateGrade action exists and works
- [ ] Frontend grade dropdown submits correctly
- [x] GPA calculation displays on Student Details
- [ ] Database supports both SQLite and SQL Server
- [ ] All tests pass

### Blockers
- None currently
```

</details>

### Creating Completion Handoffs

When finishing work for the day (even if all stories aren't done), create a completion handoff:

```markdown
# Session Completion Handoff

## Session Summary
- **Date**: 2024-01-15
- **Epic**: Grade Management Enhancement
- **Duration**: 2 hours

## What Was Accomplished
1. Story S01 (Backend) - Complete
   - UpdateGrade action in InstructorsController
   - All 6 AC verified

2. Story S03 (GPA) - Complete
   - GradeExtensions.cs created
   - All 6 AC verified

3. Story S02 (Frontend) - Partial
   - Dropdown markup added
   - Selection handling not yet working
   - See handoff-2024-01-15-story-b.md

## State of the Codebase
- All changes committed to branch `feature/grade-management`
- Build passes
- No test failures
- 2 of 4 stories complete

## Recommended Next Steps
1. Complete S02 using the handoff prompt
2. Verify S02 integration with S01 (grades actually save)
3. Begin S04 (Database Flexibility) if time permits

## Files Changed This Session
| File | Stories |
|------|---------|
| `Controllers/InstructorsController.cs` | S01 |
| `Extensions/GradeExtensions.cs` | S03 (new) |
| `Views/Instructors/Index.cshtml` | S02 (partial) |
```

### Lessons Learned Section

Track insights that help future implementation:

```markdown
## Lessons Learned

### What Worked Well
- Single-session for small, self-contained stories (S03)
- Builder follows existing patterns when pointed to examples
- Creating handoffs mid-story prevents lost context

### What Could Improve
- Verify UI changes in browser before marking complete
- Include validation tests in acceptance criteria
- Break frontend story into smaller chunks (markup, JS, styling)

### Technical Notes
- InstructorsController uses async pattern throughout
- Grade enum doesn't include null - use nullable Grade? type
- Existing JavaScript uses jQuery - stay consistent
```

### Knowledge Check

1. When should you create a handoff document?

<details>
<summary>Answer</summary>

**Create handoffs when:**
- Stopping work on an incomplete story
- Context window is getting full
- Taking a break (lunch, end of day)
- Need to switch to a different agent
- Want a review checkpoint before continuing
- Encountered a blocker that needs research

**Don't need handoff when:**
- Story is complete with all AC verified
- Just committed and taking 5-minute break
- Switching stories but continuing same session

</details>

2. What information is essential in an epic progress tracker?

<details>
<summary>Answer</summary>

**Essential information:**
- Story ID and title
- Current status (Not Started/In Progress/Blocked/Complete)
- Completion date (for finished stories)
- Agent used
- Any blockers or handoff references
- Session history for context

**Also valuable:**
- Definition of Done checklist
- Files modified per story
- Lessons learned

</details>

---

## Summary

In this lab, you learned:

- **Single-session implementation** works for small, self-contained stories with clear AC
- **The implementation workflow** is: Load Story → Execute Prompt → Verify AC → Mark Complete
- **Handoff documents** capture state for resuming work across sessions
- **Handoff structure** includes session info, status, context, and continuation prompt
- **Builder** executes implementation prompts by following existing codebase patterns
- **Verifying AC** means testing each criterion explicitly, not assuming completion
- **Progress tracking** maintains epic visibility through status tables and session notes
- **Completion handoffs** summarize session accomplishments and recommended next steps

**Key takeaway:** Effective multi-agent orchestration is as much about managing context and continuity as it is about executing prompts. Good handoffs and progress tracking ensure work isn't lost and can be resumed efficiently.

**The complete workflow:**

```
┌─────────────────────────────────────────────────────────────────┐
│              Full Implementation Cycle                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│   ┌──────────────┐    ┌──────────────┐    ┌──────────────┐     │
│   │    Epic      │───▶│   Stories    │───▶│   Execute    │     │
│   │  (Lab 2)     │    │  (Lab 3)     │    │  (Lab 4)     │     │
│   └──────────────┘    └──────────────┘    └──────────────┘     │
│                                                  │               │
│                                                  ▼               │
│                              ┌────────────────────────────────┐ │
│                              │        Verify & Track          │ │
│                              │  • Check each AC               │ │
│                              │  • Update story status         │ │
│                              │  • Create handoffs if needed   │ │
│                              │  • Update epic progress        │ │
│                              └────────────────────────────────┘ │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

Congratulations! You've completed the multi-agent orchestration lab series. You now have the skills to:
- Analyze brownfield codebases with Scout
- Create structured epics with Scribe
- Write implementation-ready stories with complete prompts
- Execute stories with Builder
- Manage context across sessions with handoffs
- Track progress through epic completion
