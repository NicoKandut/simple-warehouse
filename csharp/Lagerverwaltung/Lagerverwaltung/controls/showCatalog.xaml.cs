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

namespace Lagerverwaltung.controls
{
    /// <summary>
    /// Interaction logic for showCatalog.xaml
    /// </summary>
    public partial class showCatalog : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public showCatalog()
        {
            InitializeComponent();           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            main.switchToManageWarehouses();
        }
        private async void visibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            listBoxProducts.ItemsSource = await Database.getAllProductsOfCatalogAsync();
            listBoxProducts.Items.Refresh();
        }
    }
}
