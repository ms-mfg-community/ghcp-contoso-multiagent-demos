# Story 03: Configure GitHub Copilot Agents

## Summary

Create custom GitHub Copilot agents specific to this lab that will help learners understand agent creation and usage. Include agents for different roles in the development workflow.

## Assigned Agent

**Claude Code Agent**: `General-Purpose`

## Acceptance Criteria

- [x] AC1: At least 3 custom agents created in `.github/agents/`
- [x] AC2: Each agent has proper YAML frontmatter (description, name, tools)
- [x] AC3: Each agent has clear expertise, principles, and response approach sections
- [x] AC4: Agents cover different workflow roles (analyst, developer, tester)
- [x] AC5: Agents reference MCP tools (microsoft.docs.mcp, context7)
- [x] AC6: Agent files follow `.agent.md` naming convention

## Dependencies

- Story 01 (Setup Lab Infrastructure) - completed

## Source Materials

| Material | Location |
|----------|----------|
| Agent examples | `~/Coding_Projects/bmad-azure-module/.github/agents/` |
| BMAD PM agent | `~/Coding_Projects/bmad-azure-module/_bmad/bmm/agents/pm.md` (for reference) |

## Implementation Notes

Agents to create:
1. **brownfield-analyst.agent.md** - Analyzes existing codebases and identifies enhancement opportunities
2. **story-writer.agent.md** - Creates well-structured user stories with acceptance criteria
3. **dotnet-developer.agent.md** - Implements .NET/ASP.NET Core features following best practices

Each agent should:
- Have a memorable name (like "Scout", "Scribe", "Builder")
- Reference the Contoso University context
- Include tools: microsoft.docs.mcp, context7, fetch, edit, search

---

## Agent Prompt

```
You are creating custom GitHub Copilot agents for a multi-agent orchestration lab.

**Context:**
- Target repo: ~/Coding_Projects/ghcp-contoso-university-lab
- Reference agents: ~/Coding_Projects/bmad-azure-module/.github/agents/
- These agents will help learners implement features in a .NET Contoso University app

**Tasks:**
1. Read 2-3 agent files from ~/Coding_Projects/bmad-azure-module/.github/agents/ to understand the format
2. Create 3 new agents in .github/agents/:

   a) brownfield-analyst.agent.md ("Scout")
      - Expertise: Analyzing existing .NET codebases, identifying patterns, finding enhancement opportunities
      - Tools: microsoft.docs.mcp, context7, search

   b) story-writer.agent.md ("Scribe")
      - Expertise: Writing user stories, defining acceptance criteria, creating technical specs
      - Tools: microsoft.docs.mcp, context7, fetch

   c) dotnet-developer.agent.md ("Builder")
      - Expertise: ASP.NET Core MVC, Entity Framework, C# best practices
      - Tools: microsoft.docs.mcp, context7, edit, search

3. Each agent file must have:
   - YAML frontmatter with description, name, tools array
   - ## Expertise section
   - ## Principles section
   - ## Response Approach section

**Output:**
- Show the content of each created agent file
- Confirm YAML frontmatter is valid
- Verify tools reference MCP servers

**Acceptance Criteria to verify:**
- [ ] AC1: 3 agents created in .github/agents/
- [ ] AC2: Valid YAML frontmatter on each
- [ ] AC3: Clear expertise/principles/response sections
- [ ] AC4: Different workflow roles covered
- [ ] AC5: MCP tools referenced
- [ ] AC6: Correct naming convention
```

---

## Estimated Effort

- **Complexity**: Medium
- **Files Changed**: 3 new files

## Session Notes

### Session 1 - 2026-01-16

**Actions Taken:**
1. Read reference agents from bmad-azure-module/.github/agents/ to understand format
2. Created 3 new Contoso University-specific agents:
   - `brownfield-analyst.agent.md` (Scout) - analyzes existing .NET codebases
   - `story-writer.agent.md` (Scribe) - writes user stories and specs
   - `dotnet-developer.agent.md` (Builder) - implements .NET features

**Files Created:**
| File | Purpose |
|------|---------|
| `.github/agents/brownfield-analyst.agent.md` | Codebase analysis, pattern recognition, tech debt assessment |
| `.github/agents/story-writer.agent.md` | User stories, acceptance criteria, technical specs |
| `.github/agents/dotnet-developer.agent.md` | ASP.NET Core MVC, EF Core, C# implementation |

**Agent Structure:**
- YAML frontmatter: description, name (with nickname), tools array
- Sections: Expertise, Principles, Response Approach, Key Resources
- All reference MCP tools: microsoft.docs.mcp, context7
- Each has distinctive nickname: Scout, Scribe, Builder

**Notes:**
- AC4 mentions "tester" role but Implementation Notes and Agent Prompt specify Scout/Scribe/Builder. Followed the more specific guidance.
- Agents now total 9 in .github/agents/ (6 Azure from S01 + 3 new Contoso-specific)
