using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.Services;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class LibraryBooksTest
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

                var books = new List<Book>
                {
                    new Book { BookId = 1, Title = "Book One", Author = "Author A", Publisher = "Publisher A", PublicationDate = new DateTime(2000, 01, 01), ISBN = "1234567890123", Genre = "Genre A", Description = "Description A" },
                    new Book { BookId = 2, Title = "Book Two", Author = "Author B", Publisher = "Publisher B", PublicationDate = new DateTime(2005, 05, 05), ISBN = "1234567890124", Genre = "Genre B", Description = "Description B" },
                    new Book { BookId = 3, Title = "Book Three", Author = "Author C", Publisher = "Publisher C", PublicationDate = new DateTime(2010, 10, 10), ISBN = "1234567890125", Genre = "Genre C", Description = "Description C" }
                };

                var libraries = new List<Library>
                {
                    new Library { LibraryId = 1, Name = "Library One", Address = "Address A", NumberOfFloors = 3, NumberOfRooms = 10, Description = "Description A", Librarian = "Librarian A", Books = new List<Book>{ books[0], books[1] } },
                    new Library { LibraryId = 2, Name = "Library Two", Address = "Address B", NumberOfFloors = 2, NumberOfRooms = 8, Description = "Description B", Librarian = "Librarian B", Books = new List<Book>{ books[2] } }
                };

                context.Books.AddRange(books);
                context.Librarys.AddRange(libraries);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void Show_all_books()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddBookViewModel addBookViewModel = new AddBookViewModel(context, _dialogService);
                bool hasData = addBookViewModel.AvailableLibraries.Any();
                Assert.IsTrue(hasData);
            }
        }

        [TestMethod]
        public void Add_book_without_libraries()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddBookViewModel addBookViewModel = new AddBookViewModel(context, _dialogService)
                {
                    Title = "New Book",
                    Author = "New Author",
                    Publisher = "New Publisher",
                    PublicationDate = new DateTime(2020, 01, 01),
                    ISBN = "9876543210123",
                    Genre = "New Genre",
                    Description = "New Description"
                };

                addBookViewModel.Save.Execute(null);

                bool newBookExists = context.Books.Any(b => b.Title == "New Book" && b.Author == "New Author");
                Assert.IsTrue(newBookExists);
            }
        }

        [TestMethod]
        public void Add_library_with_books()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddBookViewModel addBookViewModel = new AddBookViewModel(context, _dialogService);
                var book = context.Books.First();

                addBookViewModel.Title = "New Book";
                addBookViewModel.Author = "New Author";
                addBookViewModel.Publisher = "New Publisher";
                addBookViewModel.PublicationDate = new DateTime(2020, 01, 01);
                addBookViewModel.ISBN = "9876543210123";
                addBookViewModel.Genre = "New Genre";
                addBookViewModel.Description = "New Description";
                addBookViewModel.AssignedLibraries.Add(context.Librarys.First());

                addBookViewModel.Save.Execute(null);

                bool newBookExists = context.Books.Any(b => b.Title == "New Book" && b.Libraries.Any(l => l.LibraryId == context.Librarys.First().LibraryId));
                Assert.IsTrue(newBookExists);
            }
        }

        [TestMethod]
        public void Add_book_without_title()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddBookViewModel addBookViewModel = new AddBookViewModel(context, _dialogService)
                {
                    Author = "Author without Title",
                    Publisher = "Publisher without Title",
                    PublicationDate = new DateTime(2020, 01, 01),
                    ISBN = "9876543210124",
                    Genre = "Genre without Title",
                    Description = "Description without Title"
                };

                addBookViewModel.Save.Execute(null);

                bool newBookExists = context.Books.Any(b => b.Author == "Author without Title");
                Assert.IsFalse(newBookExists);
            }
        }

        [TestMethod]
        public void Add_library_without_name()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService)
                {
                    Address = "Address without Name",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Description without Name",
                    Librarian = "Librarian without Name"
                };

                addLibraryViewModel.Save.Execute(null);

                bool newLibraryExists = context.Librarys.Any(l => l.Address == "Address without Name");
                Assert.IsFalse(newLibraryExists);
            }
        }
    }
}
