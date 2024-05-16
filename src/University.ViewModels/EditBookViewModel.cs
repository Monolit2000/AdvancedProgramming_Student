using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class EditBookViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private Book _book = new Book();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Title")
                {
                    if (string.IsNullOrEmpty(Title))
                    {
                        return "Title is Required";
                    }
                }
                if (columnName == "Author")
                {
                    if (string.IsNullOrEmpty(Author))
                    {
                        return "Author is Required";
                    }
                }
                if (columnName == "Publisher")
                {
                    if (string.IsNullOrEmpty(Publisher))
                    {
                        return "Publisher is Required";
                    }
                }
                if (columnName == "PublicationDate")
                {
                    if (PublicationDate is null)
                    {
                        return "Publication Date is Required";
                    }
                }
                if (columnName == "ISBN")
                {
                    if (string.IsNullOrEmpty(ISBN))
                    {
                        return "ISBN is Required";
                    }
                }
                if (columnName == "Genre")
                {
                    if (string.IsNullOrEmpty(Genre))
                    {
                        return "Genre is Required";
                    }
                }
                if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                    {
                        return "Description is Required";
                    }
                }
                return string.Empty;
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _author = string.Empty;
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _publisher = string.Empty;
        public string Publisher
        {
            get => _publisher;
            set
            {
                _publisher = value;
                OnPropertyChanged(nameof(Publisher));
            }
        }

        private DateTime? _publicationDate = null;
        public DateTime? PublicationDate
        {
            get => _publicationDate;
            set
            {
                _publicationDate = value;
                OnPropertyChanged(nameof(PublicationDate));
            }
        }

        private string _isbn = string.Empty;
        public string ISBN
        {
            get => _isbn;
            set
            {
                _isbn = value;
                OnPropertyChanged(nameof(ISBN));
            }
        }

        private string _genre = string.Empty;
        public string Genre
        {
            get => _genre;
            set
            {
                _genre = value;
                OnPropertyChanged(nameof(Genre));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        private long _bookId = 0;
        public long BookId
        {
            get => _bookId;
            set
            {
                _bookId = value;
                OnPropertyChanged(nameof(BookId));
                LoadBookData();
            }
        }




        #region Available Assigned

        private ObservableCollection<Library>? _availableLibraries = null;
        public ObservableCollection<Library> AvailableLibraries
        {
            get
            {
                if (_availableLibraries is null)
                {
                    _availableLibraries = LoadLibraries();
                    return _availableLibraries;
                }
                return _availableLibraries;
            }
            set
            {
                _availableLibraries = value;
                OnPropertyChanged(nameof(AvailableLibraries));
            }
        }


        private ObservableCollection<Library>? _assignedLibraries = null;
        public ObservableCollection<Library> AssignedLibraries
        {
            get
            {
                if (_assignedLibraries is null)
                {
                    _assignedLibraries = new ObservableCollection<Library>();
                    return _assignedLibraries;
                }
                return _assignedLibraries;
            }
            set
            {
                _assignedLibraries = value;
                OnPropertyChanged(nameof(AssignedLibraries));
            }
        }


        private ObservableCollection<Library> LoadLibraries()
        {
            _context.Database.EnsureCreated();
            _context.Librarys.Load();
            return _context.Librarys.Local.ToObservableCollection();
        }

        #endregion


        #region Add Remuve

        private ICommand? _add = null;
        public ICommand Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddLibrary);
                }
                return _add;
            }
        }

        private void AddLibrary(object? obj)
        {
            if (obj is Library book)
            {
                if (AssignedLibraries is not null && !AssignedLibraries.Contains(book))
                {
                    AssignedLibraries.Add(book);
                }
            }
        }

        private ICommand? _remove = null;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveLibrary);
                }
                return _remove;
            }
        }

        private void RemoveLibrary(object? obj)
        {
            if (obj is Library book)
            {
                if (AvailableLibraries is not null)
                {
                    AvailableLibraries.Remove(book);
                }
            }
        }

        #endregion


        #region Navigate

        private ICommand? _back = null;
        public ICommand? Back
        {
            get
            {
                if (_back is null)
                {
                    _back = new RelayCommand<object>(NavigateBack);
                }
                return _back;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.BookSubView = new BookViewModel(_context, _dialogService);
            }
        }

        private ICommand? _saveCommand = null;
        public ICommand Save => _saveCommand ??= new RelayCommand<object>(SaveData);

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var book = new Book
            {
                Title = Title,
                Author = Author,
                Publisher = Publisher,
                PublicationDate = PublicationDate.Value,
                ISBN = ISBN,
                Genre = Genre,
                Description = Description,
                Libraries = AssignedLibraries


            };

            _context.Books.Add(book);
            _context.SaveChanges();

            Response = "Data Saved";
        }

        #endregion






















        //private ICommand _back;
        //public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        //private void NavigateBack(object? obj)
        //{
        //    var instance = MainWindowViewModel.Instance();
        //    if (instance is not null)
        //    {
        //        instance.BookSubView = new BookViewModel(_context, _dialogService);
        //    }
        //}

        //private ICommand _save;
        //public ICommand Save => _save ??= new RelayCommand<object>(SaveData);

        //private void SaveData(object? obj)
        //{
        //    if (!IsValid())
        //    {
        //        Response = "Please complete all required fields";
        //        return;
        //    }

        //    var book = _context.Books.Find(BookId);
        //    if (book is null)
        //    {
        //        Response = "Book not found";
        //        return;
        //    }


        //    book.Title = Title;
        //    book.Author = Author;
        //    book.Publisher = Publisher;
        //    book.PublicationDate = PublicationDate.Value;
        //    book.ISBN = ISBN;
        //    book.Genre = Genre;
        //    book.Description = Description;

        //    _context.Entry(book).State = EntityState.Modified;
        //    _context.SaveChanges();

        //    Response = "Data Saved";
        //}

        public EditBookViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "Title", "Author", "Publisher", "PublicationDate", "ISBN", "Genre", "Description" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadBookData()
        {
            var book = _context.Books.Find(BookId);
            if (book is not null)
            {
                this.Title = book.Title;
                this.Author = book.Author;
                this.Publisher = book.Publisher;
                this.PublicationDate = book.PublicationDate;
                this.ISBN = book.ISBN;
                this.Genre = book.Genre;
                this.Description = book.Description;
            }
            else
            {
                Response = "Book not found";
            }
        }
    }
}
