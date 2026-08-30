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

            // タイマー初期化
            InitializeTimer();

            // タイマー開始
            _timer.Start();

            // ウィンドウが閉じられる際にタイマーを停止
            Closed += OnWindowClosed;

            // レイアウト完了後にウィンドウ位置を復元（ActualWidth/ActualHeight が確定してから）
            Loaded += OnWindowLoaded;
        }

        /// <summary>
        /// ウィンドウLoadedイベントハンドラー
        /// </summary>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnWindowLoaded;
            RestoreWindowPosition();

            // 位置保存を有効化
            _locationSaveEnabled = true;
        }

        /// <summary>
        /// 保存済みのウィンドウ位置を復元する
        /// </summary>
        private void RestoreWindowPosition()
        {
            var left = Settings.Default.WindowLeft;
            var top = Settings.Default.WindowTop;

            if (!double.IsNaN(left) && !double.IsInfinity(left)
                && !double.IsNaN(top) && !double.IsInfinity(top))
            {
                var clampedPosition = WindowPositionHelper.ClampToVirtualScreen(
                    requestedLeft: left,
                    requestedTop: top,
                    actualWidth: ActualWidth,
                    actualHeight: ActualHeight,
                    width: Width,
                    height: Height,
                    screenLeft: SystemParameters.VirtualScreenLeft,
                    screenTop: SystemParameters.VirtualScreenTop,
                    screenWidth: SystemParameters.VirtualScreenWidth,
                    screenHeight: SystemParameters.VirtualScreenHeight);

                Left = clampedPosition.Left;
                Top = clampedPosition.Top;
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
            if (WindowPositionHelper.CanPersistLocation(_locationSaveEnabled, IsLoaded))
            {
                Settings.Default.WindowLeft = Left;
                Settings.Default.WindowTop = Top;
                Settings.Default.Save();
            }

            // イベントハンドラーを解除
            _timer.Tick -= OnTimerTick;
            Closed -= OnWindowClosed;

            // タイマー停止
            _timer.Stop();
        }

        /// <summary>
        /// マウス左ボタン押下イベントハンドラー（ウィンドウドラッグ）
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender,
            System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton != System.Windows.Input.MouseButtonState.Pressed)
            {
                return;
            }

            try
            {
                DragMove();
            }
            catch (InvalidOperationException)
            {
                // 入力タイミング競合時のクラッシュ防止
            }
        }

        /// <summary>
        /// コンテキストメニュー「閉じる」クリックイベントハンドラー
        /// </summary>
        private void ClickMenu_Click(object sender, RoutedEventArgs e) => Close();
    }
}
