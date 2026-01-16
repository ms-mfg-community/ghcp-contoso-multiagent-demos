# 1 - Introduction to Multi-Agent Orchestration

In this lab you will learn the fundamentals of multi-agent orchestration and explore GitHub Copilot custom agents.

> Duration: 15-20 minutes

References:
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Model Context Protocol](https://modelcontextprotocol.io)
- [Custom Agents in VS Code](https://code.visualstudio.com/docs/copilot/copilot-extensibility-overview)

---

## 1.1 Multi-Agent Orchestration Concepts

**What is Multi-Agent Orchestration?**

Multi-agent orchestration is the coordination of multiple AI agents to accomplish complex tasks. Instead of relying on a single general-purpose AI, you leverage specialized agents that excel at specific domains—analysis, writing, coding, documentation—and coordinate their work to achieve outcomes that would be difficult or impossible for a single agent.

Think of it like a software development team: you have architects, developers, testers, and technical writers. Each brings specialized skills, and the project succeeds through their coordinated efforts.

### Sequential Task Execution

In sequential orchestration, agents work one after another. Each agent completes its task before the next begins.

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   Agent A   │────▶│   Agent B   │────▶│   Agent C   │
│  (Analyze)  │     │   (Design)  │     │   (Build)   │
└─────────────┘     └─────────────┘     └─────────────┘
     Step 1              Step 2              Step 3
```

**When to use sequential:**
- Tasks have natural dependencies (analysis must complete before design)
- Output from one agent is input for the next
- Order matters for correctness

**Example:** Scout analyzes the codebase → Scribe writes user stories based on findings → Builder implements the stories.

### Parallel Task Execution

In parallel orchestration, multiple agents work simultaneously on independent tasks.

```
                    ┌─────────────┐
              ┌────▶│   Agent A   │────┐
              │     │  (Task 1)   │    │
┌─────────┐   │     └─────────────┘    │     ┌─────────┐
│  Start  │───┤     ┌─────────────┐    ├────▶│  Done   │
└─────────┘   │     │   Agent B   │    │     └─────────┘
              ├────▶│  (Task 2)   │────┤
              │     └─────────────┘    │
              │     ┌─────────────┐    │
              └────▶│   Agent C   │────┘
                    │  (Task 3)   │
                    └─────────────┘
```

**When to use parallel:**
- Tasks are independent and don't share dependencies
- Speed is important and tasks can run concurrently
- Different aspects of a problem can be tackled simultaneously

**Example:** While Scout analyzes Models/, another Scout instance analyzes Controllers/, and a third analyzes Views/—all simultaneously.

### Knowledge Check

Consider this scenario: You need to modernize a legacy .NET application. The work involves:
1. Analyzing the current codebase structure
2. Identifying technical debt
3. Writing migration user stories
4. Creating API documentation

**Question:** Which tasks could run in parallel, and which must be sequential?

<details>
<summary>Answer</summary>

**Sequential (must happen in order):**
- Analysis (1) → Writing user stories (3): You need to understand the codebase before you can write stories about changing it.
- Analysis (1) → Technical debt identification (2): You need to explore the code to find the debt.

**Could be parallel (after analysis completes):**
- Writing user stories (3) and Creating API documentation (4): These are independent documentation tasks that don't depend on each other.
- Technical debt identification (2) and Creating API documentation (4): These examine different aspects of the codebase.

**Optimal orchestration:**
```
Sequential: Analysis → [Parallel: Tech Debt + User Stories + Documentation]
```

</details>

---

## 1.2 Workflow Patterns

Beyond how agents execute (sequential vs parallel), you also need to decide how to structure your work sessions.

### Single-Session Workflows

In a single-session workflow, you complete the entire task in one continuous conversation with one or more agents.

**Characteristics:**
- All context stays in the same conversation
- Agents can reference earlier discussion naturally
- Best for focused, bounded tasks
- No handoff overhead

**When to use:**
- Task can be completed in under 30 minutes
- Clear scope with well-defined inputs and outputs
- Single logical unit of work
- You have time for uninterrupted focus

**Example:** "Analyze the Student.cs model and suggest three improvements" — Scout can complete this in one session.

### Multi-Session Workflows

In a multi-session workflow, work spans multiple conversations. This requires explicit handoffs and context preservation.

**Characteristics:**
- Work is divided across multiple sessions
- Requires documentation of progress and context
- Agents need "onboarding" at the start of each session
- Better for complex, evolving work

**When to use:**
- Large tasks spanning hours or days
- Work that needs review/approval between phases
- Multiple people collaborating
- Tasks where requirements may evolve

**Context Preservation Strategies:**
1. **Session notes:** Document decisions, findings, and next steps
2. **Artifact files:** Write analysis to files that persist between sessions
3. **Handoff prompts:** Structured summaries agents can consume
4. **Progress tracking:** Checklists, story files, or project boards

**Example multi-session workflow:**

| Session | Agent | Task | Output |
|---------|-------|------|--------|
| 1 | Scout | Analyze codebase | `docs/analysis.md` |
| 2 | Scribe | Write user stories | `docs/stories/` |
| 3 | Builder | Implement first story | Code changes + PR |
| 4 | Sage | Write documentation | `docs/api-guide.md` |

### Choosing the Right Pattern

| Factor | Single-Session | Multi-Session |
|--------|---------------|---------------|
| Task complexity | Low to Medium | High |
| Time required | < 30 minutes | Hours to days |
| Review needed | No intermediate review | Review between phases |
| Context size | Fits in one conversation | Too large for one session |
| Collaboration | Solo work | Team or async work |

### Exercise: Pattern Selection

For each scenario, decide whether single-session or multi-session is more appropriate:

1. Add a validation attribute to a model property
2. Migrate an application from .NET 6 to .NET 8
3. Write unit tests for an existing service class
4. Redesign the database schema for better performance

<details>
<summary>Answers</summary>

1. **Single-session** — Small, bounded change with clear scope.

2. **Multi-session** — Large undertaking requiring analysis, planning, incremental changes, and testing phases. You'll want to review progress between major steps.

3. **Single-session** (usually) — If the service is small/medium sized, tests can be written in one focused session. For a very large service, consider multi-session with tests grouped by functionality.

4. **Multi-session** — Database redesign requires: analysis of current schema, understanding data relationships, designing new schema, planning migration strategy, implementing changes, and validating data integrity. Each phase benefits from review.

</details>

---

## 1.3 Exploring GitHub Copilot Custom Agents

Now let's get hands-on with the custom agents configured for this lab.

### Navigating to Agents

1. In VS Code, open the Explorer panel (Ctrl+Shift+E / Cmd+Shift+E)
2. Navigate to `.github/agents/`
3. You should see multiple agent files with the `.agent.md` extension

Take a moment to note the agent files present. You should see agents created specifically for Contoso University (Scout, Scribe, Builder, Sage) as well as Azure-focused agents from the template.

### Understanding Agent File Structure

Open `brownfield-analyst.agent.md` (Scout) and examine its structure:

```yaml
---
description: "Brief description of the agent's role"
name: "Nickname - Full Title"
tools:
  - tool-name-1
  - tool-name-2
---
```

**Key components:**

| Section | Purpose |
|---------|---------|
| `description` | What GitHub Copilot displays when listing agents |
| `name` | The agent's display name (includes nickname for easy reference) |
| `tools` | MCP tools and VS Code capabilities the agent can use |
| Main content | Detailed instructions including Expertise, Principles, and Response Approach |

### Hands-On: Invoke the Scout Agent

Let's try using Scout to analyze a file in the Contoso University codebase.

1. Open GitHub Copilot Chat (Ctrl+Alt+I / Cmd+Alt+I)
2. In the chat input, type `@` to see available agents
3. Select `@Scout` (or `@brownfield-analyst`)
4. Ask Scout to analyze a model file:

```
@Scout Please analyze the ContosoUniversity/Models/Student.cs file.
What patterns do you see? Are there any improvements you'd suggest?
```

5. Observe how Scout responds:
   - Does it examine the file structure?
   - Does it identify patterns (like data annotations)?
   - Does it reference .NET best practices?

### Exercise: Agent Exploration

Try invoking different agents and observe how their personalities and expertise differ:

**Task 1:** Ask Scribe (story-writer) to draft a user story
```
@Scribe Write a user story for adding a search feature to the Students list page.
```

**Task 2:** Ask Sage (pm-doc-writer) to explain a concept
```
@Sage Explain what Entity Framework migrations are and when to use them.
```

**Reflection questions:**
- How does each agent's response style differ?
- What unique value does each agent bring?
- How might you combine these agents in a workflow?

<details>
<summary>Discussion</summary>

**Response style differences:**
- **Scout** focuses on analysis, patterns, and technical assessment. Responses tend to be investigative and observational.
- **Scribe** produces structured deliverables (user stories) with consistent formatting. Responses follow templates.
- **Sage** explains concepts clearly with context. Responses are educational and often include examples.

**Unique value:**
- Scout: Understands existing code without judgment, finds opportunities
- Scribe: Translates technical findings into stakeholder-friendly stories
- Sage: Creates documentation that helps future developers (and agents)

**Combining agents:**
A typical workflow might be: Scout analyzes → Scribe documents requirements → Builder implements → Sage writes docs. Each agent's output feeds the next agent's input.

</details>

---

## Summary

In this lab, you learned:

- **Multi-agent orchestration** coordinates specialized AI agents to accomplish complex tasks
- **Sequential execution** is for dependent tasks; **parallel execution** is for independent tasks
- **Single-session workflows** suit bounded tasks; **multi-session workflows** suit complex projects
- **Custom agents** are defined in `.github/agents/` with YAML frontmatter and markdown instructions
- Each agent has a distinct personality and expertise tailored to specific tasks

**Next:** In Lab 2, you'll use Scout to perform a comprehensive analysis of the Contoso University codebase.
