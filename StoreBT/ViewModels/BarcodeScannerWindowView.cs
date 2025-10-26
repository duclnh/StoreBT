using AForge.Video;
using AForge.Video.DirectShow;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;
using ZXing.Windows.Compatibility;

namespace StoreBT.Views;
public partial class BarcodeScannerWindowView : Window
{
    private VideoCaptureDevice _videoSource;
    private DispatcherTimer _timer;
    private IBarcodeReader _reader = new BarcodeReader();
    private Bitmap _currentFrame;
    private readonly object _frameLock = new object(); // lock để tránh race condition

    public string ScannedCode { get; private set; }

    public BarcodeScannerWindowView()
    {
        InitializeComponent();
        Loaded += BarcodeScannerWindow_Loaded;
        Closing += BarcodeScannerWindow_Closing;
    }

    private void BarcodeScannerWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Lấy camera mặc định
        var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        if (videoDevices.Count == 0)
        {
            MessageBox.Show("Không tìm thấy camera!");
            this.DialogResult = false;
            return;
        }

        _videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
        _videoSource.NewFrame += VideoSource_NewFrame;
        _videoSource.Start();

        // Timer để quét barcode
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
        // Clone frame để tránh threading issues
        lock (_frameLock)
        {
            _currentFrame?.Dispose();
            _currentFrame = (Bitmap)eventArgs.Frame.Clone();
        }

        // Cập nhật preview UI (nếu muốn)
        Dispatcher.Invoke(() =>
        {
            videoPreview.Source = BitmapToBitmapImage(_currentFrame);
        });
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        Bitmap frame;
        lock (_frameLock)
        {
            if (_currentFrame == null)
            {
                scanFrame.Stroke = System.Windows.Media.Brushes.Red; // chưa có frame → đỏ
                return;
            }
            frame = (Bitmap)_currentFrame.Clone();
        }

        try
        {
            // Lấy vị trí và kích thước khu vực scanFrame trên videoPreview
            double scaleX = (double)_currentFrame.Width / videoPreview.ActualWidth;
            double scaleY = (double)_currentFrame.Height / videoPreview.ActualHeight;

            int x = (int)(Canvas.GetLeft(scanFrame) * scaleX);
            int y = (int)(Canvas.GetTop(scanFrame) * scaleY);
            int width = (int)(scanFrame.Width * scaleX);
            int height = (int)(scanFrame.Height * scaleY);

            // Giới hạn vùng không vượt quá frame
            x = Math.Max(0, Math.Min(x, frame.Width - 1));
            y = Math.Max(0, Math.Min(y, frame.Height - 1));
            width = Math.Min(width, frame.Width - x);
            height = Math.Min(height, frame.Height - y);

            // Crop bitmap theo khu vực scanFrame
            var rect = new Rectangle(x, y, width, height);
            Bitmap cropped = frame.Clone(rect, frame.PixelFormat);

            // Chuyển bitmap cropped sang LuminanceSource
            var source = new BitmapLuminanceSource(cropped);
            var result = _reader.Decode(source);

            cropped.Dispose();

            if (result != null)
            {
                scanFrame.Stroke = System.Windows.Media.Brushes.Lime; // quét thành công
                ScannedCode = result.Text;

                // Log giá trị đọc được
                Debug.WriteLine($"[Barcode] {ScannedCode}");

                _timer.Stop();
                if (_videoSource.IsRunning)
                    _videoSource.SignalToStop();
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                scanFrame.Stroke = System.Windows.Media.Brushes.Red; // chưa quét
            }
        }
        finally
        {
            frame.Dispose();
        }
    }

    private void BarcodeScannerWindow_Closing(object sender, CancelEventArgs e)
    {
        if (_videoSource != null && _videoSource.IsRunning)
            _videoSource.SignalToStop();
    }

    // Helper convert Bitmap -> BitmapImage để hiển thị WPF
    private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
    {
        using (var memory = new System.IO.MemoryStream())
        {
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze(); // freeze để thread-safe
            return bitmapImage;
        }
    }
}
