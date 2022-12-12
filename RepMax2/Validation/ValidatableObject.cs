using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMax2.Validation
{
    public class ValidatableObject<T>: ObservableObject, IValidity
    {
        public List<IValidationRule<T>> Validations { get; } = new();

        private IEnumerable<string> _errors;
        public IEnumerable<string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        private T _value;
        public T Value
        {
            get => _value;
            set => SetProperty<T>(ref _value, value);
        }

        public ValidatableObject()
        {
            _isValid = true;
            _errors = Enumerable.Empty<string>();
        }

        public bool Validate()
        {
            Errors = Validations
                ?.Where(v => !v.Check(Value))
                ?.Select(v => v.ValidationMessage)
                ?.ToArray()
                ?? Enumerable.Empty<string>();

            IsValid = !Errors.Any();
            return IsValid;
        }
    }
}
