using Microsoft.Extensions.DependencyInjection;
using StoreBT.Models;
using StoreBT.Services;
using StoreBT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace StoreBT.Views
{
    public partial class CreateOrderView : UserControl
    {
        private List<OrderItem> _cart = new();
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        public CreateOrderView(ICustomerService customerService,
            IProductService productService, IOrderService orderService,
            IOrderItemService orderItemService)
        {
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            InitializeComponent();
            this.Loaded += CreateOrderView_Loaded;
        }


        private async void CreateOrderView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProducts();
            //await LoadCustomers();
        }

        // =================== LOAD DỮ LIỆU ===================
        private async Task LoadProducts()
        {
            ProductGrid.ItemsSource = await _productService.SearchAsync(txtSearch.Text.Trim());
        }

        //private async Task LoadCustomers()
        //{
        //    CustomerComboBox.ItemsSource = await _customerService.SearchAsync("");
        //}

        // =================== SỰ KIỆN CHỌN KHÁCH HÀNG ===================
        //private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selected = CustomerComboBox.SelectedItem as Customer;
        //    if (selected is not null)
        //    {
        //        txtName.Text = selected.Name;
        //        txtPhone.Text = selected.Phone;
        //        txtAddress.Text = selected.Address;

        //    }
        //    else
        //    {
        //        txtName.Text = "";
        //        txtPhone.Text = "";
        //        txtAddress.Text = "";
        //    }
        //}

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
            if (existing == null)
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
        private async void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 sản phẩm.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //var selectedCustomer = CustomerComboBox.SelectedItem as Customer;
            //Customer customer;

            //if (selectedCustomer == null)
            //{
            //    string name = txtName.Text.Trim();
            //    string phone = txtPhone.Text.Trim();
            //    string address = txtAddress.Text.Trim();
            //    if (string.IsNullOrWhiteSpace(name))
            //    {
            //        MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        txtName.Focus();
            //        return;
            //    }

            //    if (string.IsNullOrWhiteSpace(phone))
            //    {
            //        MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        txtPhone.Focus();
            //        return;
            //    }

            //    if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9]{9,11}$"))
            //    {
            //        MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        txtPhone.Focus();
            //        return;
            //    }

            //    if (string.IsNullOrWhiteSpace(address))
            //    {
            //        MessageBox.Show("Địa chỉ không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        txtAddress.Focus();
            //        return;
            //    }

            //    customer = new Customer
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = name,
            //        Phone = phone,
            //        Address = address
            //    };
            //}
            //else
            //{
            //    customer = selectedCustomer;
            //}

            decimal.TryParse(DiscountInput.Text, out decimal discount);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderCode = $"DH{DateTime.Now:yyyyMMdd_HHmmss}",
                //CustomerId = customer.Id,
                //CustomerName = customer.Name,
                //CustomerPhone = customer.Phone,
                //CustomerAddress = customer.Address,
                //Customer = customer,
                OrderDate = DateTime.Now,
                Notes = txtNote.Text.Trim(),
                TotalAmount = _cart.Sum(p => p.UnitPrice * p.Quantity),
                Discount = discount,
                CreatedAt = DateTime.Now,
            };

            var result = await _orderService.AddAsync(order);
            if (result > 0)
            {
                await _orderItemService.AddRangeAsync(
                 _cart.Select(x =>
                     {
                         x.OrderId = order.Id;
                         return x;
                     }).ToList()
                 );
                MessageBox.Show($"✅ Đã tạo đơn hàng {order.OrderCode}\nTổng: {TotalAmountText.Text}",
                    "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset lại form
                _cart.Clear();
                RefreshCart();
                DiscountInput.Text = "0";
                txtNote.Text = "";
                //CustomerComboBox.SelectedIndex = 0;
                //txtName.Text = "";
                //txtPhone.Text = "";
                //txtAddress.Text = "";
                await LoadProducts();
            }
            else
            {
                MessageBox.Show("Tạo đơn hàng thất bại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // =================== QUAY LẠI ===================
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang Home
            if (Application.Current.MainWindow is MainWindow main)
            {
                var customerView = App.Services.GetRequiredService<OrderView>();
                main.MainVM.CurrentView = customerView;
            }
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();

            ProductGrid.ItemsSource = await _productService.SearchAsync(keyword);
        }

        private void btnScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            var scanner = new BarcodeScannerWindowView
            {
                Owner = Window.GetWindow(this)
            };

            bool? result = scanner.ShowDialog();

            if (result == true && !string.IsNullOrWhiteSpace(scanner.ScannedCode))
            {
                var productList = ProductGrid.ItemsSource as List<Product>;
                if (productList != null)
                {
                    var found = productList.FirstOrDefault(p => p.Barcode == scanner.ScannedCode);
                    if (found != null)
                    {
                        var existing = _cart.FirstOrDefault(c => c.ProductId == found.Id);
                        if (existing == null)
                        {
                            _cart.Add(new OrderItem
                            {
                                ProductId = found.Id,
                                Product = found,
                                UnitPrice = found.Price,
                                Quantity = 1
                            });
                        }

                        RefreshCart();
                    }
                    else
                    {
                        MessageBox.Show($"Không tìm thấy sản phẩm",
                                            "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderItem = button?.DataContext as OrderItem;
            if (orderItem != null)
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa '{orderItem.Product.Name}'?", "Xác nhận xóa",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _cart.Remove(orderItem);
                    RefreshCart();
                }
            }
        }

        private bool _suppressCellEdit = false;

        private void MyGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (_suppressCellEdit) return; // ⛔ Bỏ qua nếu đang tạm ngưng

            if (e.Column.Header.ToString() == "SL")
            {
                var textBox = e.EditingElement as TextBox;
                if (textBox != null && int.TryParse(textBox.Text, out int newValue))
                {
                    var item = e.Row.Item as OrderItem;
                    if (item is not null && item.Product is not null)
                    {
                        if (newValue > item.Product.Stock)
                        {
                            MessageBox.Show($"Số lượng vượt quá tồn kho ({item.Product.Stock})!",
                                            "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                            // ⏸ Tạm tắt lắng nghe trước khi cập nhật để tránh vòng lặp
                            _suppressCellEdit = true;
                            item.Quantity = item.Product.Stock;
                            textBox.Text = item.Product.Stock.ToString();
                            CartGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                            CartGrid.CommitEdit(DataGridEditingUnit.Row, true);
                            _suppressCellEdit = false; // ✅ Bật lại
                        }
                        else
                        {
                            item.Quantity = newValue;
                            RefreshCart();
                            UpdateTotal();
                        }
                    }
                }
                else
                {
                    textBox.Text = 1.ToString();
                    CartGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                    CartGrid.CommitEdit(DataGridEditingUnit.Row, true);
                    MessageBox.Show($"Số lượng không hợp lệ",
                                           "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
