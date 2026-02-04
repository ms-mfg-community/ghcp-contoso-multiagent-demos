# Story GPA-02: GPA Display in Student Views

## Summary

Add GPA display to Student Details and Index views, showing the calculated weighted GPA with proper formatting and context. Students' academic performance will be visible at-a-glance with the format "GPA: 3.45 (based on 9 completed credits)" that clearly communicates the NULL grade handling rule (Option A - completed courses only).

## Assigned Agent

**Builder** (dotnet-developer specialist)

## User Story

As a **student, instructor, or administrator**, I want to see a student's GPA displayed on both the student details page and the student list page, so that I can quickly assess academic performance and track progress.

## Business Value

- Provides immediate visibility into student academic performance
- Supports academic advising and intervention decisions
- Enables faculty to identify students who may need additional support
- Improves user experience by showing key metrics without additional navigation
- Clarifies that GPA is based on completed courses only (not in-progress courses)

## Acceptance Criteria

### AC1: GPA Display on Student Details View
**Given** a user viewing the Student Details page  
**When** the page loads for a student with graded enrollments  
**Then** the GPA displays in the following format:

**Display Format**: `GPA: X.XX (based on Y completed credits)`

**Examples**:
- Student with grades: `GPA: 3.45 (based on 9 completed credits)`
- Student with no grades: `GPA: N/A (no completed credits)`
- Student with one course: `GPA: 4.00 (based on 3 completed credits)`

**Visual Placement**:
- Display above the Enrollments table
- Use Bootstrap styling consistent with other detail sections
- Use `<dt>/<dd>` definition list format matching existing fields
- Consider color coding: Green for ≥3.5, Yellow for 2.5-3.49, Red for <2.5 (optional enhancement)

**Verification**:
- [ ] GPA displays with exactly 2 decimal places (e.g., "3.00" not "3")
- [ ] Credit count shows total credits from graded enrollments only
- [ ] Students with no enrollments show "GPA: N/A (no completed credits)"
- [ ] Students with only NULL grades show "GPA: N/A (no completed credits)"
- [ ] GPA updates when enrollments data changes
- [ ] Display follows existing view styling and Bootstrap patterns

### AC2: GPA Column in Student Index View
**Given** a user viewing the Students Index page (list of all students)  
**When** the page loads with paginated student data  
**Then** a GPA column displays for each student in the table

**Table Column Requirements**:
- Column header: "GPA"
- Column position: After "Enrollment Date" column
- Cell format: `X.XX` (2 decimals) or "N/A"
- Column should be sortable (optional - may defer to future story)

**Verification**:
- [ ] GPA column appears in the student table
- [ ] GPA displays with 2 decimal places or "N/A"
- [ ] Column header is properly labeled and styled
- [ ] Column aligns properly with other table columns
- [ ] Pagination works correctly (GPA calculated for each page)
- [ ] Table remains responsive on mobile devices

### AC3: Controller Data Loading for Details View
**Given** the StudentsController Details action needs GPA data  
**When** loading a student by ID  
**Then** the controller:
- Uses the new `GetByIdWithIncludesAsync` method from Story GPA-01
- Eagerly loads Student → Enrollments → Course relationships
- Calculates GPA using the `IGpaCalculationService`
- Passes GPA and credit count to the view

**Verification**:
- [ ] Details action uses eager loading (no N+1 queries)
- [ ] GpaCalculationService injected via constructor
- [ ] GPA calculated and passed to view via ViewBag or ViewModel
- [ ] Completed credits count calculated and passed to view
- [ ] Single database query loads all required data (check with logging)

### AC4: Controller Data Loading for Index View
**Given** the StudentsController Index action needs GPA for multiple students  
**When** loading paginated student list  
**Then** the controller:
- Loads students with enrollments and courses eagerly
- Calculates GPA for each student using `IGpaCalculationService`
- Passes GPA data to view via projection or ViewBag

