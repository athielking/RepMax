using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using RepMax2.Data;
using RepMax2.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepMax2.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        public ObservableCollection<ExerciseSet> Exercises { get; private set; }

        public ICommand NavigateCommand { get; set; }

        private readonly IRepository _repository;
        public MainPageViewModel(IRepository repository)
        {
            _repository = repository;

            NavigateCommand = new AsyncRelayCommand(() => Shell.Current.GoToAsync("AddExercise"));  

            Exercises = new ObservableCollection<ExerciseSet>( new List<ExerciseSet>() {
                new ExerciseSet
                {
                    Id = 1,
                    Name = "Deadlift",
                    Est1Rm = 400,
                    Reps = 5,
                    Weight = 365,
                    UseForPercentages = true,
                    CreatedTs = DateTimeOffset.UtcNow
                }
            });
        }

        public async Task LoadData()
        {
            await _repository.EnsureDbVersion();
            var exercises = await _repository.GetAllExerciseMaximums();

            Exercises.Clear();
            foreach (var e in exercises)
                Exercises.Add(e);
        }
    }
}
