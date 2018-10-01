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
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database.login(txtBoxName.Text, txtBoxPwd.Text);
                MessageBox.Show("Login succesful!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Login failed!\n"+ex.Message);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            main.ucLogin.Visibility = Visibility.Collapsed;
            main.ucRegister.Visibility = Visibility.Visible;
        }
    }
}
