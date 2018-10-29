﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for addOrder.xaml
    /// </summary>
    public partial class addOrder : UserControl
    {
        public bool purchase = false;
        Order order = new Order();
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public addOrder()
        {
            InitializeComponent();
        }

        private async void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (order.Amounts.Count > 0)
            {
                if (purchase)
                {
                    if (await Database.addOrderAsync(order))
                    {
                        main.ucEditWarehouse.listBoxProducts.ItemsSource = await Database.getProductsOfWarehouseAsync(order.IdWarehouse);
                        main.switchToEditWarehouse();
                        clearVariables();
                    }
                    else
                        MessageBox.Show("Not enough capacity!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    List<ProductBase> keys = order.Amounts.Keys.ToList();
                    foreach (ProductBase key in keys)
                    {
                        order.Amounts[key] *= -1;
                    }
                    if (await Database.addOrderAsync(order))
                    {
                        main.ucEditWarehouse.listBoxProducts.ItemsSource = await Database.getProductsOfWarehouseAsync(order.IdWarehouse);
                        main.switchToEditWarehouse();
                        clearVariables();
                    }
                    else
                        MessageBox.Show("Not enough capacity!");
                }
            }
            else
            {
                MessageBox.Show("No products ordered!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            clearVariables();
            main.switchToEditWarehouse();
        }
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxAmount.Text))
            {
                if (!(cbProducts.SelectedItem == null))
                {
                    Product product = (Product)main.ucEditWarehouse.Warehouse.Products.Where(x => x.Id == (cbProducts.SelectedItem as ProductBase).Id).ElementAt(0);
                    if (!purchase && (int.Parse(txtBoxAmount.Text)) > product.Amount)
                        txtBoxAmount.Text = product.Amount.ToString();         
                        if (!order.Amounts.ContainsKey(cbProducts.SelectedItem as ProductBase))
                        {
                            order.Amounts.Add(cbProducts.SelectedItem as ProductBase, int.Parse(txtBoxAmount.Text));
                            UpdateAllOrderedProductsInformation();
                        }
                        else
                        {
                            order.Amounts[cbProducts.SelectedItem as ProductBase] += int.Parse(txtBoxAmount.Text);
                            UpdateAllOrderedProductsInformation();
                        }
                }
                else
                {
                    MessageBox.Show("No product selected!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Not a valid amount!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void UpdateAllOrderedProductsInformation()
        {
            listBoxSummary.Items.Clear();
            foreach (KeyValuePair<ProductBase, int> product in order.Amounts)
            {
               listBoxSummary.Items.Add(new Product(0, product.Key.Name, null, 0, 0, product.Value));
            }
            listBoxSummary.Items.Refresh();
        }
        private async void VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                clearVariables();
                if (purchase)
                    cbProducts.ItemsSource = await Database.getAllProductsOfCatalogAsync();
                else
                    cbProducts.ItemsSource = main.ucEditWarehouse.listBoxProducts.ItemsSource;
                order = new Order();
                main.ucEditWarehouse.Warehouse.Products = main.ucEditWarehouse.listBoxProducts.ItemsSource as List<Product>;
                order.IdWarehouse = (main.ucManageWarehouses.listBoxWarehouses.SelectedItem as Warehouse).Id;
                cbProducts.Items.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //check for textinput on amount field to only allow numbers
        private void textInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxAmount.Text))
            {
                if (!(cbProducts.SelectedItem == null))
                {
                    ProductBase product = cbProducts.SelectedItem as ProductBase;
                    if (order.Amounts.ContainsKey(product))
                    {
                        if (((order.Amounts[product]) - int.Parse(txtBoxAmount.Text) > 0))
                        {
                            order.Amounts[product] -= int.Parse(txtBoxAmount.Text);
                            UpdateAllOrderedProductsInformation();                        }
                        else
                        {
                            order.Amounts.Remove(product);
                            txtBoxAmount.Text = "";
                            UpdateAllOrderedProductsInformation();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Product was not ordered!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No product selected!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Not a valid amount!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void cbProducts_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!purchase)
            {
                if (main.ucEditWarehouse.listBoxProducts.Items.Contains(cbProducts.SelectedItem))
                {
                    txtBoxAmount.Text = main.ucEditWarehouse.Warehouse.Products.Find(x => x.Id == (cbProducts.SelectedItem as ProductBase).Id).Amount.ToString();
                }
                else
                    txtBoxAmount.Text = "";
            }
        }

        private void listBoxSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listBoxSummary.SelectedItem != null)
            main.ucAddOrder.cbProducts.SelectedItem = main.ucEditWarehouse.Warehouse.Products.Find(x => x.Name == (listBoxSummary.SelectedItem as ProductBase).Name);
        }
        private void clearVariables()
        {
            txtBoxAmount.Text = "";
            listBoxSummary.Items.Clear();
            order = new Order();
        }
    }
}