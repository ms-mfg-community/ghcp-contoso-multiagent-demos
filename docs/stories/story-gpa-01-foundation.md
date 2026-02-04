# Story GPA-01: GPA Calculation Foundation - Repository & Service Layer

## Summary

Create the foundational infrastructure for GPA (Grade Point Average) calculation by fixing the repository pattern to support eager loading of related entities and implementing a GpaCalculationService with weighted GPA formula. This story addresses a critical architectural limitation blocking GPA feature development and establishes the business logic layer for accurate GPA computation.

## Assigned Agent

**Builder** (dotnet-developer specialist)

## User Story

As a **developer**, I want the repository pattern to support eager loading of Student enrollments with related Course data and a dedicated GPA calculation service, so that I can accurately compute weighted GPA for display in student views.

## Business Value

- Unblocks the GPA feature development roadmap (Phases 2-4)
- Resolves critical technical debt in the repository pattern
- Establishes reusable, testable business logic for GPA calculations
- Enables future grade-related features (transcripts, honors, academic standing)

## Acceptance Criteria

### AC1: Repository Eager Loading Support
**Given** the Student repository needs to load enrollments with course data  
**When** calling a method to retrieve a student by ID with related entities  
**Then** the repository returns the Student with:
- Enrollments collection fully populated
- Each Enrollment.Course navigation property loaded
- No additional database queries (N+1 problem avoided)

**Verification**:
- [ ] `IRepository<Student>` interface has method to support eager loading (e.g., `GetByIdWithIncludesAsync`)
- [ ] Implementation in `Repository<T>` uses `.Include()` and `.ThenInclude()` for proper eager loading
- [ ] Existing `GetByIdAsync` method remains unchanged for backward compatibility
- [ ] Database query logging shows single query with JOINs, not multiple queries

### AC2: GPA Calculation Service Interface
**Given** the need for testable, reusable GPA calculation logic  
**When** defining the service interface  
**Then** the interface exposes methods for:
- Calculating GPA from a Student entity
- Calculating GPA from an Enrollment collection
- Clear return type (decimal) and parameter contracts

**Verification**:
- [ ] `IGpaCalculationService` interface created in `ContosoUniversity.Core/Interfaces/`
- [ ] Method signature: `decimal CalculateGpa(IEnumerable<Enrollment> enrollments)`
- [ ] XML documentation comments describe weighted GPA formula
- [ ] Interface follows existing project patterns (async naming, parameter types)

### AC3: Weighted GPA Calculation Implementation
**Given** a collection of enrollments with grades and course credits  
**When** calculating the GPA using the weighted formula  
**Then** the calculation returns: `Σ(GradePoints × Credits) / Σ(Credits)` where Grade is NOT NULL

**Grade Point Mapping**:
- A = 4.0
- B = 3.0
- C = 2.0
- D = 1.0
- F = 0.0

**Formula**:
```csharp
// Example: 
// Course 1: Grade A (4.0) × 3 credits = 12.0 quality points
// Course 2: Grade B (3.0) × 3 credits = 9.0 quality points
// Course 3: Grade C (2.0) × 3 credits = 6.0 quality points
// Total: 27.0 quality points / 9 credits = 3.00 GPA
```

**Verification**:
- [ ] `GpaCalculationService` class created in `ContosoUniversity.Infrastructure/Services/`
- [ ] Implements `IGpaCalculationService` interface
- [ ] Uses correct grade point mapping (A=4.0, B=3.0, C=2.0, D=1.0, F=0.0)
- [ ] Returns result rounded to 2 decimal places (e.g., 3.67)
- [ ] Only includes enrollments where `Grade.HasValue == true` (NULL grades excluded)

### AC4: Edge Case Handling
**Given** various edge case scenarios for GPA calculation  
**When** the calculation service is called  
**Then** it handles each case gracefully without throwing exceptions:

| Scenario | Expected Result |
|----------|-----------------|
| No enrollments (empty collection) | Returns 0.0 |
| All enrollments have NULL grades | Returns 0.0 |
| Mixed NULL and graded enrollments | Calculates using only graded courses |
| Single enrollment with grade | Returns grade points as GPA (e.g., A = 4.00) |
| All courses have 0 credits | Returns 0.0 (avoid division by zero) |

**Verification**:
- [ ] Service handles null/empty collections without exceptions
- [ ] Service filters out NULL grades before calculation
- [ ] Service returns 0.0 when total credits = 0 (no division by zero)
- [ ] All edge cases have corresponding unit tests

