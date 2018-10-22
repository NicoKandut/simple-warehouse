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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool loggedIn { get; set; }
        public Owner currentOwner { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Database.init();
            ucRegister.Visibility = Visibility.Collapsed;
            ucManageWarehouses.Visibility = Visibility.Collapsed;
            ucCreateWarehouse.Visibility = Visibility.Collapsed;
            ucLogin.txtBoxName.Text = "Martin";
            ucLogin.txtBoxPwd.Password = "martin3101";
            ucLogin.btnLogin_Click(null, null);
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
                    if (ucLogin.Visibility == Visibility.Collapsed)
                        btnLogout_Click(sender, e);
                    else
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
        private void btnSettingsClick(object sender, RoutedEventArgs e)
        {
            if (currentOwner != null)
            {
                txtBoxUsername.Text = currentOwner.Name;
                if (flyoutProfile.IsOpen)
                    flyoutProfile.IsOpen = false;
                else
                    flyoutProfile.IsOpen = true;
            }
            else
            {
                MessageBox.Show("Login first!", "Login required", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        ~MainWindow()
        {
            //TODO:logout to delete token
        }

        private async void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(txtBoxNewPassword.Password))
                {
                    if (txtBoxPassword.Password == currentOwner.Password)
                    {
                        if (!String.IsNullOrWhiteSpace(txtBoxUsername.Text))
                            if(await Database.updateOwnerAsync(txtBoxUsername.Text, txtBoxNewPassword.Password))
                            {
                                flyoutProfile.IsOpen = false;
                                MessageBox.Show("Credentials changed!", "Change", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        else
                            MessageBox.Show("Username cannot be empty!","ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Wrong confirmation password", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (txtBoxUsername.Text != currentOwner.Name)
                        if (!String.IsNullOrWhiteSpace(txtBoxUsername.Text))
                            await Database.updateOwnerAsync(txtBoxUsername.Text, currentOwner.Password);
                        else
                            MessageBox.Show("Username can't be empty!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("No changes detetced", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void btnLogout_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to logout?", "Logout?", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                bool result = Database.logoutAsync().Result;
                if (result)
                {
                    ucAddOrder.Visibility = Visibility.Collapsed;
                    ucLogin.Visibility = Visibility.Visible;
                    ucEditWarehouse.Visibility = Visibility.Collapsed;
                    ucManageWarehouses.Visibility = Visibility.Collapsed;
                    ucCreateWarehouse.Visibility = Visibility.Collapsed;
                    ucRegister.Visibility = Visibility.Collapsed;
                    loggedIn = false;
                    currentOwner = null;
                    Database.Token = null;
                    Title = "Werhaus";
                    flyoutProfile.IsOpen = false;
                }
            }
        }

        private async void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want do delete your account?\nThis change is permament and cannot be reversed!", "Delete Account", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if(await Database.deleteAccountAsync())
                {
                    ucLogin.Visibility = Visibility.Visible;
                    ucManageWarehouses.Visibility = Visibility.Collapsed;
                    ucCreateWarehouse.Visibility = Visibility.Collapsed;
                    ucRegister.Visibility = Visibility.Collapsed;
                    loggedIn = false;
                    currentOwner = null;
                    Database.Token = null;
                    Title = "Werhaus";
                    flyoutProfile.IsOpen = false;
                    txtBoxNewPassword.Password = "";
                    txtBoxPassword.Password = "";
                    txtBoxUsername.Text = "";
                }
            }
        }
    }
}
