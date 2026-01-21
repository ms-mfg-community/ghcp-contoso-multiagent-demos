---
description: "Evaluate user stories against the project's story writing standards rubric with structured scoring and actionable feedback"
---

# Grade Story Against Standards

**Use the Scribe (Story Writer) agent** to evaluate the provided user story against `/docs/standards/story-writing-standards-rubric.md`.

> **Note**: This prompt delegates to the Scribe agent for story writing expertise. Ensure the `runSubagent` tool is enabled in Copilot Chat settings for isolated execution.

## Input

Provide story via:
- File reference: `#file:stories/story-001.md`
- Story content pasted in chat
- Work item reference

## Process

1. Load the rubric from `/docs/standards/story-writing-standards-rubric.md`
2. Apply the `story-writing-standards` skill
3. Score each criterion (1-10 scale):

| Criterion | Weight | Question |
|-----------|--------|----------|
| Acceptance Criteria Quality | 25% | Can an implementer verify completion? |
| Testability | 20% | Can automated tests be written directly? |
| Scope Clarity | 20% | Is it unambiguous what IS and IS NOT included? |
| Technical Detail Sufficiency | 20% | Does an AI agent have enough context? |
| Project Pattern Alignment | 15% | Does it follow project conventions? |

## Output Format

```markdown
## Story Evaluation: [Story ID - Title]

| Criterion | Score | Notes |
|-----------|-------|-------|
| AC Quality | /10 | |
| Testability | /10 | |
| Scope Clarity | /10 | |
| Technical Detail | /10 | |
| Pattern Alignment | /10 | |
| **Overall** | /10 | |

**Verdict**: [READY FOR DEV / NEEDS REFINEMENT / NEEDS REWRITE]

**Must Address**:
- [blocking issue with suggested fix]

**Suggested Rewrites**:
- Original: "[vague text]"
- Improved: "[specific text]"

**Strengths**:
- [what's well-defined]
```

## Verdict Criteria

- **READY FOR DEV**: Overall >= 7, no criterion below 6
- **NEEDS REFINEMENT**: Overall 5-6.9, or one criterion below 6
- **NEEDS REWRITE**: Overall < 5, or multiple criteria below 4