**Performance Consideration**:
- Use projection (Select) to avoid loading unnecessary data
- Calculate GPA in-memory after loading required data
- Consider pagination (already exists) to limit records per request

**Verification**:
- [ ] Index action loads enrollments with courses for displayed students
- [ ] GPA calculated for each student on current page
- [ ] Single query with JOINs (no N+1 problem)
- [ ] Pagination continues to work correctly
- [ ] Performance acceptable with seed data (< 1 second load time)

### AC5: NULL Grade Handling Clarity
**Given** a student has both graded and in-progress (NULL grade) courses  
**When** viewing GPA on either Details or Index page  
**Then** the display clearly indicates GPA is based on completed courses only

**Display Examples**:
- Student with 3 graded courses (9 credits) + 2 in-progress courses:
  - Display: `GPA: 3.45 (based on 9 completed credits)`
  - Does NOT show: "11 credits" (which would include in-progress courses)

**Verification**:
- [ ] Credit count matches only graded enrollments
- [ ] Text clearly states "completed credits" not "total credits"
- [ ] In-progress courses do not affect GPA calculation
- [ ] In-progress courses still display in enrollment table (but marked as "No grade")

### AC6: Styling and User Experience
**Given** users need clear, professional GPA presentation  
**When** viewing GPA on any page  
**Then** the display meets UX standards

**Requirements**:
- Use consistent font size and weight
- Align with existing page styling (Bootstrap 4/5)
- Ensure adequate spacing between GPA and other content
- Use readable color scheme (avoid red/green if accessibility concern)
- Mobile responsive (GPA column may wrap or abbreviate on small screens)

**Verification**:
- [ ] GPA display is visually consistent with existing UI
- [ ] No layout issues on desktop (1920×1080)
- [ ] No layout issues on tablet (768×1024)
- [ ] No layout issues on mobile (375×667)
- [ ] Color contrast meets WCAG AA standards (if color coding used)
- [ ] Font size is readable (not too small)

## Scope

### In Scope
- Add GPA display to Student Details view (above enrollment table)
- Add GPA column to Student Index table
- Update StudentsController.Details to calculate and pass GPA
- Update StudentsController.Index to calculate and pass GPA for each student
- Display format: "GPA: X.XX (based on Y completed credits)"
- Handle edge cases: no enrollments, NULL grades, mixed grades
- Use existing Bootstrap styling for consistency

### Out of Scope (Future Stories or Enhancements)
- GPA sorting in Index view (requires database optimization)
- GPA filtering (e.g., show only students with GPA > 3.0)
- GPA trends over time (historical tracking)
- Color-coded GPA ranges (can be added as enhancement)
- Export GPA data to CSV/Excel
- GPA display on other pages (Home, Department, Course)
- Transcript page with detailed GPA breakdown
- Semester/term GPA (only cumulative GPA in this story)

## Dependencies

### Prerequisites
- ✅ **Story GPA-01** (Foundation) - MUST be complete
  - Repository eager loading implemented
  - GpaCalculationService available
  - Service registered in DI container
- ✅ NULL grade handling decision: Option A (exclude from calculation)
- ✅ Display format decision: "X.XX (based on Y completed credits)"

### Blocks
- **Story GPA-03** (Testing) - Integration tests depend on view implementation

## Implementation Notes

### Files to Modify

