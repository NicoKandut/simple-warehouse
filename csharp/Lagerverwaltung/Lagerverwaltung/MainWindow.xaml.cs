using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WerhausCore;

namespace Lagerverwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool loggedIn { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ucRegister.Visibility = Visibility.Collapsed;
            ucManageWarehouses.Visibility = Visibility.Collapsed;
            ucCreateWarehouse.Visibility = Visibility.Collapsed;
            Database.connect("Data Source=192.168.128.152/ora11g;User Id=d5a07;Password=d5a;");
            if (!(Database.conn.State == System.Data.ConnectionState.Open))
                Database.connect("Data Source=212.152.179.117/ora11g;User Id=d5a07;Password=d5a;");
            printTest();

        }
        private async void printTest()
        {
            Warehouse warehouse = null;
            warehouse = await Database.getWarehouseAsync(1);
            MessageBox.Show(warehouse.ToString());
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (ucLogin.Visibility == Visibility.Visible)
                        ucLogin.btnLogin_Click(sender, e);
                    else if (ucRegister.Visibility == Visibility.Visible)
                        ucRegister.btnRegister_Click(sender, e);
                }
                else if (e.Key == Key.Escape)
                    if (ucManageWarehouses.Visibility == Visibility.Visible)
                        ucManageWarehouses.btnLogout_Click(sender, e);
                else
                        Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong...", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBoxResult.No == MessageBox.Show("Are you sure you want to quit?", "Quit?", MessageBoxButton.YesNo, MessageBoxImage.Question))
                e.Cancel = true;
        }
        private void btnSettingsClick(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
                MessageBox.Show("Not logged in!", "No login", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Logged in user: " + MainMetroWindow.Title, "User", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        ~MainWindow()
        {
            Database.closeConnection();
        }
    }
}
