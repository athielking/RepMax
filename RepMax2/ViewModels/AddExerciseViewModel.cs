using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using RepMax2.Data;
using RepMax2.Services;
using RepMax2.Validation;
using RepMax2.Validation.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public ICommand SaveCommand;

        private readonly IRepository _repository;

        public AddExerciseViewModel(IRepository repository) : base()
        {
            _repository = repository;

            Name = new ValidatableObject<string>();
            Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Exercise Name is Required" });
            
            SaveCommand = new AsyncRelayCommand(SaveExerciseSetAsync);
        }


        private async Task SaveExerciseSetAsync()
        {
            await _repository.InsertExerciseSet(Name.Value, Weight.Value, Reps.Value);

            //await IsBusyFor(async () =>
            //{
            //    IsValid = true;
            //    var isValid = Validate();
            //    if (isValid)
            //    {
            //        try
            //        {
            //            await _repository.InsertExerciseSet(Name.Value, Weight.Value, Reps.Value);
            //        }
            //        catch(Exception ex)
            //        {
            //            Debug.WriteLine($"[{nameof(SaveExerciseSetAsync)}] Error saving exercise set: {ex}");
            //        }
            //    }
            //    else
            //    {
            //        IsValid = false;
            //    }
            //});                       
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

        private bool Validate()
        {
            return Name.Validate();
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
