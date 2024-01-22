using mariage.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mariage.Commands;
using Newtonsoft.Json;
using System;

namespace Mariage.ViewModels
{
    public class MariageViewModel
    {
        private readonly string _baseUri = "http://localhost:5191/api/"; // Remplacez par l'URL de votre API
        public ObservableCollection<Mariages> Mariages { get; } = new ObservableCollection<Mariages>();

        public ICommand AddMariageCommand { get; private set; }
        public ICommand UpdateMariageCommand { get; private set; }
        public ICommand DeleteMariageCommand { get; private set; }

        public MariageViewModel()
        {
            AddMariageCommand = new RelayCommand(AddMariage);
            UpdateMariageCommand = new RelayCommand(UpdateMariage, CanUpdateMariage);
            DeleteMariageCommand = new RelayCommand(DeleteMariage, CanDeleteMariage);

            LoadMariages().ConfigureAwait(false);
        }

        private bool CanUpdateMariage(object parameter)
        {
            return parameter is Mariages mariage && Mariages.Contains(mariage);
        }

        private bool CanDeleteMariage(object parameter)
        {
            return parameter is Mariages mariage && Mariages.Contains(mariage);
        }

        private async Task LoadMariages()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_baseUri + "mariages");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var mariages = JsonConvert.DeserializeObject<ObservableCollection<Mariages>>(json);
                    Mariages.Clear();
                    foreach (var mariage in mariages)
                    {
                        Mariages.Add(mariage);
                    }
                }
            }
        }

        private async void AddMariage(object parameter)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(parameter);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(_baseUri + "mariages", content);
                if (response.IsSuccessStatusCode)
                {
                    await LoadMariages();
                }
            }
        }

        private async void UpdateMariage(object parameter)
        {
            if (parameter is Mariages mariage)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(mariage);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"{_baseUri}mariages/{mariage.Id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadMariages();
                    }
                }
            }
        }

        private async void DeleteMariage(object parameter)
        {
            if (parameter is Mariages mariage)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync($"{_baseUri}mariages/{mariage.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadMariages();
                    }
                }
            }
        }
    }
}
