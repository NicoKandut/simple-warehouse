using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for createWarehouse.xaml
    /// </summary>
    public partial class createWarehouse : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public createWarehouse()
        {
            InitializeComponent();
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(String.IsNullOrWhiteSpace(txtBoxCapacity.Text) || String.IsNullOrWhiteSpace(new TextRange(txtBoxDescription.Document.ContentStart, txtBoxDescription.Document.ContentEnd).Text) || String.IsNullOrWhiteSpace(txtBoxName.Text)))
                {
                    if (await Database.addWarehouseAsync(new Warehouse(txtBoxName.Text, new TextRange(txtBoxDescription.Document.ContentStart, txtBoxDescription.Document.ContentEnd).Text, 0, int.Parse(txtBoxCapacity.Text), main.currentOwner)))
                    {
                        main.switchToManageWarehouses();
                        main.currentOwner.Warehouses = await Database.getWarehousesOfOwnerAsync();
                        main.ucManageWarehouses.listBoxWarehouses.ItemsSource = main.currentOwner.Warehouses;
                        main.ucManageWarehouses.listBoxWarehouses.Items.Refresh();
                    }
                    else
                        MessageBox.Show("Error while trying to add warehouse!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Not all required fields filled!", "MISSING VALUES", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            catch(Exception ex)
            {
                MessageBox.Show("Error while trying to create warehouse!");
            }
        }
        //input handler for capacity of warehouse to only allow numbers
        private void textInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            main.switchToManageWarehouses();
        }
    }
}
