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
    class PersonViewModel: BaseViewModel
    {
        private HttpClient httpClient;

        public PersonViewModel()
        {
            httpClient = new HttpClient();
            Load();
        }

        private void Load()
        {
            Persons = null;
            Task<ObservableCollection<Person>> task = Task.Run(() => getPerson());
            Persons = task.Result;
        }

        private ObservableCollection<Person>? persons;
        public ObservableCollection<Person>? Persons
        {
            get { return persons; }
            set
            {
                persons = value;
                OnPropertyChanged(nameof(Person));
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

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                {
                    PersonWindow personWindow = new PersonWindow(new Person());
                    if (personWindow.ShowDialog() == true)
                    {
                        await SendClient(personWindow.Person);
                    }
                }));
            }
        }

        private async Task<ObservableCollection<Person>> getPerson()
        {
            try
            {
                StringContent content = new StringContent("getPerson");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:8888/connection/");
                request.Headers.Add("table", "person");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Person> clients = JsonSerializer.Deserialize<List<Person>>(responseText)!;
                return new ObservableCollection<Person>(clients);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Person>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Person>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Person>();
            }
        }
        private async Task SendClient(Person person)
        {
            try
            {
                JsonContent content = JsonContent.Create(person);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "person");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Пользователь с таким именем существует");
                else if (responseText == "OK") ;
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
