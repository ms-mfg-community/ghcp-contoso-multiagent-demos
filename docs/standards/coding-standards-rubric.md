# Coding Standards Rubric

**For: AI Agent Code Evaluation**
**Scope: Method, Class, File, or PR**
**Scoring: 1-10 Scale per Criterion**

---

## Purpose

This rubric enables systematic evaluation of code at any granularity - from a single method to a full pull request. Each criterion scales naturally to the scope being evaluated.

---

## Scoring Scale

| Score | Rating | Description |
|-------|--------|-------------|
| 10 | Exemplary | Exceeds standards; reference-quality code |
| 9 | Excellent | Fully meets standards with polish |
| 8 | Very Good | Meets standards; trivial improvements possible |
| 7 | Good | Meets most standards; minor issues |
| 6 | Satisfactory | Acceptable; room for improvement |
| 5 | Borderline | Minimum acceptable; needs attention |
| 4 | Below Standard | Does not meet standards; revise |
| 3 | Poor | Significant violations; rework needed |
| 2 | Very Poor | Fails most criteria; major rewrite |
| 1 | Unacceptable | Broken or dangerous; reject |

**Passing**: 6+ on all criteria | **Overall**: Average of all scores

---

## Evaluation Criteria

### 1. Clarity (Weight: 20%)

**Question**: Can a competent developer understand this code quickly without additional context?

| Score | Indicators |
|-------|------------|
| **9-10** | Intent obvious at a glance. Names reveal purpose. Logic flows naturally. No mental gymnastics required. |
| **7-8** | Clear with minimal effort. Good names. Straightforward logic. Perhaps one moment of "wait, what?" |
| **5-6** | Understandable but requires concentration. Some unclear names or convoluted logic. |
| **3-4** | Confusing. Must trace through carefully. Unclear names, nested conditions, magic values. |
| **1-2** | Incomprehensible without significant study. Misleading names, spaghetti logic. |

**What to check**:
- Are names self-documenting? (`calculateTotalCredits` vs `calc`)
- Is logic linear or nested? (early returns vs deep nesting)
- Are magic numbers/strings explained or extracted?
- Does each unit do one clear thing?

**Scope adjustments**:
- Method: Can you understand it in 30 seconds?
- Class: Does each method name tell you what the class does?
- File/PR: Is the overall purpose immediately apparent?

---

### 2. Correctness (Weight: 20%)

**Question**: Does this code do what it claims to do, including edge cases?

| Score | Indicators |
|-------|------------|
| **9-10** | Handles all cases correctly. Edge cases addressed. Null/empty/boundary conditions covered. Logic is provably correct. |
| **7-8** | Works correctly for normal cases. Most edge cases handled. Minor gaps that are unlikely to occur. |
| **5-6** | Works for happy path. Some edge cases missed. Would fail under unusual but possible conditions. |
| **3-4** | Bugs present in normal flow. Logic errors. Would fail in common scenarios. |
| **1-2** | Fundamentally broken. Does not accomplish stated purpose. |

**What to check**:
- Does the logic match the method/class name?
- What happens with null, empty, zero, negative, max values?
- Are comparisons correct? (`<` vs `<=`, `==` vs `===`)
- Are async operations awaited properly?
- Are resources disposed/closed?

**Scope adjustments**:
- Method: Does it return correct results for all inputs?
- Class: Do methods interact correctly? State management sound?
- File/PR: Do components integrate correctly?

---

### 3. Robustness (Weight: 15%)

**Question**: How does this code behave when things go wrong?

| Score | Indicators |
|-------|------------|
| **9-10** | Graceful handling of all failure modes. Specific exceptions. Informative errors. Recovers when possible, fails cleanly when not. |
| **7-8** | Good error handling for expected failures. Logging present. Errors don't cascade silently. |
| **5-6** | Basic error handling. Some paths could fail unexpectedly. Generic catches in places. |
| **3-4** | Fragile. Exceptions swallowed. Silent failures. Errors produce confusing behavior. |
| **1-2** | No error handling, or error handling that hides problems or corrupts state. |

**What to check**:
- Are exceptions specific (not bare `catch`)?
- Are errors logged with context?
- Does failure produce useful information?
- Are there silent `catch {}` blocks?
- Is validation present for external inputs?

**Scope adjustments**:
- Method: What happens if parameters are invalid? If dependencies fail?
- Class: How does it handle partial failures across methods?
- File/PR: How do components handle failures in their collaborators?

---

### 4. Security (Weight: 15%)

**Question**: Does this code introduce vulnerabilities or expose sensitive data?

| Score | Indicators |
|-------|------------|
| **9-10** | No vulnerabilities. Inputs validated. Queries parameterized. Output encoded. Auth verified. Secrets protected. |
| **7-8** | Secure against common attacks. Minor improvements possible but no exploitable issues. |
| **5-6** | Mostly secure. Some validation gaps. No critical vulnerabilities but room for hardening. |
| **3-4** | Security issues present. Missing validation, potential injection, or auth gaps. |
| **1-2** | Critical vulnerabilities. Injection possible, auth bypassed, or secrets exposed. |

**What to check**:
- Are user inputs validated before use?
- Are queries parameterized (not string concatenation)?
- Is output encoded to prevent XSS?
- Are auth/authz checks present where needed?
- Is sensitive data logged or exposed?

**Scope adjustments**:
- Method: Does it trust its inputs appropriately?
- Class: Does it enforce security at its boundary?
- File/PR: Are security concerns addressed throughout the change?

**Note**: Score 1-3 on security is often a blocking issue regardless of other scores.

