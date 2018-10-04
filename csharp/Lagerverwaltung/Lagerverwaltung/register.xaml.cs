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

        public void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBoxConfirmPwd.Password != txtBoxPwd.Password)
                    throw new Exception("Passwords don't match!");
                if (String.IsNullOrWhiteSpace(txtBoxPwd.Password) || String.IsNullOrWhiteSpace(txtBoxConfirmPwd.Password) || String.IsNullOrWhiteSpace(txtBoxName.Text))
                    throw new Exception("All fields must be filled!");
                Database.register(txtBoxName.Text, txtBoxConfirmPwd.Password);
                main.ucLogin.Visibility = Visibility.Visible;
                main.ucRegister.Visibility = Visibility.Collapsed;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Register failed!\n" + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            main.ucLogin.Visibility = Visibility.Visible;
            main.ucRegister.Visibility = Visibility.Collapsed;
        }
    }
}
