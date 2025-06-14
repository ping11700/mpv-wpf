﻿namespace Controls.Control;


/// <summary>
///     波浪进度条
/// </summary>
[TemplatePart(Name = ElementWave, Type = typeof(FrameworkElement))]
[TemplatePart(Name = ElementClip, Type = typeof(FrameworkElement))]
public class WaveProgressBar : RangeBase
{
    private const string ElementWave = "PART_Wave";

    private const string ElementClip = "PART_Clip";

    private FrameworkElement _waveElement;

    private const double TranslateTransformMinY = -20;

    private double _translateTransformYRange;

    private TranslateTransform _translateTransform;

    public WaveProgressBar() => Loaded += (s, e) => UpdateWave(Value);

    static WaveProgressBar()
    {
        FocusableProperty.OverrideMetadata(typeof(WaveProgressBar), new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
        MaximumProperty.OverrideMetadata(typeof(WaveProgressBar), new FrameworkPropertyMetadata(ValueBoxes.Double100Box));
    }

    protected override void OnMinimumChanged(double oldMinimum, double newMinimum) => UpdateWave(Value);

    protected override void OnMaximumChanged(double oldMaximum, double newMaximum) => UpdateWave(Value);

    protected override void OnValueChanged(double oldValue, double newValue)
    {
        base.OnValueChanged(oldValue, newValue);
        UpdateWave(newValue);
    }

    private void UpdateWave(double value)
    {
        if (_translateTransform == null || Util_Visual.IsVerySmall(Maximum)) return;
        var scale = 1 - value / Maximum;
        var y = _translateTransformYRange * scale + TranslateTransformMinY;
        _translateTransform.Y = y;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _waveElement = GetTemplateChild(ElementWave) as FrameworkElement;

        if (_waveElement != null && GetTemplateChild(ElementClip) is FrameworkElement clipElement)
        {
            _translateTransform = new TranslateTransform
            {
                Y = clipElement.Height
            };
            _translateTransformYRange = clipElement.Height - TranslateTransformMinY;
            _waveElement.RenderTransform = new TransformGroup
            {
                Children = { _translateTransform }
            };
        }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        "Text", typeof(string), typeof(WaveProgressBar), new PropertyMetadata(default(string)));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
        "ShowText", typeof(bool), typeof(WaveProgressBar), new PropertyMetadata(ValueBoxes.TrueBox));

    public bool ShowText
    {
        get => (bool)GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, ValueBoxes.BooleanBox(value));
    }

    public static readonly DependencyProperty WaveFillProperty = DependencyProperty.Register(
        "WaveFill", typeof(System.Windows.Media.Brush), typeof(WaveProgressBar), new PropertyMetadata(default(System.Windows.Media.Brush)));

    public System.Windows.Media.Brush WaveFill
    {
        get => (System.Windows.Media.Brush)GetValue(WaveFillProperty);
        set => SetValue(WaveFillProperty, value);
    }

    public static readonly DependencyProperty WaveThicknessProperty = DependencyProperty.Register(
        "WaveThickness", typeof(double), typeof(WaveProgressBar), new PropertyMetadata(ValueBoxes.Double0Box));

    public double WaveThickness
    {
        get => (double)GetValue(WaveThicknessProperty);
        set => SetValue(WaveThicknessProperty, value);
    }

    public static readonly DependencyProperty WaveStrokeProperty = DependencyProperty.Register(
        "WaveStroke", typeof(System.Windows.Media.Brush), typeof(WaveProgressBar), new PropertyMetadata(default(System.Windows.Media.Brush)));

    public System.Windows.Media.Brush WaveStroke
    {
        get => (System.Windows.Media.Brush)GetValue(WaveStrokeProperty);
        set => SetValue(WaveStrokeProperty, value);
    }
}

