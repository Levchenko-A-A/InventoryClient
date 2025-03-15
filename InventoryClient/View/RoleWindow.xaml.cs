﻿using System;
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
    /// Логика взаимодействия для RoleWindow.xaml
    /// </summary>
    public partial class RoleWindow : Window
    {
        public RoleWindow()
        {
            InitializeComponent();
            RoleFrame.Navigate(new PageRole());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RoleFrame.Navigate(new PagePersonrole());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RoleFrame.Navigate(new PageRole());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        
            this.Close();
        }
    }
}
