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

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            //create Warehouse with db
            main.ucManageWarehouses.Visibility = Visibility.Visible;
            main.ucCreateWarehouse.Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            main.ucManageWarehouses.Visibility = Visibility.Visible;
            main.ucCreateWarehouse.Visibility = Visibility.Collapsed;
        }
    }
}
