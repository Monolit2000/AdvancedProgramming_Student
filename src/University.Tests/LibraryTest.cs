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

        #region Add

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

        #endregion

        #region Edit

        [TestMethod]
        public void Edit_library_with_valid_data()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "Updated Address A",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 1);

                Assert.IsNotNull(updatedLibrary);
                Assert.AreEqual("Updated Library One", updatedLibrary.Name);
                Assert.AreEqual("Updated Address A", updatedLibrary.Address);
                Assert.AreEqual(4, updatedLibrary.NumberOfFloors);
                Assert.AreEqual(12, updatedLibrary.NumberOfRooms);
                Assert.AreEqual("Updated Description A", updatedLibrary.Description);
                Assert.AreEqual("Updated Librarian A", updatedLibrary.Librarian);
            }
        }

        [TestMethod]
        public void Edit_library_without_name()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 2,
                    Name = "",
                    Address = "Updated Address B",
                    NumberOfFloors = 2,
                    NumberOfRooms = 8,
                    Description = "Updated Description B",
                    Librarian = "Updated Librarian B"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 2);
                Assert.IsNotNull(updatedLibrary);
                Assert.AreNotEqual("Updated Address B", updatedLibrary.Address);
            }
        }

        [TestMethod]
        public void Edit_library_without_address()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.FirstOrDefault(l => l.LibraryId == 1);
                Assert.IsNotNull(updatedLibrary);
                Assert.AreNotEqual(4, updatedLibrary.NumberOfFloors);
            }
        }


        [TestMethod]
        public void Edit_libraries_add_books()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                var library = context.Librarys.Include(l => l.Books).FirstOrDefault(l => l.LibraryId == 1);
                var newBook = context.Books.FirstOrDefault(b => b.BookId == 3);

                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "Updated Address A",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.AssignedBooks.Add(newBook);
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.Include(l => l.Books).FirstOrDefault(l => l.LibraryId == 1);

                Assert.IsNotNull(updatedLibrary);
                Assert.AreEqual(3, updatedLibrary.Books.Count);
            }
        }

        [TestMethod]
        public void Edit_libraries_remove_books()
        {
            using UniversityContext context = new UniversityContext(_options);
            {
                var library = context.Librarys.Include(l => l.Books).FirstOrDefault(l => l.LibraryId == 1);
                var bookToRemove = library.Books.FirstOrDefault(b => b.BookId == 1);

                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(context, _dialogService)
                {
                    LibraryId = 1,
                    Name = "Updated Library One",
                    Address = "Updated Address A",
                    NumberOfFloors = 4,
                    NumberOfRooms = 12,
                    Description = "Updated Description A",
                    Librarian = "Updated Librarian A"
                };
                editLibraryViewModel.AssignedBooks.Remove(bookToRemove);
                editLibraryViewModel.Save.Execute(null);

                var updatedLibrary = context.Librarys.Include(l => l.Books).FirstOrDefault(l => l.LibraryId == 1);

                Assert.IsNotNull(updatedLibrary);
                Assert.AreEqual(1, updatedLibrary.Books.Count);
            }
        }


        #endregion

    }
}
