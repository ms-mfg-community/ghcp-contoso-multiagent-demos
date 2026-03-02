# GitHub Copilot Multi-Agent Orchestration Lab

> Learn GitHub Copilot's multi-agent orchestration capabilities through hands-on exercises using a .NET Contoso University application.

## Learning Objectives

By completing this lab, you will learn how to:

- Configure and customize GitHub Copilot agents for your codebase
- Orchestrate multiple AI agents to collaborate on complex tasks
- Create brownfield epics that analyze existing codebases
- Write implementation stories with clear acceptance criteria
- Execute stories using specialized agents for different roles

## Prerequisites

| Requirement | Version | Purpose |
|-------------|---------|---------|
| VS Code | Latest | Primary IDE with GitHub Copilot integration |
| GitHub Copilot | Business/Enterprise | Required for custom agent features |
| Node.js | 18+ | MCP server execution |
| .NET SDK | 8.0+ | Contoso University application |
| Git | Latest | Version control |

> **Tip:** A Dev Container is included for zero-config setup — just open in VS Code and select "Reopen in Container". See the [Lab Setup](/labs/setup.md) for details.

---

## Hands-on Labs

### Lab Setup

- [ ] [Lab Setup](/labs/setup.md) - Fork repository, configure environment, enable custom agents

### Lab 01: Introduction to Multi-Agent Orchestration

- [ ] _Hands-on Lab:_ :point_right: [Lab 01](/labs/lab01.md)

Learn the fundamentals of GitHub Copilot's multi-agent architecture. Understand how agents, instructions, prompts, and skills work together.

### Lab 02: Creating Brownfield Epics

- [ ] _Hands-on Lab:_ :point_right: [Lab 02](/labs/lab02.md)

Analyze an existing .NET codebase to create comprehensive epics. Use agents to understand architecture, identify patterns, and document the system.

### Lab 03: Writing Implementation Stories

- [ ] _Hands-on Lab:_ :point_right: [Lab 03](/labs/lab03.md)

Transform epics into actionable user stories with acceptance criteria. Learn to write stories that AI agents can execute effectively.

### Lab 04: Executing Stories with Agents

- [ ] _Hands-on Lab:_ :point_right: [Lab 04](/labs/lab04.md)

Put it all together by executing stories using orchestrated agents. Watch multiple agents collaborate to implement features.

---

## Learning Path

Continue your learning journey with these resources:

- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [GitHub Copilot Business Features](https://docs.github.com/en/copilot/copilot-business)
- [Model Context Protocol](https://modelcontextprotocol.io/)

---

## Repository Structure

### Custom Agent Configuration

```
.github/
├── agents/           # Custom Copilot agent definitions
├── instructions/     # Contextual instruction files
├── prompts/          # Reusable prompt templates
├── skills/           # Reference knowledge for agents
└── copilot-instructions.md  # Global Copilot instructions
```

### Lab Materials

```
labs/
├── setup.md          # Environment setup and prerequisites
├── images/           # Lab diagrams and screenshots
├── lab01.md          # Introduction to Multi-Agent Orchestration
├── lab02.md          # Creating Brownfield Epics
├── lab03.md          # Writing Implementation Stories
└── lab04.md          # Executing Stories with Agents
```

### Components

| Component | Purpose |
|-----------|---------|
| **Agents** | Specialized personas with defined expertise, tools, and response patterns |
| **Instructions** | Contextual guidelines applied to specific file patterns |
| **Prompts** | Reusable prompt templates for common tasks |
| **Skills** | Deep reference knowledge agents can access |

### Included Agents

| Agent | Nickname | Purpose |
|-------|----------|---------|
| `brownfield-analyst` | Scout | Analyzes existing codebases to understand architecture and patterns |
| `story-writer` | Scribe | Creates user stories with acceptance criteria from requirements |
| `dotnet-developer` | Builder | Implements .NET features following project patterns |
| `pm-doc-writer` | **Sage** | Documentation specialist who asks clarifying questions before writing |

> **Lab 01 Highlight**: Study `pm-doc-writer.agent.md` (Sage) as an example of custom agent creation. Notice how it combines Product Manager and Technical Writer capabilities with a distinct personality that asks questions before assuming.

### MCP Server Configuration

The `.vscode/mcp.json` file configures Model Context Protocol servers for VS Code:

- **microsoft-learn**: Access Microsoft Learn documentation, code samples, and guides
- **context7**: Access third-party library documentation (via Context7)

> **Note:** MCP servers are configured at the workspace level in `.vscode/mcp.json`. Agents automatically have access to all tools (including MCP server tools) via `tools: ["*"]` in their frontmatter.

---

## Additional Resources

### GitHub Copilot

- [Getting started with GitHub Copilot](https://docs.github.com/en/copilot/getting-started-with-github-copilot)
- [Configuring GitHub Copilot in your environment](https://docs.github.com/en/copilot/configuring-github-copilot/configuring-github-copilot-in-your-environment)
- [GitHub Copilot Trust Center](https://resources.github.com/copilot-trust-center/)

### .NET Development

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Contoso University Tutorial](https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro)

### AI-Assisted Development

- [AI-Assisted Software Engineering](https://github.blog/ai-and-ml/)
- [GitHub Copilot Blog](https://github.blog/tag/github-copilot/)

---

## Getting Started

1. Complete the [Lab Setup](/labs/setup.md)
2. Work through labs sequentially (Lab 01 through Lab 04)
3. Each lab builds on concepts from previous labs
4. Create a GitHub Issue in your fork to track progress

> **Note:** The Contoso University application source code is included in this repository under `ContosoUniversity/`. No additional cloning is required.

---

## Contributing

This is an educational repository. If you find issues or have suggestions:

1. Open an issue describing the problem or improvement
2. Reference the specific lab or section
3. Include screenshots if relevant

---

## License

This project is provided for educational purposes.
