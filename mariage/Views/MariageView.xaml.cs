using System.Windows.Controls;
using Mariage.ViewModels;

namespace mariage.Views;

public partial class MariageView : UserControl
{
    public MariageView()
    {
        InitializeComponent();
        this.DataContext = new MariageViewModel();
    }
}