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
        public ProductView()
        {
            InitializeComponent();
            _productService = new ProductService();
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
            await LoadProduct();
        }
        private async Task LoadProduct()
        {

            ProductGrid.ItemsSource = await _productService.GetAllAsync();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            PopupOverlay.Visibility = Visibility.Visible;
        }

        private void CancelPopup_Click(object sender, RoutedEventArgs e)
        {
            PopupOverlay.Visibility = Visibility.Collapsed;
        }

        private async void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var product = new Product
                {
                    Name = txtName.Text,
                    Category = txtCategory.Text,
                    Barcode = txtBarcode.Text,
                    Price = decimal.TryParse(txtPrice.Text, out var price) ? price : 0,
                    Stock = int.TryParse(txtStock.Text, out var stock) ? stock : 0,
                    Description = txtDescription.Text
                };

                await _productService.AddAsync(product);
                await LoadProduct(); // refresh lại datagrid
                PopupOverlay.Visibility = Visibility.Collapsed;

                MessageBox.Show("Đã thêm sản phẩm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnScanBarcode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Mở chức năng quét mã vạch ở đây!");
            // TODO: thêm logic quét mã vạch thật (nếu có)
        }

    }
}
