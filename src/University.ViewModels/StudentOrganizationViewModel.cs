using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class StudentOrganizationViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        private bool? dialogResult;
        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                dialogResult = value;
            }
        }

        private ObservableCollection<StudentOrganization>? organizations;
        public ObservableCollection<StudentOrganization> Organizations
        {
            get
            {
                if (organizations is null)
                {
                    organizations = new ObservableCollection<StudentOrganization>();
                    return organizations;
                }
                return organizations;
            }
            set
            {
                organizations = value;
                OnPropertyChanged(nameof(organizations));
            }
        }

        private ICommand? _add;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewOrganization);
                }
                return _add;
            }
        }

        private void AddNewOrganization(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.SubjectsSubView = new AddStudentOrganizationViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditOrganization);
                }
                return _edit;
            }
        }

        private void EditOrganization(object? obj)
        {
            // Add logic for editing an organization
        }

        private ICommand? _remove;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveOrganization);
                }
                return _remove;
            }
        }

        private void RemoveOrganization(object? obj)
        {
            // Add logic for removing an organization
        }

        public StudentOrganizationViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.StudentOrganizations.Load();
            Organizations = _context.StudentOrganizations.Local.ToObservableCollection();
        }
    }
}
