# Coding Standards

**For: GitHub Copilot Multi-Agent Orchestration Lab**
**Tech Stack: .NET/ASP.NET Core MVC, Entity Framework Core, C#, GitHub Copilot Agents**

---

## Purpose

This document defines coding standards for the Contoso University Lab project. These standards ensure consistency across human developers and AI agents, reduce cognitive load during code review, and maintain a codebase that is readable, maintainable, and secure.

---

## 1. Naming Conventions

### C# Code

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase, noun or noun phrase | `StudentController`, `CourseService` |
| Interfaces | PascalCase with `I` prefix | `IStudentRepository`, `ICourseService` |
| Methods | PascalCase, verb or verb phrase | `GetStudentById`, `EnrollInCourse` |
| Properties | PascalCase | `FirstName`, `EnrollmentDate` |
| Private fields | camelCase with `_` prefix | `_studentRepository`, `_logger` |
| Local variables | camelCase | `studentCount`, `isEnrolled` |
| Parameters | camelCase | `studentId`, `courseTitle` |
| Constants | PascalCase | `MaxEnrollmentCount`, `DefaultPageSize` |
| Async methods | Suffix with `Async` | `GetStudentByIdAsync`, `SaveChangesAsync` |

### File Naming

| File Type | Convention | Example |
|-----------|------------|---------|
| C# classes | PascalCase matching class name | `StudentController.cs` |
| Views | PascalCase matching action | `Index.cshtml`, `Details.cshtml` |
| Migrations | Timestamp prefix (auto-generated) | `20240115_AddStudentTable.cs` |
| Agent files | kebab-case with `.agent.md` suffix | `dotnet-developer.agent.md` |
| Instruction files | kebab-case with `.instructions.md` suffix | `action-guardrails.instructions.md` |
| Prompt files | kebab-case with `.prompt.md` suffix | `create-test.prompt.md` |
| Story files | `story-{NN}-{kebab-case-title}.md` | `story-01-add-student-search.md` |
| Epic files | `epic-{NN}-{kebab-case-title}.md` | `epic-01-student-management.md` |

### Database Naming

| Element | Convention | Example |
|---------|------------|---------|
| Tables | PascalCase, plural | `Students`, `Courses`, `Enrollments` |
| Columns | PascalCase | `FirstName`, `EnrollmentDate` |
| Foreign keys | `{ReferencedTable}Id` | `StudentId`, `CourseId` |
| Indexes | `IX_{Table}_{Column(s)}` | `IX_Students_LastName` |

---

## 2. File Organization

### Project Structure

```
ContosoUniversity/
├── Controllers/           # MVC controllers, one per resource
├── Models/                # Domain models and view models
│   ├── Entities/          # EF Core entity classes
│   └── ViewModels/        # View-specific models
├── Services/              # Business logic layer
│   ├── Interfaces/        # Service contracts
│   └── Implementations/   # Service implementations
├── Data/                  # Data access layer
│   ├── DbContext.cs       # EF Core context
│   └── Migrations/        # Database migrations
├── Views/                 # Razor views organized by controller
│   ├── Shared/            # Layouts, partials, components
│   └── {Controller}/      # Views for each controller
├── wwwroot/               # Static files (CSS, JS, images)
└── Tests/                 # Test projects mirror main structure
```

### Agent Organization

```
.github/
├── agents/                # Custom agent definitions
├── instructions/          # Domain-specific guardrails
├── prompts/               # Reusable task templates
└── skills/                # Deep knowledge bases
```

### Documentation Organization

```
docs/
├── epics/                 # Feature epics with progress tracking
├── stories/               # Individual story specifications
├── handoffs/              # Session continuity documents
└── standards/             # This directory - coding standards
```

### File Placement Rules

1. **One class per file** (exception: small related types like enums)
2. **File name matches primary type name**
3. **Group by feature** when project grows large (vertical slice architecture)
4. **Keep related files close**: Controller, Service, and Repository for same domain together
5. **Tests mirror source**: `StudentController.cs` → `StudentControllerTests.cs`

---

## 3. Error Handling

### Principles

1. **Fail loudly**: Never swallow exceptions silently
2. **Structured logging**: Use ILogger with structured parameters
3. **User-friendly messages**: Technical details in logs, friendly messages to users
4. **Specific exceptions**: Catch specific types, not bare `Exception`

### Controller Error Handling

