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

> **See it in action:** [Sequential Workflow Example](../docs/workflows/sequential-workflow-example.md) demonstrates this pipeline implementing a complete feature, including quality gates at each step.

**Quality Gates:** Between pipeline stages, use quality verification to catch issues early. The [coding-standards-grading](.github/skills/coding-standards-grading/SKILL.md) and [story-writing-standards](.github/skills/story-writing-standards/SKILL.md) skills provide rubrics for validating code and user stories before proceeding.

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
3. Writing migration stories
4. Creating existing API documentation

**Question:** Which tasks could run in parallel, and which must be sequential?

<details>
<summary>Answer</summary>

**Sequential (must happen in order):**
- (1) Analysis → (2)(4) → (3) Writing migration stories. You need to understand the codebase before you can identify technical debt and create existing API documentation. Furthermore, you must know the technical debt you want to address and API's that require migrating in order to write all of your migration stories.

**Could be parallel (after analysis completes):**
- (2) Identifying technical debt and (4) Creating existing API documentation These are independent documentation tasks that don't depend on each other.

**Optimal orchestration:**
```
Analysis → [Parallel: Tech Debt + Documentation] → Migration Stories
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

**Example:** "Analyze the Student.cs model. Compare the implementation against our models coding standards rubric located at ${path}. Suggest at least 3 improvements per category that recieves less than a 6/10. If all categories pass then merely output 'Model meets coding standards'" — Scout can complete this in one session.

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
3. **Handoff prompts:** Structured summaries agents can consume — use the [/create-handoff prompt](../.github/prompts/create-handoff.prompt.md) to generate consistent handoff documents
4. **Progress tracking:** Checklists, story files, or project boards
5. **Quality gates:** Verify work meets standards before proceeding — use grading skills to validate code and stories

**Example multi-session workflow:**

| Session | Agent | Task | Output |
|---------|-------|------|--------|
| 1 | Scout | Analyze codebase | `docs/analysis.md` |
| 2 | Scribe | Write user stories | `docs/stories/` |
| 3 | Builder | Implement first story | Code changes + PR |
| 4 | Sage | Write documentation | `docs/api-guide.md` |

> **See it in action:** [Multi-Session Workflow Example](../docs/workflows/multi-session-workflow-example.md) walks through a complete 4-session implementation with handoff documents between each session.

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

## 1.3 Orchestration Patterns

Beyond deciding between sequential and parallel execution, you also need to decide *who* coordinates the workflow—you or an agent.

### Human-Driven vs. Agent-Driven Orchestration

**Human-Driven Orchestration** (what this lab primarily teaches):
- You decide which agent to invoke and when
- You interpret results and determine next steps
- You manage context between sessions manually
- You apply quality gates and make judgment calls

```
┌─────────────────────────────────────────────────────────────────┐
│              Human-Driven Orchestration                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│      YOU (Orchestrator)                                          │
│        │                                                         │
│        ├──▶ @Scout "Analyze the codebase"                       │
│        │         └──▶ Returns analysis                          │
│        │    [You review, decide what matters]                   │
│        │                                                         │
│        ├──▶ @Scribe "Create stories based on analysis"          │
│        │         └──▶ Returns stories                           │
│        │    [You validate, refine, approve]                     │
│        │                                                         │
│        ├──▶ @Builder "Implement this story"                     │
│        │         └──▶ Returns code                              │
│        │    [You review, test, iterate]                         │
│        │                                                         │
│        └──▶ @Sage "Document what we did"                        │
│                  └──▶ Returns docs                              │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

**Agent-Driven Orchestration** (using an orchestrator agent):
- An orchestrator agent (Maestro) proposes workflow structure
- It decomposes goals into phases and identifies parallel opportunities
- It suggests quality gates—but you still approve and verify
- Best for structured, deterministic workflows where the path is clear

