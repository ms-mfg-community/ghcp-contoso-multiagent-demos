---
name: coding-standards-grading
description: Evaluates code against the project's coding standards rubric, providing structured assessments with scores and specific feedback. Use when reviewing code quality, grading implementations, or asking any agent to assess code at method, class, file, or PR scope.
---

# Coding Standards Grading

## Purpose

Systematically evaluate code against the 6 criteria defined in `/docs/standards/coding-standards-rubric.md`:
- **Clarity** (20%): Can a developer understand this quickly?
- **Correctness** (20%): Does it work for all cases?
- **Robustness** (15%): How does it handle failures?
- **Security** (15%): Does it introduce vulnerabilities?
- **Simplicity** (15%): Is this the simplest solution?
- **Maintainability** (15%): Can it be changed safely?

---

## MCP-First: Query Current Best Practices

**Before evaluating, query MCP tools for language-specific standards:**

### Microsoft Learn MCP

```
1. microsoft_docs_search: query="[language] coding best practices Azure"
2. microsoft_code_sample_search: query="[pattern] best practices", language="[lang]"
```

Recommended queries:
- "C# coding conventions .NET"
- "Python Azure SDK best practices"
- "TypeScript error handling patterns"
- "Entity Framework query optimization"

### Context7 MCP

```
1. resolve-library-id: libraryName="[framework]", query="best practices"
2. query-docs: libraryId from step 1, query="[specific pattern]"
```

Recommended library lookups:
- `dotnet` - C#/.NET conventions
- `typescript` - TypeScript patterns
- `python` - Python best practices

---

## How to Use This Skill

### Input

Provide one of the following:
1. **File path**: `@workspace/path/to/file.cs`
2. **Code selection**: Selected code in the editor
3. **PR reference**: GitHub PR number or URL

Optionally specify:
- **Scope**: `method`, `class`, `file`, or `pr` (auto-detected if not specified)
- **Focus**: Specific criterion to emphasize (e.g., "security", "clarity")

### Invocation Examples

```
Evaluate this code against our coding standards: @workspace/src/Services/StudentService.cs

Grade the CreateStudent method for coding standards compliance.

Review this PR against the coding standards rubric: #123
```

---

## Evaluation Process

### Step 1: Load the Rubric

Read and apply `/docs/standards/coding-standards-rubric.md` as the evaluation standard.

### Step 2: Determine Scope

| Scope | Focus |
|-------|-------|
| Method | 30-second comprehension, single responsibility |
| Class | Cohesion, method naming, state management |
| File | Overall purpose, organization, dependencies |
| PR | Change impact, consistency, integration |

### Step 3: Evaluate Each Criterion

For each of the 6 criteria:
1. Apply the rubric's scoring indicators (1-10 scale)
2. Note specific code examples supporting the score
3. Identify concrete improvements

### Step 4: Generate Assessment

Use the appropriate output template based on scope.

---

## Output Templates

### Quick Evaluation (Method/Class)

```markdown
## Code Evaluation: [identifier]

| Criterion | Score | Notes |
|-----------|-------|-------|
| Clarity | /10 | [specific observation] |
| Correctness | /10 | [specific observation] |
| Robustness | /10 | [specific observation] |
| Security | /10 | [specific observation] |
| Simplicity | /10 | [specific observation] |
| Maintainability | /10 | [specific observation] |
| **Overall** | /10 | [weighted average] |

**Verdict**: [PASS / NEEDS WORK / FAIL]

**Key Issues** (if any):
1. [Line X: specific issue and fix]

**Strengths**:
- [what's done well]
```

### Detailed Evaluation (File/PR)

```markdown
## Code Evaluation Report

**Scope**: [file path or PR reference]
**Evaluator**: GitHub Copilot
**Date**: [YYYY-MM-DD]

### Summary

| Criterion | Weight | Score | Weighted |
|-----------|--------|-------|----------|
| Clarity | 20% | /10 | |
| Correctness | 20% | /10 | |
| Robustness | 15% | /10 | |
| Security | 15% | /10 | |
| Simplicity | 15% | /10 | |
| Maintainability | 15% | /10 | |
| **Overall** | 100% | | /10 |

### Detailed Findings

#### Clarity: [X]/10
[Observations with line references]

#### Correctness: [X]/10
[Observations with line references]

#### Robustness: [X]/10
[Observations with line references]

#### Security: [X]/10
[Observations with line references]

#### Simplicity: [X]/10
[Observations with line references]

#### Maintainability: [X]/10
[Observations with line references]

### Recommendations

**Must fix** (blocking):
- [ ] [specific issue with location]

**Should fix** (important):
- [ ] [specific issue with location]

**Consider** (suggestions):
- [ ] [specific improvement]

### Verdict

[PASS / NEEDS WORK / FAIL]

Criteria:
- PASS: Overall >= 7, no criterion below 6, no blocking security issues
- NEEDS WORK: Overall 5-6.9, or one criterion below 6
- FAIL: Overall < 5, or any criterion below 4, or critical security issue
```

---

## Scoring Quick Reference

| Criterion | Core Question | Red Flags |
|-----------|---------------|-----------|
| **Clarity** | Can I understand this quickly? | Cryptic names, deep nesting, magic values |
| **Correctness** | Does it work for all cases? | Missing null checks, off-by-one, unhandled cases |
| **Robustness** | What if things go wrong? | Empty catches, silent failures, no logging |
| **Security** | Does it introduce risk? | String concat in queries, missing validation, exposed secrets |
| **Simplicity** | Is this the simplest solution? | Unused abstractions, dead code, over-engineering |
| **Maintainability** | Can this be changed safely? | Hidden coupling, implicit state, undocumented magic |

---

## Language-Specific Checks

### C# / .NET

- Async/await patterns correct
- IDisposable properly implemented
- LINQ vs loops appropriate
- Null handling (nullable reference types)
- Exception specificity

### TypeScript / JavaScript

- Type safety (no `any` abuse)
- Promise handling correct
- Event listener cleanup
- Proper error boundaries

### Python

- Type hints present
- Context managers for resources
- Exception handling specific
- Docstrings for public APIs

---

## Reference

- [Project Rubric](/docs/standards/coding-standards-rubric.md) - Authoritative scoring criteria
- [.NET Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Azure SDK Design Guidelines](https://azure.github.io/azure-sdk/general_introduction.html)
