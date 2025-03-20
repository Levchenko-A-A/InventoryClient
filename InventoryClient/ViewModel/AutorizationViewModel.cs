using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using InventoryClient.Model;
using InventoryClient.View;
using System.Windows.Input;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace InventoryClient.ViewModel
{
    class AutorizationViewModel: BaseViewModel
    {
        public string path = "http://193.104.57.148:8080/connection/";
        private static HttpClient httpClient = new HttpClient();
        JsonUser RegisterUser= new JsonUser();
        public ICommand EnterCommand { get; }

        public AutorizationViewModel()
        {
            EnterCommand = new RelayCommand(ExecuteEnter, CanExecuteEnter);
        }

        private Visibility visibility;
        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                OnPropertyChanged("Visibility");
            }
        }
        private string? login;
        public string Login
        {
            get { return login!; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        private string? password;
        public string LoginPassword
        {
            get { return password!; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(LoginPassword));
            }
        }

        public static async Task<string> VerifyPassword(string username, string password)
        {
            try
            {
                JsonUser requestData = new JsonUser()
                {
                    UserName = username,
                    Password = password
                };
                JsonContent content = JsonContent.Create(requestData);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://193.104.57.148:8080/connection/");
                request.Content = content;
                request.Headers.Add("table", "verifyPasswordPerson");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                string answer = JsonSerializer.Deserialize<string>(responseText)!;
                return answer;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка HTTP: {ex.Message}");
                return "Error";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return "Error";
            }
        }
        private async void ExecuteEnter(object parameter)
        {
            PasswordBox? password = parameter as PasswordBox;
            string userName = Login;
            string passWord = password!.Password;
            RegisterUser.UserName = Login;
            string result = await VerifyPassword(userName, passWord);
            if (result == "ok")
            {
                Visibility = Visibility.Hidden;
                BasicWindow basicWindow = new BasicWindow();
                basicWindow.Show();
            }
            else MessageBox.Show("Пользователя с таким именем или паролем не существует!");
        }

        private bool CanExecuteEnter(object parameter)
        {
            return true;
        }
    }
}
