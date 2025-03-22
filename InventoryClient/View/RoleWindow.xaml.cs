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
using InventoryClient.ViewModel;

namespace InventoryClient.View
{
    /// <summary>
    /// Логика взаимодействия для RoleWindow.xaml
    /// </summary>
    public partial class RoleWindow : Window
    {
        public static RoleWindow? Instance { get; private set; }
        public RoleWindow()
        {
            InitializeComponent();
            Instance = this;
            DataContext = new RoleWindowViewModel();
        }
    }
}