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
    public class LibraryViewModelTests
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
        public void Show_all_books_in_library_viewmodel()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService);
                bool hasData = addLibraryViewModel.AvailableBooks.Any();
                Assert.IsTrue(hasData);
            }
        }

        [TestMethod]
        public void Add_library_without_books()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService)
                {
                    Name = "New Library",
                    Address = "New Address",
                    NumberOfFloors = 2,
                    NumberOfRooms = 5,
                    Description = "New Description",
                    Librarian = "New Librarian"
                };

                addLibraryViewModel.Save.Execute(null);
                bool newLibraryExists = context.Librarys.Any(l => l.Name == "New Library" && l.Address == "New Address");
                Assert.IsTrue(newLibraryExists);
            }
        }

        [TestMethod]
        public void Add_library_with_books()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService);
                var book = context.Books.First();

                addLibraryViewModel.Name = "New Library";
                addLibraryViewModel.Address = "New Address";
                addLibraryViewModel.NumberOfFloors = 3;
                addLibraryViewModel.NumberOfRooms = 7;
                addLibraryViewModel.Description = "New Description";
                addLibraryViewModel.Librarian = "New Librarian";
                addLibraryViewModel.AssignedBooks.Add(book);

                addLibraryViewModel.Save.Execute(null); ;

                bool newLibraryExists = context.Librarys.Any(l => l.Name == "New Library" && l.Books.Any(b => b.BookId == book.BookId));
                Assert.IsTrue(newLibraryExists);
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

        [TestMethod]
        public void Add_library_without_floors()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                AddLibraryViewModel addLibraryViewModel = new AddLibraryViewModel(context, _dialogService)
                {
                    Name = "Library without Floors",
                    Address = "Address without Floors",
                    NumberOfRooms = 10,
                    Description = "Description without Floors",
                    Librarian = "Librarian without Floors"
                };

                addLibraryViewModel.Save.Execute(null);

                bool newLibraryExists = context.Librarys.Any(l => l.Name == "Library without Floors");
                Assert.IsFalse(newLibraryExists);
            }
        }
    }
}