### AC5: Unit Test Coverage
**Given** the GpaCalculationService contains critical business logic  
**When** running the test suite  
**Then** comprehensive unit tests validate all calculation scenarios

**Required Test Cases**:
1. Standard calculation with multiple graded courses (mixed grades)
2. Empty enrollments collection returns 0.0
3. Null enrollments collection returns 0.0
4. All NULL grades returns 0.0
5. Mixed NULL and graded enrollments (only graded count)
6. Single enrollment (GPA equals grade point value)
7. Zero credit courses excluded from calculation
8. Rounding accuracy (e.g., 10/3 = 3.33, not 3.3333...)
9. Sample: Carson Alexander (A, C, B in 3-credit courses) = 3.00 GPA

**Verification**:
- [ ] Test file created: `ContosoUniversity.Tests/Services/GpaCalculationServiceTests.cs`
- [ ] All 9+ test cases implemented and passing
- [ ] Tests follow AAA pattern (Arrange, Act, Assert)
- [ ] Tests use clear naming convention (e.g., `CalculateGpa_WithEmptyEnrollments_ReturnsZero`)
- [ ] Code coverage ≥ 90% for GpaCalculationService

### AC6: Dependency Injection Registration
**Given** the GpaCalculationService needs to be available to controllers  
**When** the application starts  
**Then** the service is registered in the DI container with appropriate lifetime

**Verification**:
- [ ] Service registered in `ContosoUniversity.Web/Program.cs` or DI configuration
- [ ] Registration uses appropriate lifetime (Scoped recommended for per-request calculation)
- [ ] Registration follows existing service registration patterns
- [ ] Service can be injected into controllers via constructor injection

## Scope

### In Scope
- Fix repository pattern to support eager loading via new method
- Create `IGpaCalculationService` interface and implementation
- Implement weighted GPA calculation formula with grade point mapping
- Handle all edge cases (NULL grades, empty collections, zero credits)
- Comprehensive unit tests for GpaCalculationService
- Register service in dependency injection container
- Update StudentsController.Details to use new repository method (minimal change for testing)

### Out of Scope (Future Stories)
- GPA display in views (Story GPA-02)
- Student model GPA property (Story GPA-02)
- Integration tests with UI (Story GPA-03)
- Performance optimization (caching, database storage) (Story GPA-03)
- GPA sorting/filtering in Index view (Future enhancement)
- Historical/semester GPA tracking (Future enhancement)
- Transcript page with GPA (Future enhancement)

## Dependencies

### Prerequisites
- ✅ Analysis document completed: `docs/analysis/gpa-feature-analysis.md`
- ✅ Branch created: `copilot/add-gpa-calculation-display`
- ✅ NULL grade handling decision: **Option A - Exclude from calculation**
- ✅ Test infrastructure exists: `ContosoUniversity.Tests` project

### Blocks
- **Story GPA-02** (Display) - Cannot display GPA without calculation service
- **Story GPA-03** (Testing) - Integration tests need working service

## Implementation Notes

### Files to Create

1. **Interface: `ContosoUniversity.Core/Interfaces/IGpaCalculationService.cs`**
   ```csharp
   namespace ContosoUniversity.Core.Interfaces;
   
   /// <summary>
   /// Service for calculating student Grade Point Average (GPA).
   /// Uses weighted formula: Σ(GradePoints × Credits) / Σ(Credits)
   /// </summary>
   public interface IGpaCalculationService
   {
       /// <summary>
       /// Calculates weighted GPA from enrollment collection.
       /// Excludes enrollments with NULL grades (in-progress courses).
       /// </summary>
       /// <param name="enrollments">Collection of enrollments with grades and courses</param>
       /// <returns>GPA rounded to 2 decimals, or 0.0 if no graded courses</returns>
       decimal CalculateGpa(IEnumerable<Enrollment> enrollments);
   }
   ```

