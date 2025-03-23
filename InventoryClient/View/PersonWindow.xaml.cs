using InventoryClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventoryClient.View
{
    /// <summary>
    /// Логика взаимодействия для PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        public Person Person { get; set; }
        public PersonWindow(Person c)
        {
            InitializeComponent();
            Person = c;
            DataContext = Person;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Admin_Checked(object sender, RoutedEventArgs e)//новое
        {
            Person.IsAdmin = (sender as CheckBox)?.IsChecked ?? false;
        }

        private void CheckBoxManager_Checked(object sender, RoutedEventArgs e)
        {
            Person.IsManager = (sender as CheckBox)?.IsChecked ?? false;
        }

        private void CheckBoxUser_Checked(object sender, RoutedEventArgs e)
        {
            Person.IsUser = (sender as CheckBox)?.IsChecked ?? false;
        }

        private void CheckBoxGuest_Checked(object sender, RoutedEventArgs e)
        {
            Person.IsGuest = (sender as CheckBox)?.IsChecked ?? false;
        }
    }
}
