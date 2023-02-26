using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class GradeModel
    {
        [Required(ErrorMessage = "Enter course id")]
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Enter student id")]
        public int StudentID { get; set; }

        [Range((typeof(decimal)), "0.00", "10.00")]
        public decimal Grade { get; set; }
    }
}
