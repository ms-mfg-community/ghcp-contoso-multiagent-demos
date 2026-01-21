# Example: Feature Implementation Workflow

This document demonstrates a **sequential workflow** using the Scout → Scribe → Builder → Sage agent pipeline to implement a complete feature.

> **Scenario:** Add a "Search Students" feature to the Contoso University application.

---

## Workflow Overview

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│    Scout     │────▶│    Scribe    │────▶│   Builder    │────▶│     Sage     │
│  (Analyze)   │     │   (Write)    │     │  (Implement) │     │   (Document) │
└──────────────┘     └──────────────┘     └──────────────┘     └──────────────┘
   Analysis doc        User story          Code + tests        Documentation
```

**Why sequential?** Each step depends on the previous output:
- Scribe needs Scout's analysis to write informed stories
- Builder needs the story to know what to implement
- Sage needs the implementation to document accurate behavior

---

## Step 1: Scout - Codebase Analysis

### Prompt

Select **Scout** from the agent dropdown, then enter the prompt below:

```
Analyze the Contoso University codebase for implementing a student search feature.

Focus on:
1. Current Students list implementation (Index page)
2. Existing search patterns in the codebase (if any)
3. Data access patterns for querying students
4. UI component patterns used in the Views

Document your findings in a structured analysis report.
```

### Expected Output

Scout produces `docs/analysis/student-search-analysis.md`:

```markdown
# Student Search Analysis

## Current Implementation

### Students Index Page
- **Location**: `Views/Students/Index.cshtml`
- **Controller**: `StudentsController.Index()`
- **Data Access**: EF Core query via `_context.Students.ToListAsync()`

### Patterns Identified
1. **Search pattern** found in Instructors:
   - `InstructorsController.Index(string searchString)`
   - Uses `Where()` with `Contains()` for string matching

2. **UI Components**:
   - Bootstrap table with pagination
   - Form for filters above table

### Recommendations
1. Follow existing Instructors search pattern
2. Add search parameter to Students Index action
3. Include search input field matching Instructors UI

## Files to Modify
| File | Changes |
|------|---------|
| `StudentsController.cs` | Add searchString parameter, LINQ query |
| `Views/Students/Index.cshtml` | Add search form, preserve search in pagination |

## Existing Tests
- `StudentsControllerTests.cs` - needs new test cases for search
```

### Quality Gate: Grade the Analysis

Before proceeding, optionally validate Scout's analysis quality:

```
Evaluate Scout's analysis against our standards. Does it provide:
- Concrete file paths and code references?
- Actionable recommendations?
- Clear scope for the implementation?
```

---

## Step 2: Scribe - Write User Story

### Prompt

Select **Scribe** from the agent dropdown, then enter the prompt below:

```
Based on Scout's analysis at docs/analysis/student-search-analysis.md, write a complete user story for the Student Search feature.

Requirements:
1. Follow the story template format
2. Include specific acceptance criteria that can be tested
3. Reference the actual files and patterns from the analysis
4. Keep scope focused on basic search only (no advanced filters)
```

### Expected Output

Scribe produces `stories/US-001-student-search.md`:

```markdown
# US-001: Student Search

## User Story

**As a** university administrator
**I want to** search for students by name
**So that** I can quickly find student records without scrolling through the full list

## Acceptance Criteria

- [ ] **AC1**: Search input field appears above the Students table
- [ ] **AC2**: Search matches partial strings in FirstMidName OR LastName (case-insensitive)
- [ ] **AC3**: Empty search shows all students (current behavior)
- [ ] **AC4**: Search results display within 2 seconds for up to 10,000 students
- [ ] **AC5**: Search term is preserved in the input field after form submission
- [ ] **AC6**: Pagination links include the search parameter

## Scope

**In Scope:**
- Text search on student names (first, middle, last)
- Single search input field
- URL parameter preservation for search state

**Out of Scope:**
- Search by EnrollmentDate, ID, or other fields
- Advanced filters (dropdowns, date ranges)
- Export search results
- Save search functionality

## Technical Context

### Files to Modify
| File | Changes |
|------|---------|
| `Controllers/StudentsController.cs` | Add `searchString` parameter to `Index()`, LINQ filter |
| `Views/Students/Index.cshtml` | Add search form, update pagination links |

### Pattern Reference
Follow existing pattern in `Controllers/InstructorsController.cs:Index()`:
```csharp
if (!String.IsNullOrEmpty(searchString))
{
    instructors = instructors.Where(i => i.LastName.Contains(searchString)
                           || i.FirstMidName.Contains(searchString));
}
```

### Testing Requirements
- [ ] Unit test: Search returns matching students
- [ ] Unit test: Empty search returns all students
- [ ] Unit test: Search is case-insensitive
- [ ] Unit test: Partial matches work
```

### Quality Gate: Grade the Story

**Use the story-writing-standards skill** to validate before implementation:

```
Grade this story against our story writing standards: @workspace/stories/US-001-student-search.md

Ensure:
- Acceptance Criteria Quality >= 7/10
- Testability >= 7/10
- Scope Clarity >= 7/10
```

#### Sample Grading Output

| Criterion | Score | Notes |
|-----------|-------|-------|
| Acceptance Criteria Quality | 8/10 | Specific, measurable criteria with clear test conditions |
| Testability | 8/10 | All ACs can be directly converted to test cases |
| Scope Clarity | 9/10 | Explicit in/out scope with clear boundaries |
| Technical Detail Sufficiency | 8/10 | Files, patterns, and code examples provided |
| Project Pattern Alignment | 7/10 | Follows existing patterns, references actual code |
| **Overall** | 8/10 | **READY FOR DEV** |

**Verdict: READY FOR DEV** - Proceed to implementation.

---

## Step 3: Builder - Implement the Feature

### Prompt

Select **Builder** from the agent dropdown, then enter the prompt below:

```
Implement user story US-001 (Student Search) following the story file at stories/US-001-student-search.md.

