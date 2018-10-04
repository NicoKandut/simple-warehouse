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
    /// Interaction logic for manageWarehouses.xaml
    /// </summary>
    public partial class manageWarehouses : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public manageWarehouses()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to logout?", "Logout?", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                main.ucLogin.Visibility = Visibility.Visible;
                main.ucManageWarehouses.Visibility = Visibility.Collapsed;
                main.Title = "Werhaus";
            }
        }
    }
}
