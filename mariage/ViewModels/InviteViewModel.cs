using System;
using mariage.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Mariage.Commands;
using Newtonsoft.Json;

namespace Mariage.ViewModels
{
    public class InviteViewModel : INotifyPropertyChanged // Implement INotifyPropertyChanged
    {
       private readonly string _baseUri = "http://localhost:5191/api/";
        public ObservableCollection<Invite> Invites { get; } = new ObservableCollection<Invite>();
        
        public Invite NewInvite { get; set; }

        public ICommand AddInviteCommand { get; private set; }
        public ICommand BeginEditInviteCommand { get; private set; }
        public ICommand CommitUpdateInviteCommand { get; private set; }
        public ICommand DeleteInviteCommand { get; private set; }
        // Implement the interface member for INotifyPropertyChanged
        

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Invite _selectedInvite;
        public Invite SelectedInvite
        {
            get { return _selectedInvite; }
            set
            {
                if (_selectedInvite != value)
                {
                    _selectedInvite = value;
                    OnPropertyChanged(nameof(SelectedInvite));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public InviteViewModel()
        {
            NewInvite = new Invite();
            AddInviteCommand = new RelayCommand(AddInviteExecute, AddInviteCanExecute);

            BeginEditInviteCommand = new RelayCommand(BeginEditInvite);
            CommitUpdateInviteCommand = new RelayCommand(CommitUpdateInvite, CanUpdateInvite);
            DeleteInviteCommand = new RelayCommand(DeleteInvite, CanDeleteInvite);

            LoadInvites().ConfigureAwait(false);
        }
        private async void AddInviteExecute(object parameter)
        {
            if (NewInvite != null)
            {
                using (var httpClient = new HttpClient())
                {
                    // Configurez httpClient si nécessaire, par exemple en ajoutant des en-têtes
                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "votre_token");

                    var json = JsonConvert.SerializeObject(NewInvite);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    try
                    {
                        var response = await httpClient.PostAsync(_baseUri + "invites", content);
                        if (response.IsSuccessStatusCode)
                        {
                            // Si l'ajout est réussi, rechargez la liste des invités
                            await LoadInvites();
                            // Vous pouvez également effacer NewInvite ou notifier l'utilisateur de la réussite
                            NewInvite = new Invite(); // Réinitialiser pour le prochain ajout
                            OnPropertyChanged(nameof(NewInvite));
                        }
                        else
                        {
                            // Gérez ici les réponses d'erreur, comme l'affichage d'un message à l'utilisateur
                            var errorContent = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Erreur lors de l'ajout de l'invité: {errorContent}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Gérez ici les exceptions, comme l'affichage d'un message à l'utilisateur
                        MessageBox.Show($"Erreur lors de la connexion à l'API: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        private bool AddInviteCanExecute(object parameter)
        {
            // Valider les informations de NewInvite avant d'autoriser l'ajout
            return !string.IsNullOrEmpty(NewInvite.Nom) && !string.IsNullOrEmpty(NewInvite.Prenom) && NewInvite.MariageId > 0;
        }

        private void BeginEditInvite(object parameter)
        {
            if (parameter is Invite invite)
            {
                SelectedInvite = invite; // Set the selected invite for editing
            }
        }

        private async void CommitUpdateInvite(object parameter)
        {
            if (SelectedInvite != null)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(SelectedInvite);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"{_baseUri}invites/{SelectedInvite.Id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadInvites();
                        SelectedInvite = null; // Clear selection after update
                    }
                }
            }
        }


        private bool CanUpdateInvite(object parameter)
        {
            // Check if the parameter is an Invite and it exists in the Invites collection
            return parameter is Invite invite && Invites.Contains(invite);
        }

        private bool CanDeleteInvite(object parameter)
        {
            return parameter is Invite invite && Invites.Contains(invite);
        }

        private async Task LoadInvites()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_baseUri + "invites");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var invites = JsonConvert.DeserializeObject<ObservableCollection<Invite>>(json);
                    Invites.Clear();
                    foreach (var invite in invites)
                    {
                        Invites.Add(invite);
                    }
                }
            }
        }

        private async void AddInvite(object parameter)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(_baseUri + "invites", content);
                if (response.IsSuccessStatusCode)
                {
                    await LoadInvites();
                }
            }
        }

      

        private void DeleteInvite(object parameter)
        {
            if (parameter is Invite invite)
            {
                var deleteConfirmed = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer l'invité : {invite.Nom} {invite.Prenom} (ID: {invite.Id}) ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (deleteConfirmed == MessageBoxResult.Yes)
                {
                    PerformDeleteInvite(invite).ConfigureAwait(false);
                }
            }
        }

// Make this method private if it's only called here
        private async Task PerformDeleteInvite(Invite invite)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{_baseUri}invites/{invite.Id}");
                if (response.IsSuccessStatusCode)
                {
                    await LoadInvites();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression de l'invité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
