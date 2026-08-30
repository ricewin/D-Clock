using System;

namespace D_Clock
{
    /// <summary>
    /// ウィンドウ位置のクランプと永続化判定を提供するヘルパークラス
    /// </summary>
    public static class WindowPositionHelper
    {
        /// <summary>
        /// 指定した座標をバーチャルスクリーン内に収まるようにクランプして返します
        /// </summary>
        public static (double Left, double Top) ClampToVirtualScreen(
            double requestedLeft,
            double requestedTop,
            double actualWidth,
            double actualHeight,
            double width,
            double height,
            double screenLeft,
            double screenTop,
            double screenWidth,
            double screenHeight)
        {
            var windowWidth = GetValidWindowSize(actualWidth, width);
            var windowHeight = GetValidWindowSize(actualHeight, height);

            var maxLeft = screenLeft + Math.Max(0d, screenWidth - windowWidth);
            var maxTop = screenTop + Math.Max(0d, screenHeight - windowHeight);

            var clampedLeft = Math.Max(screenLeft, Math.Min(requestedLeft, maxLeft));
            var clampedTop = Math.Max(screenTop, Math.Min(requestedTop, maxTop));

            return (clampedLeft, clampedTop);
        }

        /// <summary>
        /// ウィンドウ位置の永続化が可能かどうかを返します
        /// </summary>
        public static bool CanPersistLocation(bool isLocationSaveEnabled, bool isLoaded)
            => isLocationSaveEnabled && isLoaded;

        private static double GetValidWindowSize(double actualSize, double fallbackSize)
        {
            if (IsUsableSize(actualSize))
            {
                return actualSize;
            }

            if (IsUsableSize(fallbackSize))
            {
                return fallbackSize;
            }

            return 0d;
        }

        private static bool IsUsableSize(double value)
            => !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;
    }
}