1. **StudentsController: `ContosoUniversity.Web/Controllers/StudentsController.cs`**

   **Update Details Action**:
   ```csharp
   private readonly IRepository<Student> _studentRepository;
   private readonly IGpaCalculationService _gpaCalculationService;
   
   public StudentsController(
       IRepository<Student> studentRepository,
       IGpaCalculationService gpaCalculationService)
   {
       _studentRepository = studentRepository;
       _gpaCalculationService = gpaCalculationService;
   }
   
   [Authorize(Roles = "Admin,Teacher")]
   public async Task<IActionResult> Details(int? id)
   {
       if (id == null)
           return BadRequest();
       
       // Use eager loading from Story GPA-01
       var student = await _studentRepository.GetByIdWithIncludesAsync(
           id.Value,
           s => s.Enrollments.Select(e => e.Course)
       );
       
       if (student == null)
           return NotFound();
       
       // Calculate GPA and completed credits
       var gpa = _gpaCalculationService.CalculateGpa(student.Enrollments);
       var completedCredits = student.Enrollments
           .Where(e => e.Grade.HasValue)
           .Sum(e => e.Course.Credits);
       
       // Pass to view
       ViewBag.Gpa = gpa;
       ViewBag.CompletedCredits = completedCredits;
       
       return View(student);
   }
   ```

   **Update Index Action**:
   ```csharp
   public async Task<IActionResult> Index(
       string sortOrder,
       string currentFilter,
       string searchString,
       int? pageNumber)
   {
       // Existing sorting/filtering code...
       
       // Load students with enrollments and courses
       var studentsQuery = _studentRepository.GetQueryable()
           .Include(s => s.Enrollments)
           .ThenInclude(e => e.Course);
       
       // Apply filters...
       
       int pageSize = 10;
       var students = await PaginatedList<Student>.CreateAsync(
           studentsQuery.AsNoTracking(), 
           pageNumber ?? 1, 
           pageSize);
       
       // Calculate GPA for each student on current page
       var studentGpas = students.ToDictionary(
           s => s.ID,
           s => new
           {
               Gpa = _gpaCalculationService.CalculateGpa(s.Enrollments),
               Credits = s.Enrollments
                   .Where(e => e.Grade.HasValue)
                   .Sum(e => e.Course.Credits)
           }
       );
       
       ViewBag.StudentGpas = studentGpas;
       
       return View(students);
   }
   ```

2. **Student Details View: `ContosoUniversity.Web/Views/Students/Details.cshtml`**

   **Add GPA Section** (before enrollments table, around line 30):
   ```cshtml
   <dl class="row">
       <dt class="col-sm-2">
           Name
       </dt>
       <dd class="col-sm-10">
           @Html.DisplayFor(model => model.FullName)
       </dd>
       <dt class="col-sm-2">
           Enrollment Date
       </dt>
       <dd class="col-sm-10">
           @Html.DisplayFor(model => model.EnrollmentDate)
       </dd>
       
       @* NEW: GPA Display *@
       <dt class="col-sm-2">
           GPA
       </dt>
       <dd class="col-sm-10">
           @if (ViewBag.CompletedCredits > 0)
           {
               <strong>@ViewBag.Gpa.ToString("F2")</strong>
               <span class="text-muted">(based on @ViewBag.CompletedCredits completed credits)</span>
           }
           else
           {
               <span class="text-muted">N/A (no completed credits)</span>
           }
       </dd>
   </dl>
   ```

3. **Student Index View: `ContosoUniversity.Web/Views/Students/Index.cshtml`**

   **Add GPA Column** (after Enrollment Date column, around line 45):
   ```cshtml
   <table class="table">
       <thead>
           <tr>
               <th>
                   <a asp-action="Index" asp-route-sortOrder="@ViewData["NameSortParm"]">
                       Name
                   </a>
               </th>
               <th>
                   <a asp-action="Index" asp-route-sortOrder="@ViewData["DateSortParm"]">
                       Enrollment Date
                   </a>
               </th>
               @* NEW: GPA Column *@
               <th>
                   GPA
               </th>
               <th></th>
           </tr>
       </thead>
       <tbody>
           @foreach (var item in Model)
           {
               var studentGpa = ViewBag.StudentGpas[item.ID];
               <tr>
                   <td>
                       @Html.DisplayFor(modelItem => item.FullName)
                   </td>
                   <td>
                       @Html.DisplayFor(modelItem => item.EnrollmentDate)
                   </td>
                   @* NEW: GPA Cell *@
                   <td>
                       @if (studentGpa.Credits > 0)
                       {
                           @studentGpa.Gpa.ToString("F2")
                       }
                       else
                       {
                           <span class="text-muted">N/A</span>
                       }
                   </td>
                   <td>
                       <a asp-action="Edit" asp-route-id="@item.ID">Edit</a> |
                       <a asp-action="Details" asp-route-id="@item.ID">Details</a> |
                       <a asp-action="Delete" asp-route-id="@item.ID">Delete</a>
                   </td>
               </tr>
           }
       </tbody>
   </table>
   ```

