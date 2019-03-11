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
            //////initialize database class to be ready for use/////////
            Database.init();
            configManager.init();
            ///////login while testing ui//////
            switchToLogin();
            ucLogin.txtBoxName.Text = "Martin";
            ucLogin.txtBoxPwd.Password = "martin3101";
            ucLogin.btnLogin_Click(null, null);
            /////////////////////////////
        }
        //keydown eventhandler for enhanced user experience(e.g. esc = back)
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
                configManager.showErrorMessage(ex);
            }
        }
        //check if user really wants to quit the application
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBoxResult.No == MessageBox.Show("Are you sure you want to quit?", "Quit?", MessageBoxButton.YesNo, MessageBoxImage.Question))
                e.Cancel = true;
        }
        //event handler for settings button which triggers the flyout
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
        //event handler for saving changed user settings in flyout
        private async void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(txtBoxNewPassword.Password))
                {
                    if (txtBoxPassword.Password == currentOwner.Password)
                    {
                        if (!String.IsNullOrWhiteSpace(txtBoxUsername.Text))
                        {
                            if (await Database.updateOwnerAsync(txtBoxUsername.Text, txtBoxNewPassword.Password))
                            {
                                flyoutProfile.IsOpen = false;
                                MessageBox.Show("Credentials changed!", "Change", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                            MessageBox.Show("Username cannot be empty!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            catch (Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }
        //event handler for button logout with check
        public async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to logout?", "Logout?", MessageBoxButton.YesNo, MessageBoxImage.Question))
                {
                    bool result = await Database.logoutAsync();
                    if (result)
                    {
                        switchToLogin();
                    }
                }
            }
            catch(Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }
        //event handler for button delete account with check
        private async void btnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want do delete your account?\nThis change is permament and cannot be reversed!", "Delete Account", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (await Database.deleteAccountAsync())
                {
                    switchToLogin();
                }
            }
        }
        public void log(string text)
        {
            lblMessage.Content = text;
        }
        #region visibility handling methods
        public void switchToEditWarehouse()
        {
            switchAllOff();
            ucEditWarehouse.Visibility = Visibility.Visible;
        }
        public void switchToCreateWarehouse()
        {
            switchAllOff();
            ucCreateWarehouse.Visibility = Visibility.Visible;
            ucManageWarehouses.Visibility = Visibility.Visible;
        }
        public void switchToAddOrder()
        {
            switchAllOff();
            ucEditWarehouse.Visibility = Visibility.Visible;
            ucAddOrder.Visibility = Visibility.Visible;
        }
        public void switchToHistography()
        {
            switchAllOff();
            ucHistography.Visibility = Visibility.Visible;
        }
        public void switchToLogin()
        {
            switchAllOff();
            ucLogin.Visibility = Visibility.Visible;
            txtBoxNewPassword.Password = "";
            txtBoxPassword.Password = "";
            txtBoxUsername.Text = "";
            Title = "Werhaus";
            btnSettings.Content = "Profile";
            flyoutProfile.IsOpen = false;
            loggedIn = false;
            currentOwner = null;
            Database.Token = null;
        }
        public void switchToManageWarehouses()
        {
            switchAllOff();
            ucManageWarehouses.Visibility = Visibility.Visible;
        }
        public void switchToRegister()
        {
            switchAllOff();
            ucRegister.Visibility = Visibility.Visible;
        }
        public void switchAllOff()
        {
            ucRegister.Visibility = Visibility.Collapsed;
            ucLogin.Visibility = Visibility.Collapsed;
            ucEditWarehouse.Visibility = Visibility.Collapsed;
            ucManageWarehouses.Visibility = Visibility.Collapsed;
            ucCreateWarehouse.Visibility = Visibility.Collapsed;
            ucAddOrder.Visibility = Visibility.Collapsed;
            ucHistography.Visibility = Visibility.Collapsed;
        }
        public void switchAllOn()
        {
            ucRegister.Visibility = Visibility.Visible;
            ucLogin.Visibility = Visibility.Visible;
            ucEditWarehouse.Visibility = Visibility.Visible;
            ucManageWarehouses.Visibility = Visibility.Visible;
            ucCreateWarehouse.Visibility = Visibility.Visible;
            ucAddOrder.Visibility = Visibility.Visible;
        }
        #endregion
        ~MainWindow()
        {
            Database.logoutAsync();
        }

    }
}
