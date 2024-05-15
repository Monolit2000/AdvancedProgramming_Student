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

        private ObservableCollection<StudentOrganization>? _organizations = null;
        public ObservableCollection<StudentOrganization>? Organizations
        {
            get
            {
                if (_organizations is null)
                {
                    _organizations = new ObservableCollection<StudentOrganization>();
                    return _organizations;
                }
                return _organizations;
            }
            set
            {
                _organizations = value;
                OnPropertyChanged(nameof(Organizations));
            }
        }

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
            }
        }

        private ICommand? _add = null;
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
                instance.StudentOrganizationSubView = new AddStudentOrganizationViewModel(_context, _dialogService);
            };
        }

        private ICommand? _edit = null;
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
            if (obj is not null)
            {
                long studentOrganizationId = (long)obj;
                EditStudentOrganizationViewModel editStudentOrganizationViewModel = new EditStudentOrganizationViewModel(_context, _dialogService)
                {
                    StudentOrganizationId = studentOrganizationId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.StudentOrganizationSubView = editStudentOrganizationViewModel;
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
                    _remove = new RelayCommand<object>(RemoveOrganization);
                }
                return _remove;
            }
        }

        private void RemoveOrganization(object? obj)
        {
            if (obj is not null)
            {
                long studentOrganizationId = (long)obj;
                StudentOrganization? studentOrganization = _context.StudentOrganizations.Find(studentOrganizationId);
                if (studentOrganization is not null)
                {
                    DialogResult = _dialogService.Show(studentOrganization.Name + " " + studentOrganization.Description);
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.StudentOrganizations.Remove(studentOrganization);
                    _context.SaveChanges();
                }
            }
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
