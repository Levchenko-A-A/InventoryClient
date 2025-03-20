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
    class DeviceViewModel: BaseViewModel
    {
        public string path = "http://193.104.57.148:8080/connection/";
        private HttpClient httpClient;

        public DeviceViewModel()
        {
            httpClient = new HttpClient();
            Load();
        }

        private void Load()
        {
            Devices = null;
            Task<ObservableCollection<Device>> task = Task.Run(() => getDevises());
            Devices = task.Result;
        }

        private ObservableCollection<Device>? devices;
        public ObservableCollection<Device>? Devices
        {
            get { return devices; }
            set
            {
                devices = value;
                OnPropertyChanged(nameof(devices));
            }
        }
        private Device? selectedDevice;
        public Device? SelectedDevice
        {
            get => selectedDevice;
            set
            {
                selectedDevice = value;
                OnPropertyChanged(nameof(selectedDevice));
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                {
                    DeviceWindow deviceWindow = new DeviceWindow(new Device());
                    if (deviceWindow.ShowDialog() == true)
                    {
                        await sendDevice(deviceWindow.device);
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
                    Device? device = selectedItem as Device;
                    if (device == null) return;
                    DeviceWindow deviceWindow = new DeviceWindow(device);
                    if (deviceWindow.ShowDialog() == true)
                    {
                        await updateDevice(deviceWindow.device);
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
                    Device? device = selectedItem as Device;
                    if (device == null) return;
                    if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        await delDevice(device.Deviceid);
                    }
                }));
            }
        }
        private async Task<ObservableCollection<Device>> getDevises()
        {
            try
            {
                StringContent content = new StringContent("getLocation");
                using var request = new HttpRequestMessage(HttpMethod.Get, path);
                request.Headers.Add("table", "device");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Device> devices = JsonSerializer.Deserialize<List<Device>>(responseText)!;
                return new ObservableCollection<Device>(devices);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Device>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Device>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Device>();
            }
        }

        private async Task sendDevice(Device device)
        {
            try
            {
                JsonContent content = JsonContent.Create(device);
                var request = new HttpRequestMessage(HttpMethod.Post, path);
                request.Content = content;
                request.Headers.Add("table", "device");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Устройство с таким именем существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Устройство добавлено");
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
        public async Task delDevice(int deviceId)
        {
            try
            {
                JsonContent content = JsonContent.Create(deviceId);
                var request = new HttpRequestMessage(HttpMethod.Delete, path);
                request.Content = content;
                request.Headers.Add("table", "device");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Устройство с таким именем не существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Устройство удалено");
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
        private async Task updateDevice(Device device)
        {
            try
            {
                JsonContent content = JsonContent.Create(device);
                var request = new HttpRequestMessage(HttpMethod.Put, path);
                request.Content = content;
                request.Headers.Add("table", "device");
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
