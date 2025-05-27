namespace Controls.Control;

public class AdornedControl : ContentControl
{
    #region Dependency Properties

    /// <summary>
    /// Dependency properties.
    /// </summary>
    public bool IsAdornerVisible { get => (bool)GetValue(IsAdornerVisibleProperty); set { SetValue(IsAdornerVisibleProperty, value); } }
    public static readonly DependencyProperty IsAdornerVisibleProperty = DependencyProperty.Register(
        "IsAdornerVisible", typeof(bool), typeof(AdornedControl), new FrameworkPropertyMetadata(IsAdornerVisible_PropertyChanged));


    /// <summary>
    /// Used in XAML to define the UI content of the adorner.
    /// </summary>
    public FrameworkElement AdornerContent { get => (FrameworkElement)GetValue(AdornerContentProperty); set { SetValue(AdornerContentProperty, value); } }
    public static readonly DependencyProperty AdornerContentProperty = DependencyProperty.Register(
        "AdornerContent", typeof(FrameworkElement), typeof(AdornedControl), new FrameworkPropertyMetadata(AdornerContent_PropertyChanged));


    public AdornerPlacement HorizontalAdornerPlacement { get => (AdornerPlacement)GetValue(HorizontalAdornerPlacementProperty); set { SetValue(HorizontalAdornerPlacementProperty, value); } }
    public static readonly DependencyProperty HorizontalAdornerPlacementProperty = DependencyProperty.Register(
        "HorizontalAdornerPlacement", typeof(AdornerPlacement), typeof(AdornedControl), new FrameworkPropertyMetadata(AdornerPlacement.Outside));


    public AdornerPlacement VerticalAdornerPlacement { get => (AdornerPlacement)GetValue(VerticalAdornerPlacementProperty); set { SetValue(VerticalAdornerPlacementProperty, value); } }
    public static readonly DependencyProperty VerticalAdornerPlacementProperty = DependencyProperty.Register(
        "VerticalAdornerPlacement", typeof(AdornerPlacement), typeof(AdornedControl), new FrameworkPropertyMetadata(AdornerPlacement.Outside));


    /// <summary>
    /// X offset of the adorner.
    /// </summary>
    public double AdornerOffsetX { get => (double)GetValue(AdornerOffsetXProperty); set { SetValue(AdornerOffsetXProperty, value); } }
    public static readonly DependencyProperty AdornerOffsetXProperty = DependencyProperty.Register(
        "AdornerOffsetX", typeof(double), typeof(AdornedControl));


    /// <summary>
    /// Y offset of the adorner.
    /// </summary>
    public double AdornerOffsetY { get => (double)GetValue(AdornerOffsetYProperty); set { SetValue(AdornerOffsetYProperty, value); } }
    public static readonly DependencyProperty AdornerOffsetYProperty = DependencyProperty.Register(
        "AdornerOffsetY", typeof(double), typeof(AdornedControl));


    /// <summary>
    /// Set to 'true' to make the adorner automatically fade-in and become visible when the mouse is hovered
    /// over the adorned control.  Also the adorner automatically fades-out when the mouse cursor is moved
    /// aware from the adorned control (and the adorner).
    /// </summary>
    public bool IsMouseOverShowEnabled { get => (bool)GetValue(IsMouseOverShowEnabledProperty); set { SetValue(IsMouseOverShowEnabledProperty, value); } }
    public static readonly DependencyProperty IsMouseOverShowEnabledProperty = DependencyProperty.Register(
        "IsMouseOverShowEnabled", typeof(bool), typeof(AdornedControl), new FrameworkPropertyMetadata(true, IsMouseOverShowEnabled_PropertyChanged));


    /// <summary>
    /// Specifies the time (in seconds) it takes to fade in the adorner.
    /// </summary>
    public double FadeInTime { get => (double)GetValue(FadeInTimeProperty); set { SetValue(FadeInTimeProperty, value); } }
    public static readonly DependencyProperty FadeInTimeProperty = DependencyProperty.Register(
        "FadeInTime", typeof(double), typeof(AdornedControl), new FrameworkPropertyMetadata(0.25));


    /// <summary>
    /// Specifies the time (in seconds) it takes to fade out the adorner.
    /// </summary>
    public double FadeOutTime { get => (double)GetValue(FadeOutTimeProperty); set { SetValue(FadeOutTimeProperty, value); } }
    public static readonly DependencyProperty FadeOutTimeProperty = DependencyProperty.Register(
        "FadeOutTime", typeof(double), typeof(AdornedControl), new FrameworkPropertyMetadata(1.0));