### Technical Considerations

1. **Performance for Index View**
   - Loading enrollments for all students could be expensive
   - Current solution: Eager load for current page only (pagination limits to 10-20 students)
   - Future optimization: Cache GPA in Student table (requires migration)
   - Monitor query performance with SQL logging during testing

2. **Decimal Formatting**
   - Use `.ToString("F2")` to ensure 2 decimal places (e.g., "3.00" not "3")
   - Razor syntax: `@ViewBag.Gpa.ToString("F2")`
   - Alternative: Use `[DisplayFormat]` attribute on ViewModel property

3. **ViewBag vs. ViewModel**
   - Current implementation uses ViewBag for simplicity
   - Alternative: Create `StudentDetailsViewModel` with Gpa and CompletedCredits properties
   - ViewModel is more type-safe but requires more code
   - Choose based on project patterns (check existing controllers)

4. **Completed Credits Calculation**
   ```csharp
   var completedCredits = enrollments
       .Where(e => e.Grade.HasValue)  // Only graded courses
       .Sum(e => e.Course.Credits);
   ```
   - Must match the same filtering logic as GPA calculation
   - Ensures "based on X credits" matches actual GPA calculation

5. **Mobile Responsiveness**
   - Bootstrap table classes handle responsive layout
   - Consider adding `table-responsive` wrapper for horizontal scrolling
   - GPA column may need abbreviated header on mobile ("GPA" instead of "Grade Point Average")

6. **Null Safety**
   - Check `ViewBag.CompletedCredits > 0` before displaying GPA
   - Handle null student scenario (should not occur, but defensive)
   - Ensure service returns 0.0m not null for edge cases

### Testing Strategy

**Manual Testing Scenarios**:
1. ✅ Student with grades → GPA displays correctly
2. ✅ Student with no enrollments → "N/A (no completed credits)"
3. ✅ Student with only NULL grades → "N/A (no completed credits)"
4. ✅ Student with mixed NULL/graded → GPA from graded only
5. ✅ Carson Alexander (seed data) → "GPA: 3.00 (based on 9 completed credits)"
6. ✅ Index page → All students show GPA or N/A
7. ✅ Pagination → GPA calculates correctly for each page
8. ✅ Mobile view → Layout is responsive

**Integration Testing** (Story GPA-03):
- Automated tests for controller actions
- View rendering tests with mock data
- Performance tests with large datasets

### Display Format Examples

**Student with Multiple Graded Courses**:
```
GPA: 3.45 (based on 12 completed credits)
```

**Student with One Graded Course**:
```
GPA: 4.00 (based on 3 completed credits)
```

**Student with No Graded Courses** (but has enrollments with NULL grades):
```
GPA: N/A (no completed credits)
```

**Student with No Enrollments**:
```
GPA: N/A (no completed credits)
```

**Student with All Fs**:
```
GPA: 0.00 (based on 9 completed credits)
```

### Code Quality Standards

- Follow existing view conventions (check other .cshtml files)
- Use Tag Helpers consistently (`asp-action`, `asp-controller`)
- Maintain Bootstrap version consistency (check _Layout.cshtml)
- Add XML comments to new controller methods
- Use meaningful ViewBag key names (e.g., `ViewBag.Gpa`, not `ViewBag.Data`)
- Ensure proper indentation in Razor views
- Test on multiple browsers (Chrome, Edge, Firefox)

