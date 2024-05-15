using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;


namespace University.ViewModels
{
    public class AddLibraryViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrEmpty(Name))
                {
                    return "Name is Required";
                }
                if (columnName == "Address" && string.IsNullOrEmpty(Address))
                {
                    return "Address is Required";
                }
                if (columnName == "NumberOfFloors" && NumberOfFloors <= 0)
                {
                    return "Number of Floors must be greater than 0";
                }
                if (columnName == "NumberOfRooms" && NumberOfRooms <= 0)
                {
                    return "Number of Rooms must be greater than 0";
                }
                if (columnName == "Description" && string.IsNullOrEmpty(Description))
                {
                    return "Description is Required";
                }
                if (columnName == "Librarian" && string.IsNullOrEmpty(Librarian))
                {
                    return "Librarian is Required";
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
            get => _response;
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

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
                instance.LibrarySubView = new LibraryViewModel(_context, _dialogService);
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

            var library = new Library
            {
                Name = Name,
                Address = Address,
                NumberOfFloors = NumberOfFloors,
                NumberOfRooms = NumberOfRooms,
                Description = Description,
                Librarian = Librarian
            };

            _context.Librarys.Add(library);
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public AddLibraryViewModel(UniversityContext context, IDialogService dialogService)
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
    }
}