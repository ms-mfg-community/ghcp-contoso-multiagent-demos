using ContosoUniversity.Core.Models;
using ContosoUniversity.Infrastructure.Services;
using Xunit;

namespace ContosoUniversity.Tests.Services
{
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
            IEnumerable<Enrollment>? enrollments = null;

            // Act
            var result = _service.CalculateGpa(enrollments!);

            // Assert
            Assert.Equal(0.0m, result);
        }

        [Fact]
        public void CalculateGpa_WithAllNullGrades_ReturnsZero()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = null,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = null,
                    Course = new Course { Credits = 4 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            Assert.Equal(0.0m, result);
        }

        [Fact]
        public void CalculateGpa_WithMixedNullAndGradedEnrollments_CalculatesFromGradedOnly()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = null, // In-progress, should be excluded
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.B,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 3 + 3.0 * 3) / 6 = 21.0 / 6 = 3.50
            Assert.Equal(3.50m, result);
        }

        [Fact]
        public void CalculateGpa_WithSingleEnrollment_ReturnsGradePointValue()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 3) / 3 = 4.00
            Assert.Equal(4.00m, result);
        }

        [Fact]
        public void CalculateGpa_WithAllZeroCredits_ReturnsZero()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 0 }
                },
                new Enrollment
                {
                    Grade = Grade.B,
                    Course = new Course { Credits = 0 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Avoid division by zero
            Assert.Equal(0.0m, result);
        }

        [Fact]
        public void CalculateGpa_WithStandardMixedGrades_CalculatesCorrectly()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.C,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.B,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 3 + 2.0 * 3 + 3.0 * 3) / 9 = 27.0 / 9 = 3.00
            Assert.Equal(3.00m, result);
        }

        [Fact]
        public void CalculateGpa_WithRoundingScenario_RoundsToTwoDecimalPlaces()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.B,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 3 + 4.0 * 3 + 3.0 * 3) / 9 = 33.0 / 9 = 3.67 (rounded)
            Assert.Equal(3.67m, result);
        }

        [Fact]
        public void CalculateGpa_WithCarsonAlexanderData_ReturnsThreePointZero()
        {
            // Arrange - Sample data from Carson Alexander (seed data)
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A, // Chemistry
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.C, // Microeconomics
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.B, // Macroeconomics
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 3 + 2.0 * 3 + 3.0 * 3) / 9 = 27.0 / 9 = 3.00
            Assert.Equal(3.00m, result);
        }

        [Fact]
        public void CalculateGpa_WithDifferentCreditHours_WeightsCorrectly()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 4 } // 4-credit course
                },
                new Enrollment
                {
                    Grade = Grade.C,
                    Course = new Course { Credits = 2 } // 2-credit course
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 4 + 2.0 * 2) / 6 = (16.0 + 4.0) / 6 = 20.0 / 6 = 3.33
            Assert.Equal(3.33m, result);
        }

        [Fact]
        public void CalculateGpa_WithAllGradeTypes_CalculatesCorrectly()
        {
            // Arrange
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
                    Grade = Grade.C,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.D,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.F,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 + 3.0 + 2.0 + 1.0 + 0.0) * 3 / 15 = 30.0 / 15 = 2.00
            Assert.Equal(2.00m, result);
        }

        [Fact]
        public void CalculateGpa_WithLowGPA_CalculatesCorrectly()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.D,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.F,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (1.0 * 3 + 0.0 * 3) / 6 = 3.0 / 6 = 0.50
            Assert.Equal(0.50m, result);
        }

        [Fact]
        public void CalculateGpa_WithPerfectGPA_ReturnsFourPointZero()
        {
            // Arrange
            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                },
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 4 }
                },
                new Enrollment
                {
                    Grade = Grade.A,
                    Course = new Course { Credits = 3 }
                }
            };

            // Act
            var result = _service.CalculateGpa(enrollments);

            // Assert
            // Expected: (4.0 * 3 + 4.0 * 4 + 4.0 * 3) / 10 = 40.0 / 10 = 4.00
            Assert.Equal(4.00m, result);
        }
    }
}
