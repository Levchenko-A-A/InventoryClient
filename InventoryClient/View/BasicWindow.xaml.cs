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
using InventoryClient.ViewModel;

namespace InventoryClient.View
{
    /// <summary>
    /// Логика взаимодействия для BasicWindow.xaml
    /// </summary>
    public partial class BasicWindow : Window
    {
        public static BasicWindow? Instance { get; private set; }
        public BasicWindow()
        {
            InitializeComponent();
            Instance = this;
            DataContext = new BasicWindowViewModel();
        }
    }
}
