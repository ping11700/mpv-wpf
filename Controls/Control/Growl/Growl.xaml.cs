using Controls.Resources;

namespace Controls.Control;


/// <summary>
///     消息提醒
/// </summary>
[TemplatePart(Name = ElementPanelMore, Type = typeof(Panel))]
[TemplatePart(Name = ElementGridMain, Type = typeof(Grid))]
[TemplatePart(Name = ElementButtonClose, Type = typeof(Button))]
public class Growl : System.Windows.Controls.Control
{
    #region Constants

    private const string ElementPanelMore = "PART_PanelMore";

    private const string ElementGridMain = "PART_GridMain";

    private const string ElementButtonClose = "PART_ButtonClose";

    #endregion Constants

    #region Data

    //private static GrowlWindow GrowlWindow;

    private Panel? _panelMore;

    private Grid? _gridMain;

    private Button? _buttonClose;

    private bool _showCloseButton;

    private bool _staysOpen;

    private int _waitTime = 6;

    /// <summary>
    ///     计数
    /// </summary>
    private int _tickCount;

    /// <summary>
    ///     关闭计时器
    /// </summary>
    private DispatcherTimer _timerClose;

    private static readonly Dictionary<string, Panel> PanelDic = new();

    private static readonly List<string> _onlyOneWarnMsg = new List<string>() { "网络未连接,请检查网络后重试～", "登录信息已失效 请重新登录" };
    #endregion Data

    public Growl()
    {
        CommandBindings.Add(new CommandBinding(ControlCommands.Close, ButtonClose_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Cancel, ButtonCancel_OnClick));
        CommandBindings.Add(new CommandBinding(ControlCommands.Confirm, ButtonOk_OnClick));
    }

    public static void Register(string token, Panel panel)
    {
        if (string.IsNullOrEmpty(token) || panel == null) return;
        PanelDic[token] = panel;
        InitGrowlPanel(panel);
    }

    public static void Unregister(string token, Panel panel)
    {
        if (string.IsNullOrEmpty(token) || panel == null) return;

        if (PanelDic.ContainsKey(token))
        {
            if (ReferenceEquals(PanelDic[token], panel))
            {
                PanelDic.Remove(token);
                panel.ContextMenu = null;
                //panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
            }
        }
    }

    public static void Unregister(Panel panel)
    {
        if (panel == null) return;
        var first = PanelDic.FirstOrDefault(item => ReferenceEquals(panel, item.Value));
        if (!string.IsNullOrEmpty(first.Key))
        {
            PanelDic.Remove(first.Key);
            panel.ContextMenu = null;
            //panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
        }
    }

    public static void Unregister(string token)
    {
        if (string.IsNullOrEmpty(token)) return;

        if (PanelDic.ContainsKey(token))
        {
            var panel = PanelDic[token];
            PanelDic.Remove(token);
            panel.ContextMenu = null;
            //panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
        }
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        _buttonClose?.Show(_showCloseButton);
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        _buttonClose?.Collapse();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _panelMore = GetTemplateChild(ElementPanelMore) as Panel;
        _gridMain = GetTemplateChild(ElementGridMain) as Grid;
        _buttonClose = GetTemplateChild(ElementButtonClose) as Button;

        CheckNull();
        Update();
    }

    private void CheckNull()
    {
        if (_panelMore == null || _gridMain == null || _buttonClose == null) throw new Exception();
    }

    private Func<bool, bool> ActionBeforeClose { get; set; }

    /// <summary>
    ///     消息容器
    /// </summary>
    public static Panel? GrowlPanel { get; set; }
    /// <summary>
    /// 当前 GrowlPanel 对应的 window
    /// </summary>
    private static Window CurrentWindow { get; set; }

