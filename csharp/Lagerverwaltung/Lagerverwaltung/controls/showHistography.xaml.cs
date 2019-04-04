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
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;

namespace Lagerverwaltung
{
    /// <summary>
    /// Interaction logic for showHistography.xaml
    /// </summary>
    public partial class showHistography : UserControl
    {
        public SeriesCollection collStock { get; set; }
        public SeriesCollection collInOut { get; set; }
        public SeriesCollection coll { get; set; }
        public double Step { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }
        MainWindow main = (MainWindow)Application.Current.MainWindow;
        public showHistography()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.switchToEditWarehouse();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                chart.Visibility = Visibility.Visible;
                chartPie.Visibility = Visibility.Hidden;
                chartInOut.Visibility = Visibility.Hidden;
                ChartValues<HistoryEntry> columns = new ChartValues<HistoryEntry>();
                List<HistoryEntry> entries = await Calculation.getHistographyData((main.ucEditWarehouse.Warehouse));
                foreach (HistoryEntry entry in entries)
                {                   
                    columns.Add(entry);
                }
                
                var dayConfig = Mappers.Xy<HistoryEntry>()
                .X(dayModel => dayModel.Timestamp.ToOADate())
                .Y(dayModel => dayModel.stock);
                coll = new SeriesCollection(dayConfig)
                    {
                        new StepLineSeries
                        {
                            Title = "Stock",
                            Values = columns
                        }
                    };
                YFormatter = value => value.ToString();
                XFormatter = value => DateTime.FromOADate(value).ToString();
                chart.Series = coll;
                chart.Update();
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace +"\nError happens right here", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void visibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(main.ucEditWarehouse.Warehouse != null)
            Button_Click_1(sender, null);
        }

        private async void btnStockShare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chart.Visibility = Visibility.Hidden;
                chartPie.Visibility = Visibility.Visible;
                chartInOut.Visibility = Visibility.Hidden;

                SeriesCollection newCollStock = new SeriesCollection();
                Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
                List<Product> products = await Database.getProductsOfWarehouseAsync(main.ucEditWarehouse.Warehouse.Id);
                int idx = 0;
                ChartValues<int> vals;
                int totalAmount = 0;

                for (; idx < products.Count; idx++)
                {
                    newCollStock.Add(new PieSeries() { Title = products[idx].Name, DataLabels = true, LabelPoint = labelPoint });
                    vals = new ChartValues<int>();
                    vals.Add(products[idx].Amount * products[idx].Space);
                    newCollStock[idx].Values = vals;
                    totalAmount += products[idx].Amount * products[idx].Space;
                }

                newCollStock.Add(new PieSeries() { Title = "Empty", DataLabels = true, LabelPoint = labelPoint, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8D8D8")) });
                vals = new ChartValues<int>();
                vals.Add(main.ucEditWarehouse.Warehouse.Capacity - totalAmount);
                newCollStock[idx].Values = vals;

                collStock = newCollStock;
                chartPie.Series = collStock;
                DataContext = this;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void btnStockChangeDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chart.Visibility = Visibility.Hidden;
                chartPie.Visibility = Visibility.Hidden;
                chartInOut.Visibility = Visibility.Visible;
                List<List<HistoryEntry>> inOutList = await Calculation.getInOutData(main.ucEditWarehouse.Warehouse);
                List<HistoryEntry> inData = new List<HistoryEntry>();
                List<HistoryEntry> outData = new List<HistoryEntry>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
