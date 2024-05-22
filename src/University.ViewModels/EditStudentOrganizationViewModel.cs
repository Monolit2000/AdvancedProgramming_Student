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
        private StudentOrganization? _studentOrganization = new StudentOrganization();

        public string Error => string.Empty;

        #region props
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
                if (columnName == "Advisor")
                {
                    if (string.IsNullOrEmpty(Advisor))
                    {
                        return "Advisor is Required";
                    }
                }
                if (columnName == "President")
                {
                    if (string.IsNullOrEmpty(President))
                    {
                        return "President is Required";
                    }
                }
                if (columnName == "Email")
                {
                    if (string.IsNullOrEmpty(Email))
                    {
                        return "Email is Required";
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

        private long _studentOrganizationId = 0;
        public long StudentOrganizationId
        {
            get => _studentOrganizationId;
            set
            {
                _studentOrganizationId = value;
                OnPropertyChanged(nameof(StudentOrganizationId));
                LoadStudentOrganizationData();
            }
        }

        #endregion

        #region  Available Assigned
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

        #endregion

        #region Add Remuve


        private ICommand? _add = null;
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

        #endregion

        #region Navigations 
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
                instance.StudentOrganizationSubView = new StudentOrganizationViewModel(_context, _dialogService);
            }
        }


        private ICommand? _save = null;
        public ICommand Save => _save ??= new RelayCommand<object>(SaveData);
 

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            var studentOrganization = _context.StudentOrganizations.Find(StudentOrganizationId);
            if (studentOrganization is null)
            {
                Response = "Student organization not found";
                return;
            }

            studentOrganization.Name = Name;
            studentOrganization.Advisor = Advisor;
            studentOrganization.President = President;
            studentOrganization.Description = Description;
            studentOrganization.MeetingSchedule = MeetingSchedule;
            studentOrganization.Email = Email;
            studentOrganization.Students = AssignedStudents;

            _context.Entry(studentOrganization).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        #endregion


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
            string[] properties = { "Name", "Advisor", "President", "Email" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadStudentOrganizationData()
        {
            var studentOrganization = _context.StudentOrganizations.Find(StudentOrganizationId);
            if (studentOrganization is null)
            {
                Response = "StudentOrganization not found";
                return;
            }
            this.Name = studentOrganization.Name;
            this.Advisor = studentOrganization.Advisor;
            this.President = studentOrganization.President;
            this.Description = studentOrganization.Description;
            this.MeetingSchedule = studentOrganization.MeetingSchedule;
            this.Email = studentOrganization.Email;
            if (studentOrganization.Students is not null)
            {
                this.AssignedStudents =
                    new ObservableCollection<Student>(studentOrganization.Students);
            }
        }
    }
}
