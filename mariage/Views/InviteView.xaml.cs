using System.Windows.Controls;
using Mariage.ViewModels;

namespace mariage.Views;

public partial class InviteView : UserControl
{
    public InviteView()
    {
        InitializeComponent();
        this.DataContext = new InviteViewModel();
    }

}