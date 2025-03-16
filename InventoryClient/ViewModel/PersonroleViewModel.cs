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
using System.Windows.Input;

namespace InventoryClient.ViewModel
{
    class PersonroleViewModel: BaseViewModel
    {
        private HttpClient httpClient;
        public ICommand AddPersonRoleCommand { get; }
        public ICommand UpdatePersonRoleCommand { get; }
        public ICommand DeletePersonRoleCommand { get; }

        public PersonroleViewModel()
        {
            httpClient = new HttpClient();
            Load();
            AddPersonRoleCommand = new RelayCommand(OnAddPersonRoleClick, CanExecuteOnAddPersonRoleClick);
            UpdatePersonRoleCommand = new RelayCommand(OnUpdatePersonRoleClick, CanExecuteUpdatePersonRoleClick);
            DeletePersonRoleCommand = new RelayCommand(OnDeletePersonRoleClick, CanExecuteDeletePersonRoleClick);
        }
        private void Load()
        {
            PersonRoles = null;
            Task<ObservableCollection<Personrole>> task = Task.Run(() => getPersonRole());
            PersonRoles = task.Result;
            
        }
        private ObservableCollection<Personrole>? personRoles;
        public ObservableCollection<Personrole>? PersonRoles
        {
            get { return personRoles; }
            set
            {
                personRoles = value;
                OnPropertyChanged(nameof(PersonRoles));
            }
        }
        private Personrole? selectedPersonRole;
        public Personrole? SelectedPersonRole
        {
            get => selectedPersonRole;
            set
            {
                selectedPersonRole = value;
                OnPropertyChanged(nameof(SelectedPersonRole));
            }
        }

        //private RelayCommand addPersonRoleCommand;
        //public RelayCommand AddPersonRoleCommand
        //{
        //    get
        //    {
        //        return addPersonRoleCommand ?? (addPersonRoleCommand = new RelayCommand(async obj =>
        //        {
        //            RoleAddUpdateWindow roleAddUpdateWindow = new RoleAddUpdateWindow(new Role());
        //            if (roleAddUpdateWindow.ShowDialog() == true)
        //            {
        //                await sendPersonRole(roleAddUpdateWindow.Role);
        //            }
        //        }));
        //    }
        //}
        //private RelayCommand deletePersonRoleCommand;
        //public RelayCommand DeletePersonRoleCommand
        //{
        //    get
        //    {
        //        return deletePersonRoleCommand ?? (deletePersonRoleCommand = new RelayCommand(async (selectedItem) =>
        //        {
        //            Personrole? role = selectedItem as Personrole;
        //            if (role == null) return;
        //            if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
        //            {
        //                await delPersonRole(role.Userroleid);
        //            }
        //        }));
        //    }
        //}
        //private RelayCommand updatePersonRoleCommand;
        //public RelayCommand UpdatePersonRoleCommand
        //{
        //    get
        //    {
        //        return updatePersonRoleCommand ?? (updatePersonRoleCommand = new RelayCommand(async (selectedItem) =>
        //        {
        //            Personrole? role = selectedItem as Personrole;
        //            if (role == null) return;
        //            RoleAddUpdateWindow roleAddUpdateWindow = new RoleAddUpdateWindow(role);
        //            if (roleAddUpdateWindow.ShowDialog() == true)
        //            {
        //                MessageBox.Show(roleAddUpdateWindow.Role.Description);
        //                await updatePersonRole(roleAddUpdateWindow.Role);
        //            }
        //        }));
        //    }
        //}
        private async Task<ObservableCollection<Personrole>> getPersonRole()
        {
            try
            {
                StringContent content = new StringContent("getPersonRole");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:8888/connection/");
                request.Headers.Add("table", "personrole");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Personrole> roles = JsonSerializer.Deserialize<List<Personrole>>(responseText)!;
                return new ObservableCollection<Personrole>(roles);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Personrole>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Personrole>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Personrole>();
            }
        }
        private async Task sendPersonRole(Personrole role)
        {
            try
            {
                JsonContent content = JsonContent.Create(role);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "personrole");
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
        public async Task delPersonRole(int personroleId)
        {
            try
            {
                JsonContent content = JsonContent.Create(personroleId);
                var request = new HttpRequestMessage(HttpMethod.Delete, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "personrole");
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
        public async Task updatePersonRole(Personrole personrole)
        {
            try
            {
                JsonContent content = JsonContent.Create(personrole);
                var request = new HttpRequestMessage(HttpMethod.Put, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "personrole");
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
        private async void OnAddPersonRoleClick(object parameter)
        {
            //RoleAddUpdateWindow roleAddUpdateWindow = new RoleAddUpdateWindow(new Role());
            //if (roleAddUpdateWindow.ShowDialog() == true)
            //{
            //    await sendPersonRole(roleAddUpdateWindow.Role);
            //}
        }

        private bool CanExecuteOnAddPersonRoleClick(object parametr)
        {
            return false;
        }
        private async void OnUpdatePersonRoleClick(object parameter)
        {
        }

        private bool CanExecuteUpdatePersonRoleClick(object parametr)
        {
            return false;
        }
        private async void OnDeletePersonRoleClick(object parameter)
        {
        }

        private bool CanExecuteDeletePersonRoleClick(object parametr)
        {
            return false;
        }
    }
}
