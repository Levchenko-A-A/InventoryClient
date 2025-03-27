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
using System.Reflection.Metadata;
using System.Collections.ObjectModel;

namespace InventoryClient.ViewModel
{
    internal class AutorizationViewModel: BaseViewModel
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

        private RelayCommand? enterCommand;
        public RelayCommand EnterCommand
        {
            get
            {
                return enterCommand ??
                  (enterCommand = new RelayCommand(async obj =>
                  {
                      PasswordBox? password = obj as PasswordBox;
                      string userName = Login;
                      string passWord = password!.Password;
                      RegisterUser.UserName = Login;
                      string result = await VerifyPassword(userName, passWord);

                      if (result != "Error")
                      {
                          List<Personrole> personroles = await getPersonRole();
                          //RegisterUser.UserAllId = personroles.Where(p => p.Personid == int.Parse(result)).ToList();

                          RegisterUser.access_token = result;
                          MessageBox.Show(result);
                          //Application.Current.Properties["JwtToken"] = token;
                          //string valToken = await ValidateToken(token);
                          //MessageBox.Show(valToken);
                          //Visibility = Visibility.Hidden;
                          //BasicWindow basicWindow = new BasicWindow();
                          //basicWindow.Show();
                      }
                      else MessageBox.Show("Пользователя с таким именем или паролем не существует!");
                  }));
            }
        }
        public static async Task<string> ValidateToken(string token)
        {
            try
            {
                JsonContent content = JsonContent.Create(token);
                var request = new HttpRequestMessage(HttpMethod.Post, ServerPath.Path);
                httpClient.DefaultRequestHeaders.Add("Authorization", RegisterUser.access_token);
                request.Content = content;
                request.Headers.Add("table", "ValidateToken");
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

        public static async Task<string> VerifyPassword(string username, string password)
        {
            try
            {
                JsonUser authData = new JsonUser()
                {
                    UserName = username,
                    Password = password
                };

                JsonContent content = JsonContent.Create(authData);
                var request = new HttpRequestMessage(HttpMethod.Post, ServerPath.Path);
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
        private async Task<List<Personrole>> getPersonRole()
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
                return personroles;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new List<Personrole>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new List<Personrole>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new List<Personrole>();
            }
        }
    }
}
