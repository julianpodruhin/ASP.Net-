using Microsoft.AspNetCore.Mvc;
using School.Context;
using School.Models;
using System.Diagnostics;

namespace School.Controllers
{
    // Общее замечание - при добавлении оценки в базу данных нужно правильно указывать
    // Id курса и Id студента, в противном случае будет выдана ошибка из базы данных о несуществующих айдишниках
    // Master страница находится в папке Views/Shared. Называется Layout.
    // Также там находятся все 

    public class HomeController : Controller
    {
        // строка подключения к базе данных. Нужно изменить наименование компьютера. В данном случае имя компьютера USER-PC
        private const string Connection = @"Data Source=DESKTOP-BUJ3NLL\SQLEXPRESS;Database=School;TrustServerCertificate=True;Trusted_Connection=True";

        public HomeController()
        {
            
        }

        // Данный метод срабатывает, когда мы нажимаем на странице ссылку 'Grades'
        [HttpGet]
        public IActionResult ShowGradesSum()
        {
            using (var context = new SchoolDbContext(Connection))
            {
                var grades = GetGrades(context);
                return View("ShowGradesSum", grades);
            }
        }

        // Данный метод срабатывает, когда на странице выбирается пункт 'Best students'
        [HttpGet]
        public IActionResult ShowBest()
        {
            using (var context = new SchoolDbContext(Connection))
            {
                var best = GetGrades(context).TakeLast(5); // Берем пять студентов с самого конца коллекции
                return View("ShowGradesSum", best);
            }           
        }

        // Данный метод срабатывает, когда на странице выбирается пункт 'Worst students'
        [HttpGet]
        public IActionResult ShowWorst()
        {
            using (var context = new SchoolDbContext(Connection))
            {
                var worst = GetGrades(context).Take(5); // Берем пять студентов с самого начала коллекции
                return View("ShowGradesSum", worst);
            }           
        }

        // Данный метод вызывается, когда мы нажимает на ссылку 'Add a student'
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View("AddStudent");
        }

        // Данный метод вызывается, когда форма заполнена и нажата кнопка Create
        [HttpPost]
        public IActionResult AddStudent(PersonModel person)
        {
            if (ModelState.IsValid) // Проверка на валидный ввод
            {
                var student = new Person()
                {
                    LastName = person.LastName,
                    FirstName = person.FirstName,
                    EnrollmentDate = person.EnrollmentDate,
                    HireDate = person.HireDate,
                };
                using (var context = new SchoolDbContext(Connection))
                {
                    context.Person.Add(student); // добавление студента в базу данных
                    context.SaveChanges(); // сохранение изменений
                }
            }
            return View("AddStudent"); // Возврат на ту же страницу
        }

        // Данный метод вызывается, когда мы добавляем оценку для студента
        [HttpGet]
        public IActionResult AddGrade()
        {
            return View();
        }

        // Данный метод вызывается, когда форма заполнена и нажата кнопка Create
        [HttpPost]
        public IActionResult AddGrade(GradeModel gradeModel)
        {
            if (ModelState.IsValid)
            {
                var studentGrade = new StudentsGrade()
                {
                    CourseID = gradeModel.CourseID,
                    StudentID = gradeModel.StudentID,
                    Grade = gradeModel.Grade
                };
                using (var context = new SchoolDbContext(Connection))
                {
                    context.StudentGrade.Add(studentGrade);
                    context.SaveChanges();
                }
            }
            return View();
        }

        // Данный метод вызывается на старте
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Запрос на извлечение данных о сумме оценок студентов из базы данных
        // Данный запрос сопоставляет айдишники из базы данных со студентами с
        // айдишниками студентов из базы данных с оценками. Затем он группирует
        // студентов и для каждого считает сумму балов и возвращает коллекцию этих студентов
        private IEnumerable<PersonGradeModel> GetGrades(SchoolDbContext context)
        {
            static PersonGradeModel CreateModel(Person p, IEnumerable<StudentsGrade> g)
            {
                return new PersonGradeModel()
                {
                    LastName = p.LastName,
                    FirstName = p.FirstName,
                    Grade = g.Sum(x => x.Grade),
                    EnrollmentDate = p.EnrollmentDate,
                    HireDate = p.HireDate
                };
            }
            var persons = context.Person.ToList();
            var grades = context.StudentGrade.Where(_ => _.Grade != null).ToList();
            return persons
                .GroupJoin(grades, p => p.PersonId, g => g.StudentID, CreateModel)
                .OrderBy(_ => _.Grade)
                .Where(_ => _.Grade > 0)
                .ToList();
        }
    }
}