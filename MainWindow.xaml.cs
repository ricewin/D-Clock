using System;
using System.Windows;
using System.Windows.Threading;

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

        private void ClickMenu_Click(object sender, RoutedEventArgs e) => Close();
    }
}
