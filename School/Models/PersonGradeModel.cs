using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class PersonGradeModel
    {
        [Required(ErrorMessage = "Enter person's last name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Enter person's first name")]
        public string FirstName { get; set; } = null!;

        public DateTime? HireDate { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public decimal? Grade { get; set; }
    }
}