    /// <summary>
    /// Specifies the time (in seconds) after the mouse cursor moves away from the 
    /// adorned control (or the adorner) when the adorner begins to fade out.
    /// </summary>
    public double CloseAdornerTimeOut { get => (double)GetValue(CloseAdornerTimeOutProperty); set { SetValue(CloseAdornerTimeOutProperty, value); } }
    public static readonly DependencyProperty CloseAdornerTimeOutProperty = DependencyProperty.Register(
        "CloseAdornerTimeOut", typeof(double), typeof(AdornedControl), new FrameworkPropertyMetadata(2.0, CloseAdornerTimeOut_PropertyChanged));

    /// <summary>
    /// By default this property is set to null.
    /// When set to non-null it specifies the part name of a UI element
    /// in the visual tree of the AdornedControl content that is to be adorned.
    /// When this property is null it is the AdornerControl content that is adorned,
    /// however when it is set the visual-tree is searched for a UI element that has the
    /// specified part name, if the part is found then that UI element is adorned, otherwise
    /// an exception "Failed to find part ..." is thrown.        /// 
    /// </summary>
    public string AdornedTemplatePartName { get => (string)GetValue(AdornedTemplatePartNameProperty); set { SetValue(AdornedTemplatePartNameProperty, value); } }
    public static readonly DependencyProperty AdornedTemplatePartNameProperty = DependencyProperty.Register(
        "AdornedTemplatePartName", typeof(string), typeof(AdornedControl), new FrameworkPropertyMetadata(default));



    /// <summary>
    ///  指定AdornerLayer显示
    /// </summary>
    public string AdornerLayerElement
    {
        get => (string)GetValue(AdornerLayerElementProperty);
        set => SetValue(AdornerLayerElementProperty, value);
    }
    /// <summary>
    /// 
    /// </summary>
    public static readonly DependencyProperty AdornerLayerElementProperty = DependencyProperty.Register(
        "AdornerLayerElement", typeof(string), typeof(AdornedControl), new FrameworkPropertyMetadata(default));



    /// <summary>
    ///   同一个父级 同时只显示一个
    /// </summary>
    public bool IsMutex
    {
        get => (bool)GetValue(IsMutexProperty);
        set => SetValue(IsMutexProperty, ValueBoxes.BooleanBox(value));
    }

    public static void SetIsMutex(DependencyObject element, bool value) => element.SetValue(IsMutexProperty, ValueBoxes.BooleanBox(value));
    public static bool GeIsMutex(DependencyObject element) => (bool)element.GetValue(IsMutexProperty);
    /// <summary>
    /// 同一个父级 同时只显示一个
    /// </summary>
    public static readonly DependencyProperty IsMutexProperty = DependencyProperty.RegisterAttached(
            "AutoHide", typeof(bool), typeof(AdornedControl), new PropertyMetadata(ValueBoxes.FalseBox));




