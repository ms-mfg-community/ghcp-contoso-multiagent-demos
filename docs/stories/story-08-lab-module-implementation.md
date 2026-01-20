# Story 08: Write Lab Module 4 - Implementation

## Summary

Write the fourth lab module that teaches learners how to execute stories using multi-agent orchestration, handle handoffs between sessions, and track progress.

## Assigned Agent

**Claude Code Agent**: `General-Purpose`

## Acceptance Criteria

- [x] AC1: `labs/lab04.md` created with proper header format
- [x] AC2: Duration estimate included (30-45 minutes)
- [x] AC3: Section 4.1 covers single-session implementation workflow
- [x] AC4: Section 4.2 covers multi-session handoffs with handoff document creation
- [x] AC5: Section 4.3 demonstrates executing one complete story with @Builder
- [x] AC6: Section 4.4 covers progress tracking and epic updates
- [x] AC7: Includes handoff document template and example

## Dependencies

- Story 07 (Lab Module 3) - completed

## Source Materials

| Material | Location |
|----------|----------|
| Lab format reference | `~/Coding_Projects/gh-abcs-actions/labs/` |
| Handoff examples | `docs/handoffs/` in target repo |
| Feature implementation | ghcp-contoso-university brownfield-feature-advanced branch |

## Implementation Notes

Lab 04 is the culmination - actually executing implementation:
1. Single-session: Complete a small story in one conversation
2. Multi-session: Create handoff document, resume in new session
3. Using the Builder agent with specific prompts
4. Updating epic progress tracker

This lab should reference the actual code changes needed:
- InstructorsController.cs modifications
- View changes
- GradeExtensions.cs creation

---

## Agent Prompt

```
You are writing Lab Module 4 for a multi-agent orchestration tutorial.

**Context:**
- Target file: ~/Coding_Projects/ghcp-contoso-university-lab/labs/lab04.md
- This module teaches executing stories and managing handoffs
- Final module that brings everything together

**Actual Implementation Details:**
Backend changes needed:
- Add IRepository<Enrollment> to InstructorsController
- Add UpdateGrade POST action
- Use GetQueryable with Include for eager loading

Frontend changes needed:
- Add form with grade dropdown to Index.cshtml
- Add GPA calculation to Students/Details.cshtml

New file needed:
- GradeExtensions.cs with ToGradePoints() method

**Tasks:**
1. Create labs/lab04.md with:

   Header:
   ```markdown
   # 4 - Executing Stories with Agents
   In this lab you will implement stories using multi-agent orchestration and learn to manage session handoffs.
   > Duration: 30-45 minutes

   References:
   - [GitHub Copilot Chat](https://docs.github.com/en/copilot/using-github-copilot/asking-github-copilot-questions-in-your-ide)
   - [Context Management Best Practices](https://docs.github.com/en/copilot/customizing-copilot)
   ```

2. Write section 4.1: Single-Session Implementation
   - When to use single-session (small, focused stories)
   - Workflow: Load story → Execute prompt → Verify AC → Mark complete
   - Exercise: Implement GradeExtensions.cs (small, self-contained)

3. Write section 4.2: Multi-Session Handoffs
   - When handoffs are needed (context limits, time breaks)
   - Handoff document structure:
     - Current story status
     - What was completed
     - What remains
     - Next session prompt
     - Date/time and agent used
   - Create handoff document template
   - Exercise: Create a handoff mid-implementation

4. Write section 4.3: Implementing with @Builder
   - Load the backend story prompt
   - Execute with @Builder agent
   - Watch the agent make changes
   - Verify acceptance criteria
   - Handle any issues that arise

5. Write section 4.4: Progress Tracking
   - Update story status in docs/stories/
   - Update epic progress tracker
   - Create completion handoff document
   - Lessons learned section

6. Include actual code snippets from the feature

**Output:**
- Complete lab04.md content
- Handoff document template included
- Real implementation guidance provided

**Acceptance Criteria to verify:**
- [ ] AC1: labs/lab04.md created
- [ ] AC2: Duration estimate present
- [ ] AC3: Section 4.1 covers single-session workflow
- [ ] AC4: Section 4.2 covers multi-session handoffs
- [ ] AC5: Section 4.3 has hands-on implementation
- [ ] AC6: Section 4.4 covers progress tracking
- [ ] AC7: Handoff template included
```

---

## Estimated Effort

- **Complexity**: High
- **Files Changed**: 1 file

## Session Notes

### Session: 2026-01-16

**Status**: Complete

**Files Created**:
| File | Lines | Content |
|------|-------|---------|
| `labs/lab04.md` | 956 | Lab Module 4 - Executing Stories with Agents |

**AC Verification**:
- AC1: ✓ Header follows `# 4 - Title` format with description, duration, references
- AC2: ✓ Duration: 30-45 minutes included
- AC3: ✓ Section 4.1 covers single-session workflow (when to use, workflow diagram, GradeExtensions exercise)
- AC4: ✓ Section 4.2 covers multi-session handoffs (when needed, structure diagram, template, exercise)
- AC5: ✓ Section 4.3 demonstrates @Builder execution (Story A backend implementation, Story B frontend exercise)
- AC6: ✓ Section 4.4 covers progress tracking (status updates, epic tracker, completion handoffs, lessons learned)
- AC7: ✓ Handoff document template at line 232, example usage at lines 239 and 312

**Lab Structure**:
- 4.1: Single-Session Implementation - workflow, when to use, GradeExtensions exercise
- 4.2: Multi-Session Handoffs - structure, template, mid-implementation exercise
- 4.3: Implementing with @Builder - full Story A execution, Story B exercise
- 4.4: Progress Tracking - status updates, epic progress, completion handoffs, lessons learned
- Summary with complete workflow diagram

**Continuity from Previous Labs**:
- Uses Grade Management feature context from lab02/lab03
- References Story A (Backend), Story B (Frontend), Story C (GPA) from lab03
- Maintains consistent formatting: ASCII diagrams, tables, collapsible details, Knowledge Checks
