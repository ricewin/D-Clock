using System;
using System.Windows;
using System.Windows.Threading;
using D_Clock.Properties;

namespace D_Clock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 時刻設定用のタイマー
        /// </summary>
        private readonly DispatcherTimer _timer = new();

        /// <summary>
        /// 位置保存を有効にするフラグ（初期化完了後に true にする）
        /// </summary>
        private bool _locationSaveEnabled;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // 保存済みのウィンドウ位置を復元
            RestoreWindowPosition();

            // タイマー初期化
            InitializeTimer();

            // タイマー開始
            _timer.Start();

            // 位置保存を有効化
            _locationSaveEnabled = true;

            // ウィンドウが閉じられる際にタイマーを停止
            Closed += OnWindowClosed;
        }

        /// <summary>
        /// 保存済みのウィンドウ位置を復元する
        /// </summary>
        private void RestoreWindowPosition()
        {
            var left = Settings.Default.WindowLeft;
            var top = Settings.Default.WindowTop;

            if (!double.IsNaN(left) && !double.IsNaN(top))
            {
                // 画面内に収まるようにクランプ
                var screenWidth = SystemParameters.VirtualScreenWidth;
                var screenHeight = SystemParameters.VirtualScreenHeight;
                var screenLeft = SystemParameters.VirtualScreenLeft;
                var screenTop = SystemParameters.VirtualScreenTop;

                Left = Math.Max(screenLeft, Math.Min(left, screenLeft + screenWidth - Width));
                Top = Math.Max(screenTop, Math.Min(top, screenTop + screenHeight - Height));
            }
        }

        /// <summary>
        /// タイマー初期化
        /// </summary>
        private void InitializeTimer()
        {
            // イベント発生間隔の設定
            _timer.Interval = TimeSpan.FromMilliseconds(500);

            // イベント登録
            _timer.Tick += OnTimerTick;
        }

        /// <summary>
        /// タイマーTickイベントハンドラー
        /// </summary>
        private void OnTimerTick(object? sender, EventArgs e)
        {
            // 現在時刻を設定
            TimeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// ウィンドウClosedイベントハンドラー
        /// </summary>
        private void OnWindowClosed(object? sender, EventArgs e)
        {
            // イベントハンドラーを解除
            _timer.Tick -= OnTimerTick;
            Closed -= OnWindowClosed;

            // タイマー停止
            _timer.Stop();
        }

        private void Window_MouseLeftButtonDown(object sender,
            System.Windows.Input.MouseButtonEventArgs e) => DragMove();

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            if (!_locationSaveEnabled) return;

            Settings.Default.WindowLeft = Left;
            Settings.Default.WindowTop = Top;
            Settings.Default.Save();
        }

        private void ClickMenu_Click(object sender, RoutedEventArgs e) => Close();
    }
}