```csharp
// GOOD: Specific exception handling with logging
public async Task<IActionResult> GetStudent(int id)
{
    try
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null)
        {
            _logger.LogWarning("Student not found: {StudentId}", id);
            return NotFound();
        }
        return View(student);
    }
    catch (DbException ex)
    {
        _logger.LogError(ex, "Database error retrieving student {StudentId}", id);
        return StatusCode(500, "Unable to retrieve student data");
    }
}

// BAD: Silent failure, generic catch
public async Task<IActionResult> GetStudent(int id)
{
    try
    {
        return View(await _studentService.GetByIdAsync(id));
    }
    catch (Exception)
    {
        return View(new Student()); // Silent failure - never do this
    }
}
```

### Service Layer Error Handling

```csharp
// Throw meaningful exceptions from services
public async Task<Student> GetByIdAsync(int id)
{
    if (id <= 0)
        throw new ArgumentException("Student ID must be positive", nameof(id));

    var student = await _context.Students.FindAsync(id);
    return student; // Let caller handle null
}
```

### Validation

1. **Validate at boundaries**: Controller actions, API endpoints, service entry points
2. **Use Data Annotations** for model validation
3. **Return validation errors explicitly**: Don't proceed with invalid data

```csharp
// Model validation
public class StudentCreateModel
{
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }
}
```

---

## 4. Testing Requirements

### Test Coverage Expectations

| Layer | Minimum Coverage | Focus |
|-------|------------------|-------|
| Services | 80% | Business logic, edge cases |
| Controllers | 70% | Action routing, model binding |
| Repositories | 60% | Query correctness |
| Integration | Key flows | End-to-end scenarios |

### Test Naming Convention

```
{MethodUnderTest}_{Scenario}_{ExpectedBehavior}
```

Examples:
- `GetByIdAsync_ValidId_ReturnsStudent`
- `GetByIdAsync_InvalidId_ReturnsNull`
- `EnrollStudent_AlreadyEnrolled_ThrowsException`

### Test Structure (Arrange-Act-Assert)

```csharp
[Fact]
public async Task GetByIdAsync_ValidId_ReturnsStudent()
{
    // Arrange
    var expectedStudent = new Student { Id = 1, LastName = "Smith" };
    _mockContext.Setup(c => c.Students.FindAsync(1))
        .ReturnsAsync(expectedStudent);

    // Act
    var result = await _service.GetByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Smith", result.LastName);
}
```

### Testing Protocol (from Project Principles)

1. **One test at a time**: Write, run, verify pass, then next
2. **No skipping**: Never use `.Skip()` to bypass failing tests
3. **Verify before marking complete**: `VERIFY: Ran [test name] — Result: PASS`
4. **Happy path AND edge cases**: Both required for coverage

### What to Test

- **Do test**: Business logic, validation rules, edge cases, error conditions
- **Don't test**: Framework code, third-party libraries, trivial getters/setters

---

## 5. Documentation Expectations

### Code Comments

1. **XML documentation** on all public APIs:

```csharp
/// <summary>
/// Retrieves a student by their unique identifier.
/// </summary>
/// <param name="id">The student's unique identifier.</param>
/// <returns>The student if found; otherwise, null.</returns>
/// <exception cref="ArgumentException">Thrown when id is less than or equal to zero.</exception>
public async Task<Student?> GetByIdAsync(int id)
```

2. **Inline comments** only for non-obvious logic:

```csharp
// EF Core tracks this entity, so changes will persist on SaveChanges
student.EnrollmentDate = DateTime.UtcNow;
```

3. **TODO comments** with ticket reference:

```csharp
// TODO(ISSUE-123): Add caching layer for frequently accessed students
```

### README Requirements

Each significant directory should have a README explaining:
- Purpose of the directory
- Key files and their roles
- How to use/extend

### Agent and Story Documentation

Follow established patterns:
- **Agents**: YAML frontmatter + Markdown body with Expertise, Principles, Response Approach
- **Stories**: Summary, Acceptance Criteria (checkboxes), Dependencies, Implementation Notes
- **Handoffs**: Session metadata, current/next story, files modified, notes for future

---

## 6. Security Considerations

### Input Validation

1. **Never trust user input**: Validate all external data
2. **Parameterized queries only**: Never concatenate user input into SQL
3. **Encode output**: Use Razor's automatic encoding, explicitly encode when needed

```csharp
// GOOD: Parameterized query via EF Core
var students = await _context.Students
    .Where(s => s.LastName == lastName)
    .ToListAsync();

// BAD: String concatenation (SQL injection risk)
var query = $"SELECT * FROM Students WHERE LastName = '{lastName}'";
```

### Authentication and Authorization

1. **Use ASP.NET Core Identity** for authentication
2. **Authorize at controller/action level**:

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : Controller { }

