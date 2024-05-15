using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class ResearchProjectViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        private ObservableCollection<ResearchProject>? _projects = null;
        public ObservableCollection<ResearchProject>? Projects
        {
            get
            {
                if (_projects is null)
                {
                    _projects = new ObservableCollection<ResearchProject>();
                    return _projects;
                }
                return _projects;
            }
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
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
                    _add = new RelayCommand<object>(AddNewProject);
                }
                return _add;
            }
        }

        private void AddNewProject(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.ResearchProjectSubView = new AddResearchProjectViewModel(_context, _dialogService);
            };
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditProject);
                }
                return _edit;
            }
        }

        private void EditProject(object? obj)
        {
            if (obj is not null)
            {
                long researchProjectId = (long)obj;
                EditResearchProjectViewModel editResearchProjectViewModel = new EditResearchProjectViewModel(_context, _dialogService)
                {
                    ResearchProjectId = researchProjectId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.ResearchProjectSubView = editResearchProjectViewModel;
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
                    _remove = new RelayCommand<object>(RemoveProject);
                }
                return _remove;
            }
        }

        private void RemoveProject(object? obj)
        {
            if (obj is not null)
            {
                long researchProjectId = (long)obj;
                ResearchProject? researchProject = _context.ResearchProjects.Find(researchProjectId);
                if (researchProject is not null)
                {
                    DialogResult = _dialogService.Show(researchProject.Title + " " + researchProject.Description);
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.ResearchProjects.Remove(researchProject);
                    _context.SaveChanges();
                }
            }
        }

        public ResearchProjectViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
             
            _context.Database.EnsureCreated();
            _context.ResearchProjects.Load();
            Projects = _context.ResearchProjects.Local.ToObservableCollection();
        }
    }
}