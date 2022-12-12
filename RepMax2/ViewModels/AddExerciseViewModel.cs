using Android.Hardware.Lights;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepMax2.Data;
using RepMax2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RepMax2.ViewModels
{
    public class AddExerciseViewModel : ObservableObject
    {
        public ValidatableObject<string> Name { get; private set; }
              

        private int? _weight;
        public int? Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        private int? _reps;
        public int? Reps
        {
            get => _reps;
            set => SetProperty(ref _reps, value);
        }

        private float _est1Rm = 0;
        public float Est1Rm
        {
            get => _est1Rm;
            set => SetProperty(ref _est1Rm, value);            
        }
        

        public ICommand SaveCommand;

        private readonly IRepository _repository;

        public AddExerciseViewModel(IRepository repository)
        {
            _repository = repository;

            SaveCommand = new AsyncRelayCommand(SaveExerciseSet);
        }


        private async Task SaveExerciseSet()
        {
            await _repository.InsertExerciseSet(Name, Weight.Value, Reps.Value);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.PropertyName)
            {
                case nameof(Reps):
                case nameof(Weight):
                    CalculateEst1Rm();
                    break;
            }
        }

        private void CalculateEst1Rm()
        {
            if (Weight.HasValue && Reps.HasValue)
            {
                var est = CalculatorService.Estimate1RM(Weight.Value, Reps.Value);
                SetProperty(ref _est1Rm, est, nameof(Est1Rm));
            }
        }
    }
}