[Authorize(Policy = "CanEditStudent")]
public async Task<IActionResult> Edit(int id) { }
```

3. **Validate ownership**: Ensure users can only access their own data

### Sensitive Data

1. **Never log sensitive data**: Passwords, tokens, PII
2. **Use secrets management**: User Secrets in dev, Key Vault in production
3. **HTTPS only**: Enforce in production

```csharp
// GOOD: Structured logging without sensitive data
_logger.LogInformation("User {UserId} logged in", user.Id);

// BAD: Logging sensitive information
_logger.LogInformation("User logged in with password {Password}", password);
```

### Dependency Security

1. **Keep packages updated**: Regular `dotnet outdated` checks
2. **Review package sources**: Use official NuGet feeds only
3. **Scan for vulnerabilities**: Use `dotnet list package --vulnerable`

---

## 7. Git and Version Control

### Commit Discipline

1. **Individual file adds**: Never use `git add .`
2. **Know what you're committing**: Review diff before commit
3. **Atomic commits**: One logical change per commit

### Commit Message Format

```
[ACTION]: [Description]

[Optional body with details]
```

**Action verbs**:
- `ADD`: New feature, file, or capability
- `FIX`: Bug fix
- `UPDATE`: Enhancement to existing feature
- `REFACTOR`: Code restructure without behavior change
- `DOCS`: Documentation only
- `TEST`: Test additions or corrections
- `CHORE`: Build, config, or tooling changes

### Branch Strategy

- `main`: Production-ready code
- `feature/{description}`: New features
- `fix/{description}`: Bug fixes
- `docs/{description}`: Documentation updates

---

## 8. Code Style

### Formatting

- **Indentation**: 4 spaces (no tabs)
- **Braces**: Allman style (opening brace on new line)
- **Line length**: 120 characters max
- **Blank lines**: One between methods, two between classes

### LINQ Style

```csharp
// GOOD: Method syntax for complex queries
var results = students
    .Where(s => s.EnrollmentDate > cutoffDate)
    .OrderBy(s => s.LastName)
    .ThenBy(s => s.FirstName)
    .Select(s => new StudentViewModel(s))
    .ToList();

// GOOD: Query syntax for joins
var enrollments =
    from e in context.Enrollments
    join s in context.Students on e.StudentId equals s.Id
    join c in context.Courses on e.CourseId equals c.Id
    select new { Student = s.FullName, Course = c.Title };
```

### Async/Await

1. **Async all the way**: Don't block on async code
2. **ConfigureAwait(false)** in library code
3. **Suffix with Async**: All async methods end in `Async`

```csharp
// GOOD
public async Task<Student> GetByIdAsync(int id)
{
    return await _context.Students.FindAsync(id);
}

// BAD: Blocking on async
public Student GetById(int id)
{
    return _context.Students.FindAsync(id).Result; // Deadlock risk
}
```

---

## 9. Agent Development Standards

### Agent File Structure

```yaml
---
description: "[Two-sentence description]"
name: "[Persona Name] - [Full Title]"
tools:
  - [required tools]
---

# [Persona Name] - [Full Title]

## Expertise
- [5-7 bullet points of capabilities]

## Principles
- [Design philosophy and values]

## Response Approach
1. [Numbered methodology steps]

## Key Resources
- [Relevant documentation links]
```

### Agent Design Principles

1. **Specialization over generalization**: Each agent has focused expertise
2. **Named personas**: Memorable names (Scout, Scribe, Builder)
3. **Principle-driven**: Clear values guide behavior
4. **Example-rich**: Include example prompts and responses
5. **Tool declarations**: Explicit about which tools the agent uses

### Instruction File Standards

```yaml
---
description: "[What this instruction covers]"
applyTo: "**/*"  # or specific file patterns
---
```

---

## 10. Explicit Reasoning Protocol

When implementing features or fixing bugs, follow the project's explicit reasoning protocol:

### Before Action

```
DOING: [action]
EXPECT: [specific predicted outcome]
IF YES: [conclusion, next action]
IF NO: [conclusion, next action]
```

### After Action

```
RESULT: [what actually happened]
MATCHES: [yes/no]
THEREFORE: [conclusion and next action]
```

### On Failure

1. State what failed (raw error)
2. State your theory about why
3. State what you want to do
4. State what you expect to happen
5. **Wait for confirmation before proceeding**

---

## Checklist for Code Review

Before submitting or approving code:

- [ ] Naming conventions followed
- [ ] Files in correct locations
- [ ] Error handling in place (no silent failures)
- [ ] Validation at boundaries
- [ ] Tests written and passing
- [ ] Public APIs documented
- [ ] No security vulnerabilities (injection, XSS, etc.)
- [ ] No sensitive data in logs
- [ ] Commits are atomic with proper messages
- [ ] Code formatted consistently

---

**Remember**: These standards exist to reduce friction and cognitive load. When in doubt, optimize for clarity and maintainability over cleverness.
