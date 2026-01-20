---
description: "Evaluate code against the project's coding standards rubric with structured scoring and actionable feedback"
agent: "Builder - .NET Developer"
tools:
  - microsoft.docs.mcp
  - context7
---

# Grade Code Against Standards

Evaluate the provided code against `/docs/standards/coding-standards-rubric.md`.

## Input

Provide code via:
- File reference: `#file:path/to/file.cs`
- Code selection in editor
- PR number

## Process

1. Load the rubric from `/docs/standards/coding-standards-rubric.md`
2. Apply the `coding-standards-grading` skill
3. Score each criterion (1-10 scale):

| Criterion | Weight | Question |
|-----------|--------|----------|
| Clarity | 20% | Can a developer understand this quickly? |
| Correctness | 20% | Does it work for all cases? |
| Robustness | 15% | How does it handle failures? |
| Security | 15% | Does it introduce vulnerabilities? |
| Simplicity | 15% | Is this the simplest solution? |
| Maintainability | 15% | Can it be changed safely? |

## Output Format

```markdown
## Code Evaluation: [identifier]

| Criterion | Score | Notes |
|-----------|-------|-------|
| Clarity | /10 | |
| Correctness | /10 | |
| Robustness | /10 | |
| Security | /10 | |
| Simplicity | /10 | |
| Maintainability | /10 | |
| **Overall** | /10 | |

**Verdict**: [PASS / NEEDS WORK / FAIL]

**Must Fix**:
- [Line X: issue and fix]

**Strengths**:
- [what's done well]
```

## Verdict Criteria

- **PASS**: Overall >= 7, no criterion below 6, no security blockers
- **NEEDS WORK**: Overall 5-6.9, or one criterion below 6
- **FAIL**: Overall < 5, any criterion below 4, or critical security issue
