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
using WerhausManager;

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
            try
            {
                if (order.products.Count > 0)
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
                        List<ProductBase> keys = order.products.Keys.ToList();
                        foreach (ProductBase key in keys)
                        {
                            order.products[key] *= -1;
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
            catch (Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            clearVariables();
            main.switchToEditWarehouse();
        }
        private bool AmountOverflows(Product product)
        {
            if (!purchase && (order.products[cbProducts.SelectedItem as ProductBase] + int.Parse(txtBoxAmount.Text)) > product.Amount)
                return true;
            return false;
        }
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = null;
                if (!String.IsNullOrWhiteSpace(txtBoxAmount.Text))
                {
                    if (cbProducts.SelectedItem != null)
                    {
                        if (!purchase)
                        {
                            product = main.ucEditWarehouse.Warehouse.Products.Where(x => x.Id == (cbProducts.SelectedItem as ProductBase).Id).ElementAt(0);
                            //if ((int.Parse(txtBoxAmount.Text)) > product.Amount)
                            //    txtBoxAmount.Text = product.Amount.ToString();
                        }
                        if (!order.products.ContainsKey(cbProducts.SelectedItem as ProductBase))
                        {
                            if(!purchase)
                            if (int.Parse(txtBoxAmount.Text) > product.Amount)
                                throw new Exception("Cannot sell more wares than stored!");
                            order.products.Add(cbProducts.SelectedItem as ProductBase, int.Parse(txtBoxAmount.Text));
                            UpdateAllOrderedProductsInformation();
                        }
                        else
                        {
                            if (AmountOverflows(product))
                                throw new Exception("Cannot sell more wares than stored!");
                            order.products[cbProducts.SelectedItem as ProductBase] += int.Parse(txtBoxAmount.Text);
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
            catch (Exception ex)
            {
                configManager.showErrorMessage(ex);
            }
        }
        private void UpdateAllOrderedProductsInformation()
        {
            listBoxSummary.Items.Clear();
            foreach (KeyValuePair<ProductBase, int> product in order.products)
            {
                listBoxSummary.Items.Add(new Product(product.Key.Id, product.Key.Name, product.Key.Description, 0, 0, product.Value));
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
                configManager.showErrorMessage(ex);
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
            if (!(cbProducts.SelectedItem == null))
            {
                if (!String.IsNullOrWhiteSpace(txtBoxAmount.Text))
                {

                    ProductBase product = cbProducts.SelectedItem as ProductBase;
                    if (order.products.ContainsKey(product))
                    {
                        if (((order.products[product]) - int.Parse(txtBoxAmount.Text) > 0))
                        {
                            order.products[product] -= int.Parse(txtBoxAmount.Text);
                            UpdateAllOrderedProductsInformation();
                        }
                        else
                        {
                            order.products.Remove(product);
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
                    MessageBox.Show("Not a valid amount!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No product selected!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void cbProducts_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!purchase && main.ucEditWarehouse.listBoxProducts.Items.Contains(cbProducts.SelectedItem))
            {
                txtBoxAmount.Text = main.ucEditWarehouse.Warehouse.Products.Find(x => x.Id == (cbProducts.SelectedItem as ProductBase).Id).Amount.ToString();
            }
            else
            {
                List<Product> p = listBoxSummary.Items.Cast<Product>().ToList().Where(x => x.Name == ((ProductBase)cbProducts.SelectedItem).Name).ToList();
                if (p.Count > 0)
                {
                    txtBoxAmount.Text = p[0].Amount.ToString();
                }
            }

        }

        private void listBoxSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxSummary.SelectedItem != null)
                if (purchase)
                    main.ucAddOrder.cbProducts.SelectedItem = cbProducts.Items.Cast<ProductBase>().ToList().Find(x => x.Id == ((Product)listBoxSummary.SelectedItem).Id);
                else
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