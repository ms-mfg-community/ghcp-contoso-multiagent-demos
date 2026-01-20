# Story 07: Write Lab Module 3 - Story Creation

## Summary

Write the third lab module that teaches learners how to break down an epic into implementation stories with acceptance criteria, agent assignments, and prompts.

## Assigned Agent

**Claude Code Agent**: `General-Purpose`

## Acceptance Criteria

- [x] AC1: `labs/lab03.md` created with proper header format
- [x] AC2: Duration estimate included (25-35 minutes)
- [x] AC3: Section 3.1 covers story structure (AC, agent, prompt format)
- [x] AC4: Section 3.2 demonstrates parallel story creation workflow
- [x] AC5: Section 3.3 provides hands-on story creation for grade update feature
- [x] AC6: Stories include: Backend changes, Frontend changes, Testing
- [x] AC7: Each story has complete agent prompt ready for execution

## Dependencies

- Story 06 (Lab Module 2) - completed

## Source Materials

| Material | Location |
|----------|----------|
| Lab format reference | `~/Coding_Projects/gh-abcs-actions/labs/` |
| Story examples | `docs/stories/` in target repo |
| Feature diff | Grade update feature from ghcp-contoso-university |

## Implementation Notes

Lab 03 teaches story creation:
1. Story structure: Summary, Agent, AC, Dependencies, Prompt
2. Writing effective acceptance criteria
3. Crafting agent prompts that are specific and actionable
4. Parallel workflow: Multiple stories can be defined simultaneously

Stories for the grade update feature:
1. Backend: Add UpdateGrade action and enrollment repository
2. Frontend: Add grade editing UI to Instructors/Index
3. Enhancement: Add GPA calculation to Students/Details
4. Infrastructure: Add SQLite support

---

## Agent Prompt

```
You are writing Lab Module 3 for a multi-agent orchestration tutorial.

**Context:**
- Target file: ~/Coding_Projects/ghcp-contoso-university-lab/labs/lab03.md
- This module teaches breaking epics into implementation stories
- Uses the grade update feature as the example

**Tasks:**
1. Create labs/lab03.md with:

   Header:
   ```markdown
   # 3 - Writing Implementation Stories
   In this lab you will break down an epic into actionable stories with acceptance criteria and agent prompts.
   > Duration: 25-35 minutes

   References:
   - [User Story Best Practices](https://www.mountaingoatsoftware.com/agile/user-stories)
   - [Acceptance Criteria Guidelines](https://www.altexsoft.com/blog/acceptance-criteria-purposes-formats-and-best-practices/)
   ```

2. Write section 3.1: Story Structure
   - Explain each section: Summary, Agent, AC, Dependencies, Prompt
   - Acceptance criteria format (testable, specific)
   - Agent assignment considerations
   - Prompt crafting best practices
   - Show the story template from docs/stories/

3. Write section 3.2: Parallel Story Planning
   - When stories can be written in parallel (no dependencies)
   - Using @Scribe for multiple story drafts
   - Identifying story dependencies
   - Exercise: Map dependencies for grade update stories

4. Write section 3.3: Creating Stories for Grade Update
   - Story A: Backend - UpdateGrade action (Builder agent)
     - AC: Controller action exists, enrollment updates work
   - Story B: Frontend - Grade editing UI (Builder agent)
     - AC: Dropdown renders, form submits, grade persists
   - Story C: GPA Calculation (Builder agent)
     - AC: GradeExtensions exists, GPA displays on details page
   - Include complete agent prompts for each story

5. Provide collapsible sections with full story file examples

**Output:**
- Complete lab03.md content
- All three stories fully specified
- Parallel workflow concept demonstrated

**Acceptance Criteria to verify:**
- [ ] AC1: labs/lab03.md created
- [ ] AC2: Duration estimate present
- [ ] AC3: Section 3.1 covers story structure
- [ ] AC4: Section 3.2 shows parallel workflow
- [ ] AC5: Section 3.3 has hands-on story creation
- [ ] AC6: Backend, Frontend, Enhancement stories included
- [ ] AC7: Complete prompts for each story
```

---

## Estimated Effort

- **Complexity**: Medium-High
- **Files Changed**: 1 file

## Session Notes

### Implementation Summary (2026-01-16)

**Created:** `labs/lab03.md` (685 lines)

### Lab Structure

**Section 3.1: Story Structure**
- Story document template (Summary, Agent, AC, Dependencies, Prompt)
- Writing effective acceptance criteria (testable, specific, observable)
- AC formats: Checklist and Given-When-Then
- Agent assignment considerations table (Scout, Scribe, Builder, Sage)
- Prompt crafting best practices with example
- Exercise: Evaluate acceptance criteria quality

**Section 3.2: Parallel Story Planning**
- When stories can run in parallel (no shared files, no data dependencies)
- Dependency mapping with ASCII diagram
- Parallel execution strategy
- Exercise: Map dependencies for grade management stories
- Using Scribe for batch story creation

**Section 3.3: Hands-on Story Creation**
- **Story A (Backend)**: UpdateGrade controller action - complete with 6 ACs and full Builder prompt
- **Story B (Frontend)**: Grade editing UI for Instructor Index - depends on Story A, 6 ACs, full prompt
- **Story C (GPA)**: GPA calculation for Student Details - independent, 6 ACs, includes unit tests, full prompt
- Execution timeline diagram showing parallel phases
- Knowledge check questions

### Key Patterns Established
- Stories contain complete, executable agent prompts
- Dependency analysis enables parallel execution
- Builder is the implementation agent
- Stories reference repository paths and existing patterns
- AC items are checkboxes that agents verify
