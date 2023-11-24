using mariage.Models;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mariage.Commands;
using Newtonsoft.Json;


namespace Mariage.ViewModels
{
    public class InviteViewModel
    {
        public ObservableCollection<Invite> Invites { get; } = new ObservableCollection<Invite>();

        public ICommand AddInviteCommand { get; private set; }
        public ICommand UpdateInviteCommand { get; private set; }
        public ICommand DeleteInviteCommand { get; private set; }

        public InviteViewModel()
        {
            AddInviteCommand = new RelayCommand(AddInvite);
            UpdateInviteCommand = new RelayCommand(UpdateInvite, CanUpdateInvite);
            DeleteInviteCommand = new RelayCommand(DeleteInvite, CanDeleteInvite);
            
            // Supposons que vous ayez une méthode pour charger vos invités
            LoadInvites();
        }

       

       

        private bool CanUpdateInvite(object parameter)
        {
            // Vérifiez si un invité est sélectionné pour être mis à jour
            return parameter is Invite invite && Invites.Contains(invite);
        }



        private bool CanDeleteInvite(object parameter)
        {
            // Vérifiez si un invité est sélectionné pour être supprimé
            return parameter is Invite invite && Invites.Contains(invite);
        }

        private async Task LoadInvites()
        {
            // Utiliser HttpClient pour charger les invités depuis l'API
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("api/invites");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var invites = JsonConvert.DeserializeObject<ObservableCollection<Invite>>(json);
                foreach (var invite in invites)
                {
                    Invites.Add(invite);
                }
            }
        }

        private async void AddInvite(object parameter)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(parameter);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/invites", content);
            if (response.IsSuccessStatusCode)
            {
                LoadInvites(); // Recharger la liste après l'ajout
            }
        }

        private async void UpdateInvite(object parameter)
        {
            if (parameter is Invite invite)
            {
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(invite);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"api/invites/{invite.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    LoadInvites(); // Recharger la liste après la mise à jour
                }
            }
        }

        private async void DeleteInvite(object parameter)
        {
            if (parameter is Invite invite)
            {
                var httpClient = new HttpClient();
                var response = await httpClient.DeleteAsync($"api/invites/{invite.Id}");
                if (response.IsSuccessStatusCode)
                {
                    LoadInvites(); // Recharger la liste après la suppression
                }
            }
        }

    }
}
