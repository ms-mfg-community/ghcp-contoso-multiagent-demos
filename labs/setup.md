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

> **Tip:** A [Dev Container](https://containers.dev/) is included in this repository for zero-config setup. Choose one of the options below based on your environment, or skip ahead to [manual setup](#fork-and-clone-the-repository).

## Option A: GitHub Codespaces (Recommended)

If your organization has [GitHub Codespaces](https://github.com/features/codespaces) enabled, this is the fastest way to get started — no local installs required.

1. Navigate to your fork on GitHub (or the upstream repo if you have access)
2. Click the green **Code** button → **Codespaces** tab → **Create codespace on main**
3. Wait for the codespace to build (the devcontainer will install .NET 8, Node.js 18, and all extensions automatically)
4. Once the VS Code editor loads in your browser (or desktop VS Code if configured), verify:
   ```bash
   dotnet --version    # Should show 8.x
   node --version      # Should show v18.x or later
   ```
5. Build the solution:
   ```bash
   dotnet build ContosoUniversity/ContosoUniversity.sln
   ```
6. Skip ahead to [Enable GitHub Copilot Custom Agents](#enable-github-copilot-custom-agents)

> **Note:** Codespaces usage may be subject to your organization's spending limits and policies.

## Option B: Dev Container with Docker Desktop

If you have [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed locally:

1. Install the [Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) in VS Code
2. Clone the repository (see [Fork and Clone](#fork-and-clone-the-repository) below)
3. Open the cloned folder in VS Code
4. When prompted **"Reopen in Container"**, click it — or use the Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`) → **"Dev Containers: Reopen in Container"**
5. Wait for the container to build and the solution to restore/build automatically
6. Verify the setup:
   ```bash
   dotnet --version    # Should show 8.x
   node --version      # Should show v18.x or later
   ```
7. Skip ahead to [Enable GitHub Copilot Custom Agents](#enable-github-copilot-custom-agents)

## Option C: Dev Container with Podman

If you cannot install Docker and use [Podman](https://podman.io/) instead:

1. Install [Podman](https://podman.io/docs/installation) (and optionally [Podman Desktop](https://podman-desktop.io/))
2. Install the [Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) in VS Code
3. Configure VS Code to use Podman as the container engine:
   - Open VS Code Settings (`Ctrl+,` / `Cmd+,`)
   - Search for **"Dev Containers: Docker Path"**
   - Set the value to `podman`
4. Clone the repository (see [Fork and Clone](#fork-and-clone-the-repository) below)
5. Open the cloned folder in VS Code
6. Use the Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`) → **"Dev Containers: Reopen in Container"**
7. If using **rootless Podman** and you encounter permission issues, you may need to add the following to `.devcontainer/devcontainer.json`:
   ```json
   "runArgs": ["--userns=keep-id"]
   ```
8. Verify the setup:
   ```bash
   dotnet --version    # Should show 8.x
   node --version      # Should show v18.x or later
   ```
9. Skip ahead to [Enable GitHub Copilot Custom Agents](#enable-github-copilot-custom-agents)

> **Note:** Podman is not officially supported by the Dev Containers specification but works well in practice. See the [Podman Desktop guide](https://podman-desktop.io/blog/2025/05/05/vs-code-with-podman-desktop) for additional tips.

## Option D: Manual Setup

If you prefer not to use containers, install the [prerequisites](#prerequisites) above and follow the steps below.

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
<summary>Dev Container not starting with Podman</summary>

1. Verify Podman is running: `podman info`
2. Ensure VS Code setting **"Dev Containers: Docker Path"** is set to `podman`
3. For rootless Podman permission errors, add `"runArgs": ["--userns=keep-id"]` to `.devcontainer/devcontainer.json`
4. On Windows, ensure Podman machine is started: `podman machine start`
5. Check the VS Code Output panel → **Dev Containers** for detailed error logs

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
