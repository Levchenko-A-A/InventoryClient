using InventoryClient.Model;
using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryClient.ViewModel
{
    internal class BasicWindowViewModel: BaseViewModel
    {
        public BasicWindowViewModel()
        {
            StatusButtom();
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
                    BasicWindow.Instance!.Close();
                    MainWindow.Instance!.Close();
                }));
            }
        }
        private bool butPersonIsE;
        public bool ButPersonIsE
        {
            get { return butPersonIsE; }
            set
            {
                butPersonIsE = value;
                OnPropertyChanged(nameof(ButPersonIsE));
            }
        }
        private bool butCategoryIsE;
        public bool ButCategoryIsE
        {
            get { return butCategoryIsE; }
            set
            {
                butCategoryIsE = value;
                OnPropertyChanged(nameof(ButCategoryIsE));
            }
        }
        private bool butDeviceIsE;
        public bool ButDeviceIsE
        {
            get { return butDeviceIsE; }
            set
            {
                butDeviceIsE = value;
                OnPropertyChanged(nameof(ButDeviceIsE));
            }
        }
        private bool butManufIsE;
        public bool ButManufIsE
        {
            get { return butManufIsE; }
            set
            {
                butManufIsE = value;
                OnPropertyChanged(nameof(ButManufIsE));
            }
        }
        private bool butLocationIsE;
        public bool ButLocationIsE
        {
            get { return butLocationIsE; }
            set
            {
                butLocationIsE = value;
                OnPropertyChanged(nameof(ButLocationIsE));
            }
        }
        private void StatusButtom()
        {
            if (RegisterUser.UserAllId != null)
            {
                //MessageBox.Show(RegisterUser.UserAllId.Any(p => p.Roleid == 4).ToString());
                if (RegisterUser.UserAllId.Any(p => p.Roleid == 4) == true)
                {
                    ButPersonIsE = false;
                    ButCategoryIsE = false;
                    ButDeviceIsE = false;
                    ButManufIsE = false;
                    ButLocationIsE = false;
                    if (RegisterUser.UserAllId.Any(p => p.Roleid == 3) == true)
                    {
                        ButPersonIsE = false;
                        ButCategoryIsE = false;
                        ButDeviceIsE = true;
                        ButManufIsE = false;
                        ButLocationIsE = false;
                    }
                    else if (RegisterUser.UserAllId.Any(p => p.Roleid == 2) == true)
                    {
                        ButPersonIsE = false;
                        ButCategoryIsE = true;
                        ButDeviceIsE = true;
                        ButManufIsE = true;
                        ButLocationIsE = true;
                    }
                    else if (RegisterUser.UserAllId.Any(p => p.Roleid == 1) == true)
                    {
                        ButPersonIsE = true;
                        ButCategoryIsE = true;
                        ButDeviceIsE = true;
                        ButManufIsE = true;
                        ButLocationIsE = true;
                    }
                }
                else if (RegisterUser.UserAllId.Any(p => p.Roleid == 3) == true)
                {
                    ButPersonIsE = false;
                    ButCategoryIsE = false;
                    ButDeviceIsE = true;
                    ButManufIsE = false;
                    ButLocationIsE = false;
                    if (RegisterUser.UserAllId.Any(p => p.Roleid == 2) == true)
                    {
                        ButPersonIsE = false;
                        ButCategoryIsE = true;
                        ButDeviceIsE = true;
                        ButManufIsE = true;
                        ButLocationIsE = true;
                    }
                    else if (RegisterUser.UserAllId.Any(p => p.Roleid == 1) == true)
                    {
                        ButPersonIsE = true;
                        ButCategoryIsE = true;
                        ButDeviceIsE = true;
                        ButManufIsE = true;
                        ButLocationIsE = true;
                    }
                }
                else if (RegisterUser.UserAllId.Any(p => p.Roleid == 2) == true)
                {
                    ButPersonIsE = false;
                    ButCategoryIsE = true;
                    ButDeviceIsE = true;
                    ButManufIsE = true;
                    ButLocationIsE = true;
                    if (RegisterUser.UserAllId.Any(p => p.Roleid == 1) == true)
                    {
                        ButPersonIsE = true;
                        ButCategoryIsE = true;
                        ButDeviceIsE = true;
                        ButManufIsE = true;
                        ButLocationIsE = true;
                    }
                }
                else if (RegisterUser.UserAllId.Any(p => p.Roleid == 1) == true)
                {
                    ButPersonIsE = true;
                    ButCategoryIsE = true;
                    ButDeviceIsE = true;
                    ButManufIsE = true;
                    ButLocationIsE = true;
                }
            }
            else
            {
                ButPersonIsE = false;
                ButCategoryIsE = false;
                ButDeviceIsE = false;
                ButManufIsE = false;
                ButLocationIsE = false;
            }
        }
    }
}
