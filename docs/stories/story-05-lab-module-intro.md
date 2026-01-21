# Story 05: Write Lab Module 1 - Introduction

## Summary

Write the first lab module that introduces learners to multi-agent orchestration concepts, GitHub Copilot custom agents, and the overall lab objectives.

## Assigned Agent

**GitHub Copilot Chat**: Agent mode

## Acceptance Criteria

- [x] AC1: `labs/lab01.md` created with proper header format
- [x] AC2: Duration estimate included (15-20 minutes)
- [x] AC3: References section links to GitHub Copilot and MCP documentation
- [x] AC4: Section 1.1 covers multi-agent orchestration concepts (sequential vs parallel)
- [x] AC5: Section 1.2 covers workflow patterns (single-session vs multi-session)
- [x] AC6: Section 1.3 introduces GitHub Copilot custom agents with hands-on exploration
- [x] AC7: Collapsible `<details>` sections for solutions/answers

## Dependencies

- Story 02 (Create Lab Structure) - completed
- Story 03 (Configure GitHub Copilot Agents) - completed

## Source Materials

| Material | Location |
|----------|----------|
| Lab format reference | `~/Coding_Projects/gh-abcs-actions/labs/lab01.md` |
| Agent files | `.github/agents/` in target repo |
| MCP documentation | https://modelcontextprotocol.io |

## Implementation Notes

Lab 01 should cover:
1. What is multi-agent orchestration?
   - Sequential: Agent A completes, then Agent B starts
   - Parallel: Agent A and B work simultaneously
2. Workflow patterns
   - Single-session: Complete task in one conversation
   - Multi-session: Handoffs between sessions with context preservation
3. GitHub Copilot custom agents
   - Explore the `.github/agents/` folder
   - Understand agent file structure
   - Try invoking an agent in Copilot Chat

---

## Agent Prompt

```
You are writing Lab Module 1 for a multi-agent orchestration tutorial.

**Context:**
- Target file: ~/Coding_Projects/ghcp-contoso-university-lab/labs/lab01.md
- Reference format: ~/Coding_Projects/gh-abcs-actions/labs/lab01.md
- This is an introduction module for learners new to multi-agent concepts

**Tasks:**
1. Read ~/Coding_Projects/gh-abcs-actions/labs/lab01.md for format reference
2. Create labs/lab01.md with:

   Header:
   ```markdown
   # 1 - Introduction to Multi-Agent Orchestration
   In this lab you will learn the fundamentals of multi-agent orchestration and explore GitHub Copilot custom agents.
   > Duration: 15-20 minutes

   References:
   - [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
   - [Model Context Protocol](https://modelcontextprotocol.io)
   - [Custom Agents in VS Code](https://code.visualstudio.com/docs/copilot)
   ```

3. Write section 1.1: Multi-Agent Orchestration Concepts
   - Define multi-agent orchestration
   - Explain sequential task execution (with diagram description)
   - Explain parallel task execution (with diagram description)
   - Include a knowledge check question

4. Write section 1.2: Workflow Patterns
   - Single-session workflows: when and why
   - Multi-session workflows: handoffs, context preservation
   - When to use each pattern

5. Write section 1.3: Exploring Custom Agents
   - Navigate to .github/agents/ folder
   - Examine agent file structure (frontmatter, sections)
   - Hands-on: Invoke Scout agent in Copilot Chat
   - Exercise: Ask the agent to analyze a file

6. Add collapsible solution sections where appropriate

**Output:**
- Complete lab01.md content
- Confirm all sections follow the reference format
- Verify hands-on exercises are clear

**Acceptance Criteria to verify:**
- [ ] AC1: labs/lab01.md created
- [ ] AC2: Duration estimate present
- [ ] AC3: References section complete
- [ ] AC4: Section 1.1 covers orchestration concepts
- [ ] AC5: Section 1.2 covers workflow patterns
- [ ] AC6: Section 1.3 has hands-on agent exploration
- [ ] AC7: Collapsible sections for solutions
```

---

## Estimated Effort

- **Complexity**: Medium
- **Files Changed**: 1 file

## Session Notes

### Session: S05 (2025-01-16)

**Completed:**
- Created `labs/lab01.md` (287 lines) covering all three main sections
- Section 1.1: Multi-agent orchestration concepts with ASCII diagrams showing sequential and parallel execution patterns
- Section 1.2: Workflow patterns comparing single-session vs multi-session approaches with decision table
- Section 1.3: Hands-on exploration of GitHub Copilot custom agents with exercises for Scout, Scribe, and Sage
- Included 3 collapsible `<details>` sections for exercise answers and discussion points
- Added Summary section pointing to Lab 2 for continuity

**Key decisions:**
- Used ASCII art diagrams (compatible with markdown) rather than image references
- Included practical exercises that invoke the agents created in Stories 03-04
- Added reflection questions to encourage learners to think about agent combinations
- Followed gh-abcs-actions format but expanded significantly for conceptual content

**Files created:**
| File | Lines | Description |
|------|-------|-------------|
| `labs/lab01.md` | 287 | Introduction to Multi-Agent Orchestration lab module |

**All ACs verified and marked complete.**