2. **Implementation: `ContosoUniversity.Infrastructure/Services/GpaCalculationService.cs`**
   ```csharp
   namespace ContosoUniversity.Infrastructure.Services;
   
   public class GpaCalculationService : IGpaCalculationService
   {
       public decimal CalculateGpa(IEnumerable<Enrollment> enrollments)
       {
           if (enrollments == null || !enrollments.Any())
               return 0.0m;
           
           // Filter to only graded enrollments (exclude NULL grades)
           var gradedEnrollments = enrollments
               .Where(e => e.Grade.HasValue)
               .ToList();
           
           if (!gradedEnrollments.Any())
               return 0.0m;
           
           // Calculate total quality points: Σ(GradePoints × Credits)
           var totalQualityPoints = gradedEnrollments
               .Sum(e => GetGradePoints(e.Grade.Value) * e.Course.Credits);
           
           // Calculate total credits: Σ(Credits)
           var totalCredits = gradedEnrollments
               .Sum(e => e.Course.Credits);
           
           // Avoid division by zero
           if (totalCredits == 0)
               return 0.0m;
           
           // Return GPA rounded to 2 decimal places
           return Math.Round(totalQualityPoints / totalCredits, 2);
       }
       
       private static decimal GetGradePoints(Grade grade) => grade switch
       {
           Grade.A => 4.0m,
           Grade.B => 3.0m,
           Grade.C => 2.0m,
           Grade.D => 1.0m,
           Grade.F => 0.0m,
           _ => throw new ArgumentException($"Invalid grade value: {grade}")
       };
   }
   ```

3. **Unit Tests: `ContosoUniversity.Tests/Services/GpaCalculationServiceTests.cs`**
   ```csharp
   namespace ContosoUniversity.Tests.Services;
   
   public class GpaCalculationServiceTests
   {
       private readonly GpaCalculationService _service;
       
       public GpaCalculationServiceTests()
       {
           _service = new GpaCalculationService();
       }
       
       [Fact]
       public void CalculateGpa_WithEmptyEnrollments_ReturnsZero()
       {
           // Arrange
           var enrollments = new List<Enrollment>();
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(0.0m, result);
       }
       
       // Add 8+ more test methods here...
   }
   ```

### Files to Modify

1. **Repository Interface: `ContosoUniversity.Core/Interfaces/IRepository.cs`**
   
   **Add new method** (preserve existing methods for backward compatibility):
   ```csharp
   /// <summary>
   /// Gets an entity by ID with related entities eagerly loaded.
   /// </summary>
   Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
   ```

2. **Repository Implementation: `ContosoUniversity.Infrastructure/Data/Repository.cs`**
   
   **Add new method implementation**:
   ```csharp
   public async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
   {
       IQueryable<T> query = _dbSet;
       
       // Apply all includes
       foreach (var include in includes)
       {
           query = query.Include(include);
       }
       
       return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "ID") == id);
   }
   ```
   
   **Note**: This resolves the critical issue documented at `StudentsController.cs` line 77-78.

3. **StudentsController: `ContosoUniversity.Web/Controllers/StudentsController.cs`**
   
   **Update Details action** to test eager loading (minimal change):
   ```csharp
   public async Task<IActionResult> Details(int? id)
   {
       if (id == null)
           return BadRequest();
       
       // Use new eager loading method
       var student = await _studentRepository.GetByIdWithIncludesAsync(
           id.Value, 
           s => s.Enrollments.Select(e => e.Course)
       );
       
       if (student == null)
           return NotFound();
       
       return View(student);
   }
   ```

4. **Dependency Injection: `ContosoUniversity.Web/Program.cs`**
   
   **Add service registration** (find the services section):
   ```csharp
   // Register GPA calculation service
   builder.Services.AddScoped<IGpaCalculationService, GpaCalculationService>();
   ```

### Technical Considerations

1. **Repository Pattern Fix**
   - The current `GetByIdAsync` uses `FindAsync` which doesn't support `.Include()`
   - Adding a new method preserves backward compatibility
   - Uses expression trees to allow flexible includes at call site
   - Alternative: Create specific `IStudentRepository` with `GetStudentWithEnrollments()` - choose based on project patterns

2. **GPA Calculation Performance**
   - Current implementation is O(n) where n = number of enrollments
   - Acceptable for typical student enrollment counts (< 50 courses)
   - Consider caching if performance becomes an issue (measure first)

3. **Decimal Precision**
   - Using `decimal` type for GPA to avoid floating-point rounding errors
   - `Math.Round(value, 2)` ensures consistent 2-decimal display
   - Business rule: "3.00" is correct, not "3" or "3.0"

4. **NULL Grade Handling (Option A - Decision Confirmed)**
   - In-progress courses with NULL grades are **excluded** from GPA
   - Only completed courses with assigned grades (A-F) count toward GPA
   - Formula uses: `where e.Grade.HasValue`
   - UI will clarify this with "based on X completed credits" (Story GPA-02)

