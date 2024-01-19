using System.Windows;
using System.Windows.Controls;
using Mariage.ViewModels;

namespace Mariage
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        public void NavigateToUserControl(UserControl newContent)
        {
            // Supposons que vous avez un ContentControl dans MainWindow pour le contenu dynamique
            ContentArea.Content = newContent;
        }
    }
}