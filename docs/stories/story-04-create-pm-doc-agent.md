# Story 04: Create Custom PM/Doc Agent

## Summary

Create a custom agent similar to the BMAD PM agent that helps write documentation and user stories. This agent will be used as an example in the lab to demonstrate custom agent creation.

## Assigned Agent

**GitHub Copilot Chat**: Agent mode

## Acceptance Criteria

- [x] AC1: New agent `pm-doc-writer.agent.md` created in `.github/agents/`
- [x] AC2: Agent combines PM and technical writing capabilities
- [x] AC3: Agent has custom personality and communication style
- [x] AC4: Agent references appropriate MCP tools
- [x] AC5: Agent includes example prompts in its response approach
- [x] AC6: Lab documentation updated to reference this agent as an example

## Dependencies

- Story 03 (Configure GitHub Copilot Agents) - completed

## Source Materials

| Material | Location |
|----------|----------|
| BMAD PM agent | Review bmad-azure-module agent manifest for PM agent pattern |
| Tech writer pattern | Review bmad-azure-module for tech-writer patterns |

## Implementation Notes

This agent should:
- Combine product management and documentation skills
- Have a personality that asks clarifying questions
- Focus on user value and clear communication
- Be named something memorable (e.g., "Sage" or "Narrator")

The agent will be highlighted in Lab 01 as an example of creating custom agents.

---

## Agent Prompt

```
You are creating a custom PM/Documentation agent for a GitHub Copilot lab.

**Context:**
- Target repo: ~/Coding_Projects/ghcp-contoso-university-lab
- This agent combines Product Manager and Technical Writer capabilities
- It will be showcased in the lab as an example of custom agent creation

**Tasks:**
1. Review the existing agents in .github/agents/ for consistency
2. Create a new agent: .github/agents/pm-doc-writer.agent.md

   Name: "Sage" - The Documentation Sage

   Capabilities:
   - Writing clear, user-focused documentation
   - Creating user stories with acceptance criteria
   - Asking clarifying questions to understand requirements
   - Translating technical concepts for different audiences

3. The agent should have:
   - YAML frontmatter:
     ```yaml
     ---
     description: "Documentation and story writing specialist who asks the right questions"
     name: "Sage"
     tools:
       - microsoft.docs.mcp
       - context7
       - fetch
     ---
     ```
   - ## Expertise: Documentation, user stories, requirements gathering
   - ## Principles: User value first, clarity over completeness, ask before assuming
   - ## Response Approach: Always ask 2-3 clarifying questions, provide structured output

4. Update labs/setup.md or README to mention this agent as an example

**Output:**
- Show the complete agent file content
- Confirm the agent follows the established pattern
- Note where documentation was updated

**Acceptance Criteria to verify:**
- [ ] AC1: pm-doc-writer.agent.md created
- [ ] AC2: Combines PM and tech writer capabilities
- [ ] AC3: Has unique personality
- [ ] AC4: References MCP tools
- [ ] AC5: Includes example prompts
- [ ] AC6: Referenced in lab docs
```

---

## Estimated Effort

- **Complexity**: Low-Medium
- **Files Changed**: 1-2 files

## Session Notes

### Implementation Summary

Created "Sage - The Documentation Sage" agent combining PM and tech writer roles:

**Agent Features:**
- Personality: "asks the right questions" - clarifies before creating
- Expertise: Documentation strategy, user story writing, requirements elicitation, audience translation
- Principles: User value first, clarity over completeness, ask before assuming
- Example prompts showing clarifying questions for docs and stories
- MCP tools: microsoft.docs.mcp, context7, fetch

**Documentation Updates:**
- Added "Included Agents" table to README.md listing all 4 Contoso-specific agents
- Added Lab 01 Highlight callout specifically referencing Sage as the example to study

**Files Created/Modified:**
| File | Action |
|------|--------|
| `.github/agents/pm-doc-writer.agent.md` | Created (65 lines, 3234 bytes) |
| `README.md` | Updated with agents table and Sage highlight |

**Verification:**
- Agent follows established pattern (YAML frontmatter + Expertise/Principles/Response Approach/Key Resources)
- All 6 acceptance criteria satisfied
