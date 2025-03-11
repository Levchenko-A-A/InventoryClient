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

namespace InventoryClient.ViewModel
{
    class AutorizationViewModel: BaseViewModel
    {
        private static HttpClient httpClient = new HttpClient();

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

        private RelayCommand? enter;
        public RelayCommand Enter
        {
            get
            {
                return enter ??
                  (enter = new RelayCommand(async obj =>
                  {
                      PasswordBox? password = obj as PasswordBox;
                      //HttpClient client = new HttpClient();
                      string userName = Login;
                      string passWord = password!.Password;
                      //User user = new User { Login = Login, Password = password!.Password };
                      //JsonContent content = JsonContent.Create(user);
                      //using var response = await client.PostAsync("http://localhost:5000/login", content);
                      //string responseText = await response.Content.ReadAsStringAsync();
                      string result = await VerifyPassword(userName, passWord);
                      if (result == "ok")
                      {
                          //Visibility = Visibility.Hidden;
                          //MenuWindow window = new MenuWindow();
                          //window.Show();
                          MessageBox.Show("OK");
                          //Response? resp = JsonSerializer.Deserialize<Response>(responseText);
                          //if (resp != null)
                          //{
                          //    RegisterUser.UserName = resp.username;
                          //    RegisterUser.access_token = resp.access_token;
                          //    Visibility = Visibility.Hidden;
                          //    MenuWindow window = new MenuWindow();
                          //    window.Show();
                          //}
                      }
                      else MessageBox.Show("Пользователь с таким именем или паролем " +
                              "не существует!");

                  }));
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
                var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8888/connection/");
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
    }
}
