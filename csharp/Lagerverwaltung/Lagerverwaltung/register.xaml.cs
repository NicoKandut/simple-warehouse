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
    /// Interaction logic for register.xaml
    /// </summary>
    public partial class register : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBoxConfirmPwd.Password != txtBoxPwd.Password)
                    throw new Exception("Passwords don't match!");
                Database.register(txtBoxName.Text, txtBoxConfirmPwd.Password);
                main.ucLogin.Visibility = Visibility.Visible;
                main.ucRegister.Visibility = Visibility.Collapsed;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Register failed!\n" + ex.Message);
            }
        }
    }
}
