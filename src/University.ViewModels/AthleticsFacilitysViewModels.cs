using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class AthleticsFacilitysViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        private ObservableCollection<AthleticsFacility>? _facilities = null;
        public ObservableCollection<AthleticsFacility>? Facilities
        {
            get
            {
                if (_facilities is null)
                {
                    _facilities = new ObservableCollection<AthleticsFacility>();
                    return _facilities;
                }
                return _facilities;
            }
            set
            {
                _facilities = value;
                OnPropertyChanged(nameof(Facilities));
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
                    _add = new RelayCommand<object>(AddNewFacility);
                }
                return _add;
            }
        }

        private void AddNewFacility(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.AthleticsFacilitySubView = new AddAthleticsFacilityViewModel(_context, _dialogService);
            };
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditFacility);
                }
                return _edit;
            }
        }

        private void EditFacility(object? obj)
        {
            if (obj is not null)
            {
                long facilityId = (long)obj;
                EditAthleticsFacilityViewModel editAthleticsFacilityViewModel = new EditAthleticsFacilityViewModel(_context, _dialogService)
                {
                    AthleticsFacilityId = facilityId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.AthleticsFacilitySubView = editAthleticsFacilityViewModel;
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
                    _remove = new RelayCommand<object>(RemoveFacility);
                }
                return _remove;
            }
        }

        private void RemoveFacility(object? obj)
        {
            if (obj is not null)
            {
                string facilityId = (string)obj;
                AthleticsFacility? facility = _context.AthleticsFacilities.Find(facilityId);
                if (facility is not null)
                {
                    DialogResult = _dialogService.Show(facility.Name + " " + facility.Description);
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.AthleticsFacilities.Remove(facility);
                    _context.SaveChanges();
                }
            }
        }

        public AthleticsFacilitysViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.AthleticsFacilities.Load();
            Facilities = _context.AthleticsFacilities.Local.ToObservableCollection();
        }
    }
}
