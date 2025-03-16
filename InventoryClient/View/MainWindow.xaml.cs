using InventoryClient.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InventoryClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public static MainWindow? Instance;
    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
        DataContext = new AutorizationViewModel();
    }

    private void Grid_KeyDown(object sender, KeyEventArgs e)
    {

    }
}