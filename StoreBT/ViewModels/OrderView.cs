using StoreBT.Models;
using StoreBT.Services;
using StoreBT.Services.Interfaces;
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

namespace StoreBT.Views
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        private readonly IOrderService _orderService;
        public OrderView()
        {
            InitializeComponent();
            _orderService = new OrderService();
            this.Loaded += OrderView_Loaded;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang Home
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new HomeView();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
         
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();

            //if (ProductGrid.ItemsSource is IEnumerable<Product> products)
            //{

            //}
        }

        private async void OrderView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrder();
        }
        private async Task LoadOrder()
        {

            OrderGrid.ItemsSource = await _orderService.GetAllAsync();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {

            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new CreateOrderView();
            }

        }
    }

}
