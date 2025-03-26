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
using System.Windows.Navigation;

namespace InventoryClient.ViewModel
{
    class PersonroleViewModel: BaseViewModel
    {
        private HttpClient httpClient;

        public PersonroleViewModel()
        {
            httpClient = new HttpClient();
            Load();
            //AddPersonRoleButVis = Visibility.Collapsed;
            AddPersonRoleButIsE = false;
            UpdatePersonRoleButIsE = false;
            DelPersonRoleButIsE = false;


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
        //private Visibility addPersonRoleButVis;
        //public Visibility AddPersonRoleButVis
        //{
        //    get { return addPersonRoleButVis; }
        //    set
        //    {
        //        addPersonRoleButVis = value;
        //        OnPropertyChanged(nameof(AddPersonRoleButVis));
        //    }
        //}
        private bool addPersonRoleButIsE;
        public bool AddPersonRoleButIsE
        {
            get { return addPersonRoleButIsE; }
            set
            {
                addPersonRoleButIsE = value;
                OnPropertyChanged(nameof(AddPersonRoleButIsE));
            }
        }
        private bool updatePersonRoleButIsE;
        public bool UpdatePersonRoleButIsE
        {
            get { return updatePersonRoleButIsE; }
            set
            {
                updatePersonRoleButIsE = value;
                OnPropertyChanged(nameof(UpdatePersonRoleButIsE));
            }
        }
        private bool delPersonRoleButIsE;
        public bool DelPersonRoleButIsE
        {
            get { return delPersonRoleButIsE; }
            set
            {
                delPersonRoleButIsE = value;
                OnPropertyChanged(nameof(DelPersonRoleButIsE));
            }
        }
        private async Task<ObservableCollection<Personrole>> getPersonRole()
        {
            try
            {
                StringContent content = new StringContent("getPersonRole");
                using var request = new HttpRequestMessage(HttpMethod.Get, ServerPath.Path);
                request.Headers.Add("table", "personrole");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Personrole> personroles = JsonSerializer.Deserialize<List<Personrole>>(responseText)!;
                return new ObservableCollection<Personrole>(personroles);
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
        public async Task delPersonRole(int personroleId)
        {
            try
            {
                JsonContent content = JsonContent.Create(personroleId);
                var request = new HttpRequestMessage(HttpMethod.Delete, ServerPath.Path);
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
                var request = new HttpRequestMessage(HttpMethod.Put, ServerPath.Path);
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
    }
}
