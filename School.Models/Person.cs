using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public DateTime? HireDate { get; set; }

        public DateTime? EnrollmentDate { get; set; }
    }
}