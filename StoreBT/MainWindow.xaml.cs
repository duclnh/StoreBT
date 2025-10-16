using StoreBT.ViewModels;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StoreBT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainView MainVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainVM = new MainView();
            DataContext = MainVM;
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
                }
                else
                {
                    DragMove();
                }
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Max_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResizeBorder_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var pos = e.GetPosition(this);
            var resizeDir = IntPtr.Zero;

            const int HTLEFT = 10, HTRIGHT = 11, HTTOP = 12, HTTOPLEFT = 13,
                      HTTOPRIGHT = 14, HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;

            double edge = 5; // độ rộng vùng resize

            if (pos.X <= edge && pos.Y <= edge) resizeDir = (IntPtr)HTTOPLEFT;
            else if (pos.X >= ActualWidth - edge && pos.Y <= edge) resizeDir = (IntPtr)HTTOPRIGHT;
            else if (pos.X <= edge && pos.Y >= ActualHeight - edge) resizeDir = (IntPtr)HTBOTTOMLEFT;
            else if (pos.X >= ActualWidth - edge && pos.Y >= ActualHeight - edge) resizeDir = (IntPtr)HTBOTTOMRIGHT;
            else if (pos.X <= edge) resizeDir = (IntPtr)HTLEFT;
            else if (pos.X >= ActualWidth - edge) resizeDir = (IntPtr)HTRIGHT;
            else if (pos.Y <= edge) resizeDir = (IntPtr)HTTOP;
            else if (pos.Y >= ActualHeight - edge) resizeDir = (IntPtr)HTBOTTOM;

            if (resizeDir != IntPtr.Zero)
            {
                SendMessage(new WindowInteropHelper(this).Handle, 0x112, (IntPtr)(61440 + resizeDir.ToInt32()), IntPtr.Zero);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    }
}