using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace StoreBT.Views
{
    public partial class HomeView : UserControl
    {
        private readonly DispatcherTimer _timer;
        public HomeView()
        {
            InitializeComponent();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) =>
            {
                txtTimeContent.Text = DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            };
            _timer.Start();

        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
                border.Opacity = 0.8;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
                border.Opacity = 1;
        }

        private void Product_Click(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new ProductView();
            }
        }
        private void Customer_Click(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                var customerView = App.Services.GetRequiredService<CustomerView>();
                main.MainVM.CurrentView = customerView;
            }
        }

        private void Order_Click(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainVM.CurrentView = new OrderView();
            }
        }

    }
}
