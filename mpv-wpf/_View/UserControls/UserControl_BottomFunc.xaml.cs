namespace mpv_wpf._View;

/// <summary>
/// UserControl_BottomFunc.xaml 的交互逻辑
/// </summary>
public partial class UserControl_BottomFunc
{

    public UserControl_BottomFunc() => InitializeComponent();





    #region Override


    /// <summary>
    /// 鼠标滚轮事件
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        //base.OnMouseWheel(e);

        ////音量调整
        //if (e.Delta > 0)
        //{
        //    this.Volume_Slider.Value += 10;
        //}
        //else
        //{
        //    this.Volume_Slider.Value -= 10;
        //}

        //e.Handled = true;
    }

    #endregion
}
