using System.ComponentModel;
using System.Linq.Expressions;


namespace Sinfonia.ViewModels
{
    /// <summary>
    /// An abstract class meant to be overriden by all viewmodels. Exposes the <see cref="GetValue{T}(Expression{Func{T}})"/> and <see cref="SetValue{T}(Expression{Func{T}}, T)"/> methods to simplify the property changed notifications.
    /// </summary>
    public abstract class PropertyChangedViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _values = [];


        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue(propertyName, value);
        }
        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value!;

            NotifyPropertyChanged(propertyName);
        }


        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }
        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            if (!_values.TryGetValue(propertyName, out object? value))
            {
                value = default(T);

                _values.Add(propertyName, value!);
            }

            return (T)value!;
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;

            if (handler != null)
            {
                PropertyChangedEventArgs e = new(propertyName);
                handler(this, e);
            }
        }
        private string GetPropertyName(LambdaExpression expression)
        {
            return expression.Body is not MemberExpression memberExpression
                ? throw new InvalidOperationException()
                : memberExpression.Member.Name;
        }
    }
}
