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
            main = (MainWindow)Application.Current.MainWindow;
        }

        public async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtBoxName.Text) || String.IsNullOrWhiteSpace(txtBoxPwd.Password))
                    throw new Exception("All fields must be filled!");
                if (await Database.loginAsync(txtBoxName.Text, txtBoxPwd.Password))
                {
                    main.currentOwner = await Database.getOwnerAsync();
                    main.currentOwner.Name = txtBoxName.Text;
                    main.currentOwner.Password = txtBoxPwd.Password;                   
                    main.loggedIn = true;
                    main.ucManageWarehouses.listBoxWarehouses.ItemsSource = main.currentOwner.Warehouses;
                    main.btnSettings.Content = main.currentOwner.Name;
                    main.Title = main.currentOwner.Name;
                    txtBoxName.Text = "";
                    txtBoxPwd.Password = "";
                    main.switchToManageWarehouses();

                }
                else
                {
                    MessageBox.Show("Username or password incorrect!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            }
            catch(Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }
        
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            main.switchToRegister();
        }
    }
}
