using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PlaylistMaker.Wrappers
{
    public abstract class ValidationBindingBase : BindableBase, INotifyDataErrorInfo, IValidation
    {
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            Validate<T>(value, propertyName);
            return base.SetProperty(ref storage, value, propertyName);
        }
        protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            Validate<T>(value, propertyName);
            return base.SetProperty(ref storage, value, onChanged, propertyName);
        }
        
        public void Validate<T>(T value, string propertyName)
        {
            if (errorDictionary.ContainsKey(propertyName))
                errorDictionary.Remove(propertyName);

            var validationContext = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var validationResult = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResult);

            if (validationResult.Count > 0)
            {
                var errorList = validationResult.Select(v => v.ErrorMessage);
                errorDictionary.Add(propertyName, errorList);
            }
        }

        public bool HasErrors => errorDictionary.Any();
        public bool IsValid => !errorDictionary.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorChanged([CallerMemberName] string propertyName = null)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) &&
                errorDictionary.ContainsKey(propertyName) &&
                errorDictionary[propertyName].Count() > 0)
                return errorDictionary[propertyName];
            else
                return null;
        }

        private Dictionary<string, IEnumerable<string>> errorDictionary =
            new Dictionary<string, IEnumerable<string>>();
    }
}