    public bool HideIfMouseLeaveAdornerLayer
    {
        get { return (bool)GetValue(HideIfMouseLeaveAdornerLayerProperty); }
        set { SetValue(HideIfMouseLeaveAdornerLayerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for HideIfMouseLeaveAdornerLayer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HideIfMouseLeaveAdornerLayerProperty =
        DependencyProperty.Register("HideIfMouseLeaveAdornerLayer", typeof(bool), typeof(AdornedControl), new PropertyMetadata(ValueBoxes.FalseBox));




    #endregion Dependency Properties

    #region Commands

    /// <summary>
    /// Commands.
    /// </summary>
    public static readonly RoutedCommand ShowAdornerCommand = new RoutedCommand("ShowAdorner", typeof(AdornedControl));
    public static readonly RoutedCommand FadeInAdornerCommand = new RoutedCommand("FadeInAdorner", typeof(AdornedControl));
    public static readonly RoutedCommand HideAdornerCommand = new RoutedCommand("HideAdorner", typeof(AdornedControl));
    public static readonly RoutedCommand FadeOutAdornerCommand = new RoutedCommand("FadeOutAdorner", typeof(AdornedControl));

    #endregion Commands

    public AdornedControl()
    {
        this.Focusable = false; // By default don't want 'AdornedControl' to be focusable.

        this.DataContextChanged += new DependencyPropertyChangedEventHandler(AdornedControl_DataContextChanged);

        _closeAdornerTimer.Tick += new EventHandler(CloseAdornerTimer_Tick);
        _closeAdornerTimer.Interval = TimeSpan.FromSeconds(CloseAdornerTimeOut);

        this.Cursor = Cursors.Hand;
    }

    /// <summary>
    /// Show the adorner.
    /// </summary>
    public void ShowAdorner()
    {
        IsAdornerVisible = true;
    }

    /// <summary>
    /// Hide the adorner.
    /// </summary>
    public void HideAdorner()
    {
        IsAdornerVisible = false;
    }

    /// <summary>
    /// Fade the adorner in and make it visible.
    /// </summary>
    public void FadeInAdorner()
    {
        if (!this.IsMouseOver)
        {
            return;
        }
        if (IsAdornerVisible &&
            adornerShowState == AdornerShowState.Visible ||
            adornerShowState == AdornerShowState.FadingIn)
        {
            return;
        }

        this.ShowAdorner();

        if (adorner == null)
        {
            return;
        }

        if (!adorner.IsMouseOver)
        {
            return;
        }

        if (adornerShowState != AdornerShowState.FadingOut)
        {
            adorner.Opacity = 0.0;
        }

        var doubleAnimation = new DoubleAnimation(1.0, new Duration(TimeSpan.FromSeconds(FadeInTime)));
        doubleAnimation.Completed += new EventHandler(FadeInAnimation_Completed);
        doubleAnimation.Freeze();

        adorner.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);

        adornerShowState = AdornerShowState.FadingIn;
    }

    /// <summary>
    /// Fade the adorner out and make it visible.
    /// </summary>
    public void FadeOutAdorner()
    {
        if (this.IsMouseOver)
        {
            return;
        }
        if (adornerShowState == AdornerShowState.FadingOut)
        {
            //
            // Already fading out.
            //
            return;
        }

        if (adornerShowState == AdornerShowState.Hidden)
        {
            //
            // Adorner has already been hidden.
            //
            return;
        }

        if (adorner == null)
        {
            return;
        }

        if (adorner.IsMouseOver)
        {
            return;
        }

        var fadeOutAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(FadeOutTime)));
        fadeOutAnimation.Completed += new EventHandler(FadeOutAnimation_Completed);
        fadeOutAnimation.Freeze();

        adorner.BeginAnimation(FrameworkElement.OpacityProperty, fadeOutAnimation);

        adornerShowState = AdornerShowState.FadingOut;
    }

    /// <summary>
    /// Shows or hides the adorner.
    /// Set to 'true' to show the adorner or 'false' to hide the adorner.
    /// </summary>


    #region Private Data Members

    /// <summary>
    /// Command bindings.
    /// </summary>
    private static readonly CommandBinding ShowAdornerCommandBinding = new CommandBinding(ShowAdornerCommand, ShowAdornerCommand_Executed);
    private static readonly CommandBinding FadeInAdornerCommandBinding = new CommandBinding(FadeInAdornerCommand, FadeInAdornerCommand_Executed);
    private static readonly CommandBinding HideAdornerCommandBinding = new CommandBinding(HideAdornerCommand, HideAdornerCommand_Executed);
    private static readonly CommandBinding FadeOutAdornerCommandBinding = new CommandBinding(FadeOutAdornerCommand, FadeOutAdornerCommand_Executed);

    /// <summary>
    /// Specifies the current show/hide state of the adorner.
    /// </summary>
    private enum AdornerShowState
    {
        Visible,
        Hidden,
        FadingIn,
        FadingOut,
    }

    /// <summary>
    /// Specifies the current show/hide state of the adorner.
    /// </summary>
    private AdornerShowState adornerShowState = AdornerShowState.Hidden;

    /// <summary>
    /// Caches the adorner layer.
    /// </summary>
    private AdornerLayer adornerLayer = null;

    /// <summary>
    /// The actual adorner create to contain our 'adorner UI content'.
    /// </summary>
    private FrameworkElementAdorner adorner = null;

    /// <summary>
    /// This timer is used to fade out and close the adorner.
    /// </summary>
    private readonly DispatcherTimer _closeAdornerTimer = new DispatcherTimer();

    #endregion

    #region Private/Internal Functions

