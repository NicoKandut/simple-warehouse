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
    /// Interaction logic for showHistography.xaml
    /// </summary>
    public partial class showHistography : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public showHistography()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculation.getHistographyData((main.ucEditWarehouse.Warehouse.Id));

            // main.switchToEditWarehouse();
        }
    }
}
