﻿using MahApps.Metro.Controls;
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

        public void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to logout?", "Logout?", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                main.ucLogin.Visibility = Visibility.Visible;
                main.ucManageWarehouses.Visibility = Visibility.Collapsed;
                main.ucCreateWarehouse.Visibility = Visibility.Collapsed;
                main.ucRegister.Visibility = Visibility.Collapsed;
                main.loggedIn = false;
                main.Title = "Werhaus";
            }
        }

        private void btnAddWarehouse_Click(object sender, RoutedEventArgs e)
        {
            main.ucCreateWarehouse.Visibility = Visibility.Visible;
        }

        private void btnDeleteWarehouse_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NOT IMPLEMENTED");
        }
    }
}
