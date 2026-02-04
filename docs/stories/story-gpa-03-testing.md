# Story GPA-03: GPA Feature Testing & Edge Case Validation

## Summary

Implement comprehensive automated testing for the GPA feature, including unit tests for edge cases, integration tests for controller and view behavior, and end-to-end tests using Playwright. Validate performance with seed data and ensure all edge cases (no enrollments, NULL grades, mixed scenarios) are handled correctly across the application.

## Assigned Agent

**Builder + Testing** (dotnet-developer + azure-testing specialists)

## User Story

As a **quality engineer and development team**, I want comprehensive automated tests for the GPA feature covering all edge cases and integration scenarios, so that we can confidently deploy the feature and prevent regressions in future releases.

## Business Value

- Ensures accuracy of GPA calculations across all scenarios
- Prevents regressions when modifying grade or enrollment logic
- Validates performance meets acceptable standards (< 1 second page loads)
- Documents expected behavior through test specifications
- Enables confident refactoring and future enhancements
- Catches edge cases before users encounter them

## Acceptance Criteria

### AC1: Comprehensive Unit Tests for GpaCalculationService
**Given** the GpaCalculationService contains critical business logic  
**When** running the complete unit test suite  
**Then** all edge cases and calculation scenarios are validated

**Required Test Cases** (minimum 12 tests):
1. ✅ Standard calculation with multiple mixed grades (A, B, C)
2. ✅ All same grade (e.g., all As → 4.00)
3. ✅ Empty enrollments collection → 0.0
4. ✅ Null enrollments collection → 0.0
5. ✅ All NULL grades (in-progress courses only) → 0.0
6. ✅ Mixed NULL and graded enrollments → calculates from graded only
7. ✅ Single enrollment with A grade → 4.00
8. ✅ Single enrollment with F grade → 0.00
9. ✅ Rounding precision (e.g., 10.0/3 = 3.33, not 3.333...)
10. ✅ Zero-credit courses excluded from calculation
11. ✅ All zero-credit courses → 0.0 (avoid division by zero)
12. ✅ Real seed data: Carson Alexander → 3.00 GPA (9 credits)

**Verification**:
- [ ] All 12+ test cases implemented in `GpaCalculationServiceTests.cs`
- [ ] All tests pass consistently (no flaky tests)
- [ ] Test names clearly describe scenario (e.g., `CalculateGpa_WithAllNullGrades_ReturnsZero`)
- [ ] Tests use AAA pattern (Arrange, Act, Assert)
- [ ] Code coverage ≥ 95% for GpaCalculationService class
- [ ] Tests use meaningful test data (not just random numbers)

### AC2: Integration Tests for StudentsController
**Given** the StudentsController uses GpaCalculationService and repository  
**When** running integration tests with mocked dependencies  
**Then** controller actions behave correctly for all GPA scenarios

**Required Test Scenarios**:
1. ✅ Details action loads student with enrollments (eager loading works)
2. ✅ Details action calculates GPA and passes to view (ViewBag populated)
3. ✅ Details action handles student with no enrollments
4. ✅ Details action handles student with all NULL grades
5. ✅ Index action loads multiple students with GPA calculations
6. ✅ Index action handles pagination correctly (GPA for current page only)
7. ✅ Index action performance acceptable with 50+ students

**Verification**:
- [ ] Test file created: `ContosoUniversity.Tests/Controllers/StudentsControllerTests.cs`
- [ ] All 7+ controller test scenarios implemented
- [ ] Tests use mocked repository and service
- [ ] Tests verify ViewBag contains expected GPA and credits
- [ ] Tests verify correct HTTP status codes (200, 404, 400)
- [ ] Tests validate eager loading prevents N+1 queries

### AC3: View Rendering Integration Tests
**Given** the Student views display GPA information  
**When** rendering views with test data  
**Then** the correct GPA format and values appear in HTML output

