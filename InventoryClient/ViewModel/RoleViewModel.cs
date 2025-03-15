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
    class RoleViewModel: BaseViewModel
    {
        private HttpClient httpClient;

        public RoleViewModel()
        {
            httpClient = new HttpClient();
            Load();
        }

        private void Load()
        {
            Roles = null;
            Task<ObservableCollection<Role>> task = Task.Run(() => getRole());
            Roles = task.Result;
        }

        private ObservableCollection<Role>? roles;
        public ObservableCollection<Role>? Roles
        {
            get { return roles; }
            set
            {
                roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        private Role? selectedRole;
        public Role? SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        //private RelayCommand addRoleCommand;
        //public RelayCommand AddRoleCommand
        //{
        //    get
        //    {
        //        return addRoleCommand ?? (addRoleCommand = new RelayCommand(async obj =>
        //        {
        //            PersonWindow personWindow = new PersonWindow(new Role());
        //            if (personWindow.ShowDialog() == true)
        //            {
        //                await sendRole(personWindow.Role);
        //            }
        //        }));
        //    }
        //}

        private RelayCommand deleteRoleCommand;
        public RelayCommand DeleteRoleCommand
        {
            get
            {
                return deleteRoleCommand ?? (deleteRoleCommand = new RelayCommand(async (selectedItem) =>
                {
                    Role? role = selectedItem as Role;
                    if (role == null) return;
                    if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        await delRole(role.Roleid);
                    }
                }));
            }
        }

        private async Task<ObservableCollection<Role>> getRole()
        {
            try
            {
                StringContent content = new StringContent("getPerson");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:8888/connection/");
                request.Headers.Add("table", "role");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Role> roles = JsonSerializer.Deserialize<List<Role>>(responseText)!;
                return new ObservableCollection<Role>(roles);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Role>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Role>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Role>();
            }
        }
        private async Task sendRole(Role role)
        {
            try
            {
                JsonContent content = JsonContent.Create(role);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "role");
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
        public async Task delRole(int clientId)
        {
            try
            {
                JsonContent content = JsonContent.Create(clientId);
                var request = new HttpRequestMessage(HttpMethod.Delete, "http://127.0.0.1:8888/connection/");
                request.Content = content;
                request.Headers.Add("table", "role");
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
