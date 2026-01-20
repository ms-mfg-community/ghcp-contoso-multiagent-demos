# Story 02: Create Lab Structure

## Summary

Create the lab folder structure following the patterns from gh-abcs-actions, including the labs directory, setup documentation, and README structure.

## Assigned Agent

**GitHub Copilot Chat**: Agent mode

## Acceptance Criteria

- [x] AC1: `labs/` directory exists at repository root
- [x] AC2: `labs/setup.md` exists with fork/clone instructions
- [x] AC3: `labs/images/` directory exists for lab diagrams
- [x] AC4: Main `README.md` has module matrix with checkboxes for progress tracking
- [x] AC5: README includes learning path section linking to lab modules
- [x] AC6: Lab file naming convention documented (lab01.md, lab02.md, etc.)

## Dependencies

- Story 01 (Setup Lab Infrastructure) - completed

## Source Materials

| Material | Location |
|----------|----------|
| Lab structure reference | `~/Coding_Projects/gh-abcs-actions/labs/` |
| README pattern | `~/Coding_Projects/gh-abcs-actions/README.md` |
| Setup pattern | `~/Coding_Projects/gh-abcs-actions/labs/setup.md` |

## Implementation Notes

1. Follow the exact structure from gh-abcs-actions
2. Lab modules for this repo:
   - Lab 01: Introduction to Multi-Agent Orchestration
   - Lab 02: Creating Brownfield Epics
   - Lab 03: Writing Implementation Stories
   - Lab 04: Executing Stories with Agents
3. Each lab should have duration estimate and references section
4. Use GitHub-flavored markdown with collapsible sections

---

## Agent Prompt

```
You are creating the lab structure for a GitHub Copilot multi-agent orchestration tutorial.

**Context:**
- Reference repo: ~/Coding_Projects/gh-abcs-actions
- Target repo: ~/Coding_Projects/ghcp-contoso-university-lab
- This lab has 4 modules teaching multi-agent workflows

**Tasks:**
1. Read ~/Coding_Projects/gh-abcs-actions/labs/setup.md for setup pattern
2. Read ~/Coding_Projects/gh-abcs-actions/README.md for module matrix pattern
3. Create the following structure:
   ```
   labs/
   ├── setup.md
   └── images/
   ```
4. Create labs/setup.md with:
   - Duration estimate
   - Prerequisites (VS Code, GitHub Copilot, Node.js for MCP)
   - Fork/clone instructions
   - Enable GitHub Copilot custom agents instructions
5. Update or create README.md with:
   - Project title and description
   - Module matrix with 4 labs:
     - [ ] Lab 01: Introduction to Multi-Agent Orchestration
     - [ ] Lab 02: Creating Brownfield Epics
     - [ ] Lab 03: Writing Implementation Stories
     - [ ] Lab 04: Executing Stories with Agents
   - Learning objectives section
   - Prerequisites section
   - Links to additional resources

**Output:**
- Confirm all directories created
- Show the README module matrix
- Confirm setup.md follows the pattern

**Acceptance Criteria to verify:**
- [ ] AC1: labs/ directory exists
- [ ] AC2: labs/setup.md exists with proper content
- [ ] AC3: labs/images/ directory exists
- [ ] AC4: README has module matrix with checkboxes
- [ ] AC5: README has learning path section
- [ ] AC6: Naming convention is clear
```

---

## Estimated Effort

- **Complexity**: Low
- **Files Changed**: 2-3 files

## Session Notes

### Session 2 - 2025-01-16

**Completed by:** GitHub Copilot Chat

**Actions Taken:**
1. Read reference materials from gh-abcs-actions (setup.md and README.md patterns)
2. Created `labs/` directory and `labs/images/` subdirectory
3. Created `labs/setup.md` with:
   - Duration estimate (15-20 minutes)
   - Prerequisites table (VS Code, GitHub Copilot, Node.js, .NET SDK, Git)
   - Fork/clone instructions
   - GitHub Copilot custom agents setup instructions
   - MCP server configuration (optional)
   - Verification checklist
   - Troubleshooting section with collapsible details
   - Lab file naming convention table
4. Rewrote `README.md` with:
   - Learning objectives section
   - Prerequisites table
   - Module matrix with 5 checkbox items (setup + 4 labs)
   - Learning Path section with resource links
   - Repository structure documentation
   - Additional resources (GitHub Copilot, .NET, AI-Assisted Dev)
   - Getting Started instructions

**Files Created:**
- `labs/setup.md` (4784 bytes)
- `labs/images/` (directory for future lab diagrams)

**Files Modified:**
- `README.md` (complete rewrite following gh-abcs-actions pattern)

**Verification:**
- All 6 acceptance criteria verified with grep/ls commands
- Module matrix contains 5 checkbox items for progress tracking
- Learning Path section present at line 59
- Naming convention documented in setup.md (lab01.md through lab04.md)

**Status:** COMPLETE
