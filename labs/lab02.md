# 2 - Creating Brownfield Epics

In this lab you will analyze an existing codebase and create an epic for a new feature enhancement.

> Duration: 20-30 minutes

References:
- [Brownfield Development](https://en.wikipedia.org/wiki/Brownfield_(software_development))
- [Epic Documentation Best Practices](https://www.atlassian.com/agile/project-management/epics)
- [GitHub Copilot Custom Agents](https://docs.github.com/en/copilot)

---

## 2.1 Analyzing the Existing Codebase

Before you can propose enhancements to an application, you need to understand what already exists. This is the essence of brownfield development—working with existing code rather than starting from scratch.

### What is Brownfield Development?

**Brownfield** refers to software development on existing systems. Unlike greenfield (new) projects, brownfield work requires:

- Understanding existing architecture before making changes
- Respecting established patterns and conventions
- Identifying enhancement opportunities that align with current design
- Assessing technical debt and deciding what to address

### Using Scout for Codebase Analysis

Scout (the Brownfield Analyst agent) is designed specifically for exploring and understanding existing codebases. Scout approaches legacy code with curiosity rather than judgment, recognizing that past design decisions had context that may not be immediately visible.

**Scout's key capabilities:**
- Navigating unfamiliar codebases and tracing dependencies
- Identifying architectural patterns (MVC, Repository, Unit of Work)
- Cataloging technical debt with severity assessments
- Finding high-value, low-risk improvement areas

### Hands-On: Analyze InstructorsController

Let's use Scout to analyze a key file in the Contoso University application.

1. Open GitHub Copilot Chat (Ctrl+Alt+I / Cmd+Alt+I)
2. Invoke Scout by typing `@Scout` or `@brownfield-analyst`
3. Ask Scout to analyze the instructors controller:

```
@Scout Please analyze the ContosoUniversity/Controllers/InstructorsController.cs file.

Focus on:
1. What CRUD operations does it support?
2. How does it handle related data (courses, enrollments)?
3. Are there any patterns that suggest how we might add new features?
```

**Observe Scout's response:**
- Does it identify the MVC pattern?
- Does it trace relationships to other entities?
- Does it suggest potential enhancement areas?

<details>
<summary>Example Scout Analysis Output</summary>

A thorough Scout analysis might reveal:

**Current State:**
- Standard CRUD operations: Index, Details, Create, Edit, Delete
- Uses Entity Framework for data access
- Eager loading of related Courses via `.Include()`
- Edit action handles course assignments through a checkbox list

**Patterns Identified:**
- Repository pattern through DbContext
- ViewModels for complex views (AssignedCourseData)
- Separate concerns: Controller handles HTTP, DbContext handles persistence

**Enhancement Opportunity:**
- The Index view shows instructors with their courses and enrollments (read-only)
- No ability to update student grades from this view
- Adding grade update would follow existing patterns for editing related data

</details>

### Exercise: Map the Data Flow

Use Scout to trace how student enrollment data flows through the application.

```
@Scout Trace the data flow for student enrollments in this application.

1. Where is enrollment data stored (which model/entity)?
2. Which views display enrollment information?
3. What controller actions interact with enrollments?
```

<details>
<summary>Discussion</summary>

**Data Model:**
- `Enrollment` entity links `Student` and `Course` with a `Grade` property
- Grade is nullable (students can be enrolled without a grade)

**Views displaying enrollments:**
- `Instructors/Index.cshtml` - shows enrollments for selected course
- `Students/Details.cshtml` - shows student's enrollments and grades
- `Courses/Details.cshtml` - shows enrolled students

**Controller interactions:**
- Currently read-only display
- No existing action to update grades
- This is our enhancement opportunity!

</details>

---

## 2.2 Epic Document Structure

An epic is a large body of work that can be broken down into smaller user stories. For brownfield development, epics typically represent feature additions or significant improvements to existing functionality.

### Why Write Epics?

Epics serve multiple purposes:

| Purpose | Benefit |
|---------|---------|
| **Scope Definition** | Clearly defines what's included and excluded |
| **Context Capture** | Documents the "why" behind the work |
| **Story Organization** | Groups related stories for coordinated delivery |
| **Progress Tracking** | Enables visibility into large initiatives |
| **Agent Context** | Provides AI agents with consistent background |

### Epic Document Components

A well-structured epic contains:

```
┌─────────────────────────────────────────────────────┐
│                      EPIC                           │
├─────────────────────────────────────────────────────┤
│  Overview                                           │
│  - What problem does this solve?                    │
│  - Why is it valuable?                              │
├─────────────────────────────────────────────────────┤
│  Learning Objectives / Business Value               │
│  - Measurable outcomes                              │
│  - Success criteria                                 │
├─────────────────────────────────────────────────────┤
│  Feature Context                                    │
│  - Technical background                             │
│  - Current state vs. desired state                  │
├─────────────────────────────────────────────────────┤
│  Stories Table                                      │
│  - ID, Title, Agent, Status                         │
├─────────────────────────────────────────────────────┤
│  Progress Tracker                                   │
│  - Phases and milestones                            │
│  - Session history                                  │
├─────────────────────────────────────────────────────┤
│  Definition of Done                                 │
│  - Checklist of completion criteria                 │
└─────────────────────────────────────────────────────┘
```

### Example: Epic Overview Section

Here's how to write an effective Overview:

```markdown
## Overview

Enable instructors to update student grades directly from the Instructor Index
page, eliminating the need to navigate to individual enrollment records. This
enhancement improves workflow efficiency for instructors managing course grades
and provides students with real-time GPA calculation visibility.
```

**Key elements:**
- Starts with the capability being added
- Explains the user benefit (workflow efficiency)
- Mentions affected user groups (instructors, students)

### Example: Stories Table

Stories should be sequenced logically:

| ID | Story | Agent | Status |
|----|-------|-------|--------|
| S01 | Add Grade Update UI to Instructor Index | Builder | Not Started |
| S02 | Implement Grade Update Controller Action | Builder | Not Started |
| S03 | Add GPA Calculation to Student Details | Builder | Not Started |
| S04 | Add Database Provider Flexibility | Builder | Not Started |

**Principles for story sequencing:**
- Foundation work first (data access, models)
- UI changes after backend support exists
- Integration and polish last

### Knowledge Check

Which of these is a better Overview statement?

**Option A:** "Update the instructor page to add a dropdown."

**Option B:** "Enable instructors to update student grades directly from the Instructor Index page, reducing navigation steps and improving grading workflow efficiency."

<details>
<summary>Answer</summary>

**Option B** is better because it:
- Describes the capability (update grades)
- Specifies the location (Instructor Index page)
- Explains the value (reducing navigation, improving workflow)

**Option A** fails because it:
- Describes implementation (dropdown) rather than capability
- Provides no context on why this matters
- Could be misinterpreted (what dropdown? for what purpose?)

</details>

---

## 2.3 Creating Your Epic

Now let's create an epic for the grade management feature using the sequential workflow: **Scout analyzes → Scribe documents**.

### The Feature: Grade Management Enhancement

Based on Scout's analysis, we've identified this enhancement opportunity:

**Current State:**
- Instructors view enrollments as read-only on Instructor Index
- Students see grades but no GPA calculation
- Application only supports SQL Server

**Desired State:**
- Instructors can update grades directly from Instructor Index
- Students see calculated GPA on their Details page
- Application supports both SQLite (dev) and SQL Server (production)

### Step 1: Scout Identifies the Enhancement Scope

First, let's have Scout summarize what needs to change:

```
@Scout Based on your analysis of the Contoso University application, what would
need to change to allow instructors to update student grades from the Instructor
Index page?

Consider:
- Controller changes
- View modifications
- Any new helper classes needed
```

<details>
<summary>Expected Scout Response</summary>

**Controller Changes:**
- Add new `UpdateGrade` action to `InstructorsController`
- Action should accept enrollment ID and new grade value
- Need to inject enrollment repository or access through DbContext

**View Modifications:**
- `Views/Instructors/Index.cshtml` needs:
  - Grade dropdown for each enrollment row
  - Save button or auto-save functionality
  - Form handling for grade updates

**Supporting Classes:**
- Consider a `GradeExtensions` class for grade point calculations
- May need to add enrollment repository to DI container

**Database Considerations:**
- Current grade storage handles nullable grades (ungraded enrollments)
- No schema changes needed for basic grade update

</details>

### Step 2: Scribe Structures the Epic

Now use Scribe to formalize Scout's findings into a proper epic document:

```
@Scribe Create an epic document for a "Grade Management Enhancement" feature
for the Contoso University application.

Context from Scout's analysis:
- Need to add grade update capability to Instructor Index
- Need to add GPA calculation to Student Details
- Should support SQLite for development flexibility

Include:
- Overview section with business value
- 3-4 user stories in a table
- Definition of Done checklist
```

<details>
<summary>Expected Scribe Output</summary>

```markdown
# Epic: Grade Management Enhancement

## Overview

Enable efficient grade management for instructors and provide GPA visibility
for students in the Contoso University application. This enhancement reduces
workflow friction by allowing inline grade updates and adds academic standing
insight through calculated GPA display.

## Business Value

- **Instructor Efficiency**: Update grades without navigating away from course roster
- **Student Insight**: View calculated GPA to understand academic standing
- **Development Flexibility**: SQLite support enables local development without SQL Server

## Stories

| ID | Story | Description | Status |
|----|-------|-------------|--------|
| S01 | Grade Update UI | Add grade dropdown and save button to Instructor Index | Not Started |
| S02 | Grade Update Backend | Implement UpdateGrade action in InstructorsController | Not Started |
| S03 | GPA Calculation | Add GPA display to Student Details page | Not Started |
| S04 | Database Flexibility | Support SQLite and SQL Server via configuration | Not Started |

## Definition of Done

- [ ] Instructors can update grades from Index page
- [ ] Grade changes persist to database
- [ ] Students see calculated GPA on Details page
- [ ] Application works with both SQLite and SQL Server
- [ ] All existing tests pass
- [ ] New functionality has test coverage
```

</details>

### Step 3: Save Your Epic

Create the epic file in your project:

1. Create the directory if it doesn't exist: `docs/epics/`
2. Create a new file: `docs/epics/epic-grade-management.md`
3. Paste Scribe's output and refine as needed

**Tips for refinement:**
- Ensure Overview answers "what" and "why"
- Verify stories are sequenced logically (backend before UI)
- Add technical notes for complex stories
- Include links to relevant documentation

### The Sequential Workflow in Action

You've just completed a sequential multi-agent workflow:

```
┌─────────────┐                    ┌─────────────┐
│   Scout     │───────────────────▶│   Scribe    │
│  (Analyze)  │   findings.md      │  (Document) │
└─────────────┘                    └─────────────┘
     │                                   │
     ▼                                   ▼
  Understands                      Structures into
  existing code                    formal epic with
  and identifies                   stories and DoD
  opportunities
```

**Why this order matters:**
- Scribe needs Scout's technical analysis to write accurate stories
- Scout's findings become Scribe's input
- Each agent's expertise is used where it's most valuable

### Exercise: Extend the Epic

Try adding another story to the epic using Scribe:

```
@Scribe Add a story to the Grade Management epic for input validation.

The story should ensure:
- Only valid grade values (A, B, C, D, F, or null) are accepted
- Invalid input shows a user-friendly error message
- Validation happens both client-side and server-side
```

<details>
<summary>Sample Story</summary>

| ID | Story | Description | Status |
|----|-------|-------------|--------|
| S05 | Grade Validation | Add client and server-side validation for grade input | Not Started |

**Acceptance Criteria:**
- [ ] Grade dropdown only offers valid options (A, B, C, D, F)
- [ ] Server validates grade value before persisting
- [ ] Invalid API requests return appropriate error response
- [ ] Error messages are user-friendly

</details>

---

## Summary

In this lab, you learned:

- **Brownfield development** requires understanding existing code before proposing changes
- **Scout** excels at codebase analysis, pattern recognition, and identifying enhancement opportunities
- **Epic documents** capture the "what" and "why" of large feature initiatives
- **Epic structure** includes Overview, Business Value, Stories, and Definition of Done
- **Scribe** transforms technical findings into structured, actionable documentation
- **Sequential workflows** (Scout → Scribe) use each agent's strengths in the right order

**Key takeaway:** The quality of your epic depends on the quality of your analysis. Scout provides the foundation; Scribe builds the structure.

**Next:** In Lab 3, you'll break down the epic into detailed user stories with acceptance criteria.
