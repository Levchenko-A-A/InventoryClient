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

namespace InventoryClient.ViewModel
{
    class CategoryViewModel: BaseViewModel
    {
        private HttpClient httpClient;

        public CategoryViewModel()
        {
            httpClient = new HttpClient();
            Load();
        }

        private void Load()
        {
            Categories = null;
            Task<ObservableCollection<Category>> task = Task.Run(() => getCategory());
            Categories = task.Result;
        }

        private ObservableCollection<Category>? categories;
        public ObservableCollection<Category>? Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(categories));
            }
        }
        private Category? selectedCategories;
        public Category? SelectedCategories
        {
            get => selectedCategories;
            set
            {
                selectedCategories = value;
                OnPropertyChanged(nameof(SelectedCategories));
            }
        }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(async obj =>
                {
                    CategoryWindow categoryWindow = new CategoryWindow(new Category());
                    if (categoryWindow.ShowDialog() == true)
                    {
                        await sendLocation(categoryWindow.category);
                    }
                }));
            }
        }
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ?? (updateCommand = new RelayCommand(async (selectedItem) =>
                {
                    Category? category = selectedItem as Category;
                    if (category == null) return;
                    CategoryWindow categoryWindow = new CategoryWindow(category);
                    if (categoryWindow.ShowDialog() == true)
                    {
                        await updateCategory(categoryWindow.category);
                    }
                }));
            }
        }
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand(async (selectedItem) =>
                {
                    Category? category = selectedItem as Category;
                    if (category == null) return;
                    if (MessageBox.Show("Вы действительно хотите удалить элемент?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        await delCategory(category.Categoryid);
                    }
                }));
            }
        }
        private async Task<ObservableCollection<Category>> getCategory()
        {
            try
            {
                StringContent content = new StringContent("getCategoryAll");
                using var request = new HttpRequestMessage(HttpMethod.Get, "http://193.104.57.148:8080/connection/");
                request.Headers.Add("table", "category");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                List<Category> clients = JsonSerializer.Deserialize<List<Category>>(responseText)!;
                return new ObservableCollection<Category>(clients);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
                return new ObservableCollection<Category>();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return new ObservableCollection<Category>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
                return new ObservableCollection<Category>();
            }
        }

        private async Task sendLocation(Category category)
        {
            try
            {
                JsonContent content = JsonContent.Create(category);
                var request = new HttpRequestMessage(HttpMethod.Post, "http://193.104.57.148:8080/connection/");
                request.Content = content;
                request.Headers.Add("table", "category");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Категория с таким именем существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Категория добавлена");
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
        public async Task delCategory(int CategoryId)
        {
            try
            {
                JsonContent content = JsonContent.Create(CategoryId);
                var request = new HttpRequestMessage(HttpMethod.Delete, "http://193.104.57.148:8080/connection/");
                request.Content = content;
                request.Headers.Add("table", "category");
                using var response = await httpClient.SendAsync(request);
                string responseText = await response.Content.ReadAsStringAsync();
                if (responseText == "Error")
                    MessageBox.Show("Категория с таким именем не существует");
                else if (responseText == "OK")
                {
                    MessageBox.Show("Категория удалена");
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

        public async Task updateCategory(Category category)
        {
            try
            {
                JsonContent content = JsonContent.Create(category);
                var request = new HttpRequestMessage(HttpMethod.Put, "http://193.104.57.148:8080/connection/");
                request.Content = content;
                request.Headers.Add("table", "category");
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
