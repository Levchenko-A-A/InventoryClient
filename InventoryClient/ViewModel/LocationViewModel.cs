using InventoryClient.Model;
using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryClient.ViewModel
{
    class LocationViewModel: BaseViewModel
    {
        private HttpClient httpClient;

        public LocationViewModel()
        {
            httpClient = new HttpClient();
            Load();
        }

        private void Load()
        {
            Locations = null;
            Task<ObservableCollection<Location>> task = Task.Run(() => getLocation());
            Locations = task.Result;
        }

        private ObservableCollection<Location>? locations;
        public ObservableCollection<Location>? Locations
        {
            get { return locations; }
            set
            {
                locations = value;
                OnPropertyChanged(nameof(locations));
            }
        }
        private Location? selectedLocations;
        public Location? SelectedLocations
        {
            get => selectedLocations;
            set
            {
                selectedLocations = value;
                OnPropertyChanged(nameof(SelectedLocations));
            }
        }

        private RelayCommand addLocCommand;
        public RelayCommand AddLocCommand
        {
            get
            {
                return addLocCommand ?? (addLocCommand = new RelayCommand(async obj =>
                {
                    LocationWindow locationWindow = new LocationWindow(new Location());
                    if (locationWindow.ShowDialog() == true)
                    {
                        await sendLocation(locationWindow.location);
                    }
                }));
            }
        }
        private RelayCommand updateLocCommand;
        public RelayCommand UpdateLocCommand
        {
            get
            {
                return updateLocCommand ?? (updateLocCommand = new RelayCommand(async (selectedItem) =>
                {
                    Location? location = selectedItem as Location;
                    if (location == null) return;
                    LocationWindow locationWindow = new LocationWindow(location);
                    if (locationWindow.ShowDialog() == true)
                    {
                        await updateLocation(locationWindow.location);
                    }
                }));
            }
        }
        private RelayCommand deleteLocCommand;
        public RelayCommand DeleteLocCommand
        {
            get
            {
                return deleteLocCommand ?? (deleteLocCommand = new RelayCommand(async (selectedItem) =>
                {
                    Location? location = selectedItem as Location;
                    if (location == null) return;
                    if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        await delLocation(location.Locationid);
                    }
                }));
            }
        }
        private async Task<ObservableCollection<Location>> getLocation()
        {
            try
            {
                StringContent content = new StringContent("getLocationAll");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:8888/connection/");
                request.Headers.Add("table", "location");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Location> clients = JsonSerializer.Deserialize<List<Location>>(responseText)!;
                return new ObservableCollection<Location>(clients);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Location>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Location>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Location>();
            }
        }

        private async Task sendLocation(Location location)
        {
            try
            {
                JsonContent content = JsonContent.Create(location);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "location");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Локация с таким именем существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Локация добавлена");
                    Load();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка HTTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        public async Task delLocation(int locationId)
        {
            try
            {
                JsonContent content = JsonContent.Create(locationId);
                var request = new HttpRequestMessage(HttpMethod.Delete, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "location");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Локация с таким именем не существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Локация удалена");
                    Load();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка HTTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        public async Task updateLocation(Location location)
        {
            try
            {
                JsonContent content = JsonContent.Create(location);
                var request = new HttpRequestMessage(HttpMethod.Put, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "location");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseText);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка HTTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
