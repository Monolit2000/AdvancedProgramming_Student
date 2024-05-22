using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class ExamViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;


        private ObservableCollection<Exam>? _exams = null;
        public ObservableCollection<Exam>? Exams
        {
            get
            {
                if (_exams is null)
                {
                    _exams = new ObservableCollection<Exam>();
                    return _exams;
                }
                return _exams;
            }
            set
            {
                _exams = value;
                OnPropertyChanged(nameof(Exams));
            }
        }

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

        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewExam);
                }
                return _add;
            }
        }

        private void AddNewExam(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.ExamSubView = new AddExamViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditExam);
                }
                return _edit;
            }
        }

        private void EditExam(object? obj)
        {
            if (obj is not null)
            {
                int examId = (int)obj;
                EditExamViewModel editExamViewModel = new EditExamViewModel(_context, _dialogService)
                {
                    ExamId = examId
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.ExamSubView = editExamViewModel;
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
                    _remove = new RelayCommand<object>(RemoveExam);
                }
                return _remove;
            }
        }

        private void RemoveExam(object? obj)
        {
            if (obj is not null)
            {
                long examId = (long)obj;
                Exam? exam = _context.Exams.Find(examId);
                if (exam is not null)
                {
                    DialogResult = _dialogService.Show(exam.Description);
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Exams.Remove(exam);
                    _context.SaveChanges();
                }
            }
        }

        public ExamViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Exams.Load();
            Exams = _context.Exams.Local.ToObservableCollection();
        }
    }
}