## Technical Context

### NULL Grade Handling (Option A - Confirmed)

**Business Rule**:
- In-progress courses with NULL grades are **EXCLUDED** from GPA calculation
- Only completed courses with assigned grades (A, B, C, D, F) count toward GPA
- Display must clarify this with "based on X **completed** credits"

**Why This Matters**:
- Students may have 5 enrollments but only 3 with grades
- Total credits = 15 (all enrollments)
- Completed credits = 9 (graded enrollments only)
- Display shows: "GPA: 3.00 (based on 9 completed credits)" ← NOT 15

### Display Format Rationale

**Format**: `GPA: 3.45 (based on 9 completed credits)`

**Components**:
1. **"GPA:"** - Clear label
2. **"3.45"** - Always 2 decimal places (F2 format)
3. **"(based on 9 completed credits)"** - Context for calculation
4. **"completed"** - Key word distinguishing from total enrolled credits

**Alternative Rejected**:
- ❌ "GPA: 3.45" - Missing context about credit count
- ❌ "GPA: 3.45 / 4.00" - Confusing (not a fraction)
- ❌ "GPA: 3.45 (9 credits)" - Ambiguous (completed vs enrolled)
- ✅ **"GPA: 3.45 (based on 9 completed credits)"** - Clear and explicit

### Bootstrap Styling Reference

**Existing Pattern** (from Details.cshtml):
```cshtml
<dl class="row">
    <dt class="col-sm-2">Label</dt>
    <dd class="col-sm-10">Value</dd>
</dl>
```

**For GPA**:
- Use `<strong>` for numeric GPA value (emphasize)
- Use `<span class="text-muted">` for explanatory text
- Use `<span class="text-muted">N/A</span>` for missing data

**Table Styling**:
- Use `table table-striped` classes (if not already present)
- Consider `table-hover` for better UX
- Ensure column headers use `<th>` tags
- Use `text-center` class for GPA column if numeric alignment preferred

## Agent Prompt

