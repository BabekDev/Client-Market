using Admin_Market.ViewModels;
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

namespace Admin_Market
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel model = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            LoadProduct();
            LoadSell();
            DataContext = new MainViewModel();
        }

        private void LoadProduct()
        {
            model.ConnectServer("LoadData");
            listBoxProduct.Foreground = Brushes.LightGray;
            foreach (var item in model.output)
            {
                listBoxProduct.Items.Add($"{item.Name} / {item.Price} $");
            }
        }
        private void LoadSell()
        {
            model.ConnectServer("LoadData_ADMIN");
            listBoxSell.Foreground = Brushes.LightGray;
            foreach (var item in model.output_sell)
            {
                listBoxSell.Items.Add($"{item.Name} / {item.Price}$ / {item.Sell}");
            }
        }
    }
}
