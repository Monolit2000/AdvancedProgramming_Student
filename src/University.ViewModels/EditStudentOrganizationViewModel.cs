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
    public class EditStudentOrganizationViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private StudentOrganization? _organization;

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(_name))
                    {
                        return "Name is Required";
                    }
                }
                // Add validation for other properties if needed
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

        private string _advisor = string.Empty;
        public string Advisor
        {
            get => _advisor;
            set
            {
                _advisor = value;
                OnPropertyChanged(nameof(Advisor));
            }
        }

        private string _president = string.Empty;
        public string President
        {
            get => _president;
            set
            {
                _president = value;
                OnPropertyChanged(nameof(President));
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

        private string _meetingSchedule = string.Empty;
        public string MeetingSchedule
        {
            get => _meetingSchedule;
            set
            {
                _meetingSchedule = value;
                OnPropertyChanged(nameof(MeetingSchedule));
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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

        private long _organizationId = 0;
        public long OrganizationId
        {
            get => _organizationId;
            set
            {
                _organizationId = value;
                OnPropertyChanged(nameof(OrganizationId));
                LoadOrganizationData();
            }
        }

        private ObservableCollection<Student>? _availableStudents = null;
        public ObservableCollection<Student> AvailableStudents
        {
            get
            {
                if (_availableStudents is null)
                {
                    _availableStudents = LoadStudents();
                    return _availableStudents;
                }
                return _availableStudents;
            }
            set
            {
                _availableStudents = value;
                OnPropertyChanged(nameof(AvailableStudents));
            }
        }

        private ObservableCollection<Student>? _assignedStudents = null;
        public ObservableCollection<Student>? AssignedStudents
        {
            get
            {
                if (_assignedStudents is null)
                {
                    _assignedStudents = new ObservableCollection<Student>();
                    return _assignedStudents;
                }
                return _assignedStudents;
            }
            set
            {
                _assignedStudents = value;
                OnPropertyChanged(nameof(AssignedStudents));
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
                instance.SubjectsSubView = new SubjectsViewModel(_context, _dialogService);
            }
        }

        private ICommand? _add;
        public ICommand Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddStudent);
                }
                return _add;
            }
        }

        private void AddStudent(object? obj)
        {
            if (obj is Student student)
            {
                if (AssignedStudents is not null && !AssignedStudents.Contains(student))
                {
                    AssignedStudents.Add(student);
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
                    _remove = new RelayCommand<object>(RemoveStudent);
                }
                return _remove;
            }
        }

        private void RemoveStudent(object? obj)
        {
            if (obj is Student student)
            {
                if (AssignedStudents is not null)
                {
                    AssignedStudents.Remove(student);
                }
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

            if (_organization is null)
            {
                return;
            }

            _organization.Name = Name;
            _organization.Advisor = Advisor;
            _organization.President = President;
            _organization.Description = Description;
            _organization.MeetingSchedule = MeetingSchedule;
            _organization.Email = Email;

            _context.Entry(_organization).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public EditStudentOrganizationViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private ObservableCollection<Student> LoadStudents()
        {
            _context.Database.EnsureCreated();
            _context.Students.Load();
            return _context.Students.Local.ToObservableCollection();
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(_name) &&
                   !string.IsNullOrEmpty(_advisor) &&
                   !string.IsNullOrEmpty(_president) &&
                   !string.IsNullOrEmpty(_email);
            // Add more validation rules if needed
        }

        private void LoadOrganizationData()
        {
            _organization = _context.StudentOrganizations.Find(OrganizationId);
            if (_organization != null)
            {
                Name = _organization.Name;
                Advisor = _organization.Advisor;
                President = _organization.President;
                Description = _organization.Description;
                MeetingSchedule = _organization.MeetingSchedule;
                Email = _organization.Email;
            }
        }
    }
}
