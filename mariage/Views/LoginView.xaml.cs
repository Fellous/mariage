using System.Windows;
using System.Windows.Controls;
using Mariage.ViewModels;

namespace Mariage.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        var viewModel = new LoginViewModel();

        // Subscribe to the SuccessfulLogin event
        viewModel.SuccessfulLogin += () =>
        {
            // Open the main window
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // Find and close the current login window
            Window.GetWindow(this)?.Close();
        };

        this.DataContext = viewModel;
    }
}
