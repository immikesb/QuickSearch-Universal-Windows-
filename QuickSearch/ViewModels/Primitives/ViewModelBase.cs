using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;

namespace QuickSearch.ViewModels.Primitives
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        protected ViewModelBase()
        {

        }

        #region Methods

        /// <summary>
        /// Checks if a property already matches the desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="name">Name of the property used to notify listeners.</param>
        /// <returns>**true** if the value was changed, **false** if the existing value matched the
        /// desired value.</returns>
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string name = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                OnPropertyChanging(new PropertyChangingEventArgs(name));
                field = value;
                OnPropertyChanged(new PropertyChangedEventArgs(name));
                return true;
            }
            return false;
        }

        #endregion

        #region Methods

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChanging?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion

        #region Events

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc />
        public event PropertyChangingEventHandler? PropertyChanging;

        #endregion
    }
}