**Required Scenarios**:
1. ✅ Details view shows "GPA: 3.45 (based on 9 completed credits)"
2. ✅ Details view shows "N/A (no completed credits)" for no grades
3. ✅ Details view formats GPA with exactly 2 decimals (3.00 not 3)
4. ✅ Index view displays GPA column for all students
5. ✅ Index view shows "N/A" for students without grades
6. ✅ Index view table structure includes GPA column header

**Verification**:
- [ ] View tests created in test project
- [ ] Tests parse rendered HTML to verify GPA display
- [ ] Tests check for correct CSS classes and Bootstrap styling
- [ ] Tests validate "completed credits" wording (not "total credits")
- [ ] Tests verify N/A display for edge cases
- [ ] Tests check decimal formatting (.ToString("F2"))

### AC4: End-to-End Tests with Playwright
**Given** users interact with the application through a browser  
**When** running E2E tests for GPA feature  
**Then** the complete user workflow functions correctly

**Required E2E Scenarios**:
1. ✅ Navigate to Students Index → GPA column visible
2. ✅ Click student Details → GPA displays correctly
3. ✅ Student with grades shows numeric GPA
4. ✅ Student without grades shows "N/A"
5. ✅ Page layout responsive on desktop (1920×1080)
6. ✅ Page layout responsive on mobile (375×667)

**Verification**:
- [ ] Playwright tests created in `ContosoUniversity.PlaywrightTests/` project
- [ ] Tests use seed data for predictable results
- [ ] Tests verify visual elements (not just HTML source)
- [ ] Tests include screenshots for failures
- [ ] Tests run against local development server
- [ ] All E2E tests pass in CI/CD pipeline (if applicable)

### AC5: Edge Case Validation with Test Data
**Given** specific edge case scenarios for GPA calculation  
**When** testing with seed data and synthetic test data  
**Then** all edge cases produce correct results

**Test Data Scenarios**:

| Scenario | Test Data | Expected GPA | Expected Credits |
|----------|-----------|--------------|------------------|
| All As | 3 courses, all A, 3 credits each | 4.00 | 9 |
| All Fs | 3 courses, all F, 3 credits each | 0.00 | 9 |
| Mixed grades | A, B, C (3 credits each) | 3.00 | 9 |
| Variable credits | A (4 cr), B (3 cr), C (3 cr) | 3.10 | 10 |
| One course | A (3 credits) | 4.00 | 3 |
| In-progress only | 3 courses, all NULL grades | N/A | 0 |
| Mixed NULL/graded | 2 graded (A, B), 1 NULL | 3.50 | 6 |
| No enrollments | Empty collection | N/A | 0 |
| Zero credits | 3 courses, all 0 credits | 0.00 | 0 |

**Verification**:
- [ ] Test data builder class created for generating test scenarios
- [ ] Each scenario has corresponding test method
- [ ] Tests validate both GPA and completed credit count
- [ ] Tests verify display format matches specification
- [ ] Tests document expected behavior in comments

### AC6: Performance and Load Validation
**Given** the application must perform acceptably at scale  
**When** testing with seed data and larger datasets  
**Then** GPA feature meets performance requirements

**Performance Requirements**:
- Students Index page: < 1 second load time (10 students per page)
- Student Details page: < 500ms load time
- GPA calculation: < 10ms per student (50 enrollments max)
- Database queries: Single query with JOINs (no N+1 problem)

**Test Scenarios**:
1. ✅ Load Index page with 10 students → measure response time
2. ✅ Load Details page for student with 10 enrollments → measure time
3. ✅ Calculate GPA for student with 50 enrollments → measure time
4. ✅ Verify single database query (check SQL logs)
5. ✅ Pagination works correctly with 100+ students in database

**Verification**:
- [ ] Performance tests implemented with timing assertions
- [ ] Tests measure actual execution time (use Stopwatch or similar)
- [ ] Tests verify database query count (use logging or profiler)
- [ ] Tests validate pagination limits data loading
- [ ] Performance results documented in test output
- [ ] Tests fail if performance degrades beyond thresholds

