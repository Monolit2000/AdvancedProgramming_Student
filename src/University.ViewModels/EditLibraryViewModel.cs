using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class EditLibraryViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private Library _library = new Library();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        return "Name is Required";
                    }
                }
                if (columnName == "Address")
                {
                    if (string.IsNullOrEmpty(Address))
                    {
                        return "Address is Required";
                    }
                }
                if (columnName == "NumberOfFloors")
                {
                    if (NumberOfFloors <= 0)
                    {
                        return "Number of Floors must be greater than 0";
                    }
                }
                if (columnName == "NumberOfRooms")
                {
                    if (NumberOfRooms <= 0)
                    {
                        return "Number of Rooms must be greater than 0";
                    }
                }
                if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                    {
                        return "Description is Required";
                    }
                }
                if (columnName == "Librarian")
                {
                    if (string.IsNullOrEmpty(Librarian))
                    {
                        return "Librarian is Required";
                    }
                }
                return string.Empty;
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private int _numberOfFloors = 0;
        public int NumberOfFloors
        {
            get => _numberOfFloors;
            set
            {
                _numberOfFloors = value;
                OnPropertyChanged(nameof(NumberOfFloors));
            }
        }

        private int _numberOfRooms = 0;
        public int NumberOfRooms
        {
            get => _numberOfRooms;
            set
            {
                _numberOfRooms = value;
                OnPropertyChanged(nameof(NumberOfRooms));
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

        private string _librarian = string.Empty;
        public string Librarian
        {
            get => _librarian;
            set
            {
                _librarian = value;
                OnPropertyChanged(nameof(Librarian));
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

        private long _libraryId = 0;
        public long LibraryId
        {
            get => _libraryId;
            set
            {
                _libraryId = value;
                OnPropertyChanged(nameof(LibraryId));
                LoadLibraryData();
            }
        }

        private ICommand _back;
        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.LibrarySubView = new LibraryViewModel(_context, _dialogService);
            }
        }

        private ICommand _save;
        public ICommand Save => _save ??= new RelayCommand<object>(SaveData);

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var library = _context.Librarys.Find(LibraryId);
            if (library is null)
            {
                Response = "Library not found";
                return;
            }

            library.Name = Name;
            library.Address = Address;
            library.NumberOfFloors = NumberOfFloors;
            library.NumberOfRooms = NumberOfRooms;
            library.Description = Description;
            library.Librarian = Librarian;

            _context.Entry(library).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public EditLibraryViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "Name", "Address", "NumberOfFloors", "NumberOfRooms", "Description", "Librarian" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadLibraryData()
        {
            var library = _context.Librarys.Find(LibraryId);
            if (library is not null)
            {
                this.Name = library.Name;
                this.Address = library.Address;
                this.NumberOfFloors = library.NumberOfFloors;
                this.NumberOfRooms = library.NumberOfRooms;
                this.Description = library.Description;
                this.Librarian = library.Librarian;
            }
            else
            {
                Response = "Library not found";
            }
        }
    }
}