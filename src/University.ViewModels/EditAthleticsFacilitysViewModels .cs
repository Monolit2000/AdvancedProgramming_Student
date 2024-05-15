using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class EditAthleticsFacilityViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private AthleticsFacility? _athleticsFacility = new AthleticsFacility();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name" && string.IsNullOrEmpty(Name))
                {
                    return "Name is Required";
                }
                if (columnName == "Location" && string.IsNullOrEmpty(Location))
                {
                    return "Location is Required";
                }
                if (columnName == "Type" && string.IsNullOrEmpty(Type))
                {
                    return "Type is Required";
                }
                if (columnName == "Description" && string.IsNullOrEmpty(Description))
                {
                    return "Description is Required";
                }
                if (columnName == "Capacity" && Capacity <= 0)
                {
                    return "Capacity should be greater than 0";
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

        private string _location = string.Empty;
        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private string _type = string.Empty;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
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

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
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

        private long _athleticsFacilityId = 0;
        public long AthleticsFacilityId
        {
            get => _athleticsFacilityId;
            set
            {
                _athleticsFacilityId = value;
                OnPropertyChanged(nameof(AthleticsFacilityId));
                LoadAthleticsFacilityData();
            }
        }

        private ICommand? _back = null;
        public ICommand Back
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
                instance.AthleticsFacilitySubView = new AthleticsFacilitysViewModel(_context, _dialogService);
            }
        }

        private ICommand? _save = null;
        public ICommand Save
        {
            get
            {
                if (_save is null)
                {
                    _save = new RelayCommand<object>(SaveData);
                }
                return _save;
            }
        }

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            if (_athleticsFacility is null)
            {
                return;
            }

            _athleticsFacility.Name = Name;
            _athleticsFacility.Location = Location;
            _athleticsFacility.Type = Type;
            _athleticsFacility.Description = Description;
            _athleticsFacility.Capacity = Capacity;

            _context.Entry(_athleticsFacility).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public EditAthleticsFacilityViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "Name", "Location", "Type", "Description", "Capacity" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadAthleticsFacilityData()
        {
            var athleticsFacilities = _context.AthleticsFacilities;
            if (athleticsFacilities is not null)
            {
                _athleticsFacility = athleticsFacilities.Find(AthleticsFacilityId);
                if (_athleticsFacility is null)
                {
                    return;
                }
                this.Name = _athleticsFacility.Name;
                this.Location = _athleticsFacility.Location;
                this.Type = _athleticsFacility.Type;
                this.Description = _athleticsFacility.Description;
                this.Capacity = _athleticsFacility.Capacity;
            }
        }
    }
}
