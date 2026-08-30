using D_Clock;

namespace D.Clock.Tests;

public class WindowPositionHelperTests
{
    [Fact]
    public void ClampToVirtualScreen_PartiallyOffscreenCoordinates_AreClampedWithinVirtualScreen()
    {
        var result = WindowPositionHelper.ClampToVirtualScreen(
            requestedLeft: 1900,
            requestedTop: 1000,
            actualWidth: 300,
            actualHeight: 200,
            width: 300,
            height: 200,
            screenLeft: 0,
            screenTop: 0,
            screenWidth: 1920,
            screenHeight: 1080);

        Assert.Equal(1620, result.Left);
        Assert.Equal(880, result.Top);
    }

    [Fact]
    public void ClampToVirtualScreen_NegativeVirtualScreenOrigin_IsHandledCorrectly()
    {
        var result = WindowPositionHelper.ClampToVirtualScreen(
            requestedLeft: -2500,
            requestedTop: -1400,
            actualWidth: 400,
            actualHeight: 300,
            width: 400,
            height: 300,
            screenLeft: -1920,
            screenTop: -1080,
            screenWidth: 3840,
            screenHeight: 2160);

        Assert.Equal(-1920, result.Left);
        Assert.Equal(-1080, result.Top);
    }

    [Fact]
    public void ClampToVirtualScreen_ActualSizeIsInvalid_FallsBackToWidthHeight()
    {
        var result = WindowPositionHelper.ClampToVirtualScreen(
            requestedLeft: 1900,
            requestedTop: 1000,
            actualWidth: 0,
            actualHeight: double.NaN,
            width: 300,
            height: 200,
            screenLeft: 0,
            screenTop: 0,
            screenWidth: 1920,
            screenHeight: 1080);

        Assert.Equal(1620, result.Left);
        Assert.Equal(880, result.Top);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, true, true)]
    public void CanPersistLocation_OnlyTrueWhenLoadedAndEnabled(
        bool isLocationSaveEnabled,
        bool isLoaded,
        bool expected)
    {
        var result = WindowPositionHelper.CanPersistLocation(isLocationSaveEnabled, isLoaded);

        Assert.Equal(expected, result);
    }
}
