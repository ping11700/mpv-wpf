namespace mpv_wpf._View;

/// <summary>
/// PlayerShell.xaml 的交互逻辑
/// </summary>
public partial class PlayerShell : BasePlayerWindow
{
    private readonly DispatcherTimer _timer;
    private Point _prePoint;

    /// <summary>
    /// timer 自动隐藏消失UI
    /// </summary>
    internal event Action<bool> VisibilityTimerChangeEvent;

    public PlayerShell(IPlayer player) : base(player)
    {
        InitializeComponent();

        this.DataContext = new ViewModel_PlayerBase(player);

        this.Title = Consts.APPShellName;//API_Window.CallMainWindow  调用此参数;
        this.AllowDrop = true;

        _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(6) };
        _timer.Tick += (s, e) => ChangeControlVisibility(false);
        _timer.Start();


        this.VideoHwndHost.Loaded += (s, e) => player?.SetHandle(this.VideoHwndHost?.Handle ?? IntPtr.Zero);

        this.DataContextChanged -= PlayerShell_DataContextChanged;
        this.DataContextChanged += PlayerShell_DataContextChanged;

        this.Tips_TextBlock.DataContextChanged += (s, e) => TipAnimation();

        this.ContentPresentDrawer.Content = new UserControl_PlayList();
    }

    #region Events

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlayerShell_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        //(e.OldValue as ViewModel_PlayerBase)?.Dispose();

        _timer?.Stop();
        _timer?.Start();
    }


    #endregion


    #region override 

    protected override void Setting(object sender, ExecutedRoutedEventArgs e)
    {

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="isIn"></param>
    protected override void LayoutFullScreen(bool isIn)
    {
        var wc = this.WindowChrome.Clone();
        wc.SetCurrentValue(Border.CornerRadiusProperty, isIn ? new CornerRadius(0) : new CornerRadius(12));
        this.SetCurrentValue(System.Windows.Shell.WindowChrome.WindowChromeProperty, wc);
        wc.Freeze();

        //this.TopFunc_SimplePanel.SetCurrentValue(Border.CornerRadiusProperty, isIn ? new CornerRadius(0) : new CornerRadius(12, 12, 0, 0));
        //this.BottomFunc_UC.SetCurrentValue(Attach_Border.CornerRadiusProperty, isIn ? new CornerRadius(0) : new CornerRadius(0, 0, 12, 12));

        this.TopFunc_UC.Operate_StackPanel.Visibility = isIn ? Visibility.Hidden : Visibility.Visible;
    }



    /// <summary>
    /// 
    /// </summary>
    protected override void ToggleDrawer()
    {
        TogAnimateDrawer();
    }


    #region  Play Local 本地文件播放

    /// <summary>
    /// 
    /// </summary>
    protected override void PlayLocalMedias(string[] files)
    {
        IPlayer?.Start(files, isFile: true);
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        //点击抽屉自身 不关闭
        if (e.Source is ContentPresenter { Name: "ContentPresentDrawer" })
            return;

        //关闭抽屉
        TogAnimateDrawer(false);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var position = e.GetPosition(this);
        if (_prePoint != position)
        {
            ChangeControlVisibility(true);

            _prePoint = position;

            _timer?.Stop();
            _timer?.Start();
        }
    }



    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        _timer?.Stop();
        _timer?.Start();
    }







    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        ChangeControlVisibility(true);
        _timer?.Stop();
        _timer?.Start();
    }


    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            PlayLocalMedias(files);
        }
    }


    protected override void OnClosing(CancelEventArgs e)
    {
        _timer?.Stop();

        //var vm = this.DataContext as ViewModel_PlayerBase;
        //vm?.Dispose();

        // 这里需要把 DataContext 设置为空
        // 解决 关闭主程序的时候 关闭播放器的缺失 
        this.DataContext = null;

        var b = Process.GetProcessesByName(Consts.APPName).Length > 0;
        e.Cancel = b;

        base.OnClosing(isHide: b);

        if (b == false) App.Shutdown();
    }



    #endregion


    #region Private

    #region    Animation

    /// <summary>
    /// 抽屉动画
    /// </summary>
    private void TogAnimateDrawer(bool? open = null)
    {
        var isOpen = this.ContentPresentDrawer.Width <= 0;

        this.Drawer_ToggleButton.IsChecked = isOpen;

        isOpen = open ?? isOpen;

        if (isOpen)
        {

            var animation = new DoubleAnimation(400, new Duration(TimeSpan.FromMilliseconds(500)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut },
                FillBehavior = FillBehavior.HoldEnd
            };
            this.ContentPresentDrawer.BeginAnimation(WidthProperty, animation);
        }
        else
        {
            var animation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(500)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut },
                FillBehavior = FillBehavior.HoldEnd
            };
            this.ContentPresentDrawer.BeginAnimation(WidthProperty, animation);
        }
    }


    /// <summary>
    /// Tip 动画
    /// </summary>
    private void TipAnimation()
    {
        var animation = new DoubleAnimation(0.9, new Duration(TimeSpan.FromMilliseconds(500)))
        {
            EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut },
            FillBehavior = FillBehavior.HoldEnd
        };

        var animation2 = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(1000)))
        {
            EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut },
            BeginTime = TimeSpan.FromSeconds(3)
        };

        animation.Completed += (s, e) => this.Tips_TextBlock.BeginAnimation(OpacityProperty, animation2);

        this.Tips_TextBlock.BeginAnimation(OpacityProperty, animation);
    }


    #endregion

    /// <summary>
    ///  UI控件 timer 自动隐藏, 展示
    /// </summary>
    private void ChangeControlVisibility(bool b)
    {
        if (b)
        {
            this.Cursor = Cursors.Arrow;
            this.TopFunc_UC.Opacity = 1;
            this.BottomFunc_UC.Opacity = 1;
            this.Drawer_ToggleButton.Opacity = 1;
        }
        else
        {
            this.Cursor = Cursors.None;
            this.TopFunc_UC.Opacity = 0;
            this.BottomFunc_UC.Opacity = 0;
            this.Drawer_ToggleButton.Opacity = 0;
        }

        VisibilityTimerChangeEvent?.Invoke(b);
    }

    #endregion
}