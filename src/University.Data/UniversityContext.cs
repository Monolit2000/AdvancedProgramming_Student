using University.Models;
using Microsoft.EntityFrameworkCore;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ResearchProject> ResearchProjects { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Librarys { get; set; }
        public DbSet<StudentOrganization> StudentOrganizations { get; set; }
        public DbSet<AthleticsFacility> AthleticsFacilities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("UniversityDb");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region StudentSubject
            modelBuilder.Entity<Subject>().Ignore(s => s.IsSelected);

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Wieńczysław", LastName = "Nowakowicz", PESEL = "PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                new Student { StudentId = 2, Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, Name = "Matematyka", Semester = "1", Lecturer = "Michalina Warszawa" },
                new Subject { SubjectId = 2, Name = "Biologia", Semester = "2", Lecturer = "Halina Katowice" },
                new Subject { SubjectId = 3, Name = "Chemia", Semester = "3", Lecturer = "Jan Nowak" }
            );
            #endregion

            #region ResearchProject
            modelBuilder.Entity<ResearchProject>().HasData(
               new ResearchProject
               {
                   ResearchProjectId = 1,
                   Title = "Example Research Project 1",
                   Description = "This is an example research project description 1.",
                   //TeamMembers = new List<string> { "John", "Alice", "Bob" },
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
                   //TeamMembers = new List<string> { "Alice", "Charlie", "David" },
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
                   //TeamMembers = new List<string> { "Emma", "Frank" },
                   Supervisor = "Dr. Lee",
                   StartDate = new DateTime(2024, 7, 1),
                   EndDate = new DateTime(2025, 7, 1),
                   Budget = 12000.0f
               }
           );
            #endregion

            #region Book 
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "Example Book 1",
                    Author = "John Doe",
                    Publisher = "Publisher 1",
                    PublicationDate = new DateTime(2020, 1, 1),
                    ISBN = "ISBN1",
                    Genre = "Genre 1",
                    Description = "This is an example book description 1."
                },
                new Book
                {
                    BookId = 2,
                    Title = "Example Book 2",
                    Author = "Alice Smith",
                    Publisher = "Publisher 2",
                    PublicationDate = new DateTime(2021, 2, 2),
                    ISBN = "ISBN2",
                    Genre = "Genre 2",
                    Description = "This is an example book description 2."
                },
                new Book
                {
                    BookId = 3,
                    Title = "Example Book 3",
                    Author = "Bob Johnson",
                    Publisher = "Publisher 3",
                    PublicationDate = new DateTime(2022, 3, 3),
                    ISBN = "ISBN3",
                    Genre = "Genre 3",
                    Description = "This is an example book description 3."
                }
            );

            #endregion

            #region Library
            modelBuilder.Entity<Library>().HasData(
                new Library
                {
                    LibraryId = 1,
                    Name = "Main Library",
                    Address = "123 Main Street",
                    NumberOfFloors = 3,
                    NumberOfRooms = 10,
                    Description = "This is the main library of the university.",
                    Librarian = "Alice Librarian"
                },
                new Library
                {
                    LibraryId = 2,
                    Name = "Science Library",
                    Address = "456 Science Avenue",
                    NumberOfFloors = 2,
                    NumberOfRooms = 8,
                    Description = "This is the science library of the university.",
                    Librarian = "Bob Librarian"
                },
                 new Library
                 {
                     LibraryId = 3,
                     Name = "Arts Library",
                     Address = "789 Arts Boulevard",
                     NumberOfFloors = 4,
                     NumberOfRooms = 12,
                     Description = "This is the arts library of the university.",
                     Librarian = "Charlie Librarian"
                 }
            );
            #endregion

            #region StudentOrganization
            modelBuilder.Entity<StudentOrganization>().HasData(
                  new StudentOrganization
                  {
                      StudentOrganizationId = 1,
                      Name = "Student Organization 1",
                      Advisor = "Advisor 1",
                      President = "President 1",
                      Description = "Description of Student Organization 1",
                      MeetingSchedule = "Weekly",
                      Email = "org1@example.com"
                  },
                  new StudentOrganization
                  {
                      StudentOrganizationId = 2,
                      Name = "Student Organization 2",
                      Advisor = "Advisor 2",
                      President = "President 2",
                      Description = "Description of Student Organization 2",
                      MeetingSchedule = "Bi-weekly",
                      Email = "org2@example.com"
                  },
                  new StudentOrganization
                  {
                      StudentOrganizationId = 3,
                      Name = "Student Organization 3",
                      Advisor = "Advisor 3",
                      President = "President 3",
                      Description = "Description of Student Organization 3",
                      MeetingSchedule = "Monthly",
                      Email = "org3@example.com"
                  }
           );


            #endregion

            #region AthleticsFacility


            modelBuilder.Entity<AthleticsFacility>().HasData(
                new AthleticsFacility
                {
                    AthleticsFacilityId = 1,
                    Name = "Athletics Facility 1",
                    Location = "Location 1",
                    Type = "Type 1",
                    Description = "Description of Athletics Facility 1",
                    Capacity = 100
                },
                new AthleticsFacility
                {
                    AthleticsFacilityId = 2,
                    Name = "Athletics Facility 2",
                    Location = "Location 2",
                    Type = "Type 2",
                    Description = "Description of Athletics Facility 2",
                    Capacity = 150
                },
                new AthleticsFacility
                {
                    AthleticsFacilityId = 3,
                    Name = "Athletics Facility 3",
                    Location = "Location 3",
                    Type = "Type 3",
                    Description = "Description of Athletics Facility 3",
                    Capacity = 200
                }
            );


            #endregion
        }
    }
}
