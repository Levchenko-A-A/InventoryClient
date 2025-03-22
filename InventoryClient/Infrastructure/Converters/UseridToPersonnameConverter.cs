using InventoryClient.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace InventoryClient.Infrastructure.Converters
{
    class UseridToPersonnameConverter: IValueConverter
    {
        private HttpClient httpClient = new HttpClient();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Task.Run(() => getPersonName((int)value)).Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private async Task<string> getPersonName(int personId)
        {
            try
            {
                JsonContent content = JsonContent.Create(personId);
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://193.104.57.148:8888/connection/");
                request.Headers.Add("table", "person");
                request.Content = content;
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                Person clients = JsonSerializer.Deserialize<Person>(responseText)!;
                return clients.Personname;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return "";
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return "";
            }
        }
    }
}
