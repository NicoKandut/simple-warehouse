using System;
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
                        Visibility = Visibility.Collapsed;
                        txtBoxAmount.Text = "";
                        lblSummary.Text = "";
                        order = new Order();
                    }
                    else
                        MessageBox.Show("Not enough capacity!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    foreach (KeyValuePair<ProductBase, int> product in order.Amounts)
                    {
                        order.Amounts[product.Key] *= -1;
                    }
                    if (await Database.addOrderAsync(order))
                    {
                        main.ucEditWarehouse.listBoxProducts.ItemsSource = await Database.getProductsOfWarehouseAsync(order.IdWarehouse);
                        Visibility = Visibility.Collapsed;
                        txtBoxAmount.Text = "";
                        lblSummary.Text = "";
                        order = new Order();
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
            txtBoxAmount.Text = "";
            lblSummary.Text = "";
            Visibility = Visibility.Collapsed;
            order = new Order();
        }
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtBoxAmount.Text))
            {
                if (!(cbProducts.SelectedItem == null))
                {
                    if (!order.Amounts.ContainsKey(cbProducts.SelectedItem as ProductBase))
                    {
                        order.Amounts.Add(cbProducts.SelectedItem as ProductBase, int.Parse(txtBoxAmount.Text));
                        lblSummary.Text = getAllOrderedProductsInformation();
                    }
                    else
                    {
                        order.Amounts[cbProducts.SelectedItem as ProductBase] += int.Parse(txtBoxAmount.Text);
                        lblSummary.Text = getAllOrderedProductsInformation();
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
        private string getAllOrderedProductsInformation()
        {
            string result = "";
            foreach (KeyValuePair<ProductBase, int> product in order.Amounts)
            {
                result += product.Key.Name + " | Amount: " + product.Value + "\n";
            }
            return result;
        }
        private async void VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (purchase)
                    cbProducts.ItemsSource = await Database.getAllProductsOfCatalogAsync();
                else
                    cbProducts.ItemsSource = main.ucEditWarehouse.listBoxProducts.ItemsSource;
                order.IdWarehouse = (main.ucManageWarehouses.listBoxWarehouses.SelectedItem as Warehouse).Id;
                cbProducts.Items.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
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
                            lblSummary.Text = getAllOrderedProductsInformation();
                        }
                        else
                        {
                            order.Amounts.Remove(product);
                            lblSummary.Text = getAllOrderedProductsInformation();
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
            }
        }
    }
}