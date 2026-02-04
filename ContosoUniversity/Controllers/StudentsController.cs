using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System.Diagnostics;
using ContosoUniversity.Infrastructure.Services;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : BaseController
    {
        private readonly GpaCalculationService _gpaCalculationService;

        public StudentsController()
        {
            // Initialize GPA calculation service
            // Note: In legacy MVC without full DI, we instantiate directly
            _gpaCalculationService = new GpaCalculationService();
        }
        // GET: Students - Admins and Teachers can view
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Eager load enrollments and courses for GPA calculation
            var students = db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsQueryable();
            
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var paginatedStudents = PaginatedList<Student>.Create(students, pageNumber, pageSize);

            // Calculate GPA for each student on current page
            var studentGpas = new System.Collections.Generic.Dictionary<int, dynamic>();
            foreach (var student in paginatedStudents)
            {
                var gpa = _gpaCalculationService.CalculateGpa(
                    student.Enrollments.Select(e => new ContosoUniversity.Core.Models.Enrollment
                    {
                        Grade = e.Grade.HasValue ? (ContosoUniversity.Core.Models.Grade?)e.Grade.Value : null,
                        Course = new ContosoUniversity.Core.Models.Course
                        {
                            Credits = e.Course.Credits
                        }
                    })
                );
                
                var completedCredits = student.Enrollments
                    .Where(e => e.Grade.HasValue)
                    .Sum(e => e.Course?.Credits ?? 0);

                studentGpas[student.ID] = new { Gpa = gpa, Credits = completedCredits };
            }

            ViewBag.StudentGpas = studentGpas;

            return View(paginatedStudents);
        }

        // GET: Students/Details/5 - Admins and Teachers can view details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            // Eager load enrollments and courses for GPA calculation
            Student student = db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Where(s => s.ID == id).SingleOrDefault();
            
            if (student == null)
            {
                return HttpNotFound();
            }

            // Calculate GPA and completed credits
            var gpa = _gpaCalculationService.CalculateGpa(
                student.Enrollments.Select(e => new ContosoUniversity.Core.Models.Enrollment
                {
                    Grade = e.Grade.HasValue ? (ContosoUniversity.Core.Models.Grade?)e.Grade.Value : null,
                    Course = new ContosoUniversity.Core.Models.Course
                    {
                        Credits = e.Course.Credits
                    }
                })
            );
            
            var completedCredits = student.Enrollments
                .Where(e => e.Grade.HasValue)
                .Sum(e => e.Course?.Credits ?? 0);

            // Pass GPA data to view
            ViewBag.Gpa = gpa;
            ViewBag.CompletedCredits = completedCredits;

            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var student = new Student
            {
                EnrollmentDate = DateTime.Today // Set default to today's date
            };
            return View(student);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                // Validate EnrollmentDate is not default/minimum value
                if (student.EnrollmentDate == DateTime.MinValue || student.EnrollmentDate == default(DateTime))
                {
                    ModelState.AddModelError("EnrollmentDate", "Please enter a valid enrollment date.");
                }

                // Ensure EnrollmentDate is within valid SQL Server datetime range
                if (student.EnrollmentDate < new DateTime(1753, 1, 1) || student.EnrollmentDate > new DateTime(9999, 12, 31))
                {
                    ModelState.AddModelError("EnrollmentDate", "Enrollment date must be between 1753 and 9999.");
                }

                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    
                    // Send notification for student creation
                    var studentName = $"{student.FirstMidName} {student.LastName}";
                    SendEntityNotification("Student", student.ID.ToString(), studentName, EntityOperation.CREATE);
                    
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error creating student: {ex.Message} | Student: {student?.FirstMidName} {student?.LastName} | EnrollmentDate: {student?.EnrollmentDate} | Stack: {ex.StackTrace}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                // Validate EnrollmentDate is not default/minimum value
                if (student.EnrollmentDate == DateTime.MinValue || student.EnrollmentDate == default(DateTime))
                {
                    ModelState.AddModelError("EnrollmentDate", "Please enter a valid enrollment date.");
                }

                // Ensure EnrollmentDate is within valid SQL Server datetime range
                if (student.EnrollmentDate < new DateTime(1753, 1, 1) || student.EnrollmentDate > new DateTime(9999, 12, 31))
                {
                    ModelState.AddModelError("EnrollmentDate", "Enrollment date must be between 1753 and 9999.");
                }

                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    // Send notification for student update
                    var studentName = $"{student.FirstMidName} {student.LastName}";
                    SendEntityNotification("Student", student.ID.ToString(), studentName, EntityOperation.UPDATE);
                    
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error editing student: {ex.Message} | Student ID: {student?.ID} | Student: {student?.FirstMidName} {student?.LastName} | EnrollmentDate: {student?.EnrollmentDate} | Stack: {ex.StackTrace}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                var studentName = $"{student.FirstMidName} {student.LastName}";
                db.Students.Remove(student);
                db.SaveChanges();
                
                // Send notification for student deletion
                SendEntityNotification("Student", id.ToString(), studentName, EntityOperation.DELETE);
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error deleting student: {ex.Message} | Student ID: {id} | Stack: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Unable to delete the student. Try again, and if the problem persists see your system administrator.";
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Base class will dispose db and notificationService
            }
            base.Dispose(disposing);
        }
    }
}
