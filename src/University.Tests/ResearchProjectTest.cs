using Castle.DynamicProxy.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class ResearchProjectTests
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

                List<ResearchProject> researchProjects = new List<ResearchProject>
                {
                    new ResearchProject
                    {
                        ResearchProjectId = 1,
                        Title = "Example Research Project 1",
                        Description = "This is an example research project description 1.",
                        Supervisor = "Dr. Smith",
                        StartDate = new DateTime(2024, 5, 1),
                        EndDate = new DateTime(2025, 5, 1),
                        Budget = 10000.0f
                    },
                    new ResearchProject
                    {
                        ResearchProjectId = 2,
                        Title = "Example Research Project 2",
                        Description = "This is an example research project description 2.",
                        Supervisor = "Dr. Johnson",
                        StartDate = new DateTime(2024, 6, 1),
                        EndDate = new DateTime(2025, 6, 1),
                        Budget = 15000.0f
                    },
                    new ResearchProject
                    {
                        ResearchProjectId = 3,
                        Title = "Example Research Project 3",
                        Description = "This is an example research project description 3.",
                        Supervisor = "Dr. Lee",
                        StartDate = new DateTime(2024, 7, 1),
                        EndDate = new DateTime(2025, 7, 1),
                        Budget = 12000.0f
                    }
                };


                List<Student> students = new List<Student>
                {
                    new Student { StudentId = 1, Name = "Wieсczysіaw", LastName = "Nowakowicz", PESEL="PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                    new Student { StudentId = 2, Name = "Stanisіaw", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                    new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) }
                };


                context.Students.AddRange(students);

                context.ResearchProjects.AddRange(researchProjects);
                context.SaveChanges();
            }
        }

        #region AddRegion

        [TestMethod]
        public void Show_all_research_projects()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                ResearchProjectViewModel researchProjectsViewModel = new ResearchProjectViewModel(context, _dialogService);
                bool hasData = researchProjectsViewModel.Projects.Any();
                Assert.IsTrue(hasData);
            }
        }

        [TestMethod]
        public void Add_research_project_without_team_members()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddResearchProjectViewModel addResearchProjectViewModel = new AddResearchProjectViewModel(context, _dialogService)
                {
                    Title = "New Research Project",
                    Description = "Description for new research project",
                    Supervisor = "Dr. New",
                    StartDate = new DateTime(2024, 8, 1),
                    EndDate = new DateTime(2025, 8, 1),
                    Budget = 20000.0f
                };
                addResearchProjectViewModel.Save.Execute(null);

                bool newResearchProjectExists = context.ResearchProjects.Any(rp => rp.Title == "New Research Project" && rp.Supervisor == "Dr. New");
                Assert.IsTrue(newResearchProjectExists);
            }
        }

        [TestMethod]
        public void Add_research_project_with_team_members()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                List<Student> teamMembers = context.Students.Take(2).ToList();

                AddResearchProjectViewModel addResearchProjectViewModel = new AddResearchProjectViewModel(context, _dialogService)
                {
                    Title = "New Research Project with Team",
                    Description = "Description for new research project with team",
                    StartDate = new DateTime(2024, 8, 1),
                    EndDate = new DateTime(2025, 8, 1),
                    Budget = 25000.0f,
                    AssignedStudents = new ObservableCollection<Student>(teamMembers)
                };
                addResearchProjectViewModel.Save.Execute(null);

                bool newResearchProjectExists = context.ResearchProjects.Any(rp => rp.Title == "New Research Project with Team" && rp.TeamMembers.Any());
                Assert.IsTrue(newResearchProjectExists);
            }
        }

        [TestMethod]
        public void Add_research_project_without_title()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddResearchProjectViewModel addResearchProjectViewModel = new AddResearchProjectViewModel(context, _dialogService)
                {
                    Description = "Description without title",
                    Supervisor = "Dr. NoTitle",
                    StartDate = new DateTime(2024, 8, 1),
                    EndDate = new DateTime(2025, 8, 1),
                    Budget = 30000.0f
                };
                addResearchProjectViewModel.Save.Execute(null);

                bool newResearchProjectExists = context.ResearchProjects.Any(rp => rp.Description == "Description without title" && rp.Supervisor == "Dr. NoTitle");
                Assert.IsFalse(newResearchProjectExists);
            }
        }

        [TestMethod]
        public void Add_research_project_without_supervisor()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddResearchProjectViewModel addResearchProjectViewModel = new AddResearchProjectViewModel(context, _dialogService)
                {
                    Title = "Project without Supervisor",
                    Description = "Description for project without supervisor",
                    StartDate = new DateTime(2024, 8, 1),
                    EndDate = new DateTime(2025, 8, 1),
                    Budget = 35000.0f
                };
                addResearchProjectViewModel.Save.Execute(null);

                bool newResearchProjectExists = context.ResearchProjects.Any(rp => rp.Title == "Project without Supervisor" && rp.Description == "Description for project without supervisor");
                Assert.IsTrue(newResearchProjectExists);
            }
        }

        [TestMethod]
        public void Add_research_project_without_description()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddResearchProjectViewModel addResearchProjectViewModel = new AddResearchProjectViewModel(context, _dialogService)
                {
                    Title = "Project without Description",
                    Supervisor = "Dr. NoDescription",
                    StartDate = new DateTime(2024, 8, 1),
                    EndDate = new DateTime(2025, 8, 1),
                    Budget = 40000.0f
                };
                addResearchProjectViewModel.Save.Execute(null);

                bool newResearchProjectExists = context.ResearchProjects.Any(rp => rp.Title == "Project without Description" && rp.Supervisor == "Dr. NoDescription");
                Assert.IsFalse(newResearchProjectExists);
            }
        }

        #endregion

        #region EditTests
        [TestMethod]
        public void Edit_research_project_with_valid_data()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditResearchProjectViewModel editResearchProjectViewModel = new EditResearchProjectViewModel(context, _dialogService)
                {
                    ResearchProjectId = 1,
                    Title = "Updated Research Project 1",
                    Description = "Updated description for research project 1",
                    StartDate = new DateTime(2024, 5, 1),
                    EndDate = new DateTime(2025, 5, 1),
                    Budget = 20000.0f
                };
                editResearchProjectViewModel.Save.Execute(null);

                var updatedResearchProject = context.ResearchProjects.FirstOrDefault(rp => rp.ResearchProjectId == 1);

                Assert.IsNotNull(updatedResearchProject);
                Assert.AreEqual("Updated Research Project 1", updatedResearchProject.Title);
                Assert.AreEqual("Updated description for research project 1", updatedResearchProject.Description);
                Assert.AreEqual(20000.0f, updatedResearchProject.Budget);
            }
        }

        [TestMethod]
        public void Edit_research_project_without_title()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditResearchProjectViewModel editResearchProjectViewModel = new EditResearchProjectViewModel(context, _dialogService)
                {
                    ResearchProjectId = 2,
                    Title = "",
                    Description = "Updated description without title",
                    StartDate = new DateTime(2024, 6, 1),
                    EndDate = new DateTime(2025, 6, 1),
                    Budget = 25000.0f
                };
                editResearchProjectViewModel.Save.Execute(null);

                var updatedResearchProject = context.ResearchProjects.FirstOrDefault(rp => rp.ResearchProjectId == 2);
                Assert.IsNotNull(updatedResearchProject);
                Assert.AreNotEqual("Updated description without title", updatedResearchProject.Description);
            }
        }

        [TestMethod]
        public void Edit_research_project_without_description()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditResearchProjectViewModel editResearchProjectViewModel = new EditResearchProjectViewModel(context, _dialogService)
                {
                    ResearchProjectId = 3,
                    Title = "Updated Research Project 3",
                    Description = "",
                    StartDate = new DateTime(2024, 7, 1),
                    EndDate = new DateTime(2025, 7, 1),
                    Budget = 18000.0f
                };
                editResearchProjectViewModel.Save.Execute(null);

                var updatedResearchProject = context.ResearchProjects.FirstOrDefault(rp => rp.ResearchProjectId == 3);
                Assert.IsNotNull(updatedResearchProject);
                Assert.AreNotEqual(18000.0f, updatedResearchProject.Budget);
            }
        }

        [TestMethod]
        public void Edit_research_project_add_team_members()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                var teamMembers = context.Students.Take(2).ToList();

                EditResearchProjectViewModel editResearchProjectViewModel = new EditResearchProjectViewModel(context, _dialogService)
                {
                    ResearchProjectId = 1,
                    Title = "Updated Research Project with Team",
                    Description = "Updated description for research project with team",
                    StartDate = new DateTime(2024, 5, 1),
                    EndDate = new DateTime(2025, 5, 1),
                    Budget = 22000.0f,
                    AssignedStudents = new ObservableCollection<Student>(teamMembers)
                };
                editResearchProjectViewModel.Save.Execute(null);

                var updatedResearchProject = context.ResearchProjects.Include(rp => rp.TeamMembers).FirstOrDefault(rp => rp.ResearchProjectId == 1);
                Assert.IsNotNull(updatedResearchProject);
                Assert.AreEqual(2, updatedResearchProject.TeamMembers.Count);
            }
        }

        [TestMethod]
        public void Edit_research_project_remove_team_members()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                var teamMembers = context.Students.Take(2).ToList();
                var project = context.ResearchProjects.Include(rp => rp.TeamMembers).FirstOrDefault(rp => rp.ResearchProjectId == 1);
                project.TeamMembers = teamMembers;
                context.SaveChanges();

                EditResearchProjectViewModel editResearchProjectViewModel = new EditResearchProjectViewModel(context, _dialogService)
                {
                    ResearchProjectId = 1,
                    Title = "Updated Research Project without Team",
                    Description = "Updated description for research project without team",
                    StartDate = new DateTime(2024, 5, 1),
                    EndDate = new DateTime(2025, 5, 1),
                    Budget = 22000.0f,
                    AssignedStudents = new ObservableCollection<Student>()
                };
                editResearchProjectViewModel.Save.Execute(null);

                var updatedProject = context.ResearchProjects.Include(rp => rp.TeamMembers).FirstOrDefault(rp => rp.ResearchProjectId == 1);
                Assert.IsNotNull(updatedProject);
                Assert.AreEqual(0, updatedProject.TeamMembers.Count);
            }
        }
        #endregion
    }
}