5. **Error Handling**
   - Service returns 0.0 for edge cases rather than throwing exceptions
   - Invalid grade enum values throw `ArgumentException` (defensive programming)
   - Null checks prevent `NullReferenceException`

### Testing Strategy

**Unit Tests** (Story GPA-01 - This Story):
- GpaCalculationService logic (all edge cases)
- Grade point mapping validation
- Rounding precision

**Integration Tests** (Story GPA-03):
- Repository eager loading with real DbContext
- Full controller action with mocked data
- View rendering with GPA display

### Code Quality Standards

- Follow existing project patterns in `ContosoUniversity.Infrastructure/Services/`
- Use constructor injection for dependencies
- Add XML documentation comments on public methods
- Use meaningful variable names (e.g., `totalQualityPoints`, not `sum1`)
- Follow C# naming conventions (PascalCase for public, camelCase for private)
- Ensure all unit tests pass before committing
- Run `dotnet format` for code formatting consistency

## Technical Context

### Current Repository Limitation (Critical Issue)

**Location**: `ContosoUniversity.Web/Controllers/StudentsController.cs` (lines 77-78)

**Problem**:
```csharp
// For now, we'll use GetByIdAsync for the student and handle enrollments separately
// In a real application, we would modify the repository to support eager loading
var student = await _studentRepository.GetByIdAsync(id.Value);
```

The `GetByIdAsync` method uses `DbSet.FindAsync()` which:
- ❌ Does NOT support `.Include()` for related entities
- ❌ Causes N+1 query problem (separate query per enrollment)
- ❌ May not load enrollments at all (depends on lazy loading configuration)

**Impact on GPA**:
- Cannot calculate GPA without Enrollment.Course.Credits
- Must load: Student → Enrollments → Course (2 levels deep)

**Solution in This Story**:
Add `GetByIdWithIncludesAsync` method that uses `.Include().ThenInclude()` pattern.

### Grade Point Average (GPA) Formula

**Weighted GPA Formula**:
```
GPA = Σ(GradePoints × Credits) / Σ(Credits)
```

**Where**:
- `GradePoints` = Numeric value of letter grade (A=4.0, B=3.0, etc.)
- `Credits` = Course credit hours (from Course.Credits property)
- `Σ` = Sum across all enrollments where Grade is NOT NULL

**Example Calculation** (Carson Alexander - from seed data):
```
Chemistry:      A (4.0) × 3 credits = 12.0 quality points
Microeconomics: C (2.0) × 3 credits = 6.0 quality points
Macroeconomics: B (3.0) × 3 credits = 9.0 quality points
────────────────────────────────────────────────────────
Total:          27.0 quality points / 9 credits = 3.00 GPA
```

### Entity Relationships

```
Student
  └─ Enrollments (ICollection<Enrollment>)
       ├─ Grade (nullable Grade enum: A, B, C, D, F, null)
       └─ Course
            └─ Credits (int: 0-5)
```

**Data Loading Requirement**:
To calculate GPA, we need:
1. Student entity
2. All Enrollments for that student
3. Course entity for each Enrollment (to get Credits)

**This requires eager loading**: `.Include(s => s.Enrollments).ThenInclude(e => e.Course)`

## Agent Prompt

