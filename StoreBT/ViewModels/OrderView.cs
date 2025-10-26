using Microsoft.Extensions.DependencyInjection;
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
        public OrderView(IOrderService orderService)
        {
            InitializeComponent();
            _orderService = orderService;
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
            var order = button?.DataContext as Order; // lấy Order từ dòng DataGrid

            if (order == null)
                return;

            if (Application.Current.MainWindow is MainWindow main)
            {
                var orderEditView = App.Services.GetRequiredService<OrderEditView>();

                orderEditView.LoadOrder(order.Id);

                main.MainVM.CurrentView = orderEditView;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.DataContext as Order;
            if (order != null)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa '{order.OrderCode}'?", "Xác nhận xóa",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var result = await _orderService.DeleteAsync(order);
                    if (result <= 0)
                    {
                        MessageBox.Show("Xóa đơn hàng thất bại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    await LoadOrder();
                }
            }
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();
            OrderGrid.ItemsSource = await _orderService.SearchAsync(keyword);
        }

        private async void OrderView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrder();
        }
        private async Task LoadOrder()
        {
       
            OrderGrid.ItemsSource = await _orderService.SearchAsync(txtSearch.Text.Trim());
        } 

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {

            if (Application.Current.MainWindow is MainWindow main)
            {
                var createOrderView = App.Services.GetRequiredService<CreateOrderView>();
                main.MainVM.CurrentView = createOrderView;
            }

        }
    }

}