Requirements:
1. Follow the exact acceptance criteria
2. Match the existing Instructors search pattern
3. Write unit tests for all test requirements
4. Do not add features beyond the story scope
```

### Expected Implementation

Builder modifies `StudentsController.cs`:

```csharp
// GET: Students
public async Task<IActionResult> Index(string searchString)
{
    ViewData["CurrentFilter"] = searchString;

    var students = from s in _context.Students
                   select s;

    if (!String.IsNullOrEmpty(searchString))
    {
        students = students.Where(s => s.LastName.Contains(searchString)
                               || s.FirstMidName.Contains(searchString));
    }

    return View(await students.ToListAsync());
}
```

Builder modifies `Views/Students/Index.cshtml`:

```html
<form asp-action="Index" method="get">
    <div class="form-actions no-color">
        <p>
            Find by name: <input type="text" name="searchString" value="@ViewData["CurrentFilter"]" />
            <input type="submit" value="Search" class="btn btn-primary" />
        </p>
    </div>
</form>
```

Builder adds tests in `StudentsControllerTests.cs`:

```csharp
[Fact]
public async Task Index_WithSearchString_ReturnsMatchingStudents()
{
    // Arrange - setup test data
    // Act - call Index("Smith")
    // Assert - only Smiths returned
}

[Fact]
public async Task Index_WithEmptySearch_ReturnsAllStudents()
{
    // ...
}

[Fact]
public async Task Index_SearchIsCaseInsensitive()
{
    // ...
}
```

### Quality Gate: Grade the Code

**Use the coding-standards-grading skill** to validate the implementation:

```
Grade the search implementation in StudentsController.cs against our coding standards.

Focus on:
- Clarity of the LINQ query
- Correctness of the search logic
- Robustness (null handling, edge cases)
- Security (no SQL injection risks)
```

#### Sample Grading Output

| Criterion | Score | Notes |
|-----------|-------|-------|
| Clarity | 8/10 | Clear variable names, follows existing pattern |
| Correctness | 9/10 | Handles null/empty search, case-insensitive |
| Robustness | 8/10 | EF Core parameterizes queries, handles edge cases |
| Security | 9/10 | No SQL injection risk with EF Core parameterization |
| Simplicity | 9/10 | Minimal code, no over-engineering |
| Maintainability | 8/10 | Matches project patterns, easy to extend |
| **Overall** | 8.5/10 | **PASS** |

**Verdict: PASS** - Code meets quality standards.

---

## Step 4: Sage - Write Documentation

### Prompt

Select **Sage** from the agent dropdown, then enter the prompt below:

```
Document the Student Search feature based on the implementation in StudentsController.cs and Views/Students/Index.cshtml.

Create:
1. User-facing documentation explaining how to use the search
2. Developer documentation for maintaining/extending the feature
```

### Expected Output

Sage produces `docs/features/student-search.md`:

```markdown
# Student Search

## User Guide

### How to Search

1. Navigate to the **Students** page
2. Enter a name (or partial name) in the search box
3. Click **Search** or press Enter
4. Results show students matching your search in first, middle, or last name

### Tips

- Search is **case-insensitive** ("smith" matches "Smith")
- Search works on **partial names** ("son" matches "Johnson", "Ranson")
- **Clear the search** by emptying the box and clicking Search

## Developer Documentation

### Implementation

| Component | Location |
|-----------|----------|
| Controller | `Controllers/StudentsController.cs` |
| View | `Views/Students/Index.cshtml` |
| Tests | `Tests/StudentsControllerTests.cs` |

### How It Works

The Index action accepts an optional `searchString` parameter:

```csharp
public async Task<IActionResult> Index(string searchString)
```

When provided, filters students using LINQ `Contains()` on name fields.

### Extending the Search

To add additional search fields:

1. Add new parameters to the Index action
2. Extend the LINQ `Where()` clause
3. Add corresponding input fields to the view
4. Update tests for new scenarios

### Related Stories

- US-001: Student Search (this feature)
- Future: US-010: Advanced Search Filters
```

---

## Workflow Summary

| Step | Agent | Input | Output | Quality Gate |
|------|-------|-------|--------|--------------|
| 1 | Scout | Feature requirements | Analysis document | Review for completeness |
| 2 | Scribe | Analysis document | User story with ACs | Story grading >= 7/10 |
| 3 | Builder | User story | Code + tests | Code grading >= 7/10 |
| 4 | Sage | Implementation | Documentation | Review for accuracy |

---

## When to Use This Pattern

**Ideal for:**
- New feature implementation
- Bug fixes with clear scope
- Enhancements to existing functionality
- Tasks completable in a single session

**Consider multi-session workflow when:**
- Task spans multiple hours/days
- Requires stakeholder review between phases
- Involves significant architectural decisions
- Team collaboration is needed

---

## Related Documents

- [Multi-Session Workflow Example](./multi-session-workflow-example.md)
- [Coding Standards Grading](/.github/skills/coding-standards-grading/SKILL.md)
- [Story Writing Standards](/.github/skills/story-writing-standards/SKILL.md)
- [Handoff Template](../handoffs/handoff-template.md)