```
You are implementing Story GPA-01: GPA Calculation Foundation for the Contoso University application.

**Context:**
- Branch: copilot/add-gpa-calculation-display
- Analysis: docs/analysis/gpa-feature-analysis.md (comprehensive 756-line document)
- Project: ASP.NET Core 8 MVC application with 3-layer architecture (Core/Infrastructure/Web)
- Critical Issue: Repository pattern does NOT support eager loading (lines 77-78 of StudentsController.cs)

**Your Task:**
Implement the foundational layer for GPA calculation by:

1. **Fix Repository Pattern** (CRITICAL - blocking issue)
   - Add `GetByIdWithIncludesAsync` method to IRepository<T> interface
   - Implement in Repository<T> class with .Include() support
   - Preserve backward compatibility (don't modify existing methods)
   - Test by updating StudentsController.Details to use new method

2. **Create GPA Calculation Service**
   - Create IGpaCalculationService interface in Core/Interfaces/
   - Implement GpaCalculationService in Infrastructure/Services/
   - Use weighted GPA formula: Σ(GradePoints × Credits) / Σ(Credits)
   - Grade mapping: A=4.0, B=3.0, C=2.0, D=1.0, F=0.0
   - Exclude NULL grades (Option A - confirmed decision)
   - Round to 2 decimal places

3. **Handle Edge Cases**
   - Empty enrollments → return 0.0
   - All NULL grades → return 0.0
   - Mixed NULL/graded → calculate from graded only
   - Zero credits → return 0.0 (avoid division by zero)

4. **Write Comprehensive Unit Tests**
   - Create GpaCalculationServiceTests.cs
   - Test all 9 scenarios in AC5
   - Achieve ≥90% code coverage for service
   - Use AAA pattern (Arrange, Act, Assert)

5. **Register Service in DI**
   - Add service registration in Program.cs
   - Use Scoped lifetime
   - Follow existing service registration patterns

**Acceptance Criteria:**
[Copy all 6 ACs from the story above]

**Key Technical Details:**
- Repository fix location: ContosoUniversity.Infrastructure/Data/Repository.cs
- Service location: ContosoUniversity.Infrastructure/Services/GpaCalculationService.cs
- Interface location: ContosoUniversity.Core/Interfaces/IGpaCalculationService.cs
- Tests location: ContosoUniversity.Tests/Services/GpaCalculationServiceTests.cs
- DI registration: ContosoUniversity.Web/Program.cs

**NULL Grade Handling Rule (Option A):**
In-progress courses with NULL grades are EXCLUDED from GPA calculation. Only completed courses with grades (A, B, C, D, F) count toward GPA.

**Formula Verification:**
Test with Carson Alexander data:
- Chemistry: A (4.0) × 3 credits = 12.0
- Microeconomics: C (2.0) × 3 credits = 6.0
- Macroeconomics: B (3.0) × 3 credits = 9.0
- Expected GPA: 27.0 / 9 = 3.00

**Quality Standards:**
- Run dotnet format for code formatting
- Add XML documentation comments on public methods
- Follow existing project naming conventions
- Ensure all tests pass before committing
- Use meaningful variable names

**Commit Strategy:**
```bash
git commit -m "fix: Add Include support to Repository for eager loading"
git commit -m "feat: Add IGpaCalculationService interface"
git commit -m "feat: Implement GpaCalculationService with weighted GPA formula"
git commit -m "test: Add comprehensive GpaCalculationService unit tests"
git commit -m "feat: Register GpaCalculationService in DI container"
```

**Definition of Done:**
- [ ] All 6 acceptance criteria verified and passing
- [ ] All unit tests passing (9+ test cases)
- [ ] Code coverage ≥90% for GpaCalculationService
- [ ] No compiler warnings
- [ ] Code follows project standards
- [ ] Changes committed with clear messages

**Start by:**
1. Reading the critical issue documentation at StudentsController.cs lines 77-78
2. Examining existing Repository<T> implementation pattern
3. Reviewing Grade enum definition in Core/Models/Enrollment.cs
4. Understanding Course.Credits property in Core/Models/Course.cs
```

## Estimated Effort

- **Complexity**: High (architectural fix + new service layer)
- **Time Estimate**: 4-6 hours
- **Risk Level**: 🔴 High (repository fix impacts core architecture)

**Breakdown**:
- Repository pattern fix: 1-2 hours (research + implement + test)
- GPA service interface & implementation: 1-2 hours
- Comprehensive unit tests: 1-2 hours
- DI registration and integration: 0.5 hours

**Risk Factors**:
- Repository pattern change could affect existing controllers
- Need careful testing to avoid breaking existing functionality
- Expression tree syntax can be tricky for includes

**Mitigation**:
- Add new method rather than modifying existing (backward compatible)
- Run full test suite after repository changes
- Use existing test patterns as templates

## Related Documentation

- **Analysis Document**: `docs/analysis/gpa-feature-analysis.md` (Section 2.2, 3.2, 5.1)
- **Handoff Document**: `docs/handoffs/handoff-gpa-analysis-001.md` (Section "Blockers")
- **Coding Standards**: `docs/standards/coding-standards-rubric.md`
- **Existing Tests Pattern**: `ContosoUniversity.Tests/` (review for test conventions)

## Session Notes

_To be filled in by the implementing agent during/after implementation._

### Implementation Summary
[Date] - [Agent] - [Brief summary of changes]

### Decisions Made
- [Decision 1 and rationale]
- [Decision 2 and rationale]

### Issues Encountered
- [Issue 1 and resolution]

### Testing Results
- Unit tests: [X passing / Y total]
- Code coverage: [X%]

---

**Story Status**: ⚪ Not Started  
**Last Updated**: 2025-02-02  
**Next Story**: GPA-02 (Display in Views)
