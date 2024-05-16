using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class LibraryViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        private ObservableCollection<Library>? _libraries = null;
        public ObservableCollection<Library>? Libraries
        {
            get
            {
                if (_libraries is null)
                {
                    _libraries = new ObservableCollection<Library>();
                    return _libraries;
                }
                return _libraries;
            }
            set
            {
                _libraries = value;
                OnPropertyChanged(nameof(Libraries));
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
                    _add = new RelayCommand<object>(AddNewLibrary);
                }
                return _add;
            }
        }

        private void AddNewLibrary(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.LibrarySubView = new AddLibraryViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditLibrary);
                }
                return _edit;
            }
        }

        private void EditLibrary(object? obj)
        {
            if (obj is not null)
            {
                long libraryId = (long)obj;
                EditLibraryViewModel editLibraryViewModel = new EditLibraryViewModel(_context, _dialogService)
                {
                    LibraryId = libraryId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.LibrarySubView = editLibraryViewModel;
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
            if (obj is not null)
            {
                long libraryId = (long)obj;
                Library? library = _context.Librarys.Find(libraryId);
                if (library is not null)
                {
                    DialogResult = _dialogService.Show(library.Name + " " + library.Description);
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Librarys.Remove(library);
                    _context.SaveChanges();
                }
            }
        }

        public LibraryViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Librarys.Load();
            Libraries = _context.Librarys.Local.ToObservableCollection();
        }
    }
}