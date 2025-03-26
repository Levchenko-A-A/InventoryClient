using InventoryClient.Model;
using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InventoryClient.ViewModel
{
    class PersonViewModel : BaseViewModel
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

        private int GetRoleIdByCheckbox(Person person) //новое
        {
            if (person.IsAdmin) return 1; 
            if (person.IsManager) return 2; 
            if (person.IsUser) return 3; 
            if (person.IsGuest) return 4; 
            return -1; 
        }
        private async Task InsertIntoPersonRoles(int personId, int roleId)//новое
        {
            try
            {
                Personrole personrole = new Personrole
                {
                    Personid = personId,
                    Roleid = roleId
                };
                JsonContent content = JsonContent.Create(personrole);
                var request = new HttpRequestMessage(HttpMethod.Post, ServerPath.Path);
                request.Content = content;
                request.Headers.Add("table", "personrole");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "OK")
                {
                    MessageBox.Show("Связь успешно создана.");
                }
                else
                {
                    MessageBox.Show("Ошибка при создании связи.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании связи: {ex.Message}");
            }
        }
        private async Task<bool> CheckIfLoginExists(string login)// новое
        {
            try
            {
                StringContent content = new StringContent(login);
                using var request = new HttpRequestMessage(HttpMethod.Get, ServerPath.Path);
                request.Headers.Add("table", "person"); // Используем таблицу 'person' для проверки
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                // Предположим, что сервер возвращает количество записей с данным логином
                int count = int.Parse(responseText);
                return count > 0; // Если больше нуля, значит логин занят
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке логина: {ex.Message}");
                return false;
            }
        }

        private RelayCommand addPersonCommand;
        public RelayCommand AddPersonCommand
        {
            
            get
            {
                return addPersonCommand ?? (addPersonCommand = new RelayCommand(async obj =>
                {
                    PersonWindow personWindow = new PersonWindow(new Person());
                    //if (personWindow.ShowDialog() == true)
                    //    if (personWindow.Person.IsAdmin)
                    //    {
                    //        await sendPerson(personWindow.Person);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Роль не соответствует требованиям.");
                    //    }
                    if (personWindow.ShowDialog() == true)
                    {
                        Person person = personWindow.Person;
                        bool isLoginExists = await CheckIfLoginExists(person.Personname);
                        if (!isLoginExists)
                        {
                            // Добавляем логин
                           // bool succes = await sendPerson(person);
                            //if(succes)
                            {
                                // Получение ID роли
                                int roleId = GetRoleIdByCheckbox(person);

                                // Вставка связи в таблицу personroles
                                await InsertIntoPersonRoles(person.Personid, roleId);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Логин уже существует.");
                    }
                    if (personWindow.Person.IsManager)
                    {
                        await sendPerson(personWindow.Person);
                    }
                    else
                    {
                        MessageBox.Show("Роль не соответствует требованиям.");
                    }
                    if (personWindow.Person.IsUser)
                    {
                        await sendPerson(personWindow.Person);
                    }
                    else
                    {
                        MessageBox.Show("Роль не соответствует требованиям.");
                    }
                    if (personWindow.Person.IsGuest)
                    {
                        await sendPerson(personWindow.Person);
                    }
                    else
                    {
                        MessageBox.Show("Роль не соответствует требованиям.");
                    }
                }));
            }
        }

        private RelayCommand deletePersonCommand;
        public RelayCommand DeletePersonCommand
        {
            get
            {
                return deletePersonCommand ?? (deletePersonCommand = new RelayCommand(async (selectedItem) =>
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
        //private RelayCommand updateCommand;
        //public RelayCommand UpdateCommand
        //{
        //    get
        //    {
        //        return usersCommand ?? (usersCommand = new RelayCommand(async obj =>
        //        {
        //            Person? person = selectedPerson as Person;
        //            if (person == null) return;
        //            PersonWindow personWindow = new PersonWindow(person);
        //            if (personWindow.ShowDialog() == true)
        //            {
        //                await updateClient(personWindow.Person);
        //            }
        //        }));
        //    }
        //}
        private RelayCommand usersCommand;
        public RelayCommand UsersCommand
        {
            get
            {
                return usersCommand ?? (usersCommand = new RelayCommand(obj =>
                {
                    RoleWindow roleWindow = new RoleWindow();
                    roleWindow.Show();
                }));
            }
        }

        private async Task<ObservableCollection<Person>> getPerson()
        {
            try
            {
                StringContent content = new StringContent("getPersonAll");
                using var request = new HttpRequestMessage(HttpMethod.Get, ServerPath.Path);
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
        private async Task sendPerson(Person person)
        {
            try
            {
                JsonContent content = JsonContent.Create(person);
                var request = new HttpRequestMessage(HttpMethod.Post, ServerPath.Path);
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
                var request = new HttpRequestMessage(HttpMethod.Delete, ServerPath.Path);
                request.Content = content;
                request.Headers.Add("table", "person");
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
    }
}