```
You are implementing Story GPA-02: GPA Display in Student Views for the Contoso University application.

**Context:**
- Branch: copilot/add-gpa-calculation-display
- Prerequisite: Story GPA-01 (Foundation) MUST be complete
- GpaCalculationService is available and registered in DI
- Repository eager loading is implemented
- Analysis: docs/analysis/gpa-feature-analysis.md

**Your Task:**
Add GPA display to Student Details and Index views with proper controller support.

**Implementation Steps:**

1. **Update StudentsController**
   - Inject IGpaCalculationService via constructor
   - Modify Details action:
     * Use GetByIdWithIncludesAsync to load Student → Enrollments → Course
     * Calculate GPA using service
     * Calculate completed credits (only graded enrollments)
     * Pass GPA and credits to view via ViewBag
   - Modify Index action:
     * Eager load enrollments and courses for current page students
     * Calculate GPA for each student
     * Pass GPA dictionary to view via ViewBag

2. **Update Student Details View**
   - Add GPA display section above enrollments table
   - Format: "GPA: X.XX (based on Y completed credits)"
   - Show "N/A (no completed credits)" when no graded courses
   - Use Bootstrap <dl> definition list format (match existing style)
   - Use <strong> for GPA value, <span class="text-muted"> for context

3. **Update Student Index View**
   - Add "GPA" column after "Enrollment Date"
   - Display GPA with 2 decimal places or "N/A"
   - Ensure table remains responsive
   - Align column properly with existing columns

4. **Handle Edge Cases**
   - No enrollments → "N/A (no completed credits)"
   - All NULL grades → "N/A (no completed credits)"
   - Mixed NULL/graded → Show GPA from graded courses only

**Acceptance Criteria:**
[Copy all 6 ACs from the story above]

**Display Format Examples:**
- With grades: "GPA: 3.45 (based on 9 completed credits)"
- No grades: "GPA: N/A (no completed credits)"
- One course: "GPA: 4.00 (based on 3 completed credits)"

**Key Files:**
- Controller: ContosoUniversity.Web/Controllers/StudentsController.cs
- Details View: ContosoUniversity.Web/Views/Students/Details.cshtml
- Index View: ContosoUniversity.Web/Views/Students/Index.cshtml

**NULL Grade Handling (Option A - Confirmed):**
In-progress courses with NULL grades are EXCLUDED. Only completed courses with grades (A-F) count. Display MUST say "completed credits" not "total credits".

**Testing:**
After implementation, manually test:
1. Student Details page for Carson Alexander → Should show "GPA: 3.00 (based on 9 completed credits)"
2. Students Index page → All students show GPA or N/A
3. Student with no enrollments → Shows N/A
4. Student with only NULL grades → Shows N/A

**Quality Checks:**
- Run dotnet build (no errors)
- Run existing tests (ensure no regressions)
- Test on browser (visual verification)
- Check responsive layout on mobile

**Commit Strategy:**
```bash
git commit -m "feat: Add GPA calculation to StudentsController Details action"
git commit -m "feat: Add GPA calculation to StudentsController Index action"
git commit -m "feat: Add GPA display to Student Details view"
git commit -m "feat: Add GPA column to Students Index view"
```

**Definition of Done:**
- [ ] All 6 acceptance criteria verified
- [ ] GPA displays correctly on Details and Index views
- [ ] Format matches specification: "X.XX (based on Y completed credits)"
- [ ] Edge cases handled (no grades, NULL grades)
- [ ] Views are responsive on mobile
- [ ] No compiler warnings
- [ ] Manual testing completed successfully

**Start by:**
1. Verify Story GPA-01 is complete (GpaCalculationService exists)
2. Review existing StudentsController pattern for DI injection
3. Review existing Details.cshtml for <dl> styling pattern
4. Review existing Index.cshtml for table structure
```

## Estimated Effort

- **Complexity**: Medium (straightforward UI changes, some controller logic)
- **Time Estimate**: 2-3 hours
- **Risk Level**: 🟢 Low (no architectural changes, isolated to views)

**Breakdown**:
- Controller updates (Details + Index): 1 hour
- View updates (Details + Index): 1 hour
- Manual testing and refinement: 0.5-1 hour

**Risk Factors**:
- Performance concern for Index view (loading enrollments for all students)
  - Mitigated by pagination (only 10-20 students per page)
- Responsive layout issues on mobile
  - Mitigated by Bootstrap responsive classes

## Related Documentation

- **Analysis Document**: `docs/analysis/gpa-feature-analysis.md` (Section 2.1, 2.2, 4.4)
- **Story GPA-01**: `docs/stories/story-gpa-01-foundation.md` (prerequisite)
- **Handoff Document**: `docs/handoffs/handoff-gpa-analysis-001.md` (Display format decision)
- **Bootstrap Documentation**: https://getbootstrap.com/docs/4.6/content/tables/

## Session Notes

_To be filled in by the implementing agent during/after implementation._

### Implementation Summary
[Date] - [Agent] - [Brief summary of changes]

### Decisions Made
- [Decision 1: ViewBag vs ViewModel]
- [Decision 2: Color coding for GPA ranges]

### Issues Encountered
- [Issue 1 and resolution]

### Testing Results
- Manual testing scenarios: [X passed / Y total]
- Screenshots: [Attach or reference location]

### Performance Notes
- Index page load time: [X ms]
- Details page load time: [X ms]
- Database queries: [Count and SQL shown in logs]

---

**Story Status**: ⚪ Not Started  
**Last Updated**: 2025-02-02  
**Previous Story**: GPA-01 (Foundation)  
**Next Story**: GPA-03 (Testing & Edge Cases)
