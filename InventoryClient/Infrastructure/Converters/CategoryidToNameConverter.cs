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
    class CategoryidToNameConverter : IValueConverter
    {
        private HttpClient httpClient = new HttpClient();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Task.Run(() => getCategoryName((int)value)).Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private async Task<string> getCategoryName(int categoryId)
        {
            try
            {
                JsonContent content = JsonContent.Create(categoryId);
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://http://193.104.57.148:8080/connection/");
                request.Headers.Add("table", "category");
                request.Content = content;
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                Category category = JsonSerializer.Deserialize<Category>(responseText)!;
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
