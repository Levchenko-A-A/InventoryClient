using InventoryClient.Model;
using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
        private Person? selectedPerson;
        public Person? SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        //private RelayCommand addCommand;
        //public RelayCommand AddCommand
        //{
        //    get
        //    {
        //        return addCommand ?? (addCommand = new RelayCommand(async obj =>
        //        {
        //            PersonWindow personWindow = new PersonWindow(new Person());
        //            if (personWindow.ShowDialog() == true)
        //            {
        //                await sendClient(personWindow.Person);
        //            }
        //        }));
        //    }
        //}

        //private RelayCommand deleteCommand;
        //public RelayCommand DeleteCommand
        //{
        //    get
        //    {
        //        return deleteCommand ?? (deleteCommand = new RelayCommand(async (selectedItem) =>
        //        {
        //            Person? client = selectedItem as Person;
        //            if (client == null) return;
        //            if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
        //            {
        //                await delPerson(client.Personid);
        //            }
        //        }));
        //    }
        //}
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
    }
}
