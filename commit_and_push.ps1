$ErrorActionPreference = "Stop"
Set-Location "C:\Users\codycarlson\git\agentic-workflows\ghcp-contoso-university-agent-workflow-orchestration-labs"

Write-Host "=== Step 1: Git Status ===" 
git --no-pager status --short

Write-Host "`n=== Step 2: Adding files ===" 
git add README.md labs/setup.md

Write-Host "`n=== Step 3: Committing ===" 
$commitMessage = @"
Add Docker, Podman, and Codespaces setup instructions

- Add Option A (Codespaces), Option B (Docker), Option C (Podman),
  Option D (Manual) sections to labs/setup.md
- Include Podman-specific config steps (Docker Path setting, rootless
  userns workaround)
- Add Podman troubleshooting entry
- Update README.md devcontainer tip to mention all three options

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>
"@

git commit -m $commitMessage

Write-Host "`n=== Step 4: Git Log ===" 
git --no-pager log --oneline -3

Write-Host "`n=== Step 5: Pushing ===" 
git push origin feature/devcontainer-and-prerequisites-update

Write-Host "`n=== All steps completed successfully ===" 