    internal static readonly DependencyProperty CancelStrProperty = DependencyProperty.Register(
        "CancelStr", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

    internal static readonly DependencyProperty ConfirmStrProperty = DependencyProperty.Register(
        "ConfirmStr", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty ShowDateTimeProperty = DependencyProperty.Register(
        "ShowDateTime", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.TrueBox));

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
        "Message", typeof(string), typeof(Growl), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
        "Time", typeof(DateTime), typeof(Growl), new PropertyMetadata(default(DateTime)));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        "Icon", typeof(Geometry), typeof(Growl), new PropertyMetadata(default(Geometry)));

    public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(
        "IconBrush", typeof(System.Windows.Media.Brush), typeof(Growl), new PropertyMetadata(default(System.Windows.Media.Brush)));

    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
        "Type", typeof(GrowlInfoType), typeof(Growl), new PropertyMetadata(default(GrowlInfoType)));

    public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
        "Token", typeof(string), typeof(Growl), new PropertyMetadata(default(string), OnTokenChanged));

    private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Panel panel)
        {
            if (e.NewValue == null)
            {
                Unregister(panel);
            }
            else
            {
                Register(e.NewValue.ToString(), panel);
            }
        }
    }

    public static void SetToken(DependencyObject element, string value) => element.SetValue(TokenProperty, value);

    public static string GetToken(DependencyObject element) => (string)element.GetValue(TokenProperty);

    public static readonly DependencyProperty GrowlParentProperty = DependencyProperty.RegisterAttached(
        "GrowlParent", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.FalseBox, (o, args) =>
        {
            if ((bool)args.NewValue && o is Panel panel)
            {
                SetGrowlPanel(panel);
            }
        }));

    public static void SetGrowlParent(DependencyObject element, bool value) => element.SetValue(GrowlParentProperty, ValueBoxes.BooleanBox(value));

    public static bool GetGrowlParent(DependencyObject element) => (bool)element.GetValue(GrowlParentProperty);

    private static readonly DependencyProperty IsCreatedAutomaticallyProperty = DependencyProperty.RegisterAttached(
        "IsCreatedAutomatically", typeof(bool), typeof(Growl), new PropertyMetadata(ValueBoxes.FalseBox));

    private static void SetIsCreatedAutomatically(DependencyObject element, bool value) => element.SetValue(IsCreatedAutomaticallyProperty, ValueBoxes.BooleanBox(value));

    private static bool GetIsCreatedAutomatically(DependencyObject element) => (bool)element.GetValue(IsCreatedAutomaticallyProperty);

    public GrowlInfoType Type
    {
        get => (GrowlInfoType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    internal string CancelStr
    {
        get => (string)GetValue(CancelStrProperty);
        set => SetValue(CancelStrProperty, value);
    }

    internal string ConfirmStr
    {
        get => (string)GetValue(ConfirmStrProperty);
        set => SetValue(ConfirmStrProperty, value);
    }

    public bool ShowDateTime
    {
        get => (bool)GetValue(ShowDateTimeProperty);
        set => SetValue(ShowDateTimeProperty, ValueBoxes.BooleanBox(value));
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public DateTime Time
    {
        get => (DateTime)GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public Geometry Icon
    {
        get => (Geometry)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public System.Windows.Media.Brush IconBrush
    {
        get => (System.Windows.Media.Brush)GetValue(IconBrushProperty);
        set => SetValue(IconBrushProperty, value);
    }

    /// <summary>
    ///     开始计时器
    /// </summary>
    private void StartTimer()
    {
        _timerClose = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timerClose.Tick += delegate
        {
            if (IsMouseOver)
            {
                _tickCount = 0;
                return;
            }

            _tickCount++;
            if (_tickCount >= _waitTime)
            {
                Close(true);
            }
        };
        _timerClose.Start();
    }

    /// <summary>
    ///     消息容器
    /// </summary>
    /// <param name="panel"></param>
    private static void SetGrowlPanel(Panel panel)
    {
        GrowlPanel = panel;
        InitGrowlPanel(panel);
    }

    private static void InitGrowlPanel(Panel panel)
    {
        if (panel == null) return;

        var menuItem = new MenuItem() { Header = "清除" };

        menuItem.Click += (s, e) =>
        {
            foreach (var item in panel.Children.OfType<Growl>())
            {
                item.Close(false);
            }
        };
        panel.ContextMenu = new ContextMenu
        {
            Items =
            {
                menuItem
            }
        };

    }

    private void Update()
    {
        if (Util_Visual.IsInDesignMode) return;


        if (Type == GrowlInfoType.Ask)
        {
            _panelMore.IsEnabled = true;
            _panelMore.Show();
        }

        var transform = new TranslateTransform
        {
            X = FlowDirection == FlowDirection.LeftToRight ? MaxWidth : -MaxWidth
        };
        _gridMain.RenderTransform = transform;
        transform.BeginAnimation(TranslateTransform.XProperty,
             new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(200)))
             {
                 EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
             });
        if (!_staysOpen) StartTimer();
    }



    /// <summary>
    ///     显示信息
    /// </summary>
    /// <param name="growlInfo"></param>
    private static void Show(GrowlInfo growlInfo)
    {
        Application.Current?.Dispatcher?.Invoke(() =>
        {
            var ctl = new Growl
            {
                Message = growlInfo.Message,
                Time = DateTime.Now,
                Icon = Util_Resource.GetResource<Geometry>(growlInfo.IconKey) ?? growlInfo.Icon,
                IconBrush = Util_Resource.GetResource<System.Windows.Media.Brush>(growlInfo.IconBrushKey) ?? growlInfo.IconBrush,
                _showCloseButton = growlInfo.ShowCloseButton,
                ActionBeforeClose = growlInfo.ActionBeforeClose,
                _staysOpen = growlInfo.StaysOpen,
                ShowDateTime = growlInfo.ShowDateTime,
                ConfirmStr = growlInfo.ConfirmStr,
                CancelStr = growlInfo.CancelStr,
                Type = growlInfo.Type,
                _waitTime = Math.Max(growlInfo.WaitTime, 2)
            };

            if (!string.IsNullOrEmpty(growlInfo.Token))
            {
                if (PanelDic.TryGetValue(growlInfo.Token, out var panel))
                {
                    panel?.Children.Insert(0, ctl);
                }
            }
            else
            {
                // GrowlPanel is null, we create it automatically
                GrowlPanel ??= CreateDefaultPanel(growlInfo.ParentWin);
                //多个窗体 消息载体创建
                if (GrowlPanel != null)
                {
                    var lastWindow = Window.GetWindow(GrowlPanel);

                    if (lastWindow != API_Window.ActiveWindow)
                    {
                        GrowlPanel = CreateDefaultPanel(growlInfo.ParentWin);
                    }
                }
                GrowlPanel?.Children.Insert(0, ctl);
            }
        });
    }

    private static void RemoveWindowPanel(Window lastWindow, Panel panel)
    {
        FrameworkElement element = lastWindow;
        var decorator = Util_Visual.FindChild<AdornerDecorator>(element);

        if (decorator != null)
        {
            var layer = decorator.AdornerLayer;
            var adorner = Util_Visual.FindParent<Adorner>(panel);

            if (adorner != null)
            {
                layer?.Remove(adorner);
            }
        }
    }


    /// <summary>
    /// 创建消息载体
    /// </summary>
    /// <returns></returns>
    private static Panel CreateDefaultPanel(Window? win = null)
    {
        Window? element = win ?? API_Window.ActiveWindow;
        if (element != null)
        {
            AdornerDecorator decorator;
            if (element.Content is Controls.Hwnd.HwndHostPresenter hhp)
            {
                decorator = hhp.Adornment as AdornerDecorator; //播放window AdornerDecorator
            }
            else
            {
                decorator = Util_Visual.FindChild<AdornerDecorator>(element);
            }

            if (decorator != null)
            {
                var layer = decorator.AdornerLayer;
                if (layer != null)
                {
                    var panel = new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(0, 30, 0, 0)
                    };

                    InitGrowlPanel(panel);
                    SetIsCreatedAutomatically(panel, true);

                    var scrollViewer = new ScrollViewer
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                        IsInertiaEnabled = true,
                        IsPenetrating = true,
                        Content = panel
                    };

                    var container = new BaseAdorner(layer)
                    {

                        Child = scrollViewer
                    };
                    container.SetCurrentValue(Panel.ZIndexProperty, 999);
                    layer.Add(container);

                    return panel;
                }
            }
        }
        return null;
    }


    /// <summary>
    /// 去除消息载体
    /// </summary>
    /// <param name="panel"></param>
    private static void RemoveDefaultPanel(Panel panel)
    {
        FrameworkElement element = API_Window.ActiveWindow;
        var decorator = Util_Visual.FindChild<AdornerDecorator>(element);

        if (decorator != null)
        {
            var layer = decorator.AdornerLayer;
            var adorner = Util_Visual.FindParent<Adorner>(panel);

            if (adorner != null)
            {
                layer?.Remove(adorner);
            }
        }
    }

    private static void InitGrowlInfo(ref GrowlInfo growlInfo, GrowlInfoType infoType)
    {
        if (growlInfo == null) throw new ArgumentNullException(nameof(growlInfo));
        growlInfo.Type = infoType;

        switch (infoType)
        {
            case GrowlInfoType.Success:
                if (!growlInfo.IsCustom)
                {
                    growlInfo.IconKey = ResourceToken.SuccessGeometry;
                    growlInfo.IconBrushKey = ResourceToken.SuccessBrush;
                }
                else
                {
                    growlInfo.IconKey ??= ResourceToken.SuccessGeometry;
                    growlInfo.IconBrushKey ??= ResourceToken.SuccessBrush;
                }
                break;
            case GrowlInfoType.Info:
                if (!growlInfo.IsCustom)
                {
                    growlInfo.IconKey = ResourceToken.InfoGeometry;
                    growlInfo.IconBrushKey = ResourceToken.InfoBrush;
                }
                else
                {
                    growlInfo.IconKey ??= ResourceToken.InfoGeometry;
                    growlInfo.IconBrushKey ??= ResourceToken.InfoBrush;
                }
                break;
            case GrowlInfoType.Warning:
                if (!growlInfo.IsCustom)
                {
                    growlInfo.IconKey = ResourceToken.WarningGeometry;
                    growlInfo.IconBrushKey = ResourceToken.WarningBrush;
                }
                else
                {
                    growlInfo.IconKey ??= ResourceToken.WarningGeometry;
                    growlInfo.IconBrushKey ??= ResourceToken.WarningBrush;
                }
                break;
            case GrowlInfoType.Error:
                if (!growlInfo.IsCustom)
                {
                    growlInfo.IconKey = ResourceToken.ErrorGeometry;
                    growlInfo.IconBrushKey = ResourceToken.DangerBrush;
                    growlInfo.StaysOpen = true;
                }
                else
                {
                    growlInfo.IconKey ??= ResourceToken.ErrorGeometry;
                    growlInfo.IconBrushKey ??= ResourceToken.DangerBrush;
                }
                break;
            case GrowlInfoType.Fatal:
                if (!growlInfo.IsCustom)
                {
                    growlInfo.IconKey = ResourceToken.FatalGeometry;
                    growlInfo.IconBrushKey = ResourceToken.PrimaryTextBrush;
                    growlInfo.StaysOpen = true;
                    growlInfo.ShowCloseButton = false;
                }
                else
                {
                    growlInfo.IconKey ??= ResourceToken.FatalGeometry;
                    growlInfo.IconBrushKey ??= ResourceToken.PrimaryTextBrush;
                }
                break;
            case GrowlInfoType.Ask:
                growlInfo.StaysOpen = true;
                growlInfo.ShowCloseButton = false;
                if (!growlInfo.IsCustom)
                {
                    growlInfo.IconKey = ResourceToken.AskGeometry;
                    growlInfo.IconBrushKey = ResourceToken.AccentBrush;
                }
                else
                {
                    growlInfo.IconKey ??= ResourceToken.AskGeometry;
                    growlInfo.IconBrushKey ??= ResourceToken.AccentBrush;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(infoType), infoType, null);
        }
    }

    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    public static void Success(string message, string token = "") => Success(new GrowlInfo
    {
        Message = message,
        Token = token
    });

    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void Success(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Success);
        Show(growlInfo);
    }

    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="message"></param>
    public static void SuccessGlobal(string message) => SuccessGlobal(new GrowlInfo
    {
        Message = message
    });

    /// <summary>
    ///     成功
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void SuccessGlobal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Success);
        //ShowGlobal(growlInfo);
    }

    /// <summary>
    ///     消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    public static void Info(string message, string token = "") => Info(new GrowlInfo
    {
        Message = message,
        Token = token
    });

    /// <summary>
    ///     消息
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void Info(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Info);
        Show(growlInfo);
    }

    /// <summary>
    ///     消息
    /// </summary>
    /// <param name="message"></param>
    public static void InfoGlobal(string message) => InfoGlobal(new GrowlInfo
    {
        Message = message
    });

    /// <summary>
    ///     消息
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void InfoGlobal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Info);
        //ShowGlobal(growlInfo);
    }

    /// <summary>
    ///     警告
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    public static void Warning(string message, string token = "") =>
        Warning(new GrowlInfo
        {
            Message = message,
            Token = token
        });


    /// <summary>
    ///     警告
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void Warning(GrowlInfo growlInfo, Window? win = null)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (GrowlPanel != null && GrowlPanel.Children.Cast<Growl>().Any(x => _onlyOneWarnMsg.Contains(x.Message)))
                return;

            InitGrowlInfo(ref growlInfo, GrowlInfoType.Warning);
            Show(growlInfo);
        });

    }

    /// <summary>
    ///     警告
    /// </summary>
    /// <param name="message"></param>
    public static void WarningGlobal(string message) => WarningGlobal(new GrowlInfo
    {
        Message = message
    });

    /// <summary>
    ///     警告
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void WarningGlobal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Warning);
        //ShowGlobal(growlInfo);
    }

    /// <summary>
    ///     错误
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    public static void Error(string message, string token = "") => Error(new GrowlInfo
    {
        Message = message,
        Token = token
    });

    /// <summary>
    ///     错误
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void Error(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Error);
        Show(growlInfo);
    }

    /// <summary>
    ///     错误
    /// </summary>
    /// <param name="message"></param>
    public static void ErrorGlobal(string message) => ErrorGlobal(new GrowlInfo
    {
        Message = message
    });

    /// <summary>
    ///     错误
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void ErrorGlobal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Error);
        //ShowGlobal(growlInfo);
    }

    /// <summary>
    ///     严重
    /// </summary>
    /// <param name="message"></param>
    /// <param name="token"></param>
    public static void Fatal(string message, string token = "") => Fatal(new GrowlInfo
    {
        Message = message,
        Token = token
    });

    /// <summary>
    ///     严重
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void Fatal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Fatal);
        Show(growlInfo);
    }

    /// <summary>
    ///     严重
    /// </summary>
    /// <param name="message"></param>
    public static void FatalGlobal(string message) => FatalGlobal(new GrowlInfo
    {
        Message = message
    });

    /// <summary>
    ///     严重
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void FatalGlobal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Fatal);
        //ShowGlobal(growlInfo);
    }

    /// <summary>
    ///     询问
    /// </summary>
    /// <param name="message"></param>
    /// <param name="actionBeforeClose"></param>
    /// <param name="token"></param>
    public static void Ask(string message, Func<bool, bool> actionBeforeClose, string token = "") => Ask(new GrowlInfo
    {
        Message = message,
        ActionBeforeClose = actionBeforeClose,
        Token = token
    });

    /// <summary>
    ///     询问
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void Ask(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Ask);
        Show(growlInfo);
    }

    /// <summary>
    ///     询问
    /// </summary>
    /// <param name="message"></param>
    /// <param name="actionBeforeClose"></param>
    public static void AskGlobal(string message, Func<bool, bool> actionBeforeClose) => AskGlobal(new GrowlInfo
    {
        Message = message,
        ActionBeforeClose = actionBeforeClose
    });

    /// <summary>
    ///     询问
    /// </summary>
    /// <param name="growlInfo"></param>
    public static void AskGlobal(GrowlInfo growlInfo)
    {
        InitGrowlInfo(ref growlInfo, GrowlInfoType.Ask);
        //ShowGlobal(growlInfo);
    }

    private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Close(false);

    /// <summary>
    ///     关闭
    /// </summary>
    private void Close(bool invokeParam)
    {
        if (ActionBeforeClose?.Invoke(invokeParam) == false)
        {
            return;
        }

        _timerClose?.Stop();
        var transform = new TranslateTransform();
        _gridMain.RenderTransform = transform;
        var animation =  new DoubleAnimation(FlowDirection == FlowDirection.LeftToRight ? ActualWidth : -ActualWidth, new Duration(TimeSpan.FromMilliseconds(200)))
             {
                 EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
             };

        animation.Completed += (s, e) =>
        {
            if (Parent is Panel panel)
            {
                panel.Children.Remove(this);

                //if (GrowlWindow != null)
                //{
                //    if (GrowlWindow.GrowlPanel != null && GrowlWindow.GrowlPanel.Children.Count == 0)
                //    {
                //        GrowlWindow.Close();
                //        GrowlWindow = null;
                //    }
                //}
                //else
                {
                    if (GrowlPanel != null && GrowlPanel.Children.Count == 0 && GetIsCreatedAutomatically(GrowlPanel))
                    {
                        // If the count of children is zero, we need to remove the panel, provided that the panel was created automatically  
                        RemoveDefaultPanel(GrowlPanel);
                        GrowlPanel = null;
                    }
                }
            }
        };
        transform.BeginAnimation(TranslateTransform.XProperty, animation);
    }

    /// <summary>
    ///     清除
    /// </summary>
    /// <param name="token"></param>
    public static void Clear(string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            if (PanelDic.TryGetValue(token, out var panel))
            {
                Clear(panel);
            }
        }
        else
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Clear(GrowlPanel);
            });
        }
    }

    /// <summary>
    ///     清除
    /// </summary>
    /// <param name="panel"></param>
    private static void Clear(Panel panel) => panel?.Children.Clear();

    ///// <summary>
    /////     清除
    ///// </summary>
    //public static void ClearGlobal()
    //{
    //    if (GrowlWindow == null) return;
    //    Clear(GrowlWindow.GrowlPanel);
    //    GrowlWindow.Close();
    //    GrowlWindow = null;
    //}

    private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => Close(false);

    private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => Close(true);
}