---

### 5. Simplicity (Weight: 15%)

**Question**: Is this the simplest code that could solve the problem?

| Score | Indicators |
|-------|------------|
| **9-10** | Elegant simplicity. No unnecessary abstractions. Each line earns its place. You couldn't remove anything. |
| **7-8** | Appropriately simple. Minor over-engineering or redundancy possible. |
| **5-6** | Some unnecessary complexity. Extra layers, unused parameters, or premature abstractions. |
| **3-4** | Over-engineered. Abstractions without justification. Configuration for things that don't vary. |
| **1-2** | Needlessly complex. Architecture astronautics. Code that fights itself. |

**What to check**:
- Could this be shorter without losing clarity?
- Are abstractions justified by actual variation?
- Are there parameters/options that nothing uses?
- Is there dead code or commented-out code?
- Does complexity match problem complexity?

**Scope adjustments**:
- Method: Is it doing one thing, simply?
- Class: Does it have a single, clear responsibility?
- File/PR: Is the change proportionate to the problem?

---

### 6. Maintainability (Weight: 15%)

**Question**: How easy will it be to modify this code correctly in the future?

| Score | Indicators |
|-------|------------|
| **9-10** | Highly maintainable. Changes would be localized. Dependencies are explicit. No hidden coupling. Well-documented where non-obvious. |
| **7-8** | Easy to maintain. Clear structure. Most changes would be straightforward. |
| **5-6** | Maintainable with care. Some implicit dependencies or coupling. Changes require tracing impact. |
| **3-4** | Difficult to maintain. Hidden dependencies. Changes risk breaking unrelated code. |
| **1-2** | Maintenance nightmare. Changes anywhere break things elsewhere. No one wants to touch this. |

**What to check**:
- Are dependencies explicit (injected, not created)?
- Is state mutation localized and clear?
- Are side effects obvious or documented?
- Would a change here require changes elsewhere?
- Is there adequate documentation for complex logic?

**Scope adjustments**:
- Method: Is it self-contained or coupled to external state?
- Class: How many other classes would need to change if this changes?
- File/PR: Does this change make future changes easier or harder?

---

## Evaluation Templates

### Quick Evaluation (Method/Class)

```markdown
## Code Evaluation: [method/class name]

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

**Key Issues** (if any):
1. [specific issue and fix]

**Strengths**:
- [what's done well]
```

### Detailed Evaluation (File/PR)

```markdown
## Code Evaluation Report

**Scope**: [file path or PR reference]
**Evaluator**: [agent name]
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
[Observations and specific examples]

#### Correctness: [X]/10
[Observations and specific examples]

#### Robustness: [X]/10
[Observations and specific examples]

#### Security: [X]/10
[Observations and specific examples]

#### Simplicity: [X]/10
[Observations and specific examples]

#### Maintainability: [X]/10
[Observations and specific examples]

### Recommendations

**Must fix** (blocking):
- [ ] [specific issue]

**Should fix** (important):
- [ ] [specific issue]

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

## Quick Reference

| Criterion | Core Question | Red Flags |
|-----------|---------------|-----------|
| **Clarity** | Can I understand this quickly? | Cryptic names, deep nesting, magic values |
| **Correctness** | Does it work for all cases? | Missing null checks, off-by-one, unhandled cases |
| **Robustness** | What if things go wrong? | Empty catches, silent failures, no logging |
| **Security** | Does it introduce risk? | String concat in queries, missing validation, exposed secrets |
| **Simplicity** | Is this the simplest solution? | Unused abstractions, dead code, over-engineering |
| **Maintainability** | Can this be changed safely? | Hidden coupling, implicit state, undocumented magic |

---

## Examples by Scope

### Method Example

```csharp
// Evaluating this method:
public async Task<Student?> GetByIdAsync(int id)
{
    if (id <= 0)
        throw new ArgumentException("ID must be positive", nameof(id));

    return await _context.Students
        .Include(s => s.Enrollments)
        .FirstOrDefaultAsync(s => s.Id == id);
}
```

| Criterion | Score | Rationale |
|-----------|-------|-----------|
| Clarity | 9 | Name describes intent, logic is linear |
| Correctness | 8 | Handles invalid ID, returns null for not found (caller must handle) |
| Robustness | 7 | Validates input, but DB errors would propagate unhandled |
| Security | 9 | Parameterized query via EF Core |
| Simplicity | 9 | Does exactly one thing, no over-engineering |
| Maintainability | 8 | Clear dependencies, easy to modify |
| **Overall** | **8.3** | PASS |

### Class Example (excerpt)

```csharp
// Red flags in a class:
public class StudentManager
{
    private static List<Student> _cache = new(); // Global mutable state
    private string _connectionString = "Server=..."; // Hard-coded secret

    public Student Get(int id)
    {
        try { return _cache.First(s => s.Id == id); }
        catch { return null; } // Silent failure
    }
}
```

| Criterion | Score | Rationale |
|-----------|-------|-----------|
| Clarity | 4 | Vague name, unclear caching behavior |
| Correctness | 3 | First() throws on not found, caught but wrong result |
| Robustness | 2 | Silent failure hides bugs |
| Security | 1 | Hard-coded connection string |
| Simplicity | 5 | Simple but wrong |
| Maintainability | 2 | Static state, hidden dependencies |
| **Overall** | **2.8** | FAIL - Critical security issue, multiple violations |

---

**Remember**: Adjust your focus based on scope. A method review focuses on that method's responsibilities. A PR review considers how changes interact across the codebase.