    /// <summary>
    /// Static constructor to register command bindings.
    /// </summary>
    static AdornedControl()
    {
        CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), ShowAdornerCommandBinding);
        CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), FadeOutAdornerCommandBinding);
        CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), HideAdornerCommandBinding);
        CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), FadeInAdornerCommandBinding);
    }

    /// <summary>
    /// Event raised when the DataContext of the adorned control changes.
    /// </summary>
    private void AdornedControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        UpdateAdornerDataContext();
    }

    /// <summary>
    /// Update the DataContext of the adorner from the adorned control.
    /// </summary>
    private void UpdateAdornerDataContext()
    {
        if (this.AdornerContent != null)
        {
            this.AdornerContent.DataContext = this.DataContext;
        }
    }

    /// <summary>
    /// Event raised when the Show command is executed.
    /// </summary>
    private static void ShowAdornerCommand_Executed(object target, ExecutedRoutedEventArgs e)
    {
        AdornedControl c = (AdornedControl)target;
        c.ShowAdorner();
    }

    /// <summary>
    /// Event raised when the FadeIn command is executed.
    /// </summary>
    private static void FadeInAdornerCommand_Executed(object target, ExecutedRoutedEventArgs e)
    {
        AdornedControl c = (AdornedControl)target;
        c.FadeOutAdorner();
    }

    /// <summary>
    /// Event raised when the Hide command is executed.
    /// </summary>
    private static void HideAdornerCommand_Executed(object target, ExecutedRoutedEventArgs e)
    {
        AdornedControl c = (AdornedControl)target;
        c.HideAdorner();
    }

    /// <summary>
    /// Event raised when the FadeOut command is executed.
    /// </summary>
    private static void FadeOutAdornerCommand_Executed(object target, ExecutedRoutedEventArgs e)
    {
        AdornedControl c = (AdornedControl)target;
        c.FadeOutAdorner();
    }

    /// <summary>
    /// Event raised when the value of IsAdornerVisible has changed.
    /// </summary>
    private static void IsAdornerVisible_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AdornedControl c = (AdornedControl)o;
        c.ShowOrHideAdornerInternal();
    }

    /// <summary>
    /// Event raised when the IsMouseOverShowEnabled property has changed.
    /// </summary>
    private static void IsMouseOverShowEnabled_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AdornedControl c = (AdornedControl)o;
        c._closeAdornerTimer.Stop();
        c.HideAdorner();
    }

    /// <summary>
    /// Event raised when the CloseAdornerTimeOut property has change.
    /// </summary>
    private static void CloseAdornerTimeOut_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AdornedControl c = (AdornedControl)o;
        c._closeAdornerTimer.Interval = TimeSpan.FromSeconds(c.CloseAdornerTimeOut);
    }

    /// <summary>
    /// Event raised when the value of AdornerContent has changed.
    /// </summary>
    private static void AdornerContent_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AdornedControl c = (AdornedControl)o;
        c.ShowOrHideAdornerInternal();

        FrameworkElement oldAdornerContent = (FrameworkElement)e.OldValue;
        if (oldAdornerContent != null)
        {
            oldAdornerContent.MouseEnter -= new MouseEventHandler(c.AdornerContent_MouseEnter);
            oldAdornerContent.MouseLeave -= new MouseEventHandler(c.AdornerContent_MouseLeave);
        }

        FrameworkElement newAdornerContent = (FrameworkElement)e.NewValue;
        if (newAdornerContent != null)
        {
            newAdornerContent.MouseEnter += new MouseEventHandler(c.AdornerContent_MouseEnter);
            newAdornerContent.MouseLeave += new MouseEventHandler(c.AdornerContent_MouseLeave);
        }
    }

    /// <summary>
    /// Event raised when the mouse cursor enters the area of the adorner.
    /// </summary>
    private void AdornerContent_MouseEnter(object sender, MouseEventArgs e)
    {
        MouseEnterLogic();
    }

    /// <summary>
    /// Event raised when the mouse cursor leaves the area of the adorner.
    /// </summary>
    private void AdornerContent_MouseLeave(object sender, MouseEventArgs e)
    {
        MouseLeaveLogic(true);
    }

    /// <summary>
    /// Internal method to show or hide the adorner based on the value of IsAdornerVisible.
    /// </summary>
    private void ShowOrHideAdornerInternal()
    {
        if (IsAdornerVisible)
        {
            ShowAdornerInternal();
        }
        else
        {
            HideAdornerInternal();
        }
    }



    /// <summary>
    /// Internal method to show the adorner.
    /// </summary>
    private void ShowAdornerInternal()
    {
        if (this.adorner != null)
        {
            // Already adorned.
            return;
        }

        if (this.AdornerContent != null)
        {
            if (AdornerLayerElement?.Length > 0)
            {
                var parent = Util_Visual.FindParent<FrameworkElement>(this, AdornerLayerElement);
                this.adornerLayer ??= AdornerLayer.GetAdornerLayer(parent);
            }
            else
            {
                this.adornerLayer ??= AdornerLayer.GetAdornerLayer(this);
            }

            if (this.adornerLayer != null)
            {
                FrameworkElement adornedControl = this; // The control to be adorned defaults to 'this'.

                if (!string.IsNullOrEmpty(this.AdornedTemplatePartName))
                {
                    //
                    // If 'AdornedTemplatePartName' is set to a valid string then search the visual-tree
                    // for a UI element that has the specified part name.  If we find it then use it as the
                    // adorned control, otherwise throw an exception.
                    //
                    adornedControl = Util_Visual.FindChild<FrameworkElement>(this, this.AdornedTemplatePartName);
                    if (adornedControl == null)
                    {
                        throw new ApplicationException("Failed to find a FrameworkElement in the visual-tree with the part name '" + this.AdornedTemplatePartName + "'.");
                    }
                }

                this.adorner = new FrameworkElementAdorner(this.AdornerContent, adornedControl,
                                                           this.HorizontalAdornerPlacement, this.VerticalAdornerPlacement,
                                                           this.AdornerOffsetX, this.AdornerOffsetY);
                this.adornerLayer.Add(this.adorner);

                UpdateAdornerDataContext();
            }
        }

        this.adornerShowState = AdornerShowState.Visible;
    }

    /// <summary>
    /// Internal method to hide the adorner.
    /// </summary>
    private void HideAdornerInternal()
    {
        if (this.adornerLayer == null || this.adorner == null)
        {
            // Not already adorned.
            return;
        }

        //
        // Stop the timer that might be about to fade out the adorner.
        //
        _closeAdornerTimer.Stop();
        this.adornerLayer.Remove(this.adorner);
        this.adorner.DisconnectChild();

        this.adorner = null;
        this.adornerLayer = null;

        //
        // Ensure that the state of the adorned control reflects that
        // the the adorner is no longer.
        //
        this.adornerShowState = AdornerShowState.Hidden;
    }

    /// <summary>
    /// Called to build the visual tree.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        ShowOrHideAdornerInternal();
    }

    /// <summary>
    /// Called when the mouse cursor enters the area of the adorned control.
    /// </summary>
    protected override void OnMouseEnter(MouseEventArgs e)
    {
        base.OnMouseEnter(e);

        MouseEnterLogic();
    }

    /// <summary>
    /// Called when the mouse cursor leaves the area of the adorned control.
    /// </summary>
    protected override void OnMouseLeave(MouseEventArgs e)
    {
        base.OnMouseLeave(e);

        MouseLeaveLogic();
    }

    /// <summary>
    /// Shared mouse enter code.
    /// </summary>
    private void MouseEnterLogic()
    {
        if (!IsMouseOverShowEnabled) return;

        _closeAdornerTimer.Stop();

        if (IsMutex)
        {
            var decorator = Util_Visual.FindParent<AdornerDecorator>(this);

            var ads = Util_Visual.FindChilds<AdornedControl>(decorator);

            foreach (var item in ads)
            {
                if (item != this && item.IsMutex)
                {
                    item.HideAdorner();
                }
            }
        }

        FadeInAdorner();
    }

    /// <summary>
    /// Shared mouse leave code.
    /// </summary>
    private void MouseLeaveLogic(bool isAdorner = false)
    {
        if (!IsMouseOverShowEnabled) return;

        if (HideIfMouseLeaveAdornerLayer)
        {
            if (isAdorner)
            {
                HideAdorner();
            }
            else
            {
                _closeAdornerTimer.Start();
            }
        }
        else
        {
            _closeAdornerTimer.Start();
        }
    }

    /// <summary>
    /// Called when the close adorner time-out has ellapsed, the mouse has moved
    /// away from the adorned control and the adorner and it is time to close the adorner.
    /// </summary>
    private void CloseAdornerTimer_Tick(object sender, EventArgs e)
    {
        _closeAdornerTimer.Stop();

        FadeOutAdorner();
    }

    /// <summary>
    /// Event raised when the fade in animation has completed.
    /// </summary>
    private void FadeInAnimation_Completed(object sender, EventArgs e)
    {
        adornerShowState = AdornerShowState.Visible;
    }

    /// <summary>
    /// Event raised when the fade-out animation has completed.
    /// </summary>
    private void FadeOutAnimation_Completed(object sender, EventArgs e)
    {
        if (adornerShowState == AdornerShowState.FadingOut)
        {
            // Still fading out, eg it wasn't aborted.
            this.HideAdorner();
        }
    }

    #endregion
}
