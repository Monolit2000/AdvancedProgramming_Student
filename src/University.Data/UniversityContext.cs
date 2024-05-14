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

            modelBuilder.Entity<ResearchProject>().HasData(
               new ResearchProject
               {
                   ProjectId = "PRJ001",
                   Title = "Example Research Project 1",
                   Description = "This is an example research project description 1.",
                   TeamMembers = new List<string> { "John", "Alice", "Bob" },
                   Supervisor = "Dr. Smith",
                   StartDate = new DateTime(2024, 5, 1),
                   EndDate = new DateTime(2025, 5, 1),
                   Budget = 10000.0f
               },
               new ResearchProject
               {
                   ProjectId = "PRJ002",
                   Title = "Example Research Project 2",
                   Description = "This is an example research project description 2.",
                   TeamMembers = new List<string> { "Alice", "Charlie", "David" },
                   Supervisor = "Dr. Johnson",
                   StartDate = new DateTime(2024, 6, 1),
                   EndDate = new DateTime(2025, 6, 1),
                   Budget = 15000.0f
               },

               new ResearchProject
               {
                   ProjectId = "PRJ003",
                   Title = "Example Research Project 3",
                   Description = "This is an example research project description 3.",
                   TeamMembers = new List<string> { "Emma", "Frank" },
                   Supervisor = "Dr. Lee",
                   StartDate = new DateTime(2024, 7, 1),
                   EndDate = new DateTime(2025, 7, 1),
                   Budget = 12000.0f
               }
           );
        }
    }
}
