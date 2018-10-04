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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ucRegister.Visibility = Visibility.Collapsed;
            ucManageWarehouses.Visibility = Visibility.Collapsed;
            Database.connect("Data Source=192.168.128.152/ora11g;User Id=d5a07;Password=d5a;");
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
        ~MainWindow()
        {
            Database.closeConnection();
        }
    }
}
