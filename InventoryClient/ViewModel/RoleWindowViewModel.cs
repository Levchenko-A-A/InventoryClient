using InventoryClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryClient.ViewModel
{
    internal class RoleWindowViewModel
    {
        public RoleWindowViewModel()
        {
            RoleWindow.Instance!.RoleFrame.Navigate(new PageRole());
        }

        private RelayCommand personRolCommand;
        public RelayCommand PersonRolCommand
        {
            get
            {
                return personRolCommand ??
                  (personRolCommand = new RelayCommand((o) =>
                  {
                      RoleWindow.Instance!.RoleFrame.Navigate(new PagePersonrole());
                  }));
            }
        }

        private RelayCommand rolCommand;
        public RelayCommand RolCommand
        {
            get
            {
                return rolCommand ??
                  (rolCommand = new RelayCommand((o) =>
                  {
                      RoleWindow.Instance!.RoleFrame.Navigate(new PageRole());
                  }));
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                  (exitCommand = new RelayCommand((o) =>
                  {
                      RoleWindow.Instance!.Close();
                  }));
            }
        }
    }
}
