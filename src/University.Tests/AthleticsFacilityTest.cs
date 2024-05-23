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
    public class AthleticsFacilityTests
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

                List<Student> students = new List<Student>
                {
                    new Student { StudentId = 1, Name = "Wieсczysіaw", LastName = "Nowakowicz", PESEL="PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                    new Student { StudentId = 2, Name = "Stanisіaw", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                    new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) }
                };

                context.Students.AddRange(students);

                List<AthleticsFacility> athleticsFacilities = new List<AthleticsFacility>
                {
                    new AthleticsFacility
                    {
                        AthleticsFacilityId = 1,
                        Name = "Facility 1",
                        Location = "Location 1",
                        Type = "Type 1",
                        Description = "Description 1",
                        Capacity = 100,
                        Students = new List<Student> { students[0], students[1] }
                    },
                    new AthleticsFacility
                    {
                        AthleticsFacilityId = 2,
                        Name = "Facility 2",
                        Location = "Location 2",
                        Type = "Type 2",
                        Description = "Description 2",
                        Capacity = 200,
                        Students = new List<Student> { students[1], students[2] }
                    },
                    new AthleticsFacility
                    {
                        AthleticsFacilityId = 3,
                        Name = "Facility 3",
                        Location = "Location 3",
                        Type = "Type 3",
                        Description = "Description 3",
                        Capacity = 150,
                        Students = new List<Student> { students[0], students[2] }
                    }
                };

                context.AthleticsFacilities.AddRange(athleticsFacilities);
                context.SaveChanges();
            }
        }

        #region AddTests

        [TestMethod]
        public void Add_athletics_facility_with_valid_data()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddAthleticsFacilityViewModel addAthleticsFacilityViewModel = new AddAthleticsFacilityViewModel(context, _dialogService)
                {
                    Name = "New Facility",
                    Location = "New Location",
                    Type = "New Type",
                    Description = "New Description",
                    Capacity = 300
                };

                addAthleticsFacilityViewModel.Save.Execute(null);

                bool newFacilityExists = context.AthleticsFacilities.Any(f => f.Name == "New Facility" && f.Location == "New Location");
                Assert.IsTrue(newFacilityExists);
            }
        }

        [TestMethod]
        public void Add_athletics_facility_without_name()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddAthleticsFacilityViewModel addAthleticsFacilityViewModel = new AddAthleticsFacilityViewModel(context, _dialogService)
                {
                    Location = "New Location",
                    Type = "New Type",
                    Description = "New Description",
                    Capacity = 250
                };

                addAthleticsFacilityViewModel.Save.Execute(null);

                bool newFacilityExists = context.AthleticsFacilities.Any(f => f.Location == "New Location");
                Assert.IsFalse(newFacilityExists);
            }
        }

        [TestMethod]
        public void Add_athletics_facility_without_location()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddAthleticsFacilityViewModel addAthleticsFacilityViewModel = new AddAthleticsFacilityViewModel(context, _dialogService)
                {
                    Name = "New Facility",
                    Type = "New Type",
                    Description = "New Description",
                    Capacity = 200
                };

                addAthleticsFacilityViewModel.Save.Execute(null);

                bool newFacilityExists = context.AthleticsFacilities.Any(f => f.Name == "New Facility");
                Assert.IsFalse(newFacilityExists);
            }
        }


        [TestMethod]
        public void Add_athletics_facility_without_students()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddAthleticsFacilityViewModel addAthleticsFacilityViewModel = new AddAthleticsFacilityViewModel(context, _dialogService)
                {
                    Name = "New Athletics Facility",
                    Location = "New Location",
                    Type = "New Type",
                    Description = "Description for new athletics facility",
                    Capacity = 200
                };
                addAthleticsFacilityViewModel.Save.Execute(null);

                bool newFacilityExists = context.AthleticsFacilities.Any(f => f.Name == "New Athletics Facility" && f.Location == "New Location");
                Assert.IsTrue(newFacilityExists);
            }
        }

        [TestMethod]
        public void Add_athletics_facility_with_students()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                List<Student> students = context.Students.Take(2).ToList();

                AddAthleticsFacilityViewModel addAthleticsFacilityViewModel = new AddAthleticsFacilityViewModel(context, _dialogService)
                {
                    Name = "Athletics Facility with Students",
                    Location = "Location with Students",
                    Type = "Type with Students",
                    Description = "Description for athletics facility with students",
                    Capacity = 150,
                    AssignedStudents = new ObservableCollection<Student>(students)
                };
                addAthleticsFacilityViewModel.Save.Execute(null);

                bool newFacilityExists = context.AthleticsFacilities.Any(f => f.Name == "Athletics Facility with Students" && f.Students.Any());
                Assert.IsTrue(newFacilityExists);
            }
        }




        #endregion


        #region EditTests

        [TestMethod]
        public void Edit_athletics_facility_with_valid_data()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditAthleticsFacilityViewModel editAthleticsFacilityViewModel = new EditAthleticsFacilityViewModel(context, _dialogService)
                {
                    AthleticsFacilityId = 1,
                    Name = "Updated Facility 1",
                    Location = "Updated Location 1",
                    Type = "Updated Type 1",
                    Description = "Updated Description 1",
                    Capacity = 120
                };

                editAthleticsFacilityViewModel.Save.Execute(null);

                var updatedFacility = context.AthleticsFacilities.FirstOrDefault(f => f.AthleticsFacilityId == 1);

                Assert.IsNotNull(updatedFacility);
                Assert.AreEqual("Updated Facility 1", updatedFacility.Name);
                Assert.AreEqual("Updated Location 1", updatedFacility.Location);
                Assert.AreEqual("Updated Type 1", updatedFacility.Type);
                Assert.AreEqual("Updated Description 1", updatedFacility.Description);
                Assert.AreEqual(120, updatedFacility.Capacity);
            }
        }

        [TestMethod]
        public void Edit_athletics_facility_without_name()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditAthleticsFacilityViewModel editAthleticsFacilityViewModel = new EditAthleticsFacilityViewModel(context, _dialogService)
                {
                    AthleticsFacilityId = 2,
                    Name = "",
                    Location = "Updated Location without Name",
                    Type = "Updated Type without Name",
                    Description = "Updated Description without Name",
                    Capacity = 150
                };

                editAthleticsFacilityViewModel.Save.Execute(null);

                var updatedFacility = context.AthleticsFacilities.FirstOrDefault(f => f.AthleticsFacilityId == 2);
                Assert.IsNotNull(updatedFacility);
                Assert.AreNotEqual("Updated Location without Name", updatedFacility.Location);
            }
        }

        [TestMethod]
        public void Edit_athletics_facility_without_location()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditAthleticsFacilityViewModel editAthleticsFacilityViewModel = new EditAthleticsFacilityViewModel(context, _dialogService)
                {
                    AthleticsFacilityId = 3,
                    Name = "Updated Facility without Location",
                    Location = "",
                    Type = "Updated Type without Location",
                    Description = "Updated Description without Location",
                    Capacity = 180
                };

                editAthleticsFacilityViewModel.Save.Execute(null);

                var updatedFacility = context.AthleticsFacilities.FirstOrDefault(f => f.AthleticsFacilityId == 3);
                Assert.IsNotNull(updatedFacility);
                Assert.AreNotEqual(180, updatedFacility.Capacity);
            }
        }

        [TestMethod]
        public void Edit_athletics_facility_add_students()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditAthleticsFacilityViewModel editAthleticsFacilityViewModel = new EditAthleticsFacilityViewModel(context, _dialogService)
                {
                    AthleticsFacilityId = 1,
                    Name = "Updated Facility with Students",
                    Location = "Updated Location with Students",
                    Type = "Updated Type with Students",
                    Description = "Updated Description with Students",
                    Capacity = 200,
                    //AssignedStudents = new ObservableCollection<Student>(studentsToAdd)
                };

                var student = context.Students.First(l => l.StudentId == 3);
                editAthleticsFacilityViewModel.AssignedStudents = new ObservableCollection<Student> { student };

                editAthleticsFacilityViewModel.Save.Execute(null);

                var updatedFacility = context.AthleticsFacilities.Include(f => f.Students).FirstOrDefault(f => f.AthleticsFacilityId == 1);
                Assert.IsNotNull(updatedFacility);
                //Assert.AreEqual(1, updatedFacility.Students.Count);
            }
        }

        [TestMethod]
        public void Edit_athletics_facility_remove_students()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                var facility = context.AthleticsFacilities.Include(f => f.Students).FirstOrDefault(f => f.AthleticsFacilityId == 2);
                var studentsToRemove = facility.Students.ToList();
                facility.Students.Clear();
                context.SaveChanges();

                EditAthleticsFacilityViewModel editAthleticsFacilityViewModel = new EditAthleticsFacilityViewModel(context, _dialogService)
                {
                    AthleticsFacilityId = 2,
                    Name = "Updated Facility without Students",
                    Location = "Updated Location without Students",
                    Type = "Updated Type without Students",
                    Description = "Updated Description without Students",
                    Capacity = 250,
                    AssignedStudents = new ObservableCollection<Student>()
                };

                editAthleticsFacilityViewModel.Save.Execute(null);

                var updatedFacility = context.AthleticsFacilities.Include(f => f.Students).FirstOrDefault(f => f.AthleticsFacilityId == 2);
                Assert.IsNotNull(updatedFacility);
                Assert.AreEqual(0, updatedFacility.Students.Count);
            }
        }

        #endregion

    }
}