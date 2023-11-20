using System;
using System.Windows.Input;
using Mariage.Commands;

namespace Mariage.ViewModels
{
    public class LoginViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public event Action? SuccessfulLogin;
        
        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(PerformLogin);
        }

        private void PerformLogin(object parameter)
        {
            // Suppose login credentials are verified...
            
            // Trigger the SuccessfulLogin event
            SuccessfulLogin?.Invoke();
        }
    }
}