# Story 06: Write Lab Module 2 - Brownfield Epic Creation

## Summary

Write the second lab module that teaches learners how to create a brownfield epic for an existing codebase. This module demonstrates analyzing code and documenting enhancement opportunities.

## Assigned Agent

**GitHub Copilot Chat**: Agent mode

## Acceptance Criteria

- [x] AC1: `labs/lab02.md` created with proper header format
- [x] AC2: Duration estimate included (20-30 minutes)
- [x] AC3: Section 2.1 covers brownfield project analysis using the Scout agent
- [x] AC4: Section 2.2 teaches epic document structure with examples
- [x] AC5: Section 2.3 provides hands-on epic creation for the grade update feature
- [x] AC6: Includes the target feature context (grade update, GPA calculation)
- [x] AC7: Shows sequential agent workflow (Scout analyzes → Scribe documents)

## Dependencies

- Story 05 (Lab Module 1) - completed

## Source Materials

| Material | Location |
|----------|----------|
| Lab format reference | `~/Coding_Projects/gh-abcs-actions/labs/` |
| Feature diff | `~/Coding_Projects/ghcp-contoso-university` (brownfield-feature-advanced vs local-testing) |
| Epic example | `docs/epics/epic-01-lab-creation.md` in target repo |

## Implementation Notes

Lab 02 teaches the brownfield epic workflow:
1. Use Scout agent to analyze existing codebase
2. Identify enhancement opportunities (the grade update feature)
3. Document findings in epic format
4. Use Scribe agent to formalize the epic

Feature to document:
- Grade update on Instructors/Index page
- GPA calculation on Students/Details page
- Database flexibility (SQLite/SQL Server)

---

## Agent Prompt

```
You are writing Lab Module 2 for a multi-agent orchestration tutorial.

**Context:**
- Target file: ~/Coding_Projects/ghcp-contoso-university-lab/labs/lab02.md
- This module teaches brownfield epic creation
- Feature context: Adding grade updates and GPA calculation to Contoso University

**Feature Details (from actual diff):**
- InstructorsController: Add UpdateGrade action with enrollment repository
- Index.cshtml: Add grade dropdown and save button per enrollment
- Students/Details.cshtml: Add GPA calculation display
- GradeExtensions.cs: New file with ToGradePoints() method
- DependencyInjection.cs: Add SQLite/SQL Server flexibility

**Tasks:**
1. Create labs/lab02.md with:

   Header:
   ```markdown
   # 2 - Creating Brownfield Epics
   In this lab you will analyze an existing codebase and create an epic for a new feature enhancement.
   > Duration: 20-30 minutes

   References:
   - [Brownfield Development](https://en.wikipedia.org/wiki/Brownfield_(software_development))
   - [Epic Documentation Best Practices](https://www.atlassian.com/agile/project-management/epics)
   ```

2. Write section 2.1: Analyzing the Existing Codebase
   - Use @Scout agent to explore the Contoso University structure
   - Identify the current instructor and student views
   - Document what currently exists (read-only grade display)
   - Exercise: Ask Scout to analyze InstructorsController.cs

3. Write section 2.2: Epic Document Structure
   - Overview section (what and why)
   - Learning objectives / business value
   - Story breakdown table
   - Progress tracker format
   - Definition of Done
   - Show example epic structure

4. Write section 2.3: Creating Your Epic
   - Define the feature: "Enable instructors to update student grades"
   - Use @Scribe agent to help structure the epic
   - Create epic file in docs/epics/
   - Include: Overview, 3-4 stories, progress tracker
   - Sequential workflow: Scout → Scribe

5. Include collapsible sections with example epic content

**Output:**
- Complete lab02.md content
- Feature context clearly explained
- Sequential agent workflow demonstrated

**Acceptance Criteria to verify:**
- [ ] AC1: labs/lab02.md created
- [ ] AC2: Duration estimate present
- [ ] AC3: Section 2.1 covers codebase analysis with Scout
- [ ] AC4: Section 2.2 teaches epic structure
- [ ] AC5: Section 2.3 has hands-on epic creation
- [ ] AC6: Feature context included
- [ ] AC7: Sequential workflow shown
```

---

## Estimated Effort

- **Complexity**: Medium-High
- **Files Changed**: 1 file

## Session Notes

**Session Date:** 2026-01-16

**Implementation Summary:**
- Created `labs/lab02.md` (320 lines) teaching brownfield epic creation
- Section 2.1: Explains brownfield development, Scout's capabilities, hands-on InstructorsController analysis exercise
- Section 2.2: ASCII diagram of epic structure, component explanations, example Overview and Stories table, Knowledge Check
- Section 2.3: Three-step hands-on workflow (Scout identifies scope → Scribe structures epic → Save), includes sequential workflow diagram
- Five collapsible `<details>` sections with example outputs and discussions
- Feature context throughout: grade update on Instructors/Index, GPA on Students/Details, SQLite flexibility
- Sequential workflow explicitly shown with ASCII diagram (Scout → Scribe)

**Files Modified:**
- `labs/lab02.md` - Created (320 lines)
- `docs/stories/story-06-lab-module-epic.md` - Updated (ACs checked, session notes)

**Pattern Notes:**
- Followed lab01.md format: header with duration/references, three numbered sections, collapsible solutions, summary with bullet points
- Added ASCII diagrams consistent with lab01 style
- Each section has knowledge checks or exercises with `<details>` answers
