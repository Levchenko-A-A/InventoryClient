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
                OnPropertyChanged(nameof(Persons));
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
                        await sendClient(personWindow.Person);
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
                    Person? client = selectedItem as Person;
                    if (client == null) return;
                    if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        await delPerson(client.Personid);
                    }
                }));
            }
        }
        private RelayCommand usersCommand;
        public RelayCommand UsersCommand
        {
            get
            {
                return usersCommand ?? (usersCommand = new RelayCommand(async obj =>
                {
                    RoleWindow roleWindow = new RoleWindow();
                    roleWindow.Show();
                    
                    //PersonWindow personWindow = new PersonWindow(new Person());
                    //if (personWindow.ShowDialog() == true)
                    //{
                    //    await sendClient(personWindow.Person);
                    //}
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
        private async Task sendClient(Person person)
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
                else if (responseText == "OK")
                {
                    MessageBox.Show("Пользователь добавлен");
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
                request.Headers.Add("table", "person");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Пользователь с таким именем не существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Пользователь удален");
                    //var personToRemove = Persons.FirstOrDefault(p => p.Personid == clientId);
                    //if (personToRemove != null)
                    //    Persons.Remove(personToRemove);
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
    }
}
