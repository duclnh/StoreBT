using StoreBT.Models;
using StoreBT.Services;
using StoreBT.Services.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreBT.Views
{

    public partial class CustomerView : UserControl
    {
        private readonly ICustomerService _customerService;


        private Customer? CustomerSelected = null;

        public CustomerView(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
            Loaded += CustomerView_Loaded;
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
            txtName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var customer = button?.DataContext as Customer;
            if (customer != null)
            {
                CustomerSelected = customer;
                CustomerPopup.IsOpen = true;
                txtName.Text = customer.Name;
                txtPhone.Text = customer.Phone; ;
                txtAddress.Text = customer?.Address;
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
                    _customerService.DeleteAsync(customer);
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
                string name = txtName.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string address = txtAddress.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(phone))
                {
                    MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPhone.Focus();
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9]{9,11}$"))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPhone.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(address))
                {
                    MessageBox.Show("Địa chỉ không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtAddress.Focus();
                    return;
                }
                if (CustomerSelected is null)
                {
                    var customer = new Customer
                    {
                        Name = name,
                        Phone = phone,
                        Address = address,
                    };

                    await _customerService.AddAsync(customer);
                    await LoadCustomer();
                    CustomerPopup.IsOpen = false;

                    MessageBox.Show("Đã thêm khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {

                    CustomerSelected.Name = name;
                    CustomerSelected.Phone = phone;
                    CustomerSelected.Address = address;

                    var result = await _customerService.UpdateAsync(CustomerSelected);
                    if (result == 1)
                    {
                        await LoadCustomer();
                        CustomerPopup.IsOpen = false;
                        MessageBox.Show("Cập nhật khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật khách hàng thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                CustomerSelected = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
