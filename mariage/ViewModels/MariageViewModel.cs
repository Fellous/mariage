using mariage.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mariage.Commands;
using Newtonsoft.Json;

namespace Mariage.ViewModels
{
    public class MariageViewModel
    {
        public ObservableCollection<mariage.Models.Mariages> Mariages { get; } = new ObservableCollection<mariage.Models.Mariages>();

        public ICommand AddMariageCommand { get; private set; }
        public ICommand UpdateMariageCommand { get; private set; }
        public ICommand DeleteMariageCommand { get; private set; }

        public MariageViewModel()
        {
            AddMariageCommand = new RelayCommand(AddMariage);
            UpdateMariageCommand = new RelayCommand(UpdateMariage, CanUpdateMariage);
            DeleteMariageCommand = new RelayCommand(DeleteMariage, CanDeleteMariage);

            LoadMariages();
        }

        private bool CanUpdateMariage(object parameter)
        {
            // Vérifiez si un mariage est sélectionné pour être mis à jour
            return parameter is mariage.Models.Mariages mariage && Mariages.Contains(mariage);
        }

        private bool CanDeleteMariage(object parameter)
        {
            // Vérifiez si un mariage est sélectionné pour être supprimé
            return parameter is mariage.Models.Mariages mariage && Mariages.Contains(mariage);
        }

        private async Task LoadMariages()
        {
            // Utiliser HttpClient pour charger les mariages depuis l'API
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("api/mariages");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var mariages = JsonConvert.DeserializeObject<ObservableCollection<Mariages>>(json);
                foreach (var mariage in mariages)
                {
                    Mariages.Add(mariage);
                }
            }
        }

        private async void AddMariage(object parameter)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(parameter);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("api/mariages", content);
            if (response.IsSuccessStatusCode)
            {
                LoadMariages(); // Recharger la liste après l'ajout
            }
        }

        private async void UpdateMariage(object parameter)
        {
            if (parameter is mariage.Models.Mariages mariage)
            {
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(mariage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"api/mariages/{mariage.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    LoadMariages(); // Recharger la liste après la mise à jour
                }
            }
        }

        private async void DeleteMariage(object parameter)
        {
            if (parameter is mariage.Models.Mariages mariage)
            {
                var httpClient = new HttpClient();
                var response = await httpClient.DeleteAsync($"api/mariages/{mariage.Id}");
                if (response.IsSuccessStatusCode)
                {
                    LoadMariages(); // Recharger la liste après la suppression
                }
            }
        }
    }
}
