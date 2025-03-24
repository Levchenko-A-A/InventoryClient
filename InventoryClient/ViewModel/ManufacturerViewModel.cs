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
            StatusButtom();
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
                        await sendManuf(manufacturerWindow.Manufacturer);
                    }
                }));
            }
        }
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ?? (updateCommand = new RelayCommand(async (selectedItem) =>
                {
                    Manufacturer? manuf = selectedItem as Manufacturer;
                    if (manuf == null) return;
                    ManufacturerWindow manufacturerWindow = new ManufacturerWindow(manuf);
                    if (manufacturerWindow.ShowDialog() == true)
                    {
                        MessageBox.Show(manufacturerWindow.Manufacturer.Description);
                        await updateManuf(manufacturerWindow.Manufacturer);
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
                        await delManuf(manufacturer.Manufacturerid);
                    }
                }));
            }
        }
        private bool butAddIsE;
        public bool ButAddIsE
        {
            get { return butAddIsE; }
            set
            {
                butAddIsE = value;
                OnPropertyChanged(nameof(ButAddIsE));
            }
        }
        private bool butUpdateIsE;
        public bool ButUpdateIsE
        {
            get { return butUpdateIsE; }
            set
            {
                butUpdateIsE = value;
                OnPropertyChanged(nameof(ButUpdateIsE));
            }
        }
        private bool butDeleteIsE;
        public bool ButDeleteIsE
        {
            get { return butDeleteIsE; }
            set
            {
                butDeleteIsE = value;
                OnPropertyChanged(nameof(ButDeleteIsE));
            }
        }
        private async Task<ObservableCollection<Manufacturer>> getManufacturer()
        {
            try
            {
                StringContent content = new StringContent("getManufacturerAll");
                using var request = new HttpRequestMessage(HttpMethod.Get, ServerPath.Path);
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

        private async Task sendManuf(Manufacturer manufacturer)
        {
            try
            {
                JsonContent content = JsonContent.Create(manufacturer);
                var request = new HttpRequestMessage(HttpMethod.Post, ServerPath.Path);
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
        public async Task delManuf(int manufId)
        {
            try
            {
                JsonContent content = JsonContent.Create(manufId);
                var request = new HttpRequestMessage(HttpMethod.Delete, ServerPath.Path);
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
        public async Task updateManuf(Manufacturer manuf)
        {
            try
            {
                JsonContent content = JsonContent.Create(manuf);
                var request = new HttpRequestMessage(HttpMethod.Put, ServerPath.Path);
                request.Content = content;
                request.Headers.Add("table", "manufacturer");
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
        private void StatusButtom()
        {
            if (RegisterUser.UserAllId != null)
            {
                if (RegisterUser.UserAllId.Any(p => p.Roleid == 1) == true)
                {
                    ButAddIsE = true;
                    ButUpdateIsE = true;
                    ButDeleteIsE = true;
                }
                else if (RegisterUser.UserAllId.Any(p => p.Roleid == 2) == true)
                {
                    ButAddIsE = true;
                    ButUpdateIsE = true;
                    ButDeleteIsE = true;
                }
                else if (RegisterUser.UserAllId.Any(p => p.Roleid == 3) == true)
                {
                    ButAddIsE = true;
                    ButUpdateIsE = true;
                    ButDeleteIsE = false;
                }
                else if (RegisterUser.UserAllId.Any(p => p.Roleid == 4) == true)
                {
                    ButAddIsE = false;
                    ButUpdateIsE = false;
                    ButDeleteIsE = false;
                }
            }
            else
            {
                ButAddIsE = false;
                ButUpdateIsE = false;
                ButDeleteIsE = false;
            }
        }
    }
}
