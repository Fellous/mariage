using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using mariage.Models;
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