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
    public class AddStudentOrganizationViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

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
                // Add validation for other properties as needed
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

        // Add other properties as needed for StudentOrganization

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

        // Add other properties and collections as needed for Students

        private ICommand? _back = null;
        public ICommand Back => _back ??= new RelayCommand<object>(NavigateBack);

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                // Navigate back logic
            }
        }

        private ICommand? _add = null;
        public ICommand Add => _add ??= new RelayCommand<object>(AddStudent);

        private void AddStudent(object? obj)
        {
            // Add logic to add a student to the organization
        }

        private ICommand? _remove = null;
        public ICommand Remove => _remove ??= new RelayCommand<object>(RemoveStudent);

        private void RemoveStudent(object? obj)
        {
            // Add logic to remove a student from the organization
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

            // Add logic to save the StudentOrganization to the database
        }

        public AddStudentOrganizationViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        // Add other methods as needed

        private bool IsValid()
        {
            string[] properties = { "Name" };
            foreach (string property in properties)
            {
                if (string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
