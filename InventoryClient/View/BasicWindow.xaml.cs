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
using InventoryClient.Model;

namespace InventoryClient.View
{
    /// <summary>
    /// Логика взаимодействия для BasicWindow.xaml
    /// </summary>
    public partial class BasicWindow : Window
    {
        
        public BasicWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PagePerson());            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PagePerson());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageCategory());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageDevice());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageManufacturer());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageLocation());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void SetUserInput(string userInput)
        {
            string getInput = userInput;
            if (userInput == "Admin")
            {

                //ExitButton.IsEnabled = true;
            }
            else
            {
                //ExitButton.IsEnabled = false;
            }
        }
    }
}
