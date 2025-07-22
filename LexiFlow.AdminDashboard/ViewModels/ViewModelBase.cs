using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LexiFlow.AdminDashboard.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private bool _isLoading;
        private bool _isDisposed;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Flag indicating if the view model is loading data
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Success message
        /// </summary>
        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a property value and raises the property changed event if the value changed
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="storage">Property storage</param>
        /// <param name="value">New value</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>True if the value changed, false otherwise</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Loads the view model data
        /// </summary>
        /// <param name="parameter">Optional parameter</param>
        public virtual async Task LoadAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                await LoadDataAsync(parameter);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Loads the view model data
        /// </summary>
        /// <param name="parameter">Optional parameter</param>
        protected virtual Task LoadDataAsync(object? parameter = null)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates an async command
        /// </summary>
        /// <param name="execute">Execute action</param>
        /// <param name="canExecute">Can execute function</param>
        /// <returns>Command</returns>
        protected ICommand CreateCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
        {
            return new AsyncRelayCommand(execute, canExecute);
        }

        /// <summary>
        /// Creates a command
        /// </summary>
        /// <param name="execute">Execute action</param>
        /// <param name="canExecute">Can execute function</param>
        /// <returns>Command</returns>
        protected ICommand CreateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            return new RelayCommand(execute, canExecute);
        }

        /// <summary>
        /// Disposes the view model
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the view model
        /// </summary>
        /// <param name="disposing">True if disposing, false if finalizing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed resources
            }

            _isDisposed = true;
        }
    }

    /// <summary>
    /// Relay command implementation
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Can execute changed event
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Execute action</param>
        /// <param name="canExecute">Can execute function</param>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines if the command can execute
        /// </summary>
        /// <param name="parameter">Command parameter</param>
        /// <returns>True if the command can execute, false otherwise</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">Command parameter</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Raises the can execute changed event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Async relay command implementation
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _execute;
        private readonly Func<object?, bool>? _canExecute;
        private bool _isExecuting;

        /// <summary>
        /// Can execute changed event
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Execute function</param>
        /// <param name="canExecute">Can execute function</param>
        public AsyncRelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines if the command can execute
        /// </summary>
        /// <param name="parameter">Command parameter</param>
        /// <returns>True if the command can execute, false otherwise</returns>
        public bool CanExecute(object? parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute(parameter));
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">Command parameter</param>
        public async void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }

            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();

                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Raises the can execute changed event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}