## Scope

### In Scope
- Unit tests for GpaCalculationService (all edge cases)
- Integration tests for StudentsController (Details and Index actions)
- View rendering tests for GPA display
- End-to-end Playwright tests for user workflows
- Edge case validation with comprehensive test data
- Performance testing with seed data
- Test data builders for creating test scenarios
- Documentation of test coverage and results

### Out of Scope (Future Testing Enhancements)
- Load testing with thousands of concurrent users
- Security testing (SQL injection, XSS) - covered by other stories
- Accessibility testing (WCAG compliance) - separate initiative
- Cross-browser testing beyond Chrome/Edge
- Stress testing with millions of enrollment records
- Mutation testing for test suite quality
- Property-based testing (fuzzing)

## Dependencies

### Prerequisites
- ✅ **Story GPA-01** (Foundation) - Service and repository implemented
- ✅ **Story GPA-02** (Display) - Views and controllers updated
- ✅ Test projects exist: `ContosoUniversity.Tests` and `ContosoUniversity.PlaywrightTests`
- ✅ Seed data includes Carson Alexander with known grades
- ✅ xUnit test framework installed
- ✅ Playwright configured for E2E tests

### Blocks
- None (final story in GPA feature implementation)

## Implementation Notes

### Files to Create

1. **Test Data Builder: `ContosoUniversity.Tests/TestHelpers/GpaTestDataBuilder.cs`**
   ```csharp
   namespace ContosoUniversity.Tests.TestHelpers;
   
   public class GpaTestDataBuilder
   {
       public static List<Enrollment> CreateEnrollmentsWithGrades(
           params (Grade grade, int credits)[] courses)
       {
           var enrollments = new List<Enrollment>();
           int enrollmentId = 1;
           
           foreach (var (grade, credits) in courses)
           {
               enrollments.Add(new Enrollment
               {
                   EnrollmentID = enrollmentId++,
                   Grade = grade,
                   Course = new Course
                   {
                       CourseID = enrollmentId,
                       Credits = credits,
                       Title = $"Test Course {enrollmentId}"
                   }
               });
           }
           
           return enrollments;
       }
       
       public static List<Enrollment> CreateEnrollmentsWithNullGrades(int count)
       {
           var enrollments = new List<Enrollment>();
           for (int i = 0; i < count; i++)
           {
               enrollments.Add(new Enrollment
               {
                   EnrollmentID = i + 1,
                   Grade = null,  // In-progress course
                   Course = new Course
                   {
                       CourseID = i + 1,
                       Credits = 3,
                       Title = $"In Progress Course {i + 1}"
                   }
               });
           }
           return enrollments;
       }
       
       // Add more builder methods as needed...
   }
   ```

