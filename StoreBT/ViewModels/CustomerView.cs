using StoreBT.Models;
using StoreBT.Services;
using StoreBT.Services.Interfaces;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreBT.Views
{

    public partial class CustomerView : UserControl
    {
        private readonly ICustomerService _customerService;
        public CustomerView()
        {
            InitializeComponent();
            _customerService = new CustomerService();
            this.Loaded += CustomerView_Loaded;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang Home
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new HomeView();
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerPopup.IsOpen = true;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                MessageBox.Show($"Sửa khách hàng: {product.Name}");
                // TODO: Mở popup sửa sản phẩm
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.DataContext as Customer;
            if (customer != null)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa '{customer.Name}'?", "Xác nhận xóa",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _customerService.DeleteAsync(customer.Id);
                    CustomerGrid.Items.Refresh();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();

            //if (ProductGrid.ItemsSource is IEnumerable<Product> products)
            //{

            //}
        }

        private async void CustomerView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCustomer();
        }
        private async Task LoadCustomer()
        {

            CustomerGrid.ItemsSource = await _customerService.GetAllAsync();
        }


        private void CancelPopup_Click(object sender, RoutedEventArgs e)
        {
            CustomerPopup.IsOpen = false;
        }

        private async void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = new Customer
                {
                    Name = txtName.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text,
                };

                await _customerService.AddAsync(customer);
                await LoadCustomer(); // refresh lại datagrid
                CustomerPopup.IsOpen = false;

                MessageBox.Show("Đã thêm khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
