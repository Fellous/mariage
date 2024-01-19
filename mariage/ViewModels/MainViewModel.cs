using System;
using System.Windows.Input;
using mariage.Views;
using Mariage.Commands;
using System.Windows; // Assurez-vous que vous avez cette directive using pour accéder à Application

namespace Mariage.ViewModels
{
    public class MainViewModel
    {
        public ICommand GoToInvitesCommand { get; private set; }
        public ICommand GoToMariagesCommand { get; private set; } // Ajout de la nouvelle commande

        public MainViewModel()
        {
            GoToInvitesCommand = new RelayCommand(ExecuteGoToInvitesCommand);
            GoToMariagesCommand = new RelayCommand(ExecuteGoToMariagesCommand); // Initialisation de la commande
        }

        private void ExecuteGoToInvitesCommand(object parameter)
        {
            // Logique de navigation pour les invités
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToUserControl(new InviteView());
        }

        private void ExecuteGoToMariagesCommand(object parameter)
        {
            // Logique de navigation pour les mariages
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavigateToUserControl(new MariageView()); // Assurez-vous que MariageView est bien votre UserControl pour les mariages
        }
        
        // ... autres méthodes et logiques
    }
}