using Client_Market.ViewModels;
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

namespace Client_Market.Views
{
    /// <summary>
    /// Логика взаимодействия для MarketView.xaml
    /// </summary>
    public partial class MarketView : UserControl
    {

        public MarketView()
        {
            InitializeComponent();
            LoadProduct();
        }

        private void LoadProduct()
        {
            LoggerDebug.getInstance($"Active method Loadproduct (LoadProduct())");
            listBoxProduct.Foreground = Brushes.LightGray;
            listBoxProduct.FontSize = 24;
            foreach (var item in new MarketViewModel().output)
            {
                listBoxProduct.Items.Add($"{item.Name} / {item.Price} $");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoggerDebug.getInstance($"Mail check successful (Button_Click())");
            if (listBoxProduct.SelectedItem != null)
            {
                new MarketViewModel().SellProduct(listBoxProduct.SelectedIndex);
            }
        }
    }
}
