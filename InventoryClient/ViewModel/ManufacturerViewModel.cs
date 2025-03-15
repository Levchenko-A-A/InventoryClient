using InventoryClient.Model;
using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryClient.ViewModel
{
    class ManufacturerViewModel: BaseViewModel
    {
        private HttpClient httpClient;

        public ManufacturerViewModel()
        {
            httpClient = new HttpClient();
            Load();
        }

        private void Load()
        {
            Manufacturers = null;
            Task<ObservableCollection<Manufacturer>> task = Task.Run(() => getManufacturer());
            Manufacturers = task.Result;
        }

        private ObservableCollection<Manufacturer>? manufacturers;
        public ObservableCollection<Manufacturer>? Manufacturers
        {
            get { return manufacturers; }
            set
            {
                manufacturers = value;
                OnPropertyChanged(nameof(Manufacturers));
            }
        }
        private Manufacturer? selectedManufacturer;
        public Manufacturer? SelectedManufacturer
        {
            get => selectedManufacturer;
            set
            {
                selectedManufacturer = value;
                OnPropertyChanged(nameof(SelectedManufacturer));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                {
                    ManufacturerWindow manufacturerWindow = new ManufacturerWindow(new Manufacturer());
                    if (manufacturerWindow.ShowDialog() == true)
                    {
                        await sendManufacturer(manufacturerWindow.Manufacturer);
                    }
                }));
            }
        }
        private RelayCommand updateRoleCommand;
        public RelayCommand UpdateRoleCommand
        {
            get
            {
                return updateRoleCommand ?? (updateRoleCommand = new RelayCommand(async (selectedItem) =>
                {
                    Role? role = selectedItem as Role;
                    if (role == null) return;
                    RoleAddUpdateWindow roleAddUpdateWindow = new RoleAddUpdateWindow(role);
                    if (roleAddUpdateWindow.ShowDialog() == true)
                    {
                        MessageBox.Show(roleAddUpdateWindow.Role.Description);
                        await updateRole(roleAddUpdateWindow.Role);
                    }
                }));
            }
        }
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand(async (selectedItem) =>
                {
                    Manufacturer? manufacturer = selectedItem as Manufacturer;
                    if (manufacturer == null) return;
                    if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        await delPerson(manufacturer.Manufacturerid);
                    }
                }));
            }
        }
        private async Task<ObservableCollection<Manufacturer>> getManufacturer()
        {
            try
            {
                StringContent content = new StringContent("getManufacturer");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:8888/connection/");
                request.Headers.Add("table", "manufacturer");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Manufacturer> clients = JsonSerializer.Deserialize<List<Manufacturer>>(responseText)!;
                return new ObservableCollection<Manufacturer>(clients);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Manufacturer>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Manufacturer>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Manufacturer>();
            }
        }

        private async Task sendManufacturer(Manufacturer manufacturer)
        {
            try
            {
                JsonContent content = JsonContent.Create(manufacturer);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "manufacturer");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Производитель с таким именем существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Производитель добавлен");
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
        public async Task delPerson(int clientId)
        {
            try
            {
                JsonContent content = JsonContent.Create(clientId);
                var request = new HttpRequestMessage(HttpMethod.Delete, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "manufacturer");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Пользователь с таким именем не существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Пользователь удален");
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
        public async Task updateRole(Role role)
        {
            try
            {
                JsonContent content = JsonContent.Create(role);
                var request = new HttpRequestMessage(HttpMethod.Put, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "role");
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
