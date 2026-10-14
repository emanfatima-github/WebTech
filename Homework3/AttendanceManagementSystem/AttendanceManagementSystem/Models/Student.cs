using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementSystem.Models
    {
        public class Student
        {
            public int StudentId { get; set; }

            [Required]
            [StringLength(100)]
            public string StudentName { get; set; } = string.Empty;

            [Required]
            [StringLength(30)]
            public string RegistrationNumber { get; set; } = string.Empty;

            [Required]
            [StringLength(50)]
            public string Department { get; set; } = string.Empty;

            [Range(0, 100)]
            public decimal AttendancePercentage { get; set; }
        }
    }
