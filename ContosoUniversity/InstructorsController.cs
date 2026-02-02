using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ContosoUniversity.Core.Interfaces;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Web.Models.SchoolViewModels;
using ContosoUniversity.Web.Extensions;

namespace ContosoUniversity.Web.Controllers
{
    public class InstructorsController : BaseController
    {
        private readonly IRepository<Instructor> _instructorRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly new ILogger<InstructorsController> _logger;

        public InstructorsController(
            IRepository<Instructor> instructorRepository,
            IRepository<Course> courseRepository,
            IRepository<Department> departmentRepository,
            INotificationService notificationService,
            ILogger<InstructorsController> logger) : base(notificationService, logger)
        {
            _instructorRepository = instructorRepository;
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(int? id, int? courseID)
        {
            var viewModel = new InstructorIndexData();
            
            var instructors = await _instructorRepository.GetAllAsync();
            viewModel.Instructors = instructors
                .OrderBy(i => i.LastName)
                .ToList();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                var instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                viewModel.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if (courseID != null)
            {
                ViewData["CourseID"] = courseID.Value;
                viewModel.Enrollments = viewModel.Courses
                    .Where(x => x.CourseID == courseID)
                    .Single()
                    .Enrollments;
            }

            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var instructor = new Instructor
            {
                HireDate = DateTime.Now
            };
            
            var viewModel = new InstructorCourseData
            {
                Instructor = instructor,
                AssignedCourses = new List<AssignedCourseData>()
            };
            
            return View(viewModel);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("FirstMidName,LastName,HireDate,OfficeAssignment")] Instructor instructor, string[] selectedCourses)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _instructorRepository.AddAsync(instructor);
                    await _instructorRepository.SaveChangesAsync();
                    
                    // For now, we can't handle the selected courses since we need the original repository structure
                    // with entity tracking
                    
                    // Send notification for instructor creation
                    var instructorName = $"{instructor.FirstMidName} {instructor.LastName}";
                    await SendEntityNotificationAsync("Instructor", instructor.ID.ToString(), instructorName, EntityOperation.Create);
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating instructor: {Name}", $"{instructor.FirstMidName} {instructor.LastName}");
                ModelState.AddModelError("", "Unable to create instructor. Try again, and if the problem persists, see your system administrator.");
            }
            
            var viewModel = new InstructorCourseData
            {
                Instructor = instructor,
                AssignedCourses = new List<AssignedCourseData>()
            };
            
            return View(viewModel);
        }

        // GET: Instructors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructor == null)
            {
                return NotFound();
            }
            
            var viewModel = new InstructorCourseData
            {
                Instructor = instructor,
                AssignedCourses = new List<AssignedCourseData>()
            };
            
            // Load course assignments
            var courses = await _courseRepository.GetAllAsync();
            
            // This part of the code would typically use the context directly, 
            // but we're adapting it to work with our repository pattern
            foreach (var course in courses)
            {
                viewModel.AssignedCourses.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructor.CourseAssignments
                        .Any(ca => ca.CourseID == course.CourseID)
                });
            }
            
            return View(viewModel);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id, [Bind("ID,FirstMidName,LastName,HireDate")] Instructor instructor, string[] selectedCourses)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructorToUpdate = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructorToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate,
                "",
                i => i.FirstMidName, i => i.LastName, i => i.HireDate))
            {
                try
                {
                    // Handle office assignment updates
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }
                    
                    // For now, we can't handle the selected courses since we need the original repository structure
                    // with entity tracking
                    
                    await _instructorRepository.UpdateAsync(instructorToUpdate);
                    await _instructorRepository.SaveChangesAsync();
                    
                    // Send notification for instructor update
                    var instructorName = $"{instructorToUpdate.FirstMidName} {instructorToUpdate.LastName}";
                    await SendEntityNotificationAsync("Instructor", instructorToUpdate.ID.ToString(), instructorName, EntityOperation.Update);
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If we get here, something went wrong with the update
                    ModelState.AddModelError("", "Unable to save changes. The instructor was deleted by another user.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating instructor: {ID} - {Name}", 
                        instructorToUpdate.ID, $"{instructorToUpdate.FirstMidName} {instructorToUpdate.LastName}");
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            
            // If we get here, something failed - reload the form
            var departments = await _departmentRepository.GetAllAsync();
            
            var viewModel = new InstructorCourseData
            {
                Instructor = instructorToUpdate,
                AssignedCourses = new List<AssignedCourseData>()
            };
            
            // Load course assignments
            var courses = await _courseRepository.GetAllAsync();
            
            foreach (var course in courses)
            {
                viewModel.AssignedCourses.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorToUpdate.CourseAssignments
                        .Any(ca => ca.CourseID == course.CourseID)
                });
            }
            
            return View(viewModel);
        }

        // GET: Instructors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var instructor = await _instructorRepository.GetByIdAsync(id.Value);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            var instructorName = $"{instructor.FirstMidName} {instructor.LastName}";
            
            try
            {
                await _instructorRepository.DeleteAsync(instructor);
                await _instructorRepository.SaveChangesAsync();
                
                // Send notification for instructor deletion
                await SendEntityNotificationAsync("Instructor", id.ToString(), instructorName, EntityOperation.Delete);
                
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting instructor: {ID} - {Name}", id, instructorName);
                
                // Add a custom error message
                if (ex.InnerException?.Message.Contains("FK_") ?? false)
                {
                    ModelState.AddModelError("", "Unable to delete instructor. The instructor has assigned courses or departments. You must reassign these before deletion.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to delete instructor. Try again, and if the problem persists see your system administrator.");
                }
            }
            
            return View(instructor);
        }
    }
}
