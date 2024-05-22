using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class ExamViewModelTests
    {
        private IDialogService _dialogService;
        private DbContextOptions<UniversityContext> _options;

        [TestInitialize()]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "UniversityTestDB")
                .Options;
            SeedTestDB();
            _dialogService = new DialogService();
        }

        private void SeedTestDB()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                context.Database.EnsureDeleted();

                var exams = new[]
                {
                    new Exam { ExamId = 1, CourseCode = "CS101", Date = new DateTime(2024, 5, 15), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(12, 0, 0), Location = "Room 101", Description = "Final Exam", Professor = "Prof. A" },
                    new Exam { ExamId = 2, CourseCode = "MATH201", Date = new DateTime(2024, 5, 16), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(13, 0, 0), Location = "Room 102", Description = "Midterm Exam", Professor = "Prof. B" }
                };

                context.Exams.AddRange(exams);
                context.SaveChanges();
            }
        }

        //[TestMethod]
        //public void Show_all_students_in_exam_viewmodel()
        //{
        //    using UniversityContext context = new UniversityContext(_options);
        //    {
        //        AddExamViewModel addExamViewModel = new AddExamViewModel(context, _dialogService);
        //        bool hasData = addExamViewModel.AvailableStudents.Any();
        //        Assert.IsTrue(hasData);
        //    }
        //}

        [TestMethod]
        public void Add_exam_without_students()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddExamViewModel addExamViewModel = new AddExamViewModel(context, _dialogService)
                {
                    CourseCode = "PHYS101",
                    Date = new DateTime(2024, 5, 17),
                    StartTime = new TimeSpan(11, 0, 0),
                    EndTime = new TimeSpan(14, 0, 0),
                    Location = "Room 103",
                    Description = "Physics Exam",
                    Professor = "Prof. C"
                };

                addExamViewModel.Save.Execute(null);

                bool newExamExists = context.Exams.Any(e => e.CourseCode == "PHYS101" && e.Location == "Room 103");
                Assert.IsTrue(newExamExists);
            }
        }

        //[TestMethod]
        //public void Add_exam_with_students()
        //{
        //    using UniversityContext context = new UniversityContext(_options);
        //    {
        //        AddExamViewModel addExamViewModel = new AddExamViewModel(context, _dialogService);
        //        var student = context.Students.First();

        //        addExamViewModel.CourseCode = "CHEM101";
        //        addExamViewModel.Date = new DateTime(2024, 5, 18);
        //        addExamViewModel.StartTime = new TimeSpan(12, 0, 0);
        //        addExamViewModel.EndTime = new TimeSpan(15, 0, 0);
        //        addExamViewModel.Location = "Room 104";
        //        addExamViewModel.Description = "Chemistry Exam";
        //        addExamViewModel.Professor = "Prof. D";
        //        addExamViewModel.AssignedStudents.Add(student);

        //        addExamViewModel.Save.Execute(null);

        //        bool newExamExists = context.Exams.Any(e => e.CourseCode == "CHEM101" && e.Description == "Chemistry Exam");
        //        Assert.IsTrue(newExamExists);
        //    }
        //}

        [TestMethod]
        public void Add_exam_without_course_code()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddExamViewModel addExamViewModel = new AddExamViewModel(context, _dialogService)
                {
                    Date = new DateTime(2024, 5, 19),
                    StartTime = new TimeSpan(13, 0, 0),
                    EndTime = new TimeSpan(16, 0, 0),
                    Location = "Room 105",
                    Description = "Exam without Course Code",
                    Professor = "Prof. E"
                };

                addExamViewModel.Save.Execute(null);

                bool newExamExists = context.Exams.Any(e => e.Location == "Room 105");
                Assert.IsFalse(newExamExists);
            }
        }

        [TestMethod]
        public void Add_exam_without_location()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddExamViewModel addExamViewModel = new AddExamViewModel(context, _dialogService)
                {
                    CourseCode = "HIST101",
                    Date = new DateTime(2024, 5, 20),
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    Description = "History Exam",
                    Professor = "Prof. F"
                };

                addExamViewModel.Save.Execute(null);

                bool newExamExists = context.Exams.Any(e => e.CourseCode == "HIST101");
                Assert.IsFalse(newExamExists);
            }
        }
    }
}
