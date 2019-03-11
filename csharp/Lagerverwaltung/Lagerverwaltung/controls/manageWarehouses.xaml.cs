using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            main.switchToCreateWarehouse();
        }

        private async void btnDeleteWarehouse_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxWarehouses.SelectedItem != null)
            {
                Warehouse warehouse = listBoxWarehouses.SelectedItem as Warehouse;
                if (await Database.deleteWarehouseAsnyc(warehouse.Id))
                {
                    main.currentOwner.Warehouses.Remove(warehouse);
                    listBoxWarehouses.Items.Refresh();
                }
            }
            else
                MessageBox.Show("No warehouse selected!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void btnEditWarehouse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBoxWarehouses.SelectedItem != null)
                {
                    Warehouse warehouse = (Warehouse)listBoxWarehouses.SelectedItem;
                    main.ucEditWarehouse.Warehouse = await Database.getWarehouseAsync(warehouse.Id);
                    main.switchToEditWarehouse();
                }
                else
                    throw new Exception("No warehouse selected!");
            }
            catch(Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }
        
        public async void updateWarehouseList()
        {
            try
            {
                listBoxWarehouses.ItemsSource = await Database.getWarehousesOfOwnerAsync();
                listBoxWarehouses.Items.Refresh();
            }
            catch(Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }
    }
}
