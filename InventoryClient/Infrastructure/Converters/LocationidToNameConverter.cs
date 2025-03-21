using InventoryClient.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace InventoryClient.Infrastructure.Converters
{
    class LocationidToNameConverter : IValueConverter
    {
        private HttpClient httpClient = new HttpClient();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Task.Run(() => getLocationName((int)value)).Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private async Task<string> getLocationName(int manufId)
        {
            try
            {
                JsonContent content = JsonContent.Create(manufId);
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://193.104.57.148:8080/connection/");
                request.Headers.Add("table", "location");
                request.Content = content;
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                Location category = JsonSerializer.Deserialize<Location>(responseText)!;
                return category.Name;
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
