﻿using System;
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
    /// Interaction logic for editWarehouse.xaml
    /// </summary>
    public partial class editWarehouse : UserControl
    {
        public Warehouse Warehouse
        {
            get
            {
                return warehouse;
            }
            set
            {
                if (value != null)
                {
                    warehouse = value;
                    listBoxProducts.ItemsSource = warehouse.Products;
                    listBoxProducts.Items.Refresh();
                }
                else
                    throw new Exception("No warehouse selected!");
            }
        }
        private Warehouse warehouse;
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public editWarehouse()
        {
            InitializeComponent();
            if(warehouse != null)
            txtBoxDetails.Content = warehouse.Description;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            main.ucManageWarehouses.updateWarehouseList();
            main.switchToManageWarehouses();
        }

        private void btnBuyProduct_Click(object sender, RoutedEventArgs e)
        {
            main.ucAddOrder.purchase = true;
            main.ucAddOrder.lblTitle.Content = "Buy Product";
            main.switchToAddOrder();
        }

        private void btnSellProduct_Click(object sender, RoutedEventArgs e)
        {
            main.ucAddOrder.purchase = false;
            main.ucAddOrder.lblTitle.Content = "Sell Product";
            main.switchToAddOrder();
        }

        private void listBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (main.ucAddOrder.Visibility == Visibility.Visible && listBoxProducts.SelectedItem != null)
                {
                    main.ucAddOrder.cbProducts.SelectedItem = main.ucAddOrder.cbProducts.Items.Cast<ProductBase>().ToList().Find(x => x.Id == ((Product)listBoxProducts.SelectedItem).Id);
                    Product p = main.ucAddOrder.listBoxSummary.Items.Cast<Product>().ToList().Find(x => x.Id == ((ProductBase)main.ucAddOrder.cbProducts.SelectedItem).Id);
                    if (p != null)
                    {
                        main.ucAddOrder.txtBoxAmount.Text = p.Amount.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnHistography_Click(object sender, RoutedEventArgs e)
        {           
            main.switchToHistography();
        }

        private void visibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Warehouse = (main.ucManageWarehouses.listBoxWarehouses.SelectedItem as Warehouse);
            txtBoxDetails.Content = Warehouse.Description;
            lblTitel.Content = Warehouse.Name;
            
        }
    }
}
