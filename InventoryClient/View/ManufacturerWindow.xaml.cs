﻿using InventoryClient.Model;
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
    /// Логика взаимодействия для ManufacturerWindow.xaml
    /// </summary>
    public partial class ManufacturerWindow : Window
    {
        public Manufacturer Manufacturer { get; set; }
        public ManufacturerWindow(Manufacturer m)
        {
            InitializeComponent();
            Manufacturer = m;
            DataContext = Manufacturer;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
