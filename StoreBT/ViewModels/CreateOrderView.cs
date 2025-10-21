using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using StoreBT.Models;
using StoreBT.Services;
using StoreBT.Services.Interfaces;

namespace StoreBT.Views
{
    public partial class CreateOrderView : UserControl
    {
        private List<OrderItem> _cart = new();
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService; 
        public CreateOrderView(ICustomerService customerService, IProductService productService)
        {
            _productService = productService;
            _customerService = customerService;
            InitializeComponent();
             this.Loaded += CreateOrderView_Loaded; 
          
        }


        private async void CreateOrderView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProducts();
            await LoadCustomers();
        }

        // =================== LOAD DỮ LIỆU ===================
        private async Task LoadProducts()
        {
            ProductGrid.ItemsSource = await _productService.SearchAsync("");
        }

        private async Task LoadCustomers()
        {
            CustomerComboBox.ItemsSource = await _customerService.SearchAsync("");
        }

        // =================== SỰ KIỆN CHỌN KHÁCH HÀNG ===================
        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = CustomerComboBox.SelectedItem as Customer;
            if (selected is not null)
            {
                txtName.Text = selected.Name;
                txtPhone.Text = selected.Phone;
                txtAddress.Text = selected.Address;

            }
            else
            {
                txtName.Text = "";
                txtPhone.Text = "";
                txtAddress.Text = "";
            }
        }

        // =================== GIẢM GIÁ & TÍNH TỔNG ===================
        private void DiscountInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = _cart.Sum(i => i.UnitPrice * i.Quantity);
            decimal.TryParse(DiscountInput.Text, out decimal discount);
            if (discount < 0) discount = 0;
            decimal final = total - discount;
            if (final < 0) final = 0;

            TotalAmountText.Text = $"{final:N0} ₫";
        }

        // =================== CHỌN HÀNG HÓA ===================
        private void ProductGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selected = ProductGrid.SelectedItem as Product;
            if (selected == null) return;

            var existing = _cart.FirstOrDefault(c => c.ProductId == selected.Id);
            if (existing != null)
            {
               
            }
            else
            {
                _cart.Add(new OrderItem
                {
                    ProductId = selected.Id,
                    Product = selected,
                    UnitPrice = selected.Price,
                    Quantity = 1
                });
            }

            RefreshCart();
        }

        private void RefreshCart()
        {
            CartGrid.ItemsSource = null;
            CartGrid.ItemsSource = _cart;
            UpdateTotal();
        }

        // =================== TẠO ĐƠN HÀNG ===================
        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 sản phẩm.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedCustomer = CustomerComboBox.SelectedItem as Customer;
            Customer customer;

            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập khách hàng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (selectedCustomer.Id == Guid.Empty)
            {
                // Tạo mới khách hàng
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng mới.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = txtName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };
            }
            else
            {
                customer = selectedCustomer;
            }

            decimal.TryParse(DiscountInput.Text, out decimal discount);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderCode = $"DH{DateTime.Now:yyyyMMdd_HHmmss}",
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                CustomerPhone = customer.Phone,
                OrderDate = DateTime.Now,
                TotalAmount = _cart.Sum(p => p.UnitPrice * p.Quantity),
                Discount = discount,
                CreatedAt = DateTime.Now,
                Items = _cart.ToList()
            };

            MessageBox.Show($"✅ Đã tạo đơn hàng {order.OrderCode}\nKhách: {customer.Name}\nTổng: {TotalAmountText.Text}",
                            "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

            // Reset lại form
            _cart.Clear();
            RefreshCart();
            DiscountInput.Text = "0";
            CustomerComboBox.SelectedIndex = 0;
            txtName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
        }

        // =================== QUAY LẠI ===================
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang Home
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new HomeView();
            }
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();

            ProductGrid.ItemsSource = await _productService.SearchAsync(keyword);
        }

        private void btnScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở chức năng quét mã vạch ở đây!");
            // TODO: thêm logic quét mã vạch thật (nếu có)
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderItem = button?.DataContext as OrderItem;
            if (orderItem != null)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa '{orderItem.ProductId}'?", "Xác nhận xóa",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _cart.Remove(orderItem);
                    RefreshCart();
                }
            }

        }
    }
}
