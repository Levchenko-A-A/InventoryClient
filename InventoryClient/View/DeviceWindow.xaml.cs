using InventoryClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventoryClient.View
{
    /// <summary>
    /// Логика взаимодействия для DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        public Device device { get; set; }
        private HttpClient httpClient;
        public List<Category> Categories { get; set; } =new();

        public List<Manufacturer>? Manufacturers { get; set; }= new();//новое
        

        public DeviceWindow(Device dec)
        {
            InitializeComponent();
            httpClient = new HttpClient();
            device = dec;
            DataContext = device;
            Task<List<Category>> task = Task.Run(() => getLocation());
            Categories = task.Result;
            ComboBoxCategory.ItemsSource = Categories;

            Task<List<Manufacturer>> taskMan = Task.Run(() => getManufacturer());// новое
            Manufacturers = taskMan.Result;
            ComboBoxManufacturer.ItemsSource = Manufacturers;
        }
        private async Task<List<Category>> getLocation()
        {
            try
            {
                StringContent content = new StringContent("getCategoryAll");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://193.104.57.148:8080/connection/");
                request.Headers.Add("table", "category");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Category> clients = JsonSerializer.Deserialize<List<Category>>(responseText)!;
                return clients;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new List<Category>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new List<Category>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new List<Category>();
            }
        }

        private async Task<List<Manufacturer>> getManufacturer()//новое
        {
            try
            {
                StringContent content = new StringContent("getManufacturerAll");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://193.104.57.148:8080/connection/");
                request.Headers.Add("table", "manufacturer");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Manufacturer> clients = JsonSerializer.Deserialize<List<Manufacturer>>(responseText)!;
                return new List<Manufacturer>(clients);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new List<Manufacturer>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new List<Manufacturer>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new List<Manufacturer>();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
