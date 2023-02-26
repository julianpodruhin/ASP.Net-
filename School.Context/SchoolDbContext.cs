using Microsoft.EntityFrameworkCore;
using School.Models;

namespace School.Context
{
    // Представление базы данных о студентах.
    // Каждый DbSet - это таблица в базе данных
    public sealed class SchoolDbContext : DbContext
    {
        private readonly string connectionString;

        public DbSet<Person> Person { get; set; }

        public DbSet<StudentsGrade> StudentGrade { get; set; }

        public SchoolDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString); // подклчение к базе данных
        }
    }
}