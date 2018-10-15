using MahApps.Metro.Controls;
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
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : UserControl
    {
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public login()
        {
            InitializeComponent();
        }

        public async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtBoxName.Text) || String.IsNullOrWhiteSpace(txtBoxPwd.Password))
                    throw new Exception("All fields must be filled!");
                bool result = await Database.loginAsync(txtBoxName.Text, txtBoxPwd.Password);
                if (result)
                {
                    main.currentOwner = await Database.getOwnerAsync();
                    main.currentOwner.Name = txtBoxName.Text;
                    main.currentOwner.Password = txtBoxPwd.Password;
                    main.ucLogin.Visibility = Visibility.Collapsed;
                    main.ucManageWarehouses.Visibility = Visibility.Visible;
                    main.loggedIn = true;
                    main.ucManageWarehouses.listBoxWarehouses.ItemsSource = main.currentOwner.Warehouses;
                    main.Title = txtBoxName.Text;
                    txtBoxName.Text = "";
                    txtBoxPwd.Password = "";
                }
                else
                {
                    MessageBox.Show("Login failed");
            }
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
