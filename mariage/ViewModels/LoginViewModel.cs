using System;
using System.Windows.Input;
using Mariage.Commands;
using System.Windows;
using Mariage.Views; // Ajoutez cette ligne pour accéder à la classe Application

namespace Mariage.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(PerformLogin);
        }

        private void PerformLogin(object parameter)
        {
            // Valider les identifiants de connexion ici...
            if (Username == "fellous" && Password == "95200")
            {
                // Si la validation réussit, ouvrir MainWindow et fermer la fenêtre de connexion actuelle.
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindow();
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();

                    // Fermer la fenêtre de connexion actuelle, qui devrait être la première fenêtre ouverte.
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is LoginView)
                        {
                            window.Close();
                            break;
                        }
                    }
                });
            }
            else
            {
                // Afficher un message d'erreur ou gérer l'échec de la connexion.
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}