2. **Extended Unit Tests: `ContosoUniversity.Tests/Services/GpaCalculationServiceTests.cs`**
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
       
       [Fact]
       public void CalculateGpa_WithNullEnrollments_ReturnsZero()
       {
           // Arrange
           List<Enrollment> enrollments = null;
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(0.0m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithAllNullGrades_ReturnsZero()
       {
           // Arrange
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithNullGrades(3);
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(0.0m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithMixedGrades_ReturnsWeightedAverage()
       {
           // Arrange: A (4.0) × 3 = 12, B (3.0) × 3 = 9, C (2.0) × 3 = 6
           // Total: 27 / 9 = 3.00
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 3),
               (Grade.B, 3),
               (Grade.C, 3)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(3.00m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithVariableCredits_CalculatesWeightedCorrectly()
       {
           // Arrange: A (4.0) × 4 = 16, B (3.0) × 3 = 9, C (2.0) × 3 = 6
           // Total: 31 / 10 = 3.10
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 4),
               (Grade.B, 3),
               (Grade.C, 3)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(3.10m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithRoundingNeeded_RoundsToTwoDecimals()
       {
           // Arrange: Creates scenario requiring rounding
           // A (4.0) × 3 = 12, B (3.0) × 3 = 9, D (1.0) × 3 = 3
           // Total: 24 / 9 = 2.666... → 2.67
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 3),
               (Grade.B, 3),
               (Grade.D, 3)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(2.67m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithMixedNullAndGraded_IgnoresNullGrades()
       {
           // Arrange: 2 graded courses (A, B) + 1 NULL grade
           // Should calculate: (4.0 × 3 + 3.0 × 3) / 6 = 3.50
           var enrollments = new List<Enrollment>
           {
               new Enrollment
               {
                   Grade = Grade.A,
                   Course = new Course { Credits = 3 }
               },
               new Enrollment
               {
                   Grade = Grade.B,
                   Course = new Course { Credits = 3 }
               },
               new Enrollment
               {
                   Grade = null,  // Should be ignored
                   Course = new Course { Credits = 3 }
               }
           };
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(3.50m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithSingleAGrade_ReturnsFourPointZero()
       {
           // Arrange
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 3)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(4.00m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithSingleFGrade_ReturnsZero()
       {
           // Arrange
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.F, 3)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(0.00m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithAllZeroCredits_ReturnsZero()
       {
           // Arrange: Avoid division by zero
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 0),
               (Grade.B, 0)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(0.0m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithCarsonAlexanderData_ReturnsThreePointZero()
       {
           // Arrange: Carson Alexander from seed data
           // Chemistry: A (4.0) × 3 = 12
           // Microeconomics: C (2.0) × 3 = 6
           // Macroeconomics: B (3.0) × 3 = 9
           // Total: 27 / 9 = 3.00
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 3),  // Chemistry
               (Grade.C, 3),  // Microeconomics
               (Grade.B, 3)   // Macroeconomics
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(3.00m, result);
       }
       
       [Fact]
       public void CalculateGpa_WithAllSameGrade_ReturnsGradePointValue()
       {
           // Arrange: All As should return 4.00 regardless of credits
           var enrollments = GpaTestDataBuilder.CreateEnrollmentsWithGrades(
               (Grade.A, 3),
               (Grade.A, 4),
               (Grade.A, 3)
           );
           
           // Act
           var result = _service.CalculateGpa(enrollments);
           
           // Assert
           Assert.Equal(4.00m, result);
       }
   }
   ```

3. **Controller Integration Tests: `ContosoUniversity.Tests/Controllers/StudentsControllerTests.cs`**
   ```csharp
   namespace ContosoUniversity.Tests.Controllers;
   
   public class StudentsControllerTests
   {
       private readonly Mock<IRepository<Student>> _mockRepository;
       private readonly Mock<IGpaCalculationService> _mockGpaService;
       private readonly StudentsController _controller;
       
       public StudentsControllerTests()
       {
           _mockRepository = new Mock<IRepository<Student>>();
           _mockGpaService = new Mock<IGpaCalculationService>();
           _controller = new StudentsController(_mockRepository.Object, _mockGpaService.Object);
       }
       
       [Fact]
       public async Task Details_WithValidId_ReturnsViewWithGpa()
       {
           // Arrange
           var studentId = 1;
           var student = new Student
           {
               ID = studentId,
               FirstMidName = "Test",
               LastName = "Student",
               Enrollments = new List<Enrollment>()
           };
           
           _mockRepository
               .Setup(r => r.GetByIdWithIncludesAsync(studentId, It.IsAny<Expression<Func<Student, object>>[]>()))
               .ReturnsAsync(student);
           
           _mockGpaService
               .Setup(s => s.CalculateGpa(student.Enrollments))
               .Returns(3.45m);
           
           // Act
           var result = await _controller.Details(studentId);
           
           // Assert
           var viewResult = Assert.IsType<ViewResult>(result);
           Assert.Equal(3.45m, _controller.ViewBag.Gpa);
           Assert.Equal(student, viewResult.Model);
       }
       
       [Fact]
       public async Task Details_WithNullId_ReturnsBadRequest()
       {
           // Act
           var result = await _controller.Details(null);
           
           // Assert
           Assert.IsType<BadRequestResult>(result);
       }
       
       [Fact]
       public async Task Details_WithNonexistentId_ReturnsNotFound()
       {
           // Arrange
           _mockRepository
               .Setup(r => r.GetByIdWithIncludesAsync(99, It.IsAny<Expression<Func<Student, object>>[]>()))
               .ReturnsAsync((Student)null);
           
           // Act
           var result = await _controller.Details(99);
           
           // Assert
           Assert.IsType<NotFoundResult>(result);
       }
       
       // Add more controller tests for Index action...
   }
   ```

4. **Playwright E2E Tests: `ContosoUniversity.PlaywrightTests/GpaFeatureTests.cs`**
   ```csharp
   namespace ContosoUniversity.PlaywrightTests;
   
   [Parallelizable(ParallelScope.Self)]
   [TestFixture]
   public class GpaFeatureTests : PageTest
   {
       [Test]
       public async Task StudentIndex_DisplaysGpaColumn()
       {
           // Arrange
           await Page.GotoAsync("https://localhost:7001/Students");
           
           // Act
           var gpaHeader = await Page.Locator("th:has-text('GPA')").First;
           
           // Assert
           await Expect(gpaHeader).ToBeVisibleAsync();
       }
       
       [Test]
       public async Task StudentDetails_WithGrades_DisplaysGpaValue()
       {
           // Arrange: Carson Alexander has GPA 3.00
           await Page.GotoAsync("https://localhost:7001/Students/Details/1");
           
           // Act
           var gpaText = await Page.Locator("dt:has-text('GPA') + dd").TextContentAsync();
           
           // Assert
           Assert.That(gpaText, Does.Contain("3.00"));
           Assert.That(gpaText, Does.Contain("completed credits"));
       }
       
       [Test]
       public async Task StudentDetails_WithoutGrades_DisplaysNA()
       {
           // Arrange: Navigate to student with no grades
           await Page.GotoAsync("https://localhost:7001/Students/Details/5");
           
           // Act
           var gpaText = await Page.Locator("dt:has-text('GPA') + dd").TextContentAsync();
           
           // Assert
           Assert.That(gpaText, Does.Contain("N/A"));
           Assert.That(gpaText, Does.Contain("no completed credits"));
       }
       
       [Test]
       public async Task StudentIndex_ResponsiveOnMobile()
       {
           // Arrange: Set mobile viewport
           await Page.SetViewportSizeAsync(375, 667);
           await Page.GotoAsync("https://localhost:7001/Students");
           
           // Act
           var gpaColumn = await Page.Locator("td:nth-child(3)").First;
           
           // Assert: Column is visible (may wrap but should display)
           await Expect(gpaColumn).ToBeVisibleAsync();
       }
   }
   ```

### Files to Modify

**Existing Test Files** (if they exist):
- `ContosoUniversity.Tests/Services/GpaCalculationServiceTests.cs` - Extend with AC1 tests
- `ContosoUniversity.Tests/Controllers/StudentsControllerTests.cs` - Add AC2 tests
- `ContosoUniversity.PlaywrightTests/` - Add AC4 E2E tests

### Technical Considerations

1. **Test Data Management**
   - Use test data builders for consistent, readable test data
   - Avoid magic numbers - use named constants or descriptive comments
   - Create reusable test data factories for common scenarios
   - Seed database with known data for E2E tests

2. **Mocking Strategy**
   - Mock `IRepository<Student>` and `IGpaCalculationService` in controller tests
   - Use Moq or NSubstitute (check existing project patterns)
   - Verify method calls with `.Verify()` for critical interactions
   - Avoid over-mocking - only mock external dependencies

3. **Performance Testing**
   - Use `System.Diagnostics.Stopwatch` for timing measurements
   - Set reasonable thresholds (e.g., < 1000ms for page load)
   - Run performance tests multiple times for consistency
   - Log detailed timing results for analysis

4. **Playwright Configuration**
   - Ensure test server runs before E2E tests
   - Use `[SetUp]` to start application
   - Use `[TearDown]` to clean up
   - Take screenshots on test failure for debugging

5. **Code Coverage**
   - Use Coverlet or similar tool for .NET code coverage
   - Aim for ≥90% coverage for GpaCalculationService
   - Aim for ≥80% coverage for controllers
   - Generate HTML coverage reports for review

6. **Test Organization**
   - Group related tests in nested classes or folders
   - Use descriptive test names (Given_When_Then pattern)
   - Add `[Trait]` attributes for test categorization
   - Separate unit, integration, and E2E tests

### Testing Strategy

**Test Pyramid Approach**:
```
           /\
          /  \  E2E Tests (6 tests)
         /____\
        /      \  Integration Tests (7 tests)
       /________\
      /          \  Unit Tests (12+ tests)
     /__________\
```

**Test Execution Order**:
1. Unit tests (fast, isolated)
2. Integration tests (medium speed, mocked dependencies)
3. E2E tests (slow, full stack)

**CI/CD Integration** (if applicable):
```bash
# Run all tests in CI pipeline
dotnet test --configuration Release --logger "console;verbosity=detailed"

# Generate code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run Playwright tests
cd ContosoUniversity.PlaywrightTests
dotnet test
```

### Code Quality Standards

- Follow AAA pattern (Arrange, Act, Assert) in all tests
- Use descriptive test names: `MethodName_Scenario_ExpectedResult`
- Add comments for complex test scenarios
- Use test data builders for readability
- Avoid test interdependencies (each test should be independent)
- Clean up test data after tests (if using real database)
- Use `[Theory]` with `[InlineData]` for parameterized tests where appropriate

## Technical Context

### Test Coverage Goals

**GpaCalculationService**:
- Line coverage: ≥95%
- Branch coverage: ≥90%
- All public methods tested

**StudentsController**:
- Line coverage: ≥80%
- All GPA-related actions tested
- Edge cases covered (null, not found, bad request)

**Views**:
- Rendering tests for GPA display
- Format validation (2 decimals, "completed credits" text)

### Performance Benchmarks

**Expected Performance** (based on analysis):
- GPA calculation: < 10ms per student (typical enrollment count)
- Details page load: < 500ms (with eager loading)
- Index page load: < 1 second (10-20 students per page)
- Database query: Single query with JOINs (no N+1)

**Test with Varying Data Sizes**:
- Small: 1-5 enrollments per student
- Medium: 10-20 enrollments per student
- Large: 50+ enrollments per student (edge case)

### Edge Case Matrix

| Enrollments | Grades | Expected GPA | Expected Credits | Display |
|-------------|--------|--------------|------------------|---------|
| 0 | N/A | 0.0 | 0 | N/A (no completed credits) |
| 3 | All NULL | 0.0 | 0 | N/A (no completed credits) |
| 3 | All A | 4.00 | 9 | GPA: 4.00 (based on 9 completed credits) |
| 3 | All F | 0.00 | 9 | GPA: 0.00 (based on 9 completed credits) |
| 3 | Mixed (A,B,C) | 3.00 | 9 | GPA: 3.00 (based on 9 completed credits) |
| 4 | 2 graded, 2 NULL | 3.50 | 6 | GPA: 3.50 (based on 6 completed credits) |
| 1 | A | 4.00 | 3 | GPA: 4.00 (based on 3 completed credits) |
| 3 | 0 credits each | 0.00 | 0 | GPA: 0.00 (based on 0 completed credits) |

### Test Data: Carson Alexander

**From Seed Data** (`DbInitializer.cs`):
- Chemistry: Grade A (4.0) × 3 credits = 12.0 quality points
- Microeconomics: Grade C (2.0) × 3 credits = 6.0 quality points
- Macroeconomics: Grade B (3.0) × 3 credits = 9.0 quality points
- **Expected GPA**: 27.0 / 9 = **3.00**
- **Expected Display**: "GPA: 3.00 (based on 9 completed credits)"

## Agent Prompt

```
You are implementing Story GPA-03: GPA Feature Testing & Edge Case Validation for the Contoso University application.

**Context:**
- Branch: copilot/add-gpa-calculation-display
- Prerequisites: Story GPA-01 (Foundation) and GPA-02 (Display) MUST be complete
- GpaCalculationService is implemented and tested
- Views are updated with GPA display
- Analysis: docs/analysis/gpa-feature-analysis.md

**Your Task:**
Create comprehensive automated tests covering all GPA feature scenarios, edge cases, and integration points.

**Implementation Steps:**

1. **Extend Unit Tests for GpaCalculationService**
   - Add all 12+ test cases from AC1
   - Test edge cases: empty, null, NULL grades, zero credits
   - Test rounding precision
   - Test Carson Alexander scenario (3.00 GPA)
   - Achieve ≥95% code coverage

2. **Create Integration Tests for StudentsController**
   - Test Details action with mocked dependencies
   - Test Index action with multiple students
   - Verify ViewBag.Gpa and ViewBag.CompletedCredits
   - Test error scenarios (null ID, not found)
   - Verify eager loading (no N+1 queries)

3. **Create View Rendering Tests**
   - Test GPA display format in Details view
   - Test "N/A (no completed credits)" display
   - Test GPA column in Index view
   - Verify exact wording "completed credits"
   - Verify 2-decimal formatting

4. **Create Playwright E2E Tests**
   - Test navigation: Index → Details
   - Verify GPA column visible in table
   - Verify GPA display on Details page
   - Test with student with grades (Carson Alexander)
   - Test with student without grades
   - Test responsive layout (desktop and mobile)

5. **Create Test Data Builder**
   - Create GpaTestDataBuilder class
   - Add methods for common scenarios
   - Make tests readable and maintainable

6. **Add Performance Tests**
   - Measure Details page load time
   - Measure Index page load time
   - Measure GPA calculation time
   - Verify single database query (check logs)

**Acceptance Criteria:**
[Copy all 6 ACs from the story above]

**Edge Cases to Test:**
1. Empty enrollments → 0.0
2. Null enrollments → 0.0
3. All NULL grades → 0.0
4. Mixed NULL/graded → calculate from graded only
5. Single enrollment → returns grade point value
6. All same grade → correct average
7. Variable credits → weighted correctly
8. Rounding needed → 2 decimal places
9. Zero credits → 0.0 (no division by zero)
10. Carson Alexander data → 3.00 GPA

**Key Files:**
- Unit tests: ContosoUniversity.Tests/Services/GpaCalculationServiceTests.cs
- Controller tests: ContosoUniversity.Tests/Controllers/StudentsControllerTests.cs
- E2E tests: ContosoUniversity.PlaywrightTests/GpaFeatureTests.cs
- Test builder: ContosoUniversity.Tests/TestHelpers/GpaTestDataBuilder.cs

**Carson Alexander Test Data:**
- Chemistry: A (4.0) × 3 = 12.0
- Microeconomics: C (2.0) × 3 = 6.0
- Macroeconomics: B (3.0) × 3 = 9.0
- Expected: 27.0 / 9 = 3.00 GPA (9 completed credits)

**Testing Tools:**
- Unit tests: xUnit
- Mocking: Moq or NSubstitute
- E2E: Playwright
- Coverage: Coverlet

**Run Tests:**
```bash
# Unit and integration tests
dotnet test ContosoUniversity.Tests

# E2E tests
dotnet test ContosoUniversity.PlaywrightTests

# Code coverage
dotnet test --collect:"XPlat Code Coverage"
```

**Quality Checks:**
- All tests pass (100% pass rate)
- Code coverage ≥90% for GpaCalculationService
- Code coverage ≥80% for controller GPA logic
- No flaky tests (run multiple times to verify)
- Clear test names and documentation

**Commit Strategy:**
```bash
git commit -m "test: Add comprehensive GpaCalculationService unit tests"
git commit -m "test: Add StudentsController integration tests for GPA"
git commit -m "test: Add view rendering tests for GPA display"
git commit -m "test: Add Playwright E2E tests for GPA feature"
git commit -m "test: Add performance validation tests"
git commit -m "docs: Document test coverage and results"
```

**Definition of Done:**
- [ ] All 6 acceptance criteria verified
- [ ] 12+ unit tests passing for GpaCalculationService
- [ ] 7+ integration tests passing for controllers
- [ ] 6+ E2E tests passing for user workflows
- [ ] Code coverage meets targets (≥90% service, ≥80% controller)
- [ ] Performance tests validate load times
- [ ] All tests pass consistently (no flaky tests)
- [ ] Test results documented

**Start by:**
1. Review existing test patterns in ContosoUniversity.Tests
2. Set up test data builder class for reusable test data
3. Write unit tests first (fastest feedback loop)
4. Then integration tests (controller and view)
5. Finally E2E tests (slowest, most comprehensive)
```

## Estimated Effort

- **Complexity**: Medium-High (comprehensive test coverage across multiple layers)
- **Time Estimate**: 3-4 hours
- **Risk Level**: 🟡 Medium (dependent on test infrastructure setup)

**Breakdown**:
- Unit tests: 1 hour (12+ tests)
- Integration tests: 1 hour (controller and view tests)
- E2E tests: 1 hour (Playwright setup and scenarios)
- Performance tests: 0.5 hours
- Documentation and refinement: 0.5 hours

**Risk Factors**:
- Playwright may require environment-specific configuration
- E2E tests may be flaky if not properly isolated
- Code coverage tools may need additional setup

**Mitigation**:
- Start with unit tests (lowest risk)
- Use existing test patterns as templates
- Run tests multiple times to identify flaky tests
- Document any environment-specific setup requirements

## Related Documentation

- **Analysis Document**: `docs/analysis/gpa-feature-analysis.md` (Section 6.3 - Testing Recommendations)
- **Story GPA-01**: `docs/stories/story-gpa-01-foundation.md` (service implementation)
- **Story GPA-02**: `docs/stories/story-gpa-02-display.md` (view implementation)
- **Handoff Document**: `docs/handoffs/handoff-gpa-analysis-001.md` (Carson Alexander test data)
- **xUnit Documentation**: https://xunit.net/
- **Playwright Documentation**: https://playwright.dev/dotnet/

## Session Notes

_To be filled in by the implementing agent during/after implementation._

### Implementation Summary
[Date] - [Agent] - [Brief summary of testing implementation]

### Test Results
- Unit tests: [X passing / Y total]
- Integration tests: [X passing / Y total]
- E2E tests: [X passing / Y total]
- Code coverage: [X%]

### Performance Benchmarks
- Details page load: [X ms]
- Index page load: [X ms]
- GPA calculation (typical): [X ms]
- GPA calculation (50 enrollments): [X ms]

### Issues Encountered
- [Issue 1 and resolution]
- [Flaky tests identified and fixed]

### Code Coverage Report
```
GpaCalculationService: X% line coverage, Y% branch coverage
StudentsController: X% line coverage, Y% branch coverage
```

### Recommendations
- [Any suggestions for future testing enhancements]
- [Performance optimization opportunities identified]

---

**Story Status**: ⚪ Not Started  
**Last Updated**: 2025-02-02  
**Previous Story**: GPA-02 (Display)  
**Next Steps**: Feature complete - ready for PR and merge
