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
        private List<Product> _products = new();
        private List<OrderItem> _cart = new();
        private List<Customer> _customers = new();
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService; 
        public CreateOrderView(ICustomerService customerService)
        {
            _productService = new ProductService();
            _customerService = customerService; 
            InitializeComponent();
            LoadProducts();
            LoadCustomers();
        }

        // =================== LOAD DỮ LIỆU ===================
        private void LoadProducts()
        {
            _products = _productService.GetAllAsync().Result.ToList();
            ProductGrid.ItemsSource = _products;
        }

        private void LoadCustomers()
        {
            _customers = _customerService.GetAllAsync().Result.ToList();
            CustomerComboBox.ItemsSource = _customers;
            CustomerComboBox.SelectedIndex = 0;
        }

        // =================== SỰ KIỆN CHỌN KHÁCH HÀNG ===================
        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = CustomerComboBox.SelectedItem as Customer;
            if (selected != null && selected.Id == Guid.Empty)
            {
                // Khi chọn “Nhập khách hàng mới”
                NewCustomerPanel.Visibility = Visibility.Visible;
            }
            else
            {
                NewCustomerPanel.Visibility = Visibility.Collapsed;
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
                if (string.IsNullOrWhiteSpace(CustomerNameInput.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng mới.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = CustomerNameInput.Text.Trim(),
                    Phone = CustomerPhoneInput.Text.Trim(),
                    Address = CustomerAddressInput.Text.Trim()
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
            CustomerNameInput.Text = "";
            CustomerPhoneInput.Text = "";
            CustomerAddressInput.Text = "";
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();

            //if (ProductGrid.ItemsSource is IEnumerable<Product> products)
            //{

            //}
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
