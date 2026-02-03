using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Interfaces
{
    /// <summary>
    /// Service for calculating student Grade Point Average (GPA).
    /// Uses weighted formula: Σ(GradePoints × Credits) / Σ(Credits)
    /// Grade point mapping: A=4.0, B=3.0, C=2.0, D=1.0, F=0.0
    /// </summary>
    public interface IGpaCalculationService
    {
        /// <summary>
        /// Calculates weighted GPA from enrollment collection.
        /// Excludes enrollments with NULL grades (in-progress courses).
        /// Only completed courses with assigned grades (A, B, C, D, F) count toward GPA.
        /// </summary>
        /// <param name="enrollments">Collection of enrollments with grades and courses</param>
        /// <returns>GPA rounded to 2 decimals, or 0.0 if no graded courses</returns>
        decimal CalculateGpa(IEnumerable<Enrollment> enrollments);
    }
}
