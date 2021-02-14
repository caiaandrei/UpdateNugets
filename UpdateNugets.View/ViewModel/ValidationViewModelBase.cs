using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UpdateNugets.UI.ViewModel
{
    public class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly IDictionary<string, List<ValidationResult>> _errors = new Dictionary<string, List<ValidationResult>>();

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && _errors.TryGetValue(propertyName, out List<ValidationResult> list))
            {
                return list;
            }

            return null;
        }

        public bool ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();

            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            Validator.TryValidateProperty(value, context, results);

            if (results.Count > 0)
            {
                _errors[propertyName] = results;
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

            return !_errors.ContainsKey(propertyName);
        }
    }
}
