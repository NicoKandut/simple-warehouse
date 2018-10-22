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
    /// Interaction logic for manageWarehouses.xaml
    /// </summary>
    public partial class manageWarehouses : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public manageWarehouses()
        {
            InitializeComponent();
        }



        private void btnAddWarehouse_Click(object sender, RoutedEventArgs e)
        {
            main.ucCreateWarehouse.Visibility = Visibility.Visible;
        }

        private async void btnDeleteWarehouse_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxWarehouses.SelectedItem != null)
            {
                Warehouse warehouse = listBoxWarehouses.SelectedItem as Warehouse;
                if ( await Database.deleteWarehouseAsnyc(warehouse.Id))
                {
                    main.currentOwner.Warehouses.Remove(warehouse);
                    listBoxWarehouses.Items.Refresh();
                }
            }
        }

        private async void btnEditWarehouse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxWarehouses.SelectedItem != null)
                {
                    Warehouse warehouse = (Warehouse)listBoxWarehouses.SelectedItem;
                    main.ucEditWarehouse.Warehouse = await Database.getWarehouseAsync(warehouse.Id);
                    main.ucEditWarehouse.Visibility = Visibility.Visible;
                    main.ucManageWarehouses.Visibility = Visibility.Collapsed;
                }
                else
                    throw new Exception("No warehouse selected!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
