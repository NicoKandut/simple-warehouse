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
    /// Interaction logic for editWarehouse.xaml
    /// </summary>
    public partial class editWarehouse : UserControl
    {
        public Warehouse Warehouse
        {
            get
            {
                return warehouse;
            }
            set
            {
                if (value != null)
                {
                    warehouse = value;
                    listBoxProducts.ItemsSource = warehouse.Products;
                    listBoxProducts.Items.Refresh();
                }
                else
                    throw new Exception("No warehouse selected!");
            }
        }
        private Warehouse warehouse;
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public editWarehouse()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            main.switchToManageWarehouses();
        }

        private void btnBuyProduct_Click(object sender, RoutedEventArgs e)
        {
            main.ucAddOrder.purchase = true;
            main.ucAddOrder.lblTitle.Content = "Buy Product";
            main.switchToAddOrder();
        }

        private void btnSellProduct_Click(object sender, RoutedEventArgs e)
        {
            main.ucAddOrder.purchase = false;
            main.ucAddOrder.lblTitle.Content = "Sell Product";
            main.switchToAddOrder();
        }

        private void listBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(main.ucAddOrder.Visibility == Visibility.Visible && listBoxProducts.SelectedItem != null)
            {
                main.ucAddOrder.cbProducts.SelectedItem = main.ucEditWarehouse.Warehouse.Products.Find(x => x.Name == (listBoxProducts.SelectedItem as ProductBase).Name);
            }
        }
    }
}
