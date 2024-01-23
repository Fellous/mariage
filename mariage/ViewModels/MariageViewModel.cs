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
    public class MariageViewModel : INotifyPropertyChanged
    {
        private readonly string _baseUri = "http://localhost:5191/api/";
        public ObservableCollection<Mariages> Mariages { get; } = new ObservableCollection<Mariages>();

        public ICommand AddMariageCommand { get; private set; }
        public ICommand UpdateMariageCommand { get; private set; }
        public ICommand DeleteMariageCommand { get; private set; }

        private Mariages _selectedMariage;
        public Mariages SelectedMariage
        {
            get { return _selectedMariage; }
            set
            {
                if (_selectedMariage != value)
                {
                    _selectedMariage = value;
                    OnPropertyChanged(nameof(SelectedMariage));
                }
            }
        }

        public MariageViewModel()
        {
            AddMariageCommand = new RelayCommand(AddMariage);
            UpdateMariageCommand = new RelayCommand(UpdateMariage, CanExecuteMariageCommand);
            DeleteMariageCommand = new RelayCommand(DeleteMariage, CanExecuteMariageCommand);

            LoadMariages().ConfigureAwait(false);
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
            var mariage = parameter as Mariages;
            if (mariage != null)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(mariage);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(_baseUri + "mariages", content);
                    if (response.IsSuccessStatusCode)
                    {
                        await LoadMariages();
                    }
                }
            }
        }

        private async void UpdateMariage(object parameter)
        {
            var mariage = parameter as Mariages;
            if (mariage != null && mariage == SelectedMariage)
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
            var mariage = parameter as Mariages;
            if (mariage != null && mariage == SelectedMariage)
            {
                var deleteConfirmed = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le mariage : {mariage.Date} ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (deleteConfirmed == MessageBoxResult.Yes)
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

        private bool CanExecuteMariageCommand(object parameter)
        {
            return SelectedMariage != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
