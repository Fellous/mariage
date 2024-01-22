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
        var viewModel = new InviteViewModel();
        this.DataContext = viewModel;
        viewModel.DeleteInviteRequested += ViewModel_DeleteInviteRequested;
    }

    private async void ViewModel_DeleteInviteRequested(Invite invite)
    {
        Console.WriteLine("PATATE");
        var messageBoxResult = MessageBox.Show(
            $"Êtes-vous sûr de vouloir supprimer l'invité : {invite.Nom} {invite.Prenom} (ID: {invite.Id}) ?",
            "Confirmation de suppression",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (messageBoxResult == MessageBoxResult.Yes)
        {
            // Appelez la méthode de suppression réelle ici
            await ((InviteViewModel)this.DataContext).PerformDeleteInvite(invite);
        }
    }
   



}