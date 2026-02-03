using ContosoUniversity.Core.Interfaces;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Infrastructure.Services
{
    /// <summary>
    /// Service for calculating student Grade Point Average (GPA).
    /// Implements weighted GPA formula: Σ(GradePoints × Credits) / Σ(Credits)
    /// </summary>
    public class GpaCalculationService : IGpaCalculationService
    {
        /// <summary>
        /// Calculates weighted GPA from enrollment collection.
        /// NULL grades are excluded from calculation (in-progress courses).
        /// </summary>
        /// <param name="enrollments">Collection of enrollments with grades and courses</param>
        /// <returns>GPA rounded to 2 decimals, or 0.0 if no graded courses</returns>
        public decimal CalculateGpa(IEnumerable<Enrollment> enrollments)
        {
            // Handle null or empty collections
            if (enrollments == null || !enrollments.Any())
                return 0.0m;
            
            // Filter to only graded enrollments (exclude NULL grades - in-progress courses)
            var gradedEnrollments = enrollments
                .Where(e => e.Grade.HasValue && e.Course != null)
                .ToList();
            
            // If no graded enrollments, return 0.0
            if (!gradedEnrollments.Any())
                return 0.0m;
            
            // Calculate total quality points: Σ(GradePoints × Credits)
            var totalQualityPoints = gradedEnrollments
                .Sum(e => GetGradePoints(e.Grade!.Value) * e.Course.Credits);
            
            // Calculate total credits: Σ(Credits)
            var totalCredits = gradedEnrollments
                .Sum(e => e.Course.Credits);
            
            // Avoid division by zero (all courses have 0 credits)
            if (totalCredits == 0)
                return 0.0m;
            
            // Return GPA rounded to 2 decimal places
            return Math.Round(totalQualityPoints / totalCredits, 2);
        }
        
        /// <summary>
        /// Maps letter grade to grade point value.
        /// </summary>
        /// <param name="grade">Letter grade (A, B, C, D, F)</param>
        /// <returns>Grade point value (4.0, 3.0, 2.0, 1.0, 0.0)</returns>
        private static decimal GetGradePoints(Grade grade) => grade switch
        {
            Grade.A => 4.0m,
            Grade.B => 3.0m,
            Grade.C => 2.0m,
            Grade.D => 1.0m,
            Grade.F => 0.0m,
            _ => throw new ArgumentException($"Invalid grade value: {grade}", nameof(grade))
        };
    }
}
