using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClient.ViewModel
{
    internal class BasicWindowViewModel
    {
        public BasicWindowViewModel()
        {
            BasicWindow.Instance!.MainFrame.Navigate(new PageDevice());
        }

        private RelayCommand? personCommand;
        public RelayCommand? PersonCommand
        {
            get
            {
                return personCommand ?? (personCommand = new RelayCommand((o) =>
                {
                    BasicWindow.Instance!.MainFrame.Navigate(new PagePerson());
                }));
            }
        }

        private RelayCommand? categoryCommand;
        public RelayCommand? CategoryCommand
        {
            get
            {
                return categoryCommand ?? (categoryCommand = new RelayCommand((o) =>
                {
                    BasicWindow.Instance!.MainFrame.Navigate(new PageCategory());
                }));
            }
        }

        private RelayCommand? deviceCommand;
        public RelayCommand? DeviceCommand
        {
            get
            {
                return deviceCommand ?? (deviceCommand = new RelayCommand((o) =>
                {
                    BasicWindow.Instance!.MainFrame.Navigate(new PageDevice());
                }));
            }
        }

        private RelayCommand? manufacturerCommand;
        public RelayCommand? ManufacturerCommand
        {
            get
            {
                return manufacturerCommand ?? (manufacturerCommand = new RelayCommand((o) =>
                {
                    BasicWindow.Instance!.MainFrame.Navigate(new PageManufacturer());
                }));
            }
        }

        private RelayCommand? locationCommand;
        public RelayCommand? LocationCommand
        {
            get
            {
                return locationCommand ?? (locationCommand = new RelayCommand((o) =>
                {
                    BasicWindow.Instance!.MainFrame.Navigate(new PageLocation());
                }));
            }
        }

        private RelayCommand? exitCommand;
        public RelayCommand? ExitCommand
        {
            get
            {
                return exitCommand ?? (exitCommand = new RelayCommand((o) =>
                {
                    //this.Close();
                    //MainWindow mainWindow = new MainWindow();
                    //mainWindow.Show();
                    //foreach (Window window in Application.Current.Windows)
                    //{
                    //    if (window is MainWindow main)
                    //    {
                    //        main.Visibility = Visibility.Visible;
                    //    }
                    //}
                }));
            }
        }

    }
}
