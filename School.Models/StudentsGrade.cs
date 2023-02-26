using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class StudentsGrade
    {
        [Key]
        public int EnrollmentId { get; set; }

        public int CourseID { get; set; }

        public int StudentID { get; set; }

        public decimal? Grade { get; set; }
    }
}
