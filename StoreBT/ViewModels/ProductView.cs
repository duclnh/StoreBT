using StoreBT.Models;
using StoreBT.Services;
using StoreBT.Services.Interfaces;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoreBT.Views
{
    public partial class ProductView : UserControl
    {
        private readonly IProductService _productService;
        private Product? _selectedProduct = null;
        private readonly IOrderItemService _orderItemService;
        public ProductView(IProductService productService, IOrderItemService orderItemService)
        {
            InitializeComponent();
            _productService = productService;
            _orderItemService = orderItemService;
            this.Loaded += ProductView_Loaded;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang Home
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new HomeView();
            }
        }

        private async void ProductView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadProducts();
        }
        private async Task LoadProducts()
        {

            ProductGrid.ItemsSource = await _productService.SearchAsync(txtSearch.Text.Trim());
        }

        private async void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBarcode.Text))
                {
                    MessageBox.Show(Application.Current.MainWindow, "Mã vạch không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }
                var productExist = await _productService.GetProductByBarCode(txtBarcode.Text.Trim());
                if (productExist is not null && (_selectedProduct is not null ? productExist.Id != _selectedProduct.Id : true))
                {
                    MessageBox.Show(Application.Current.MainWindow, "Mã vạch đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }


                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show(Application.Current.MainWindow, "Tên sản phẩm không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCategory.Text))
                {
                    MessageBox.Show(Application.Current.MainWindow, "Vui lòng nhập loại sản phẩm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtCategory.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out var price) || price < 0)
                {
                    MessageBox.Show(Application.Current.MainWindow, "Giá sản phẩm không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtPrice.Focus();
                    return;
                }

                if (!int.TryParse(txtStock.Text, out var stock) || stock < 0)
                {
                    MessageBox.Show(Application.Current.MainWindow, "Số lượng tồn kho không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtStock.Focus();
                    return;
                }


                if (_selectedProduct == null)
                {
                    var product = new Product
                    {
                        Name = txtName.Text.Trim(),
                        Category = txtCategory.Text.Trim(),
                        Barcode = txtBarcode.Text.Trim(),
                        Price = price,
                        Stock = stock,
                        Description = txtDescription.Text.Trim(),
                    };

                    await _productService.AddAsync(product);
                    MessageBox.Show(Application.Current.MainWindow, "Đã thêm sản phẩm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _selectedProduct.Name = txtName.Text.Trim();
                    _selectedProduct.Category = txtCategory.Text.Trim();
                    _selectedProduct.Barcode = txtBarcode.Text.Trim();
                    _selectedProduct.Price = price;
                    _selectedProduct.Stock = stock;
                    _selectedProduct.Description = txtDescription.Text.Trim();
                    _selectedProduct.UpdatedAt = DateTime.Now;

                    await _productService.UpdateAsync(_selectedProduct);
                    MessageBox.Show(Application.Current.MainWindow, "Đã cập nhật sản phẩm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                    _selectedProduct = null;
                }

                await LoadProducts();
                ClosePopUp();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
        private void OpenProductOverlay_Click(object sender, RoutedEventArgs e)
        {
            ProductOverlay.Visibility = Visibility.Visible;
        }

        private void CancelProductOverlay_Click(object sender, RoutedEventArgs e)
        {
            ClosePopUp();
        }

        private void ClosePopUp()
        {
            ProductOverlay.Visibility = Visibility.Collapsed;
            txtName.Text = "";
            txtCategory.Text = "";
            txtBarcode.Text = "";
            txtPrice.Text = "";
            txtStock.Text = "";
            txtDescription.Text = "";

            _selectedProduct = null;

            ProductOverlay.Visibility = Visibility.Collapsed;
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
                txtBarcode.Text = scanner.ScannedCode;
                if (ProductOverlay.Visibility == Visibility.Collapsed)
                {
                    txtSearch.Text = scanner.ScannedCode;
                }
                else
                {
                    txtBarcode.Text = scanner.ScannedCode;
                }
            }
        }


        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();
            ProductGrid.ItemsSource = await _productService.SearchAsync(keyword);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                txtName.Text = product.Name;
                txtCategory.Text = product.Category;
                txtBarcode.Text = product.Barcode;
                txtPrice.Text = product.Price.ToString();
                txtStock.Text = product.Stock.ToString();
                txtDescription.Text = product.Description;
                _selectedProduct = product;

                ProductOverlay.Visibility = Visibility.Visible;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Product;
            if (product != null)
            {
                var anyProduct = await _orderItemService.AnyAsync(product.Id);

                if (anyProduct)
                {
                    MessageBox.Show($"Sản phẩm '{product.Name}' đang được sử dụng trong đơn hàng, không thể xóa!",
                                    "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Ngưng xóa
                }

                if (MessageBox.Show($"Bạn có chắc muốn xóa '{product.Name}'?", "Xác nhận xóa",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    await _productService.DeleteAsync(product);
                    await LoadProducts();
                }
            }
        }

    }
}
