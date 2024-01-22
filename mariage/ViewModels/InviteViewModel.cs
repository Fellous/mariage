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

        public ICommand AddInviteCommand { get; private set; }
        public ICommand UpdateInviteCommand { get; private set; }
        public ICommand DeleteInviteCommand { get; private set; }

        public InviteViewModel()
        {
            AddInviteCommand = new RelayCommand(AddInvite);
            UpdateInviteCommand = new RelayCommand(UpdateInvite, CanUpdateInvite);
            DeleteInviteCommand = new RelayCommand(DeleteInvite, CanDeleteInvite);
            
            LoadInvites().ConfigureAwait(false);
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
                    OnPropertyChanged(nameof(SelectedInvite)); // Notify the view of the change
                }
            }
        }
        

        // Implement the interface member for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void UpdateInvite(object parameter)
        {
            if (SelectedInvite != null && parameter is Invite updatedInvite)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(updatedInvite);
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