```
┌─────────────────────────────────────────────────────────────────┐
│              Agent-Driven Orchestration                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│      YOU ──▶ @Maestro "Plan the grade management feature"       │
│                │                                                 │
│                ▼                                                 │
│         ┌─────────────┐                                         │
│         │  Maestro    │ (Produces workflow plan)                │
│         │  - Phases   │                                         │
│         │  - Prompts  │                                         │
│         │  - Gates    │                                         │
│         └─────────────┘                                         │
│                │                                                 │
│                ▼                                                 │
│         YOU execute the plan, invoking agents as directed       │
│         YOU verify quality gates and make go/no-go decisions    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### When to Use Each Pattern

| Factor | Human-Driven | Agent-Driven |
|--------|--------------|--------------|
| **Best for** | Complex work requiring judgment, nuanced decisions, novel problems | Structured, repeatable, deterministic workflows |
| **Control** | Maximum—you decide everything | Agent proposes, you approve and execute |
| **Quality gates** | You review and decide | Agent suggests criteria, you verify |
| **Flexibility** | High—pivot anytime based on discoveries | Follows structured plan |
| **Oversight** | Built-in—you're in the loop | You must actively insert yourself |

**Use human-driven orchestration when:**
- Work requires nuanced judgment or domain expertise
- Quality gates need human review (code review, stakeholder approval)
- The problem is novel or exploratory
- Decisions have significant consequences
- You're learning or need to understand the details
- Requirements may change based on what you discover

**Use agent-driven orchestration when:**
- The workflow is well-understood and repeatable
- You want a consistent structure across similar projects
- The path from goal to completion is deterministic
- You need help identifying parallel execution opportunities
- You want a starting plan to customize

> **Key insight**: Agent-driven orchestration produces a *plan*—you still execute it, verify quality gates, and make decisions. The orchestrator agent doesn't replace your judgment; it provides structure.

### The Orchestrator Agent: Maestro

This lab includes an orchestrator agent called **Maestro** (`.github/agents/workflow-orchestrator.agent.md`) that can help plan workflows involving Scout, Scribe, Builder, and Sage.

**What Maestro does well:**
- Decomposes goals into standard phases (analysis → documentation → implementation → handoff)
- Identifies which tasks might run in parallel
- Suggests quality gate criteria
- Produces a structured plan with sample prompts

**What Maestro doesn't do:**
- Execute the plan autonomously (you do that)
- Make judgment calls about quality (you verify)
- Adapt to unexpected discoveries (you pivot)
- Replace domain expertise (you provide context)

### Parallel Execution with Subagents

GitHub Copilot supports running agents as **subagents**—isolated agents that work independently and return only their final result. When tasks are independent, you can run multiple subagents in parallel.

**Sequential (dependent tasks):**
```
@Scout Analyze Models/
[wait for result]
@Scout Analyze Controllers/
[wait for result]
```

**Parallel (independent tasks using subagents):**
```
#runSubagent @Scout Analyze Models/ for entity patterns
#runSubagent @Scout Analyze Controllers/ for action patterns
#runSubagent @Scout Analyze Views/ for Razor patterns
```

All three analyses run concurrently, and results return to your main conversation when complete.

**When parallel helps:**
- Independent analysis of different code areas
- Research tasks that don't depend on each other
- Creating documentation for separate components
- Running validations on independent files

**When to stay sequential:**
- Tasks depend on prior results
- You need to review and decide before continuing
- Quality gates require human verification

### Exercise: Compare Orchestration Approaches

Let's see how Maestro plans a workflow and compare it to manual orchestration.

1. Open GitHub Copilot Chat
2. Invoke Maestro:

```
@Maestro Plan the implementation of a student search feature for the Contoso
University application. The feature should allow searching students by name
from the Students Index page.
```

3. Review Maestro's output and consider:
   - Is the phase decomposition helpful?
   - Are the suggested quality gates appropriate?
   - What would you do differently?
   - Where would you want more control?

<details>
<summary>Expected Maestro Output</summary>

Maestro should produce a workflow plan similar to:

```markdown
# Workflow Plan: Student Search Feature

## Goal
Enable searching students by name from the Students Index page.

## Execution Overview

| Phase | Specialist | Execution | Your Role |
|-------|-----------|-----------|-----------|
| 1. Analysis | Scout | Can parallelize | Review findings |
| 2. Stories | Scribe | Sequential | Validate & approve |
| 3. Implementation | Builder | Sequential | Review & test |
| 4. Handoff | Sage | Sequential | Verify completeness |

## Phases

### Phase 1: Analysis
**Specialist**: @Scout
**Parallel opportunity**: Multiple Scout subagents for independent areas

**Sample Prompts**:
> #runSubagent @Scout Analyze existing search patterns in the codebase
> #runSubagent @Scout Analyze StudentsController.cs for Index action patterns

**Your quality gate**: Review analysis, confirm approach before stories

### Phase 2: Story Creation
**Specialist**: @Scribe
**Sample Prompt**:
> @Scribe Create a user story for student search with acceptance criteria.

**Your quality gate**: Validate story against rubric, refine if needed

### Phase 3: Implementation
**Specialist**: @Builder
**Sample Prompt**:
> @Builder Implement the student search story following existing patterns.

**Your quality gate**: Review code, run tests, verify AC met

### Phase 4: Documentation
**Specialist**: @Sage
**Sample Prompt**:
> @Sage Document the student search feature.

**Your quality gate**: Verify documentation accuracy
```

</details>

### Reflection Questions

1. **Where does Maestro's plan help?**
   - Consistent phase structure
   - Identified parallel opportunities
   - Sample prompts ready to use

2. **Where do you still need judgment?**
   - Verifying analysis is sufficient
   - Approving story quality
   - Reviewing code correctness
   - Deciding when to deviate from the plan

3. **When would you skip the orchestrator?**
   - Small, quick tasks
   - Exploratory work
   - When you need maximum flexibility

<details>
<summary>Discussion</summary>

**Orchestrator value:**
- Provides structure for repeatable workflows
- Helps identify parallel execution opportunities
- Gives you a starting point to customize
- Ensures you don't skip phases

**Human orchestration value:**
- Better for complex, judgment-heavy work
- Allows pivoting based on discoveries
- You learn more about the codebase
- Quality gates are truly verified, not just checked off

**Recommended approach for most work:**
- Use Maestro to generate a plan for unfamiliar workflows
- Execute manually, staying in the loop at each phase
- Apply human judgment at every quality gate
- Deviate from the plan when discoveries warrant it

</details>

---

## 1.4 Exploring GitHub Copilot Custom Agents

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
@Scout Grade ContosoUniversity/Models/Student.cs against the coding standards rubric. Provide scores for each criterion and an overall verdict.
```

5. Observe how Scout responds:
   - Does it score each criterion (Clarity, Correctness, Robustness, Security, Simplicity, Maintainability)?
   - Does it provide an overall verdict (PASS / NEEDS WORK / FAIL)?
   - Does it reference specific line numbers with actionable feedback?
   - Does it identify both strengths and issues?

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
- **Human-driven orchestration** is best for complex work requiring judgment; **agent-driven orchestration** helps with structured, repeatable workflows
- **Subagents** enable parallel execution of independent tasks, returning results to the main conversation
- **Orchestrator agents** (like Maestro) produce plans—but you execute them and verify quality gates
- **Custom agents** are defined in `.github/agents/` with YAML frontmatter and markdown instructions
- Each agent has a distinct personality and expertise tailored to specific tasks

**Next:** In Lab 2, you'll use Scout to perform a comprehensive analysis of the Contoso University codebase.
