# Lab Setup

In this lab you will fork the repository, configure your development environment, and enable GitHub Copilot custom agents.

> Duration: 15-20 minutes

References:
- [Fork a repository](https://docs.github.com/en/get-started/quickstart/fork-a-repo)
- [GitHub Copilot documentation](https://docs.github.com/en/copilot)
- [Model Context Protocol](https://modelcontextprotocol.io/)

## Prerequisites

Before starting this lab, ensure you have the following installed:

| Requirement | Version | Purpose |
|-------------|---------|---------|
| VS Code | Latest | Primary IDE with GitHub Copilot integration |
| GitHub Copilot | Business/Enterprise | Required for agent orchestration features |
| Node.js | 18+ | Required for MCP server execution |
| .NET SDK | 8.0+ | Required for the Contoso University application |
| Git | Latest | Version control |

> **Tip:** A [Dev Container](https://containers.dev/) is included in this repository for zero-config setup. Open the repo in VS Code and select **"Reopen in Container"** when prompted, or use the Command Palette → **"Dev Containers: Reopen in Container"**. The container includes .NET 8 SDK, Node.js 18, and all recommended extensions pre-installed.

## Fork and Clone the Repository

1. Fork this repository from [ms-mfg-community/ghcp-contoso-multiagent-demos](https://github.com/ms-mfg-community/ghcp-contoso-multiagent-demos)

2. Clone your forked repository:
   ```bash
   git clone https://github.com/YOUR_USERNAME/ghcp-contoso-multiagent-demos.git
   cd ghcp-contoso-multiagent-demos
   ```

3. Verify the `.github/` directory structure exists:
   ```
   .github/
   ├── agents/           # Custom Copilot agent definitions
   ├── instructions/     # Contextual instruction files
   ├── prompts/          # Reusable prompt templates
   ├── skills/           # Reference knowledge for agents
   └── copilot-instructions.md
   ```
## Enable GitHub Copilot Custom Agents

1. Open VS Code and ensure the GitHub Copilot extension is installed and active

2. Open the Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`)

3. Search for "GitHub Copilot: Enable Custom Agents" and enable the feature

4. Verify agents are loaded:
   - Open GitHub Copilot Chat
   - Type `@` to see available agents
   - You should see the custom agents defined in `.github/agents/`

## Configure MCP Servers (Optional)

The `.vscode/mcp.json` file in this repository configures Model Context Protocol servers at the workspace level. This configuration is automatically loaded by VS Code when you open the workspace.

**Included MCP Servers:**

| Server | Purpose |
|--------|---------|
| `microsoft-learn` | Access Microsoft Learn documentation, code samples, and guides |
| `context7` | Access third-party library documentation via Context7 |

**Setup Steps:**

1. Ensure Node.js 18+ is installed (required for the Context7 MCP server)

2. Open the workspace in VS Code - the `.vscode/mcp.json` configuration is automatically detected

3. When prompted, allow VS Code to start the MCP servers

4. Verify MCP servers are connected in the Copilot Chat panel

> **Note:** All agents in this repository use `tools: ["*"]` which grants access to all available tools, including MCP server tools.

## Verify Your Setup

Run through this checklist to confirm everything is working:

- [ ] Repository cloned successfully
- [ ] `.github/agents/` directory contains agent definitions
- [ ] VS Code opened with GitHub Copilot extension active
- [ ] Custom agents visible in Copilot Chat (type `@` to check)
- [ ] .NET project builds: `dotnet build ContosoUniversity/ContosoUniversity.sln`
- [ ] (Optional) MCP servers connected

## Track Your Progress

Create a GitHub Issue in your forked repository to track your lab progress:

- **Title:** Lab Progress - Multi-Agent Orchestration
- **Content:**
```markdown
### Lab Progress Tracker

- [x] Lab Setup
- [ ] Lab 01: Introduction to Multi-Agent Orchestration
- [ ] Lab 02: Creating Brownfield Epics
- [ ] Lab 03: Writing Implementation Stories
- [ ] Lab 04: Executing Stories with Agents
```

## Troubleshooting

<details>
<summary>Custom agents not appearing in Copilot Chat</summary>

1. Ensure you have a GitHub Copilot subscription that supports custom agents
2. Check that the `.github/agents/` directory exists and contains `.agent.md` files
3. Restart VS Code and try again
4. Check VS Code's Output panel for Copilot-related errors

</details>

<details>
<summary>MCP servers not connecting</summary>

1. Verify Node.js is installed: `node --version`
2. Ensure npx is available: `npx --version`
3. Check your network connection
4. Review the VS Code Output panel for MCP-related errors

</details>

<details>
<summary>.NET build fails</summary>

1. Verify .NET 8 SDK is installed: `dotnet --list-sdks`
2. Restore packages: `dotnet restore`
3. Check for any missing dependencies in the error output

</details>

---

## Lab File Naming Convention

Labs in this repository follow a consistent naming pattern:

| File | Description |
|------|-------------|
| `setup.md` | This setup guide |
| `lab01.md` | Lab 01: Introduction to Multi-Agent Orchestration |
| `lab02.md` | Lab 02: Creating Brownfield Epics |
| `lab03.md` | Lab 03: Writing Implementation Stories |
| `lab04.md` | Lab 04: Executing Stories with Agents |

Each lab file includes:
- Duration estimate
- Learning objectives
- Prerequisites (completed labs)
- Step-by-step instructions
- Verification checkpoints
- References section

---

[Next: Lab 01 - Introduction to Multi-Agent Orchestration](lab01.md)
