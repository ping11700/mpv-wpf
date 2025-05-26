namespace Controls.Hwnd;

internal class HwndAdornmentRoot : ContentControl
{
    internal DependencyObject? UIParentCore { get; set; }

    protected override DependencyObject GetUIParentCore() => UIParentCore ?? base.GetUIParentCore();
}
